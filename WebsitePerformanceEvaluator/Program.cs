using System;
using System.ComponentModel.DataAnnotations.Schema;
using WebsitePerformanceEvaluator.Crawlers;
using WebsitePerformanceEvaluator.WorkWithNet;

namespace WebsitePerformanceEvaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write your website urls for crawling \n");

            var urlsForCrawling = Console.ReadLine();

            //Create WebsiteCrawler to crawl without sitemap.xml
            var websiteCrawler = new WebsiteCrawler();
            var crawled =  websiteCrawler.Crawl(urlsForCrawling);

            //Create SitemapCrawler to crawl using sitmap.xml
            var siteMapCrawle = new SitemapCrawler();
            var crawledBySitemap = siteMapCrawle.CrawlBySitemap(urlsForCrawling);

            //Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site
            var foundOnlySitemap = crawledBySitemap.Where(x => !crawled.Contains(x)).ToList();


            Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:\r\n");

            var index = 1;
            foreach(var el in foundOnlySitemap)
            {
                Console.WriteLine($"{index}) {el}");
                index++;
            }
                


            //Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml
            var foundOnlyWebsiteCrawler = crawled.Where(x => !crawledBySitemap.Contains(x)).ToList();

            Console.WriteLine("\r\nUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:\r\n");
            index = 1;
            foreach (var el in foundOnlyWebsiteCrawler)
            {
                Console.WriteLine($"{index}) {el}");
                index++;
            }


            //Measuring responce time for each url and output it
            Console.WriteLine("\r\nTimig:");
            var fullUrlList = crawled.Union(crawledBySitemap).ToList();
            var urlsWithTimings = UrlResponseCounter.GetResponceTime(fullUrlList);

            index = 1;
            foreach(var el in urlsWithTimings)
            {
                Console.WriteLine($"{index}) {el.Url}; {el.ResponceTime}ms");
                index++;
            }

            Console.WriteLine($"\r\nUrls(html documents) found after crawling a website: {crawled.Count}\r\n");

            Console.WriteLine($"Urls found in sitemap: {crawledBySitemap.Count}");

        }
    }
}
