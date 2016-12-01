using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.ViewModels
{
    public class OverallRatingViewModel
    {
        public string Area { get; set; }
        public decimal Total { get; set; }
        public decimal Appeals { get; set; }
        public decimal Complaints { get; set; }
    }
}
