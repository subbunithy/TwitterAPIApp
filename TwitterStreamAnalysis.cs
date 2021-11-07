using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TwitterAPIApp
{
    class TwitterStreamAnalysis
    {        
        public static void Main()
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

                TimeSpan duration = DateTime.Parse(FinalTime.ToString()).Subtract(DateTime.Parse(currentTime.ToString()));
                var averageMinCount = totalCount / duration.TotalMinutes;
                var averageSecCount = totalCount / duration.TotalSeconds;

                Console.WriteLine("Total tweets: " + totalCount);
                Console.WriteLine("");
                Console.WriteLine("Average tweets per minute: " + averageMinCount);
                Console.WriteLine("");
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
                        //Added count logic as  the 1% stream itself returns large data.
                        // uncomment below for testing less no. of tweets.
                        //if (line == null || count > 10)
                        if (line == null)
                        {
                            break;
                        }
                        Console.WriteLine("tweet............. " + line); 
                        Console.WriteLine("");
                        Console.WriteLine("count of tweets............. " + count);
                        Console.WriteLine("");
                        count++;
                    }
                }
                return count;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while getting the tweets  " + e.Message);
                return 0;
            }

        }
    }
}
