using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsitePerformanceEvaluator.Abstractions
{
    public interface IWebstiteCrawler
    {
        public List<string> Crawl();
    }
}
