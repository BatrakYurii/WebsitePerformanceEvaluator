using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebsitePerformanceEvaluator.WorkWithNet
{
    public static class UrlResponseCounter
    {
        public static List<UrlModel> GetResponceTime(List<string> urls)
        {
            List<UrlModel> urlResponses = new List<UrlModel>();

            foreach (string url in urls)
            {
                try
                {
                    //Creating httpwebrequest
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "HEAD";

                    //Starting timer
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    //Sending request
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //Stoping timer
                    stopwatch.Stop();

                    //Creating urlModel
                    urlResponses.Add(new UrlModel { Url = url, ResponceTime = stopwatch.ElapsedMilliseconds });
                }
                catch(WebException e)
                {
                    Console.WriteLine($"{e.Message} Souce: {url}");
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }

            // Order by lowest time responce to highest
            urlResponses = urlResponses.OrderBy(x => x.ResponceTime).ToList();
            return urlResponses;
        }
    }
}
