using LinqToTwitter;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterAPIApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var oauth_token = "96769910-rioPLEPHBZFaFHGAMY8MvDno62vUAXDe7ZEVyeADb";
            //var oauth_token_secret = "txKXzGSpc3Qw13fCUpHZRDUM3QKAIwZqtD71W58PIAsUI";
            //var oauth_consumer_key = "xxxSARaIQAQ1bHXiPhaJVx23IKJInnXYdwcTcicWcIcfJBHddk4EZ";
            //var oauth_consumer_secret = "AUMr1pAqedh2NNEIlWxBYicCO";

            //// oauth implementation details
            //var oauth_version = "1.0";
            //var oauth_signature_method = "HMAC-SHA1";

            //// unique request details
            //var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            //var timeSpan = DateTime.UtcNow
            //    - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();


            //// create oauth signature
            //var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
            //                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";

            //var baseString = string.Format(baseFormat,
            //                            oauth_consumer_key,
            //                            oauth_nonce,
            //                            oauth_signature_method,
            //                            oauth_timestamp,
            //                            oauth_token,
            //                            oauth_version,
            //                            Uri.EscapeDataString(q)
            //                            );


            //baseString = string.Concat("GET&", Uri.EscapeDataString("https://api.twitter.com/2/tweets/sample/stream"), " & ", Uri.EscapeDataString(baseString));

            //var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
            //                        "&", Uri.EscapeDataString(oauth_token_secret));

            //string oauth_signature;
            //using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            //{
            //    oauth_signature = Convert.ToBase64String(
            //        hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            //}

            //// create the request header
            //var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
            //                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
            //                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
            //                   "oauth_version=\"{6}\"";

            //var authHeader = string.Format(headerFormat,
            //                        Uri.EscapeDataString(oauth_nonce),
            //                        Uri.EscapeDataString(oauth_signature_method),
            //                        Uri.EscapeDataString(oauth_timestamp),
            //                        Uri.EscapeDataString(oauth_consumer_key),
            //                        Uri.EscapeDataString(oauth_token),
            //                        Uri.EscapeDataString(oauth_signature),
            //                        Uri.EscapeDataString(oauth_version)
            //                );

            var access_token = "AAAAAAAAAAAAAAAAAAAAAOyMVQEAAAAA8KWm7pl4WvYPh5DMGtSfX3z9S5c%3DJuTLlp629Rz9wpIt7u6XtBCvmfY5lLvmnI8vyOQ7lNxoLMIzoX";
            var gettimeline = WebRequest.Create("https://api.twitter.com/1.1/statuses/user_timeline.json?count=3&screen_name=twitterapi") as HttpWebRequest;

            var twitterStream = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,text") as HttpWebRequest;

            gettimeline.Method = "GET";
            twitterStream.Method = "GET";
            gettimeline.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            string html = "";
            try
            {
                string respbody = null;
                using (var resp = twitterStream.GetResponse().GetResponseStream())//there request sends
                {
                    var respR = new StreamReader(resp);
                    //respbody = respR.ReadToEnd();
                    respbody = await respR.ReadToEndAsync();
                }

                //var response = (HttpWebResponse)twitterStream.GetResponse();
                //var reader = new StreamReader(response.GetResponseStream());
                //var objText = reader.ReadToEnd();
                ////TODO use a library to parse json 
                //try
                //{
                //    JArray jsonDat = JArray.Parse(respbody);
                //    for (int x = 0; x < jsonDat.Count(); x++)
                //    {
                //        //html += jsonDat[x]["id"].ToString() + "<br/>";
                //        html += jsonDat[x]["text"].ToString() + "<br/>";
                //        // html += jsonDat[x]["name"].ToString() + "<br/>";
                //        html += jsonDat[x]["created_at"].ToString() + "<br/>";

                //    } 
                //}
                //catch (Exception twit_error)
                //{
                //    Console.WriteLine(twit_error);
                //}
            }
            catch (Exception e)//401 (access token invalid or expired)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine(html);

            //var twitterCtx = new TwitterContext(auth);
            //Console.WriteLine("Enter the username whose tweets you want to see");
            //var userInput = Console.ReadLine();
            //Console.WriteLine("Loading Top 10 Tweets\n");

            //var tweets = (from tweet in twitterCtx.Status
            //              where tweet.Type == StatusType.User
            //              && tweet.ScreenName == userInput
            //             select tweet).Take(10);

            //foreach (var tweet in tweets)
            //{
            //    Console.WriteLine($"{tweet.User.Name} - {tweet.Text.Trim()}\n");
            //}
            //Console.ReadLine();
        }

        public void test()
        {
            string stream_url = ConfigurationManager.AppSettings["stream_url"];

            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            StreamReader responseStream = null;
            MessageQueue q = null;
            string useQueue = ConfigurationManager.AppSettings["use_queue"];
            string postparameters = (ConfigurationManager.AppSettings["track_keywords"].Length == 0 ? string.Empty : "&track=" + ConfigurationManager.AppSettings["track_keywords"]) +
                                    (ConfigurationManager.AppSettings["follow_userid"].Length == 0 ? string.Empty : "&follow=" + ConfigurationManager.AppSettings["follow_userid"]) +
                                    (ConfigurationManager.AppSettings["location_coord"].Length == 0 ? string.Empty : "&locations=" + ConfigurationManager.AppSettings["location_coord"]);

            if (!string.IsNullOrEmpty(postparameters))
            {
                if (postparameters.IndexOf('&') == 0)
                    postparameters = postparameters.Remove(0, 1).Replace("#", "%23");
            }

            int wait = 250;
            string jsonText = "";

            Logger logger = new Logger();


            try
            {
                //Message Queue
                if (useQueue == "true")
                {
                    if (MessageQueue.Exists(@".\private$\Twitter"))
                        q = new MessageQueue(@".\private$\Twitter");
                    else
                        q = MessageQueue.Create(@".\private$\Twitter");
                }

                while (true)
                {
                    try
                    {
                        //Connect
                        webRequest = (HttpWebRequest)WebRequest.Create(stream_url);
                        webRequest.Timeout = -1;
                        webRequest.Headers.Add("Authorization", GetAuthHeader(stream_url + "?" + postparameters));

                        Encoding encode = Encoding.GetEncoding("utf-8");
                        if (postparameters.Length > 0)
                        {
                            webRequest.Method = "POST";
                            webRequest.ContentType = "application/x-www-form-urlencoded";

                            byte[] _twitterTrack = encode.GetBytes(postparameters);

                            webRequest.ContentLength = _twitterTrack.Length;
                            Stream _twitterPost = webRequest.GetRequestStream();
                            _twitterPost.Write(_twitterTrack, 0, _twitterTrack.Length);
                            _twitterPost.Close();
                        }

                        webResponse = (HttpWebResponse)webRequest.GetResponse();
                        responseStream = new StreamReader(webResponse.GetResponseStream(), encode);

                        //Read the stream.
                        while (true)
                        {
                            jsonText = responseStream.ReadLine();
                            //Post each message to the queue.
                            if (useQueue == "true")
                            {
                                Message message = new Message(jsonText);
                                q.Send(message);
                            }

                            //Success
                            wait = 250;

                            //Write Status
                            Console.Write(jsonText);
                        }
                        //Abort is needed or responseStream.Close() will hang.
                        webRequest.Abort();
                        responseStream.Close();
                        responseStream = null;
                        webResponse.Close();
                        webResponse = null;
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.append(ex.Message, Logger.LogLevel.ERROR);
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            //-- From Twitter Docs -- 
                            //When a HTTP error (> 200) is returned, back off exponentially. 
                            //Perhaps start with a 10 second wait, double on each subsequent failure, 
                            //and finally cap the wait at 240 seconds. 
                            //Exponential Backoff
                            if (wait < 10000)
                                wait = 10000;
                            else
                            {
                                if (wait < 240000)
                                    wait = wait * 2;
                            }
                        }
                        else
                        {
                            //-- From Twitter Docs -- 
                            //When a network error (TCP/IP level) is encountered, back off linearly. 
                            //Perhaps start at 250 milliseconds and cap at 16 seconds.
                            //Linear Backoff
                            if (wait < 16000)
                                wait += 250;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.append(ex.Message, Logger.LogLevel.ERROR);
                    }
                    finally
                    {
                        if (webRequest != null)
                            webRequest.Abort();
                        if (responseStream != null)
                        {
                            responseStream.Close();
                            responseStream = null;
                        }

                        if (webResponse != null)
                        {
                            webResponse.Close();
                            webResponse = null;
                        }
                        Console.WriteLine("Waiting: " + wait);
                        Thread.Sleep(wait);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.append(ex.Message, Logger.LogLevel.ERROR);
                Console.WriteLine("Waiting: " + wait);
                Thread.Sleep(wait);
            }
        }

    }
}
}
