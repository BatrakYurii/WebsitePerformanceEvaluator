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
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "HEAD";
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    stopwatch.Stop();
                    urlResponses.Add(new UrlModel { Url = url, ResponceTime = stopwatch.ElapsedMilliseconds });
                }
                catch(WebException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
            urlResponses = urlResponses.OrderBy(x => x.ResponceTime).ToList();
            return urlResponses;
        }
    }
}
