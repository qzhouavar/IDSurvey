using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.ViewModels
{
    public class CompositeScoreByAreaViewModel2
    {
        public string Area { get; set; }
        public decimal Overall { get; set; }
        public int Overall_N { get; set; }
        public decimal OverallBeneficiary { get; set; }
        public int OverallBeneficiary_N { get; set; }
        public decimal OverallRepresentative { get; set; }
        public int OverallRepresentative_N { get; set; }

        public decimal Communication { get; set; }
        public int Communication_N { get; set; }
        public decimal CommunicationBeneficiary { get; set; }
        public int CommunicationBeneficiary_N { get; set; }
        public decimal CommunicationRepresentative { get; set; }
        public int CommunicationRepresentative_N { get; set; }



        public decimal CourtesyAndRespect { get; set; }
        public int CourtesyAndRespect_N { get; set; }
        public decimal CourtesyBeneficiary { get; set; }
        public int CourtesyBeneficiary_N { get; set; }
        public decimal CourtesyRepresentative { get; set; }
        public int CourtesyRepresentative_N { get; set; }


        public decimal AccessAndResponsiveness { get; set; }
        public int AccessAndResponsiveness_N { get; set; }
        public decimal ResponsivenessBeneficiary { get; set; }
        public int ResponsivenessBeneficiary_N { get; set; }
        public decimal ResponsivenessRepresentative { get; set; }
        public int ResponsivenessRepresentative_N { get; set; }
    }
}
