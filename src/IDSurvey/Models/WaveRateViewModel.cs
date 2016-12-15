using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.Models
{
    public class WaveRateViewModel
    {
       
        public List<CompleteRate> quarterRates;

        public SelectList quarters;
        public String quarter { get; set; }
       

        [Required]
        public string QTR { get; set; }
        [Required]
        public int WAVE { get; set; }
        [Required]
        public string TYPE { get; set; }
        [Required]
        public string SERVICE_AREA { get; set; }

        [Required]
        public int TOTAL { get; set; }

        [Required]
        public int COMPLETE { get; set; }
    }
}
