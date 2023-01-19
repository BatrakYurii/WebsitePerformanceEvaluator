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
        public List<string> CrawlBySitemap(string url)
        {
            List<string> urls = new List<string>();
            try
            {
                var urlXmlPage = url + "/sitemap.xml";

                
                //Create webclient to get data
                using WebClient wc = new WebClient();                    
                wc.Encoding = Encoding.UTF8;
                    
                //Download sitemap as a string
                string sitemapString = wc.DownloadString(urlXmlPage);

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
