using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.Models
{
    public class RateViewModel
    {
        public List<CompleteRate> totalRates;

        public List<CompleteRate> quarterRates;
        public List<CompleteRate> appealsRates;
        public List<CompleteRate> complaintsRates;
        public List<CompleteRate> typeRates;
        public List<CompleteRate> appealsQuarter;
        public List<CompleteRate> complaintsQuarter;
        public SelectList quarters;
        public String quarter { get; set; }
    }
}
