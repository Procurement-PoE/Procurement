using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using POEApi.Infrastructure;
using System.Security;
using POEApi.Infrastructure.Events;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudflareSolverRe; 

namespace POEApi.Transport
{
    public class HttpTransport : ITransport
    {
        protected enum HttpMethod { GET, POST }

        private string _email;
        private CookieContainer credentialCookies;

        private bool _useProxy = false;
        private string _proxyUser;
        private string _proxyPassword;
        private string _proxyDomain;

        private const string LoginURL = @"https://www.pathofexile.com/login";
        private const string AccountURL = @"https://www.pathofexile.com/my-account";
        private const string AccountNameURL = @"https://api.pathofexile.com/profile";
        private const string CharacterURL = @"https://www.pathofexile.com/character-window/get-characters?&realm={0}";
        private const string StashURL = @"https://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}&accountName={2}&realm={3}";
        private const string InventoryURL = @"https://www.pathofexile.com/character-window/get-items?character={0}&accountName={1}&realm={2}";
        private const string HashRegEx = "name=\\\"hash\\\" value=\\\"(?<hash>[a-zA-Z0-9-]{1,})\\\"";
        private const string TitleRegex = @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>";

        private const string UpdateShopURL = @"https://www.pathofexile.com/forum/edit-thread/{0}";
        private const string BumpShopURL = @"https://www.pathofexile.com/forum/post-reply/{0}";

        protected const string UserAgent =
            @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; " +
            @".NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; " +
            @".NET CLR 1.1.4322)";
        protected const string ContentType = "application/x-www-form-urlencoded";

        public event ThottledEventHandler Throttled;

        private static TaskThrottle _taskThrottle = new TaskThrottle(TimeSpan.FromMinutes(1), 42, 42);

        public HttpTransport(string login)
        {
            credentialCookies = new CookieContainer();
            _email = login;
            _taskThrottle.Throttled += new ThottledEventHandler(instance_Throttled);
        }

        public HttpTransport(string login, string proxyUser, string proxyPassword, string proxyDomain)
            : this(login)
        {
            _proxyUser = proxyUser;
            _proxyPassword = proxyPassword;
            _proxyDomain = proxyDomain;
            _useProxy = true;
        }

        private void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(this, e);
        }

        public bool Authenticate(string email, SecureString password)
        {
            // A lot of users are reporting issue with logging in. Trimming their SessionID will hopefully improve
            // the situation.
            var unwrappedPassword = password.UnWrap();
            if (!string.IsNullOrEmpty(unwrappedPassword))
            {
                unwrappedPassword = unwrappedPassword.Trim();
            }

            credentialCookies.Add(new Cookie("POESESSID", unwrappedPassword, "/", ".pathofexile.com"));

            TraditionalSessionIdLogin();
            return true;
        }

        private void CloudFlareSessionIdLogin()
        {
            try
            {
                using (var clearanceHandler = new ClearanceHandler { })
                using (var handler = new HttpClientHandler() { CookieContainer = credentialCookies, Proxy = GetProxySettings() })
                {
                    clearanceHandler.InnerHandler = handler;

                    using (var client = new HttpClient(clearanceHandler))
                    {
                        var result = client.GetStringAsync(new Uri(LoginURL)).Result;
                    }
                }
            }
            catch (AggregateException ex) when (ex.InnerException is CloudflareSolverRe.Exceptions.CloudflareClearanceException)
            {
                // After all retries, clearance still failed.
                throw new Exception("Cloud flare clearance failed, please wait one minute and try again", ex);
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private void TraditionalSessionIdLogin()
        {
            using (var sessionIdLoginResponse = BuildHttpRequestAndGetResponse(HttpMethod.GET, AccountURL))
            {
                if (sessionIdLoginResponse.ResponseUri.ToString() != AccountURL)
                {
                    throw new LogonFailedException();
                }
            }
        }

        private HttpWebRequest GetHttpRequest(HttpMethod method, string url, bool? allowAutoRedirects = null,
            string requestData = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.CookieContainer = credentialCookies;
            request.UserAgent = UserAgent;
            request.Method = method.ToString();
            request.Proxy = GetProxySettings();
            request.ContentType = ContentType;

            if (allowAutoRedirects.HasValue)
            {
                request.AllowAutoRedirect = allowAutoRedirects.Value;
            }

            if (method == HttpMethod.POST && requestData != null)
            {
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(requestData);
                request.ContentLength = byteData.Length;
                Stream postStream = request.GetRequestStream();
                postStream.Write(byteData, 0, byteData.Length);
            }

            return request;
        }

        private IWebProxy GetProxySettings()
        {
            if (_useProxy)
                return ProcessProxySettings();

            return null;
        }

        public IWebProxy ProcessProxySettings()
        {
            var proxy = WebRequest.DefaultWebProxy;
            proxy.Credentials = new NetworkCredential(_proxyUser, _proxyPassword, _proxyDomain);

            return proxy;
        }

        protected HttpWebResponse BuildHttpRequestAndGetResponse(HttpMethod method, string url,
            bool? allowAutoRedirects = null, string requestData = null)
        {
            _taskThrottle.StartTask();
            try
            {
                HttpWebRequest request = GetHttpRequest(method, url, allowAutoRedirects, requestData);
                return (HttpWebResponse)request.GetResponse();
            }
            catch (System.Net.WebException ex)
            {
                Logger.Log(string.Format("Failed to build HTTP request and get response for: method={0}, url='{1}', " +
                    "allowAutoRedirects={2}, requestData='{3}': {4}", method.ToString(), url, allowAutoRedirects,
                    requestData, ex.Message));
                // It's possible (guaranteed?) that the WebException will have a response, which can be a valid page.
                // For example, for a 503 error when the site is down for maintenance:
                // ((HttpWebResponse)ex.Response).StatusCode --> ServiceUnavailable (enum)
                // ((HttpWebResponse)ex.Response).StatusDescription --> "Service Temporarily Unavailable"
                // In theory, we could return this response, and the caller would need to figure out if it is valid.
                throw;
            }
            finally
            {
                _taskThrottle.CompleteTask();
            }
        }

        protected MemoryStream PerformHttpRequest(HttpMethod method, string url, bool? allowAutoRedirects = null,
            string requestData = null)
        {
            using (var response = BuildHttpRequestAndGetResponse(method, url, allowAutoRedirects, requestData))
            {
                MemoryStream responseStream = GetMemoryStreamFromResponse(response);
                return responseStream;
            }
        }

        // The refresh parameter in this ITransport implementation is ignored.
        public Stream GetStash(int index, string league, string accountName, string realm, bool refresh)
        {
            var url = string.Format(StashURL, league, index, accountName, realm);
            return PerformHttpRequest(HttpMethod.GET, url);
        }

        public Stream GetStash(int index, string league, string accountName, string realm)
        {
            return GetStash(index, league, accountName, realm, false);
        }

        public Stream GetCharacters(string realm )
        {
            return PerformHttpRequest(HttpMethod.GET, string.Format(CharacterURL, realm));
        }

        public Stream GetAccountName(string realm )
        {
            return PerformHttpRequest(HttpMethod.GET, string.Format(AccountNameURL, realm));
        }

        // TODO(20180928): Throttle performing these requests?
        public Stream GetImage(string url)
        {
            WebClient client = new WebClient();
            client.Proxy = ProcessProxySettings();
            return new MemoryStream(client.DownloadData(url));
        }

        public Stream GetInventory(string characterName, bool forceRefresh, string accountName, string realm )
        {
            var url = string.Format(InventoryURL, characterName, accountName, realm);
            return PerformHttpRequest(HttpMethod.GET, url);
        }

        private MemoryStream GetMemoryStreamFromResponse(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            byte[] buffer = reader.ReadAllBytes();

            return new MemoryStream(buffer);
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            try
            {
                string threadHash = GetThreadHash(string.Format(UpdateShopURL, threadID), HashRegEx);
                if (string.IsNullOrEmpty(threadHash))
                {
                    throw new ForumThreadException("Unable to obtain thread hash to update thread.");
                }

                StringBuilder data = new StringBuilder();
                data.Append("title=" + Uri.EscapeDataString(threadTitle));
                data.Append("&content=" + Uri.EscapeDataString(threadText));
                data.Append("&hash=" + threadHash);

                var response = BuildHttpRequestAndGetResponse(HttpMethod.POST, string.Format(UpdateShopURL, threadID),
                    true, data.ToString());
                // TODO: Check if response.ResponseUri is for a view-thread URI or an edit-thread URI to determine if
                // the update request was a success or failure, respectively.

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Error updating shop thread: " + ex.ToString());
                return false;
            }
        }

        public bool BumpThread(string threadID, string threadTitle)
        {
            try
            {
                string threadHash = ValidateAndGetHash(string.Format(BumpShopURL, threadID), threadTitle,
                    HashRegEx);
                if (string.IsNullOrEmpty(threadHash))
                {
                    throw new ForumThreadException("Unable to obtain thread hash to bump thread.");
                }

                StringBuilder data = new StringBuilder();
                data.Append("hash=" + threadHash);
                data.Append("&content=" + Uri.EscapeDataString(
                    "[url=https://github.com/Stickymaddness/Procurement/]Bumped with Procurement![/url]"));
                data.Append("&post_submit=" + Uri.EscapeDataString("Submit"));

                var response = BuildHttpRequestAndGetResponse(HttpMethod.POST, string.Format(BumpShopURL, threadID),
                    true, data.ToString());
                // TODO: Check if response.ResponseUri is for a view-thread URI or an post-reply URI to determine if
                // the post request was a success or failure, respectively.

                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Error bumping shop thread: " + ex.ToString());
                return false;
            }
        }

        private string ValidateAndGetHash(string url, string threadTitle, string hashRegex)
        {
            string html = DownloadPageData(url);

            var title = Regex.Match(html, TitleRegex).Groups["Title"].Value;

            if (!title.ToLower().Contains(threadTitle.ToLower()))
                throw new ForumThreadException();

            return Regex.Match(html, hashRegex).Groups["hash"].Value;
        }

        private string GetThreadHash(string url, string regex)
        {
            string html = DownloadPageData(url);
            return Regex.Match(html, regex).Groups["hash"].Value;
        }

        private string DownloadPageData(string url)
        {
            var response = BuildHttpRequestAndGetResponse(HttpMethod.GET, url);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public class Realm
    {
        public const string PC = "pc";
        public const string XBOX = "xbox";
        public const string SONY = "sony";

        public static IEnumerable<string> AvailableRealms = new List<string>() {PC, XBOX, SONY};
    }
}
