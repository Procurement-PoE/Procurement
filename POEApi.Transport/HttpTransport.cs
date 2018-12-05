using System;
using System.IO;
using System.Net;
using System.Text;
using POEApi.Infrastructure;
using System.Security;
using POEApi.Infrastructure.Events;
using System.Text.RegularExpressions;

namespace POEApi.Transport
{
    public class HttpTransport : ITransport
    {
        protected enum HttpMethod { GET, POST }

        private string email;
        private CookieContainer credentialCookies;

        private bool useProxy = false;
        private string proxyUser;
        private string proxyPassword;
        private string proxyDomain;

        private const string loginURL = @"https://www.pathofexile.com/login";
        private const string accountNameURL = @"https://www.pathofexile.com/character-window/get-account-name";
        private const string characterURL = @"https://www.pathofexile.com/character-window/get-characters";
        private const string stashURL = @"https://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}&accountName={2}";
        private const string inventoryURL = @"http://www.pathofexile.com/character-window/get-items?character={0}&accountName={1}";
        private const string hashRegEx = "name=\\\"hash\\\" value=\\\"(?<hash>[a-zA-Z0-9-]{1,})\\\"";
        private const string titleRegex = @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>";

        private const string updateShopURL = @"https://www.pathofexile.com/forum/edit-thread/{0}";
        private const string bumpShopURL = @"https://www.pathofexile.com/forum/post-reply/{0}";

        protected const string USER_AGENT =
            @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; " +
            @".NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; " +
            @".NET CLR 1.1.4322)";
        protected const string CONTENT_TYPE = "application/x-www-form-urlencoded";

        public event ThottledEventHandler Throttled;

        private static TaskThrottle taskThrottle = new TaskThrottle(TimeSpan.FromMinutes(1), 42, 42);

        public HttpTransport(string login)
        {
            credentialCookies = new CookieContainer();
            this.email = login;
            taskThrottle.Throttled += new ThottledEventHandler(instance_Throttled);
        }

        public HttpTransport(string login, string proxyUser, string proxyPassword, string proxyDomain)
            : this(login)
        {
            this.proxyUser = proxyUser;
            this.proxyPassword = proxyPassword;
            this.proxyDomain = proxyDomain;
            this.useProxy = true;
        }

        private void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(this, e);
        }

        public bool Authenticate(string email, SecureString password, bool useSessionID)
        {
            if (useSessionID)
            {
                // A lot of users are reporting issue with logging in. Trimming their SessionID will hopefully improve
                // the situation.
                var unwrappedPassword = password.UnWrap();
                if (!string.IsNullOrEmpty(unwrappedPassword))
                {
                    unwrappedPassword = unwrappedPassword.Trim();
                }

                credentialCookies.Add(new Cookie("POESESSID", unwrappedPassword, "/", ".pathofexile.com"));
                using (var sessionIdLoginResponse = BuildHttpRequestAndGetResponse(HttpMethod.GET, loginURL))
                {
                    // If the response URI is the login URL, then the login action failed.
                    if (sessionIdLoginResponse.ResponseUri.ToString() == loginURL)
                        throw new LogonFailedException();
                }
                return true;
            }

            var loginResponseStream = PerformHttpRequest(HttpMethod.GET, loginURL);
            string loginResponse = Encoding.Default.GetString(loginResponseStream.ToArray());
            string hashValue = Regex.Match(loginResponse, hashRegEx).Groups["hash"].Value;

            StringBuilder data = new StringBuilder();
            data.Append("login_email=" + Uri.EscapeDataString(email));
            data.Append("&login_password=" + Uri.EscapeDataString(password.UnWrap()));
            data.Append("&remember_me=0");
            data.Append("&hash=" + hashValue);
            data.Append("&login=Login");

            var response = BuildHttpRequestAndGetResponse(HttpMethod.POST, loginURL, false, data.ToString());

            // If the response is not a redirect, then the login action failed.
            if (response.StatusCode != HttpStatusCode.Found)
                throw new LogonFailedException(this.email);

            return true;
        }

        private HttpWebRequest getHttpRequest(HttpMethod method, string url, bool? allowAutoRedirects = null,
            string requestData = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.CookieContainer = credentialCookies;
            request.UserAgent = USER_AGENT;
            request.Method = method.ToString();
            request.Proxy = getProxySettings();
            request.ContentType = CONTENT_TYPE;

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

        private IWebProxy getProxySettings()
        {
            if (useProxy)
                return processProxySettings();

            return null;
        }

        public IWebProxy processProxySettings()
        {
            var proxy = WebRequest.DefaultWebProxy;
            proxy.Credentials = new NetworkCredential(proxyUser, proxyPassword, proxyDomain);

            return proxy;
        }

        protected HttpWebResponse BuildHttpRequestAndGetResponse(HttpMethod method, string url,
            bool? allowAutoRedirects = null, string requestData = null)
        {
            taskThrottle.StartTask();
            try
            {
                HttpWebRequest request = getHttpRequest(method, url, allowAutoRedirects, requestData);
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
                taskThrottle.CompleteTask();
            }
        }

        protected MemoryStream PerformHttpRequest(HttpMethod method, string url, bool? allowAutoRedirects = null,
            string requestData = null)
        {
            using (var response = BuildHttpRequestAndGetResponse(method, url, allowAutoRedirects, requestData))
            {
                MemoryStream responseStream = getMemoryStreamFromResponse(response);
                return responseStream;
            }
        }

        // TODO(20180928): Remove the refresh parameter?
        public Stream GetStash(int index, string league, string accountName, bool refresh)
        {
            var url = string.Format(stashURL, league, index, accountName);
            return PerformHttpRequest(HttpMethod.GET, url);
        }

        public Stream GetStash(int index, string league, string accountName)
        {
            return GetStash(index, league, accountName, false);
        }

        public Stream GetCharacters()
        {
            return PerformHttpRequest(HttpMethod.GET, characterURL);
        }

        public Stream GetAccountName()
        {
            return PerformHttpRequest(HttpMethod.GET, accountNameURL);
        }

        // TODO(20180928): Throttle performing these requests?
        public Stream GetImage(string url)
        {
            WebClient client = new WebClient();
            client.Proxy = processProxySettings();
            return new MemoryStream(client.DownloadData(url));
        }

        public Stream GetInventory(string characterName, bool forceRefresh, string accountName)
        {
            var url = string.Format(inventoryURL, characterName, accountName);
            return PerformHttpRequest(HttpMethod.GET, url);
        }

        private MemoryStream getMemoryStreamFromResponse(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            byte[] buffer = reader.ReadAllBytes();

            return new MemoryStream(buffer);
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            try
            {
                string threadHash = getThreadHash(string.Format(updateShopURL, threadID), hashRegEx);
                if (string.IsNullOrEmpty(threadHash))
                {
                    throw new ForumThreadException("Unable to obtain thread hash to update thread.");
                }

                StringBuilder data = new StringBuilder();
                data.Append("title=" + Uri.EscapeDataString(threadTitle));
                data.Append("&content=" + Uri.EscapeDataString(threadText));
                data.Append("&hash=" + threadHash);

                var response = BuildHttpRequestAndGetResponse(HttpMethod.POST, string.Format(updateShopURL, threadID),
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
                string threadHash = validateAndGetHash(string.Format(bumpShopURL, threadID), threadTitle,
                    hashRegEx);
                if (string.IsNullOrEmpty(threadHash))
                {
                    throw new ForumThreadException("Unable to obtain thread hash to bump thread.");
                }

                StringBuilder data = new StringBuilder();
                data.Append("hash=" + threadHash);
                data.Append("&content=" + Uri.EscapeDataString(
                    "[url=https://github.com/Stickymaddness/Procurement/]Bumped with Procurement![/url]"));
                data.Append("&post_submit=" + Uri.EscapeDataString("Submit"));

                var response = BuildHttpRequestAndGetResponse(HttpMethod.POST, string.Format(bumpShopURL, threadID),
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

        private string validateAndGetHash(string url, string threadTitle, string hashRegex)
        {
            string html = downloadPageData(url);

            var title = Regex.Match(html, titleRegex).Groups["Title"].Value;

            if (!title.ToLower().Contains(threadTitle.ToLower()))
                throw new ForumThreadException();

            return Regex.Match(html, hashRegex).Groups["hash"].Value;
        }

        private string getThreadHash(string url, string regex)
        {
            string html = downloadPageData(url);
            return Regex.Match(html, regex).Groups["hash"].Value;
        }

        private string downloadPageData(string url)
        {
            var response = BuildHttpRequestAndGetResponse(HttpMethod.GET, url);
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}