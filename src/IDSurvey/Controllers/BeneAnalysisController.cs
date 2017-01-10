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
    public class BeneAnalysisController : Controller
    {
        internal static readonly IEnumerable<string> SurveyTypeList = new[] { "All", "Appeals", "Complaints" };
        internal static readonly List<string> ServiceAreaList = new List<string>() { "National","1", "2", "3", "4", "5"};
        internal static readonly string[] AllSurveyTypes = new[] { "APPEALS","COMPLAINTS"};
        internal static readonly List<string> ChartCategoryList= new List<string>() { "OverallAppeals", "CommunicationAppeals", "CourtesyAppeals", "ResponsivenessAppeals", "OverallComplaints", "CommunicationComplaints", "CourtesyComplaints", "ResponsivenessComplaints"};
     

        private readonly ApplicationDbContext _context;

        public BeneAnalysisController(ApplicationDbContext context)
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
        [HttpGet("[action]/{wave}", Name = "GetCompositeScoreByArea2")]
        public IActionResult GetCompositeScoreByArea2(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<BeneAnalysisByAreaViewModel>>();
            var allResult = new List<BeneAnalysisByAreaViewModel>();
            var appealResult = new List<BeneAnalysisByAreaViewModel>();
            var complaintResult = new List<BeneAnalysisByAreaViewModel>();
            foreach(var area in ServiceAreaList)
            {
                allResult.Add(AverageOneAreaCompositeScore2(area, waveList, AllSurveyTypes));
                appealResult.Add(AverageOneAreaCompositeScore2(area, waveList, new[] { "APPEALS" }));
                complaintResult.Add(AverageOneAreaCompositeScore2(area, waveList, new[] { "COMPLAINTS" }));
            }
            result.Add("ALL", allResult);
            result.Add("APPEALS", appealResult);
            result.Add("COMPLAINTS", complaintResult);
            return Json(result);
        }


        // return Table VI-1, VI-2, VI-3
        [HttpGet("[action]/{wave}", Name = "GetCompositeScoreFigure2")]
        public IActionResult GetCompositeScoreFigure2(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<BeneAnalysisFigureViewModel>>();

            var allResult = new List<BeneAnalysisFigureViewModel>();
          
            foreach (var category in ChartCategoryList)
            {
                allResult.Add(AverageCompositeScoreFigure2(category, waveList));
            }
            result.Add("ALL", allResult);
            return Json(result);
        }

       

        private BeneAnalysisFigureViewModel AverageCompositeScoreFigure2(string chartCategory, int[] waveList)
        {
            decimal beneficiary = 0.0M;
            decimal beneRepresentative = 0.0M;
            int number = 0;
         
            if (chartCategory== "OverallAppeals")
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("APPEALS") && m.ContactType.Equals("Beneficiary")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("APPEALS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("APPEALS")).Count();

            }
            else if (chartCategory.Equals("CommunicationAppeals"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("CourtesyAppeals"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("ResponsivenessAppeals"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("OverallComplaints"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("CommunicationComplaints"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("CourtesyComplaints"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            else if (chartCategory.Equals("ResponsivenessComplaints"))
            {
                beneficiary = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                beneRepresentative = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS") && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                number = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType.Equals("COMPLAINTS")).Count();
            }
            var result = new BeneAnalysisFigureViewModel
            {
                ChartCategory = chartCategory,
                Beneficiary = Math.Round(beneficiary, 1, MidpointRounding.AwayFromZero),
                BeneRepresentative = Math.Round(beneRepresentative, 1, MidpointRounding.AwayFromZero),
                Number= number
                };
            return result;
        }

        private BeneAnalysisByAreaViewModel AverageOneAreaCompositeScore2(string area, int[] waveList, string[] surveyTypes)
        {
            decimal overall = 0.0M;
            int overall_N = 0;
            decimal overallBene= 0.0M;
            int overallBene_N = 0;
            decimal overallBR = 0.0M;
            int overallBR_N = 0;

            decimal communication = 0.0M;
            int communication_N = 0;
            decimal communicationBene = 0.0M;
            int communicationBene_N = 0;
            decimal communicationBR = 0.0M;
            int communicationBR_N = 0;

            decimal responsiveness = 0.0M;
            int responsiveness_N = 0;
            decimal responsivenessBene = 0.0M;
            int responsivenessBene_N = 0;
            decimal responsivenessBR = 0.0M;
            int responsivenessBR_N = 0;

            decimal courtesy = 0.0M;
            int courtesy_N = 0;
            decimal courtesyBene = 0.0M;
            int courtesyBene_N = 0;
            decimal courtesyBR = 0.0M;
            int courtesyBR_N = 0;

            decimal q19Total = 0.0M;
            int q19Total_N = 0;
            decimal q19Bene = 0.0M;
            int q19Bene_N = 0;
            decimal q19BR = 0.0M;
            int q19BR_N = 0;

            if (area.Equals("National"))
            {
                overall = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overall_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();
                overallBene = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)&&m.ContactType.Equals("Beneficiary")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overallBene_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Count();
                overallBR = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overallBR_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Count();


                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communication_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();
                communicationBene = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communicationBene_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Count();
                communicationBR = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communicationBR_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Count();

                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsiveness_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();
                responsivenessBene = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsivenessBene_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Count();
                responsivenessBR = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsivenessBR_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Count();

                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesy_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();
                courtesyBene = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesyBene_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Count();
                courtesyBR = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesyBR_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Count();

                q19Total = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Total_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();
                q19Bene = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Bene_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary")).Count();
                q19BR = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19BR_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.ContactType.Equals("Beneficiary Representative")).Count();

            }
            else
            {
                overall = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode==int.Parse(area)).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overall_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();
                overallBene = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overallBene_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Count();
                overallBR = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overallBR_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Count();

                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communication_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();
                communicationBene = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communicationBene_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Count();
                communicationBR = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communicationBR_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Count();

                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsiveness_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();
                responsivenessBene = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsivenessBene_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Count();
                responsivenessBR = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsivenessBR_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Count();

                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesy_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();
                courtesyBene = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesyBene_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Count();
                courtesyBR = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesyBR_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Count();

                q19Total = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Total_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();
                q19Bene = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Bene_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary")).Count();
                q19BR = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19BR_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area) && m.ContactType.Equals("Beneficiary Representative")).Count();
            }



            var result = new BeneAnalysisByAreaViewModel
            {
                Area = area,
                Overall = Math.Round(overall, 1, MidpointRounding.AwayFromZero),
                Overall_N = overall_N,
                OverallBeneficiary = Math.Round(overallBene, 1, MidpointRounding.AwayFromZero),
                OverallBeneficiary_N = overallBene_N,
                OverallRepresentative = Math.Round(overallBR, 1, MidpointRounding.AwayFromZero),
                OverallRepresentative_N = overallBR_N,

                Communication = Math.Round(communication, 1, MidpointRounding.AwayFromZero),
                Communication_N = communication_N,
                CommunicationBeneficiary= Math.Round(communicationBene, 1, MidpointRounding.AwayFromZero),
                CommunicationBeneficiary_N = communicationBene_N,
                CommunicationRepresentative= Math.Round(communicationBR, 1, MidpointRounding.AwayFromZero),
                CommunicationRepresentative_N = communicationBR_N,

                AccessAndResponsiveness = Math.Round(responsiveness, 1, MidpointRounding.AwayFromZero),
                AccessAndResponsiveness_N = responsiveness_N,
                ResponsivenessBeneficiary= Math.Round(responsivenessBene, 1, MidpointRounding.AwayFromZero),
                ResponsivenessBeneficiary_N = responsivenessBene_N,
                ResponsivenessRepresentative= Math.Round(responsivenessBR, 1, MidpointRounding.AwayFromZero),
                ResponsivenessRepresentative_N = responsivenessBR_N,

                CourtesyAndRespect = Math.Round(courtesy, 1, MidpointRounding.AwayFromZero),
                CourtesyAndRespect_N = courtesy_N,
                CourtesyBeneficiary= Math.Round(courtesyBene, 1, MidpointRounding.AwayFromZero),
                CourtesyBeneficiary_N = courtesyBene_N,
                CourtesyRepresentative= Math.Round(courtesyBR, 1, MidpointRounding.AwayFromZero),
                CourtesyRepresentative_N = courtesyBR_N,

                Q19Total= Math.Round(q19Total, 1, MidpointRounding.AwayFromZero),
                Q19Total_N=q19Total_N,
                Q19Beneficiary= Math.Round(q19Bene, 1, MidpointRounding.AwayFromZero),
                Q19Beneficiary_N = q19Bene_N,
                Q19Representative= Math.Round(q19BR, 1, MidpointRounding.AwayFromZero),
                Q19Representative_N = q19BR_N
            };

            return result;
        }

    }
}