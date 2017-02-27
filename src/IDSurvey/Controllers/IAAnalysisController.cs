using IDSurvey.Data;
using IDSurvey.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IDSurvey.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin,Manager,Member")]
    public class IAAnalysisController : Controller
    {
        internal static readonly IEnumerable<string> SurveyTypeList = new[] { "All", "Appeals", "Complaints" };
        internal static readonly List<string> ServiceAreaList = new List<string>() { "National", "1", "2", "3", "4", "5" };
        internal static readonly string[] AllSurveyTypes = new[] { "APPEALS", "COMPLAINTS" };
        //internal static readonly List<string> ChartCategoryList= new List<string>() { "OverallAppeals", "CommunicationAppeals", "CourtesyAppeals", "ResponsivenessAppeals", "OverallComplaints", "CommunicationComplaints", "CourtesyComplaints", "ResponsivenessComplaints"};

        private readonly ApplicationDbContext _context;

        public IAAnalysisController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // return Table VI-1, VI-2, VI-3
        [HttpGet("[action]/{wave}", Name = "GetIAAnalysisByArea")]
        public IActionResult GetIAAnalysisByArea(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<IAAnalysisByAreaViewModel>>();

            var complaintResult = new List<IAAnalysisByAreaViewModel>();
            var complaintBeneficiaryResult = new List<IAAnalysisByAreaViewModel>();
            var complaintRepresentativeResult = new List<IAAnalysisByAreaViewModel>();
            foreach (var area in ServiceAreaList)
            {
                complaintResult.Add(AverageOneAreaIAAnalysis(area, waveList, new[] { "COMPLAINTS" }, new[] { "Beneficiary", "Beneficiary Representative" }));
                complaintBeneficiaryResult.Add(AverageOneAreaIAAnalysis(area, waveList, new[] { "COMPLAINTS" }, new[] { "Beneficiary" }));
                complaintRepresentativeResult.Add(AverageOneAreaIAAnalysis(area, waveList, new[] { "COMPLAINTS" }, new[] { "Beneficiary Representative" }));
            }
            result.Add("COMPLAINTS", complaintResult);
            result.Add("COMPLAINTSBeneficiary", complaintBeneficiaryResult);
            result.Add("COMPLAINTSRepresentative", complaintRepresentativeResult);

            return Json(result);
        }

        private IAAnalysisByAreaViewModel AverageOneAreaIAAnalysis(string area, int[] waveList, string[] surveyTypes, string[] beneTypes)
        {
            if (area.Equals("National"))
            {
                var overall = (from m in _context.MailSurveyResult
                               where m.OverallComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)
                               select new Tuple<decimal, string>(m.OverallComp.Value, m.CaseType)
                            ).ToList();

                var commTemp = (from m in _context.MailSurveyResult
                                where m.CommunicationComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)
                                select new Tuple<decimal, string>(m.CommunicationComp.Value, m.CaseType)
                                ).ToList();

                var resTemp = (from m in _context.MailSurveyResult
                               where m.ResponsivenessComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)
                               select new Tuple<decimal, string>(m.ResponsivenessComp.Value, m.CaseType)
                               ).ToList();

                var courtTemp = (from m in _context.MailSurveyResult
                                 where m.CourtesyComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)
                                 select new Tuple<decimal, string>(m.CourtesyComp.Value, m.CaseType)
                                 ).ToList();

                var q19Temp = (from m in _context.MailSurveyResult
                               where m.q19.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)
                               select new Tuple<decimal, string>(m.q19.Value, m.CaseType)
                               ).ToList();

                return CalcuateIARecord(area, overall, commTemp, resTemp, courtTemp, q19Temp);
            }
            else
            {
                var overall = (from m in _context.MailSurveyResult
                               where m.OverallComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)
                               select new Tuple<decimal, string>(m.OverallComp.Value, m.CaseType)
                                ).ToList();

                var commTemp = (from m in _context.MailSurveyResult
                                where m.CommunicationComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)
                                select new Tuple<decimal, string>(m.CommunicationComp.Value, m.CaseType)
                                ).ToList();

                var resTemp = (from m in _context.MailSurveyResult
                               where m.ResponsivenessComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)
                               select new Tuple<decimal, string>(m.ResponsivenessComp.Value, m.CaseType)
                               ).ToList();

                var courtTemp = (from m in _context.MailSurveyResult
                                 where m.CourtesyComp.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)
                                 select new Tuple<decimal, string>(m.CourtesyComp.Value, m.CaseType)
                                 ).ToList();

                var q19Temp = (from m in _context.MailSurveyResult
                               where m.q19.HasValue && beneTypes.Contains(m.ContactType) && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)
                               select new Tuple<decimal, string>(m.q19.Value, m.CaseType)
                               ).ToList();

                return CalcuateIARecord(area, overall, commTemp, resTemp, courtTemp, q19Temp);
            }
        }

        private IAAnalysisByAreaViewModel CalcuateIARecord(string area, List<Tuple<decimal, string>> overall, List<Tuple<decimal, string>> commTemp, List<Tuple<decimal, string>> resTemp, List<Tuple<decimal, string>> courtTemp, List<Tuple<decimal, string>> q19Temp)
        {
            return new IAAnalysisByAreaViewModel
            {
                Area = area,
                Overall = overall.Count() != 0 ? Math.Round(overall.Average(x => x.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                Overall_N = overall.Count(),
                OverallBeneficiary = overall.Count(m => m.Item2 == "Immediate Advocacy") !=0 ? Math.Round(overall.Where(m => m.Item2 == "Immediate Advocacy").Average(m => m.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                OverallBeneficiary_N = overall.Count(m => m.Item2 == "Immediate Advocacy"),
                OverallRepresentative = overall.Count(m => m.Item2 == "Medical Record Review") != 0 ? Math.Round(overall.Where(m => m.Item2 == "Medical Record Review").Average(m => m.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                OverallRepresentative_N = overall.Count(m => m.Item2 == "Medical Record Review"),

                Communication = commTemp.Count() != 0 ? Math.Round(commTemp.Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                Communication_N = commTemp.Count(),
                CommunicationBeneficiary = commTemp.Count(c => c.Item2 == "Immediate Advocacy") != 0 ? Math.Round(commTemp.Where(c => c.Item2 == "Immediate Advocacy").Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                CommunicationBeneficiary_N = commTemp.Count(c => c.Item2 == "Immediate Advocacy"),
                CommunicationRepresentative = commTemp.Count(c => c.Item2 == "Medical Record Review") != 0 ? Math.Round(commTemp.Where(c => c.Item2 == "Medical Record Review").Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                CommunicationRepresentative_N = commTemp.Count(c => c.Item2 == "Medical Record Review"),

                AccessAndResponsiveness = resTemp.Count() != 0 ? Math.Round(resTemp.Average(r => r.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                AccessAndResponsiveness_N = resTemp.Count(),
                ResponsivenessBeneficiary = resTemp.Count(r => r.Item2 == "Immediate Advocacy") != 0 ? Math.Round(resTemp.Where(r => r.Item2 == "Immediate Advocacy").Average(r => r.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                ResponsivenessBeneficiary_N = resTemp.Count(r => r.Item2 == "Immediate Advocacy"),
                ResponsivenessRepresentative = resTemp.Count(r => r.Item2 == "Medical Record Review") != 0 ? Math.Round(resTemp.Where(r => r.Item2 == "Medical Record Review").Average(r => r.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                ResponsivenessRepresentative_N = resTemp.Count(r => r.Item2 == "Medical Record Review"),

                CourtesyAndRespect = courtTemp.Count() != 0 ? Math.Round(courtTemp.Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                CourtesyAndRespect_N = courtTemp.Count(),
                CourtesyBeneficiary = courtTemp.Count(c => c.Item2 == "Immediate Advocacy") != 0 ? Math.Round(courtTemp.Where(c => c.Item2 == "Immediate Advocacy").Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                CourtesyBeneficiary_N = courtTemp.Count(c => c.Item2 == "Immediate Advocacy"),
                CourtesyRepresentative = courtTemp.Count(c => c.Item2 == "Medical Record Review") != 0 ? Math.Round(courtTemp.Where(c => c.Item2 == "Medical Record Review").Average(c => c.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                CourtesyRepresentative_N = courtTemp.Count(c => c.Item2 == "Medical Record Review"),

                Q19Total = q19Temp.Count() != 0 ? Math.Round(q19Temp.Average(q => q.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                Q19Total_N = q19Temp.Count(),
                Q19Beneficiary = q19Temp.Count(q => q.Item2 == "Immediate Advocacy") != 0 ? Math.Round(q19Temp.Where(q => q.Item2 == "Immediate Advocacy").Average(q => q.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                Q19Beneficiary_N = q19Temp.Count(q => q.Item2 == "Immediate Advocacy"),
                Q19Representative = q19Temp.Count(q => q.Item2 == "Medical Record Review") !=0 ? Math.Round(q19Temp.Where(q => q.Item2 == "Medical Record Review").Average(q => q.Item1), 1, MidpointRounding.AwayFromZero) : 0.0m,
                Q19Representative_N = q19Temp.Count(q => q.Item2 == "Medical Record Review")
            };
        }
    }
}