using LinqToTwitter;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterAPIApp
{
    class Twitter
    {
        public static string access_token = "AAAAAAAAAAAAAAAAAAAAAOyMVQEAAAAA%2FY9uvUXxUeAlHvXBZpGwzVs8rus%3D7AP5OEtsvFpGpF3v5ZBDQu11vVvWfbukvdQVbTNmQGENCgufez";
        public static HttpWebRequest twitterStream = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,text") as HttpWebRequest;    



        public static async Task Main()
        {

          
            Task<int> taskGetTweetTotalCount = GetTweetTotalCount();
            Console.WriteLine("Total No. of Tweets : " + taskGetTweetTotalCount.Result);
           // taskGetTweetTotalCount.Wait();

            Console.WriteLine("GetTweetCountPerMinute");
            Task<int> taskGetTweetCountPerMinute = GetTweetCountPerMinute();
            Console.WriteLine("Total no. of tweet per minute " + taskGetTweetCountPerMinute.Result);


            await Task.WhenAll(taskGetTweetTotalCount, taskGetTweetCountPerMinute);

            Console.ReadLine();
        }
        public static Task<int> GetTweetCountPerMinute()
        {

            twitterStream.Timeout = 25000;
            twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            twitterStream.Method = "GET";

            int tweetCount = 0;

            using (StreamReader sr = new StreamReader(twitterStream.GetResponse().GetResponseStream()))
            {
                var finalTime = DateTime.Now.AddMinutes(1);
                string line =  sr.ReadLine();
                while (line != null)
                {
                    if (DateTime.Now < finalTime)
                    {
                        tweetCount++;
                        Console.WriteLine("counting no. of tweets per minutes............. " + tweetCount);
                    }
                    else
                    {
                        break;
                    }

                }
                return tweetCount;
            }
        }



        public static async Task<int> GetTweetTotalCount()
        {
            twitterStream.Timeout = 25000;
            twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            twitterStream.Method = "GET";

            int count = 0;

            using (StreamReader sr = new StreamReader(twitterStream.GetResponse().GetResponseStream()))
            {
                while (true)
                {
                    string line = await sr.ReadLineAsync();
                    if (line == null)
                    {
                        break;
                    }
                    Console.WriteLine("count of tweets............. " + count);
                    count++;
                }
            }
            Console.WriteLine("At end of CountLinesAsync" + count);
            return count; 
        }
        //public static void GetTweets()
        //{
        //    var access_token = "AAAAAAAAAAAAAAAAAAAAAOyMVQEAAAAA%2FY9uvUXxUeAlHvXBZpGwzVs8rus%3D7AP5OEtsvFpGpF3v5ZBDQu11vVvWfbukvdQVbTNmQGENCgufez";
        //    var twitterStream = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,text") as HttpWebRequest;
        //    twitterStream.Timeout = 25000;
        //    twitterStream.Method = "GET";
        //    int tweetCount = 0;
        //    twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
        //    try
        //    {

        //        using (StreamReader sr = new StreamReader(twitterStream.GetResponse().GetResponseStream()))
        //        {
        //            while (await sr.ReadLineAsync() != null)
        //            {
        //                tweetCount++;
        //                Console.WriteLine("Total no. of tweet per minute " + tweetCount);
        //            }
        //            var finalTime = DateTime.Now.AddMinutes(1);

        //            while (sr.ReadLine() != null)
        //            {
        //                if (DateTime.Now < finalTime)
        //                {
        //                    tweetCount++;

        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        Console.WriteLine("Total no. of tweet per minute " + tweetCount);
        //    }
        //    catch (Exception e)//401 (access token invalid or expired)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}
    }
}