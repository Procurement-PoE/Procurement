using System;
using System.IO;
using System.Net;
using System.Text;
using POEApi.Infrastructure;
using System.Security;
using POEApi.Infrastructure.Events;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace POEApi.Transport
{
    public class HttpTransport : ITransport
    {
        private string email;
        private CookieContainer credentialCookies;

        private bool useProxy = false;
        private string proxyUser;
        private string proxyPassword;
        private string proxyDomain;

        private enum HttpMethod { GET, POST }

        private const string loginURL = @"https://www.pathofexile.com/login";
        private const string characterURL = @"https://www.pathofexile.com/character-window/get-characters";
        private const string stashURL = @"https://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}&accountName={2}";
        private const string inventoryURL = @"https://www.pathofexile.com/character-window/get-items?character={0}&accountName={1}";
        private const string hashRegEx = "name=\\\"hash\\\" value=\\\"(?<hash>[a-zA-Z0-9]{1,})\\\"";

        private const string updateThreadHashEx = "name=\\\"forum_thread\\\" value=\\\"(?<hash>[a-zA-Z0-9]{1,})\\\"";
        private const string bumpThreadHashEx = "name=\\\"forum_post\\\" value=\\\"(?<hash>[a-zA-Z0-9]{1,})\\\"";
        private const string titleRegex = @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>";

        private const string updateShopURL = @"https://www.pathofexile.com/forum/edit-thread/{0}";
        private const string bumpShopURL = @"https://www.pathofexile.com/forum/post-reply/{0}";

        private const string myAccountURL = @"https://www.pathofexile.com/my-account";

        //Garena+ RU strings
        private const string loginURL_ru = @"https://web.poe.garena.ru/login";
        private const string myAccountURL_ru = @"https://web.poe.garena.ru/my-account";
        private const string characterURL_ru = @"https://web.poe.garena.ru/character-window/get-characters";
        private const string stashURL_ru = @"https://web.poe.garena.ru/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}";
        private const string inventoryURL_ru = @"https://web.poe.garena.ru/character-window/get-items?character={0}";
        private const string updateShopURL_ru = @"https://web.poe.garena.ru/forum/edit-thread/{0}";
        private const string bumpShopURL_ru = @"https://web.poe.garena.ru/forum/post-reply/{0}";

        public event ThottledEventHandler Throttled;

        public HttpTransport(string login)
        {
            credentialCookies = new CookieContainer();
            this.email = login;
            RequestThrottle.Instance.Throttled += new ThottledEventHandler(instance_Throttled);
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

        public string Authenticate(string email, SecureString password, bool useSessionID, string current_accname, string server_type)
        {
            if (useSessionID)
            {
                credentialCookies.Add(new System.Net.Cookie("PHPSESSID", password.UnWrap(), "/", "www.pathofexile.com"));
                HttpWebRequest confirmAuth = getHttpRequest(HttpMethod.GET, loginURL);
                HttpWebResponse confirmAuthResponse = (HttpWebResponse)confirmAuth.GetResponse();

                if (confirmAuthResponse.ResponseUri.ToString() == loginURL)
                    throw new LogonFailedException(server_type);
                return "<SessionID used>";
            }

            if (server_type=="Garena (RU)")
            {
                credentialCookies.Add(new System.Net.Cookie("PHPSESSID", password.UnWrap(), "/", "web.poe.garena.ru"));
                HttpWebRequest confirmAuth = getHttpRequest(HttpMethod.GET, loginURL_ru);
                HttpWebResponse confirmAuthResponse = (HttpWebResponse)confirmAuth.GetResponse();

                if (confirmAuthResponse.ResponseUri.ToString() != getServerTypeURLmyaccount(server_type))
                    throw new LogonFailedException(server_type);
                return "<SessionID используется>";
            }

            HttpWebRequest getHash = getHttpRequest(HttpMethod.GET, loginURL);
            HttpWebResponse hashResponse = (HttpWebResponse)getHash.GetResponse();
            string loginResponse = Encoding.Default.GetString(getMemoryStreamFromResponse(hashResponse).ToArray());
            string hashValue = Regex.Match(loginResponse, hashRegEx).Groups["hash"].Value;

            HttpWebRequest request = getHttpRequest(HttpMethod.POST, loginURL);
            request.AllowAutoRedirect = true;//allow 2 seq redirects to myaccount page - Status 302->Status 301->Status 200(OK)

            StringBuilder data = new StringBuilder();
            data.Append("login_email=" + Uri.EscapeDataString(email));
            data.Append("&login_password=" + Uri.EscapeDataString(password.UnWrap()));
            data.Append("&hash=" + hashValue);
            data.Append("&login=Login");

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            request.ContentLength = byteData.Length;

            Stream postStream = request.GetRequestStream();
            postStream.Write(byteData, 0, byteData.Length);

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();

            //If we didn't get a my-account page, your gonna have a bad time.
            if (response.ResponseUri.AbsoluteUri != myAccountURL)
                throw new LogonFailedException(this.email,server_type);

            //get my-account HTML text
            StreamReader http_resp = new StreamReader(response.GetResponseStream());
            string s_http_resp = http_resp.ReadToEnd();
            http_resp.Close();

            //extract account name from HTML text
            //<span class="profile-link" ><a href="/account/view-profile/accname">accname</a></span>
            string regexp_pattern = @"\<a href=""/account/view-profile/.*?\>(?<accname>.+?)\<\/a\>";

            Regex regexp = new Regex(regexp_pattern, RegexOptions.ExplicitCapture);
            MatchCollection matches = regexp.Matches(s_http_resp);

            if (matches.Count > 0)
            {
                //return AccountName
                return matches[0].Groups["accname"].Value;
            }
            else
            {
                return "";
            }
        }

        private HttpWebRequest getHttpRequest(HttpMethod method, string url)
        {
            HttpWebRequest request = (HttpWebRequest)RequestThrottle.Instance.Create(url);

            request.CookieContainer = credentialCookies;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Method = method.ToString();
            request.Proxy = getProxySettings();
            
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (method.ToString()=="POST") request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;

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
            System.Net.IWebProxy proxy = System.Net.WebRequest.GetSystemWebProxy();
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(proxyUser, proxyPassword, proxyDomain);
            proxy.Credentials = credentials;

            return proxy;
        }

        public Stream GetStash(int index, string league, bool refresh, string accname, string server_type)
        {
            string active_url = getServerTypeURLstash(server_type);

            HttpWebRequest request=null;
            if (accname.Contains("<SessionID"))
            {
                //accountName parameter not used with SessionID
                request = getHttpRequest(HttpMethod.GET, string.Format(active_url.Replace("&accountName={2}",""), league, index, ""));
            }
            else
            {
                request = getHttpRequest(HttpMethod.GET, string.Format(active_url, league, index, accname));
            }
            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return getMemoryStreamFromResponse(response);
        }

        public Stream GetStash(int index, string league, string accname, string server_type)
        {
            return GetStash(index, league, false, accname, server_type);
        }

        public Stream GetCharacters(string server_type)
        {
            string active_url=getServerTypeURLcharacter(server_type);

            HttpWebRequest request = getHttpRequest(HttpMethod.GET, active_url);
            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return getMemoryStreamFromResponse(response);
        }

        public Stream GetImage(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Proxy = processProxySettings();

                return new MemoryStream(client.DownloadData(url));
            }
        }

        public Stream GetInventory(string characterName, bool forceRefresh, string accname, string server_type)
        {
            string active_url = getServerTypeURLinventory(server_type);

            HttpWebRequest request=null;
            if (accname.Contains("<SessionID"))
            {
                //accountName parameter not used with SessionID
                request = getHttpRequest(HttpMethod.GET, string.Format(active_url.Replace("&accountName={1}", ""), characterName, ""));
            }
            else
            {
                request = getHttpRequest(HttpMethod.GET, string.Format(active_url, characterName, accname));
            }
            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return getMemoryStreamFromResponse(response);
        }

        private MemoryStream getMemoryStreamFromResponse(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            byte[] buffer = reader.ReadAllBytes();

            RequestThrottle.Instance.Complete();
            response.Close();
            reader.Close();

            return new MemoryStream(buffer);
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText, string server_type)
        {
            try
            {
                string active_url = getServerTypeURLupdateshop(server_type);
                string threadHash = getThreadHash(string.Format(active_url, threadID), updateThreadHashEx);

                StringBuilder data = new StringBuilder();
                data.Append("title=" + Uri.EscapeDataString(threadTitle));
                data.Append("&content=" + Uri.EscapeDataString(threadText));
                data.Append("&forum_thread=" + threadHash);

                HttpStatusCode update_status = postToForum(data.ToString(), string.Format(active_url, threadID));

                if (update_status == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Logger.Log("Error updating shop thread - status not OK: " + update_status.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error updating shop thread: " + ex.ToString());
                return false;
            }
        }

        public bool BumpThread(string threadID, string threadTitle, string server_type)
        {
            try
            {
                string active_url = getServerTypeURLbumpshop(server_type);
                string threadHash = validateAndGetHash(string.Format(active_url, threadID), threadTitle, bumpThreadHashEx);

                StringBuilder data = new StringBuilder();
                data.Append("forum_post=" + threadHash);
                data.Append("&content=" + Uri.EscapeDataString("[url=https://github.com/medvedttn/Procurement/]Bumped with Procurement Medved Edition![/url]"));
                data.Append("&post_submit=" + Uri.EscapeDataString("Submit"));

                HttpStatusCode bump_response = postToForum(data.ToString(), string.Format(active_url, threadID));

                if (bump_response == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Logger.Log("Error bumping shop thread - status not OK: " + bump_response.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex is ForumThreadException)
                    throw;

                Logger.Log("Error bumping shop thread: " + ex.ToString());
                return false;
            }
        }

        private HttpStatusCode postToForum(string data, string url)
        {
            HttpWebRequest request = getHttpRequest(HttpMethod.POST, url);
            request.AllowAutoRedirect = true;
            request.Timeout = 60000;

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data);

            request.ContentLength = byteData.Length;

            Stream postStream = request.GetRequestStream();
            postStream.Write(byteData, 0, byteData.Length);

            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            HttpStatusCode update_status = response.StatusCode;

            postStream.Close();
            response.Close();
            return update_status;
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
            HttpWebRequest getHashRequest = getHttpRequest(HttpMethod.GET, url);
            getHashRequest.Timeout = 60000;
            getHashRequest.KeepAlive = true;
            getHashRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            HttpWebResponse hashResponse = (HttpWebResponse)getHashRequest.GetResponse();

            using (StreamReader reader = new StreamReader(hashResponse.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private string getServerTypeURLcharacter(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = characterURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = characterURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLinventory(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = inventoryURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = inventoryURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLstash(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = stashURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = stashURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLlogin(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = loginURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = loginURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLmyaccount(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = myAccountURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = myAccountURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLbumpshop(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = bumpShopURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = bumpShopURL_ru;
            }

            return active_url;
        }

        private string getServerTypeURLupdateshop(string server_type)
        {
            string active_url = "";
            if (server_type == "International")
            {
                active_url = updateShopURL;
            }
            else if (server_type == "Garena (RU)")
            {
                active_url = updateShopURL_ru;
            }

            return active_url;
        }
    }
}