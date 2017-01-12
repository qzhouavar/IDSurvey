using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.ViewModels
{
    public class FormTypeByAreaViewModel
    {
        public string FormType { get; set; }
        public string Area { get; set; }
        public decimal Overall_Percent { get; set; }
        public int Overall_N { get; set; }
      
        public decimal Communication_Percent { get; set; }
        public int Communication_N { get; set; }
       
        public decimal CourtesyAndRespect_Percent { get; set; }
        public int CourtesyAndRespect_N { get; set; }
   

        public decimal AccessAndResponsiveness_Percent { get; set; }
        public int AccessAndResponsiveness_N { get; set; }
       
    }
}
