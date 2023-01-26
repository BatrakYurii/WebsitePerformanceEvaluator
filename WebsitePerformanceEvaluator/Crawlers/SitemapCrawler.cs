using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsitePerformanceEvaluator.Abstractions;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Web;

namespace WebsitePerformanceEvaluator.Crawlers
{
    public class SitemapCrawler : ISitemapCrawler
    {        
        public List<string> CrawlBySitemap(string url)
        {
            List<string> urls = new List<string>();
            Queue<string> queue = new Queue<string>();

            var urlXmlPage = url + "sitemap.xml";
            queue.Enqueue(urlXmlPage);
            try
            {
                while(queue.Count > 0)
                {
                    var currentUrl = queue.Dequeue();

                    //Create webclient to get data
                    using WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;

                    //Download sitemap as a string
                    string sitemapString = wc.DownloadString(currentUrl);

                    //Using XmlReader to read each element innertext. 
                    //It is going to help avoid exceptions with specifical symbols when load it to XmlDocument
                    using (XmlReader reader = XmlReader.Create(new StringReader(sitemapString)))
                    {
                        while (reader.Read())
                        {
                            if (reader.Name == "loc")
                            {
                                var element = reader.ReadElementContentAsString();
                                if (element.Contains("sitemap"))
                                    queue.Enqueue(element);
                                else
                                    urls.Add(element);
                            }
                        }
                    }                        
                }

                //Remove all identical strings from the list
                urls = urls.Select(x => x.TrimEnd('/')).Distinct().Select(x => x + '/').ToList();
            }
            catch(WebException e)
            {
                Console.WriteLine($"{e.Message} Probably you are trying to parse russian website");
            }
            catch(DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return urls;
        }
    }
}
