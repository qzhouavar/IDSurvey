using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.ViewModels
{
    public class BeneAnalysisFigureViewModel
    {
        public string ChartCategory { get; set; }
        public decimal Beneficiary { get; set; }
        public decimal BeneRepresentative { get; set; }
        public int Number  { get; set; }
    }
}
