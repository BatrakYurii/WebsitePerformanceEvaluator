using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsitePerformanceEvaluator.Abstractions;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace WebsitePerformanceEvaluator.Crawlers
{
    public class WebsiteCrawler : IWebstiteCrawler
    {
        public List<string> Crawl(string url)
        {
            List<string> finalUrlsList = new List<string>();
            
            
            //Create HasSet to contain unique links. Created queue to crawl links and add founded.
            HashSet<string> checkedUrl = new HashSet<string>();
            Queue<string> queue = new Queue<string>();

            //Regular expression. It is filtering list to find links
            Regex linkRegex = new Regex(@"^(https?:\/\/|\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");

            //Add first url to queue
            queue.Enqueue(url);
            checkedUrl.Add(url);

            while (queue.Count > 0)
            {
                //Take one url to crawl this webpage
                var currentUrl = queue.Dequeue();

                try
                {
                    //Create HtmlWeb class to get html page
                    HtmlWeb htmlWeb = new HtmlWeb();
                    HtmlDocument htmldoc = htmlWeb.Load(currentUrl);

                    //Select all html links from document
                    var linkNodes = htmldoc.DocumentNode.SelectNodes("//a[@href]");
                    if (linkNodes == null)
                        continue;
                    var linksForCurrent = linkNodes.Select(x => x.Attributes["href"].Value.ToString()).ToList();

                    //Filter all urls to find a sutiblies
                    var fullLinks = linksForCurrent.Where(x => linkRegex.IsMatch(x) && !x.EndsWith(".jpg") && !x.EndsWith(".png")).ToList();
                    var firstUrlPart = url.Last() == '/' ? url.Substring(0, url.Length - 1) : url;

                    //We can have some partial links like: /souces/barbeque/. We need to concate this string with base url
                    fullLinks.AddRange(linksForCurrent.Where(x => x.StartsWith("/")).Select(x => $"{firstUrlPart}{x}"));

                    //Remove part after anchor if exist
                    fullLinks = fullLinks.Select(x => {
                        var index = x.IndexOf("#");
                        return index != -1 ? x.Substring(0, index) : x;
                    }).ToList();

                    foreach (var link in fullLinks)
                    {
                        //Add link if it is unique and not null and contain base url
                        if (!checkedUrl.Contains(link)  && !string.IsNullOrEmpty(link) && link.StartsWith(url))
                        {
                            checkedUrl.Add(link);
                            queue.Enqueue(link);
                        }
                    }

                    finalUrlsList.AddRange(checkedUrl);

                    
                }
                catch(ArgumentNullException e)
                {
                    Console.WriteLine($"{e.Message} ParamsName:{e.ParamName}");
                }     
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            //Remove all identical strings from the list
            finalUrlsList = finalUrlsList.Select(x => x.TrimEnd('/')).Distinct().Select(x => x + '/').ToList();

            return finalUrlsList;
        }
    }
}
