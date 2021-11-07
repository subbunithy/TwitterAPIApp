using Microsoft.Build.Utilities;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TwitterAPIApp
{
    class TwitterStreamAnalysis
    {        
        public static System.Threading.Tasks.Task Main()
        {
            string bearerToken = ConfigurationManager.AppSettings["BEARERKEY"] ;
            string uri = ConfigurationManager.AppSettings["URI"];

            if (bearerToken != null && uri != null)
            {
                HttpWebRequest twitterStream = WebRequest.Create(uri) as HttpWebRequest;
                GetStream(bearerToken, twitterStream);
            }
            else
            {
                Console.WriteLine("Error while getting the bearer token and uri.");

            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public static void GetStream(string bearerTokem, HttpWebRequest twitterStream)
        {
            try
            {
                twitterStream.Timeout = 180000000;
                twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerTokem;
                twitterStream.Method = "GET"; 
                twitterStream.Referer = "https://subbunithy.postman.co/workspace/My-Workspace~e6f893fd-01ea-4ad3-829a-47c0b97ad25e/request/18190746-759d6c78-7020-4987-99a0-cabed247bf61";

                var currentTime = DateTime.Now;
                int totalCount = GetTweetTotalCount(twitterStream).Result;
                var FinalTime = DateTime.Now;

                TimeSpan duration = DateTime.Parse(currentTime.ToString()).Subtract(DateTime.Parse(FinalTime.ToString()));
                var averageMinCount = totalCount / duration.TotalMinutes;
                var averageSecCount = totalCount / duration.TotalSeconds;

                Console.WriteLine("Total tweets: " + totalCount);
                Console.WriteLine("Average tweets per minute: " + averageMinCount);
                Console.WriteLine("Average tweets per second: " + averageSecCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while reading the tweets " + e.Message);
            }
        }
    public static async Task<int> GetTweetTotalCount(HttpWebRequest twitterStream)
        {
            try
            {

                Console.WriteLine("GetTweetTotalCount");
                int count = 0;
                using (StreamReader sr = new StreamReader(twitterStream.GetResponse().GetResponseStream()))
                {
                    while (true)
                    {
                        string line = await sr.ReadLineAsync();
                        if (line == null || count > 10000)
                        {
                            break;
                        }
                        Console.WriteLine("tweets............. " + line);
                        Console.WriteLine("count of tweets............. " + count);
                        count++;
                    }
                }
                return count;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while getting the tweets " + e.Message);
                return 0;
            }

        }
    }
}
