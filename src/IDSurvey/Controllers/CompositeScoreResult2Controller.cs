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
    public class CompositeScoreResult2Controller : Controller
    {
        internal static readonly IEnumerable<string> SurveyTypeList = new[] { "All", "Appeals", "Complaints" };
        internal static readonly List<string> ServiceAreaList = new List<string>() { "National","1", "2", "3", "4", "5"};
        internal static readonly string[] AllSurveyTypes = new[] { "APPEALS","COMPLAINTS"};
        internal static readonly List<string> ChartCategoryList= new List<string>() { "CommunicationComp", "CourtesyComp", "ResponsivenessComp","q7","q8","q9", "q6", "q10","q11", "q12" };


        private readonly ApplicationDbContext _context;

        public CompositeScoreResult2Controller(ApplicationDbContext context)
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
            var result = new Dictionary<string, List<CompositeScoreByAreaViewModel2>>();
            var allResult = new List<CompositeScoreByAreaViewModel2>();
            var appealResult = new List<CompositeScoreByAreaViewModel2>();
            var complaintResult = new List<CompositeScoreByAreaViewModel2>();
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
            var result = new Dictionary<string, List<CompositeScoreFigureViewModel2>>();
          
            var appealResult = new List<CompositeScoreFigureViewModel2>();
            var complaintResult = new List<CompositeScoreFigureViewModel2>();
            foreach (var category in ChartCategoryList)
            {
                appealResult.Add(AverageCompositeScoreFigure2(category, waveList, new[] { "APPEALS" }));
                complaintResult.Add(AverageCompositeScoreFigure2(category, waveList, new[] { "COMPLAINTS" }));
            }
            result.Add("APPEALS", appealResult);
            result.Add("COMPLAINTS", complaintResult);
            return Json(result);
        }

        [HttpGet("[action]/{wave}", Name = "GetOverallRatingByArea2")]
        public IActionResult GetOverallRatingByArea2(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();

            var result = new List<OverallRatingViewModel>();
            foreach (var area in ServiceAreaList)
            {
                result.Add(AverageOverallRating2(area, waveList));
            }
            return Json(result);
        }

        private CompositeScoreFigureViewModel2 AverageCompositeScoreFigure2(string chartCategory, int[] waveList, string[] surveyTypes)
        {
            decimal area1 = 0.0M;
            decimal area2 = 0.0M;
            decimal area3 = 0.0M;
            decimal area4 = 0.0M;
            decimal area5 = 0.0M;

            //var re = _context.MailSurveyResult.Select(m => m.q10).ToList();


            if (chartCategory.Equals("CommunicationComp"))
            {
                area1 = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                area2 = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                area3 = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                area4 = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                area5 = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
            }
            else if (chartCategory.Equals("ResponsivenessComp"))
            {
                area1 = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                area2 = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                area3 = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                area4 = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                area5 = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();

            }
            else if (chartCategory.Equals("CourtesyComp"))
            {

                area1 = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                area2 = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                area3 = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                area4 = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                area5 = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();

            }
            else if (chartCategory.Equals("q7"))
            {
                var countSum =_context.MailSurveyResult.Where(m => m.q7.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 =countSum==0?0: System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q7.HasValue && m.q7.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100.00 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q7.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q7.HasValue && m.q7.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q7.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q7.HasValue && m.q7.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q7.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q7.HasValue && m.q7.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q7.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q7.HasValue && m.q7.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q8"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q8.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q8.HasValue && m.q8.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q8.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q8.HasValue && m.q8.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 /countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q8.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q8.HasValue && m.q8.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 /countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q8.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q8.HasValue && m.q8.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 /countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q8.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q8.HasValue && m.q8.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q9"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q9.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q9.HasValue && m.q9.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q9.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q9.HasValue && m.q9.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 /countSum );
                countSum = _context.MailSurveyResult.Where(m => m.q9.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q9.HasValue && m.q9.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q9.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q9.HasValue && m.q9.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q9.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q9.HasValue && m.q9.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q6"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q6.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q6.HasValue && m.q6.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q6.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q6.HasValue && m.q6.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 /countSum );
                countSum = _context.MailSurveyResult.Where(m => m.q6.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q6.HasValue && m.q6.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 /countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q6.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q6.HasValue && m.q6.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q6.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q6.HasValue && m.q6.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q10"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q10.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q10.HasValue && m.q10.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q10.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q10.HasValue && m.q10.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q10.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q10.HasValue && m.q10.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q10.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q10.HasValue && m.q10.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q10.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode ==5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q10.HasValue && m.q10.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q11"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q11.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q11.HasValue && m.q11.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q11.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q11.HasValue && m.q11.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q11.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q11.HasValue && m.q11.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q11.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q11.HasValue && m.q11.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q11.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q11.HasValue && m.q11.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }
            else if (chartCategory.Equals("q12"))
            {
                var countSum = _context.MailSurveyResult.Where(m => m.q12.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count();
                area1 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q12.HasValue && m.q12.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 1).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q12.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count();
                area2 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q12.HasValue && m.q12.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 2).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q12.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count();
                area3 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q12.HasValue && m.q12.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 3).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q12.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count();
                area4 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q12.HasValue && m.q12.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 4).Count() * 100 / countSum);
                countSum = _context.MailSurveyResult.Where(m => m.q12.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count();
                area5 = countSum == 0 ? 0 : System.Convert.ToDecimal(_context.MailSurveyResult.Where(m => m.q12.HasValue && m.q12.Value < 3 && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == 5).Count() * 100 / countSum);
            }

                var result = new CompositeScoreFigureViewModel2
            {
                ChartCategory = chartCategory,
                Area1 = Math.Round(area1, 1, MidpointRounding.AwayFromZero),
                Area2 = Math.Round(area2, 1, MidpointRounding.AwayFromZero),
                Area3 = Math.Round(area3, 1, MidpointRounding.AwayFromZero),
                Area4 = Math.Round(area4, 1, MidpointRounding.AwayFromZero),
                Area5 = Math.Round(area5, 1, MidpointRounding.AwayFromZero)
            };

            return result;
        }

        private CompositeScoreByAreaViewModel2 AverageOneAreaCompositeScore2(string area, int[] waveList, string[] surveyTypes)
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
               
                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communication_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();

                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsiveness_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();

                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesy_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();

                q19Total= _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Total_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Count();

            }
            else
            {
                overall = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode==int.Parse(area)).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                overall_N = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();

                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                communication_N = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();

                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                responsiveness_N = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();

                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
                courtesy_N = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();

                q19Total = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                q19Total_N = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Count();

            }



            var result = new CompositeScoreByAreaViewModel2
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

        private OverallRatingViewModel AverageOverallRating2(string area, int[] waveList)
        {
            decimal total = 0.0m;
            decimal appeals = 0.0m;
            decimal complaints = 0.0m;

            if (area.Equals("National"))
            {
                total = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                appeals = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType=="APPEALS").Select(m => m.q19.Value).DefaultIfEmpty().Average();
                complaints = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && m.SurveyType == "COMPLAINTS").Select(m => m.q19.Value).DefaultIfEmpty().Average();
            }
            else
            {
                total = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && m.RegionCode == int.Parse(area)).Select(m => m.q19.Value).DefaultIfEmpty().Average();
                appeals = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && m.RegionCode == int.Parse(area) && m.SurveyType == "APPEALS").Select(m => m.q19.Value).DefaultIfEmpty().Average();
                complaints = _context.MailSurveyResult.Where(m => m.q19.HasValue && waveList.Contains(m.SurveyRound) && m.RegionCode == int.Parse(area) && m.SurveyType == "COMPLAINTS").Select(m => m.q19.Value).DefaultIfEmpty().Average();
            }

            var result = new OverallRatingViewModel
            {
                Area = area,
                Total = Math.Round(total, 1, MidpointRounding.AwayFromZero),
                Appeals = Math.Round(appeals, 1, MidpointRounding.AwayFromZero),
                Complaints = Math.Round(complaints, 1, MidpointRounding.AwayFromZero),
            };

            return result;

        }
    }
}