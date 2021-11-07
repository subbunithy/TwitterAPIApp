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
        public static string access_token = "AAAAAAAAAAAAAAAAAAAAAIIWVgEAAAAAb3V1pOhAJ5QLpzL0JVu4UBUBkyA%3DJX8CTeFNGHaa3387zABNHPCgJtbIv0qQEvOkmJQ4i75fe5Pvih";
        public static HttpWebRequest twitterStream = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=id,text") as HttpWebRequest;

        public static int tweetCount = 0;


        public static void Main()
        {

            Console.WriteLine("GetTweetCountPerMinute");
            twitterStream.Timeout = 25000;
            twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            twitterStream.Method = "GET";
            int count = 0;
            using (StreamReader sr = new StreamReader(twitterStream.GetResponse().GetResponseStream()))
            {
                twitterStream.AllowWriteStreamBuffering = false;
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    count++; 
                    while (line != null)
                    {
                        GetTweetCountPerMinute(line);
                    }
                }
            }

            Console.WriteLine("count of tweets............. " + count);
        }
        public static void GetTweetCountPerMinute(string line)
        {
          
            var finalTime = DateTime.Now.AddMinutes(1);
            
                if (DateTime.Now < finalTime)
                {
                    tweetCount++; 
                } 
                 
            Console.WriteLine("Total no. of tweet per minute " + tweetCount);

        }



        public static async Task GetTweetTotalCount(HttpWebRequest twitterStream)
        {
            Console.WriteLine("GetTweetTotalCount");
            twitterStream.Timeout = 25000;
            twitterStream.Headers[HttpRequestHeader.Authorization] = "Bearer " + access_token;
            twitterStream.Method = "GET";

            twitterStream.AllowWriteStreamBuffering = false;
            int count = 0;

           
            Console.WriteLine("Total No. of Tweets : " + count); 
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