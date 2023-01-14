using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsitePerformanceEvaluator
{
    public class UrlModel
    {
        public string? Url { get; set; }
        public TimeSpan ResponceTime { get; set; }
    }
}
