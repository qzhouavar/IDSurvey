using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDSurvey.Data;
using IDSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace IDSurvey.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin,Manager,Member")]
    public class FormTypeController : Controller
    {
        internal static readonly IEnumerable<string> SurveyTypeList = new[] { "All", "Appeals", "Complaints" };
        internal static readonly List<string> ServiceAreaList = new List<string>() { "Total","1", "5",  "2", "3", "4"};
        internal static readonly string[] AllSurveyTypes = new[] { "APPEALS","COMPLAINTS"};
        internal static readonly List<string> FormTypeList= new List<string>() { "Livanta", "Livanta_1", "Livanta_5", "Kepro", "Kepro_2", "Kepro_3", "Kepro_4" };
        private readonly ApplicationDbContext _context;

        public FormTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Figure()
        {
            return View();
        }

        // return Table VI-1, VI-2, VI-3
        [HttpGet("[action]/{wave}", Name = "GetFormTypeByArea")]
        public IActionResult GetFormTypeByArea(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<FormTypeByAreaViewModel>>();
            var allResult = new List<FormTypeByAreaViewModel>();
            var appealResult = new List<FormTypeByAreaViewModel>();
            var complaintResult = new List<FormTypeByAreaViewModel>();
            foreach (var formType in FormTypeList){
                foreach (var area in ServiceAreaList)
                {
                    if(formType.Equals("Livanta")&&area == "Total" || formType.Equals("Livanta_1") && area =="1"|| formType.Equals("Livanta_5") && area == "5"|| formType.Equals("Kepro") && area == "Total"|| formType.Equals("Kepro_2") && area == "2" || formType.Equals("Kepro_3")&&area == "3" || formType.Equals("Kepro_4")&&area == "4")
                    {
                        allResult.Add(AverageOneAreaFormType(formType, area, waveList, AllSurveyTypes));
                        appealResult.Add(AverageOneAreaFormType(formType, area, waveList, new[] { "APPEALS" }));
                        complaintResult.Add(AverageOneAreaFormType(formType, area, waveList, new[] { "COMPLAINTS" }));
                    }
                  
                }
            }
            result.Add("ALL", allResult);
            result.Add("APPEALS", appealResult);
            result.Add("COMPLAINTS", complaintResult);
            return Json(result);
        }


        private FormTypeByAreaViewModel AverageOneAreaFormType(string formType, string area,int[] waveList, string[] surveyTypes)
        {
            decimal overall_Percent = 0.0M;
            int overall_N = 0;

            decimal communication_Percent = 0.0M;
            int communication_N = 0;
        
            decimal responsiveness_Percent = 0.0M;
            int responsiveness_N = 0;
         
            decimal courtesy_Percent = 0.0M;
            int courtesy_N = 0;
          
            var context = from m in _context.MailSurveyResult.Where(m => waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)) select new {
                OverallComp=m.OverallComp,
                ContactType=m.ContactType,
                CommunicationComp=m.CommunicationComp,
                ResponsivenessComp=m.ResponsivenessComp,
                CourtesyComp=m.CourtesyComp,
                RegionCode=m.RegionCode
            };
            if (formType.Contains("Livanta"))
            {
                if (area.Equals("Total"))
                {

                    context = from m in context.Where(m => m.RegionCode == 1 || m.RegionCode == 5) select m;
                    var temp = from m in context.Where(m => m.OverallComp.HasValue) select m;
                    overall_Percent = temp.Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                    overall_N = temp.Count();

                    temp = from m in context.Where(m => m.CommunicationComp.HasValue) select m;
                    communication_Percent = temp.Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                    communication_N = temp.Count();


                    temp = from m in context.Where(m => m.ResponsivenessComp.HasValue) select m;
                    responsiveness_Percent = temp.Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                    responsiveness_N = temp.Count();

                    temp = from m in context.Where(m => m.CourtesyComp.HasValue) select m;
                    courtesy_Percent = temp.Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                    courtesy_N = temp.Count();
                }

                else if (area == "1" || area == "5")
                {
                    context = from m in context.Where(m => m.RegionCode == int.Parse(area)) select m;

                    var temp = from m in context.Where(m => m.OverallComp.HasValue) select m;
                    overall_Percent = temp.Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                    overall_N = temp.Count();

                    temp = from m in context.Where(m => m.CommunicationComp.HasValue) select m;
                    communication_Percent = temp.Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                    communication_N = temp.Count();

                    temp = from m in context.Where(m => m.ResponsivenessComp.HasValue) select m;
                    responsiveness_Percent = temp.Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                    responsiveness_N = temp.Count();


                    temp = from m in context.Where(m => m.CourtesyComp.HasValue) select m;
                    courtesy_Percent = temp.Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                    courtesy_N = temp.Count();
                }
            }
            if (formType.Contains("Kepro"))
            {
                if (area.Equals("Total"))
                {
                    context = from m in context.Where(m => m.RegionCode == 2 || m.RegionCode == 3 || m.RegionCode == 4) select m;
                    var temp = from m in context.Where(m => m.OverallComp.HasValue) select m;
                    overall_Percent = temp.Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                    overall_N = temp.Count();

                    temp = from m in context.Where(m => m.CommunicationComp.HasValue) select m;
                    communication_Percent = temp.Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                    communication_N = temp.Count();


                    temp = from m in context.Where(m => m.ResponsivenessComp.HasValue) select m;
                    responsiveness_Percent = temp.Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                    responsiveness_N = temp.Count();

                    temp = from m in context.Where(m => m.CourtesyComp.HasValue) select m;
                    courtesy_Percent = temp.Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                    courtesy_N = temp.Count();
                }
                else if (area == "2" || area == "3" || area == "4")
                {
                    context = from m in context.Where(m => m.RegionCode == int.Parse(area)) select m;

                    var temp = from m in context.Where(m => m.OverallComp.HasValue) select m;
                    overall_Percent = temp.Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                    overall_N = temp.Count();

                    temp = from m in context.Where(m => m.CommunicationComp.HasValue) select m;
                    communication_Percent = temp.Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                    communication_N = temp.Count();

                    temp = from m in context.Where(m => m.ResponsivenessComp.HasValue) select m;
                    responsiveness_Percent = temp.Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                    responsiveness_N = temp.Count();


                    temp = from m in context.Where(m => m.CourtesyComp.HasValue) select m;
                    courtesy_Percent = temp.Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                    courtesy_N = temp.Count();
                }
            }
            var result = new FormTypeByAreaViewModel
            {
                FormType=formType,
                Area = area,
                Overall_Percent = Math.Round(overall_Percent, 1, MidpointRounding.AwayFromZero),
                Overall_N = overall_N,


                Communication_Percent = Math.Round(communication_Percent, 1, MidpointRounding.AwayFromZero),
                Communication_N = communication_N,

                AccessAndResponsiveness_Percent = Math.Round(responsiveness_Percent, 1, MidpointRounding.AwayFromZero),
                AccessAndResponsiveness_N = responsiveness_N,


                CourtesyAndRespect_Percent = Math.Round(courtesy_Percent, 1, MidpointRounding.AwayFromZero),
                CourtesyAndRespect_N = courtesy_N,
            };

            return result;
        }

    }
}