using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.ViewModels
{
    public class CompositeScoreByAreaViewModel
    {
        public string Area { get; set; }
        public decimal Overall { get; set; }
        public decimal Communication { get; set; }
        public decimal CourtesyAndRespet { get; set; }
        public decimal AccessAndResponsiveness { get; set; }
    }
}
