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

                using WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;

                foreach (var site in websitesForSearching)
                {
                    string sitemapString = wc.DownloadString(site);
                    XDocument urldoc = XDocument.Parse(sitemapString);
                    if (urldoc == null)
                        throw new Exception("One of links is invalid");

                    Console.WriteLine(urldoc.ToString());

                    var elements = urldoc.Descendants("loc").Select(x => x.Value).ToList();
                    
                    //var elements = from url in urldoc.Descendants("url")
                               //select url.Element("loc").Value.ToList();

                    urls.AddRange(elements);
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
            //urldoc.LoadXml(sitemapString);
            //XmlNodeList xmlSitemapList = urldoc.GetElementsByTagName("");
        }
    }
}
