using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsitePerformanceEvaluator.Abstractions;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace WebsitePerformanceEvaluator.Crawlers
{
    public class SitemapCrawler : ISitemapCrawler
    {
        public List<string> websitesForSearching;

        public SitemapCrawler(List<string> websiteForSeaching)
        {
            this.websitesForSearching = websiteForSeaching;
        }

        public List<string> CrawlBySitemap()
        {
            List<string> urls = new List<string>();
            try
            {
                websitesForSearching = websitesForSearching.Select(x => $"{x}/sitemap.xml").ToList();

                foreach (var site in websitesForSearching)
                {
                    //Create webclient to get data
                    using WebClient wc = new WebClient();                    
                    wc.Encoding = Encoding.UTF8;
                    
                    //Download sitemap as a string
                    string sitemapString = wc.DownloadString(site);

                    //Create xml document and serialize sitemap string to xml
                    XmlDocument urldoc = new XmlDocument();
                    urldoc.LoadXml(sitemapString);
                    if (urldoc == null)
                        throw new Exception("One of links is invalid");

                    Console.WriteLine(urldoc.ToString());

                    //Get all loc elements that contains all website links
                    XmlNodeList elements = urldoc.GetElementsByTagName("loc");

                    foreach (XmlNode el in elements)
                        urls.Add(el.InnerText);
                }
                
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
