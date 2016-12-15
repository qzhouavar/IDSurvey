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
    public class CompositeScoreResultController : Controller
    {
        internal static readonly IEnumerable<string> SurveyTypeList = new[] { "All", "Appeals", "Complaints" };
        internal static readonly List<string> ServiceAreaList = new List<string>() { "1", "2", "3", "4", "5", "National" };
        internal static readonly string[] AllSurveyTypes = new[] { "APPEALS","COMPLAINTS"};
        internal static readonly List<string> ChartCategoryList= new List<string>() { "CommunicationComp", "CourtesyComp", "ResponsivenessComp","q7","q8","q9", "q6", "q10","q11", "q12" };


        private readonly ApplicationDbContext _context;

        public CompositeScoreResultController(ApplicationDbContext context)
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
        [HttpGet("[action]/{wave}", Name = "GetCompositeScoreByArea")]
        public IActionResult GetCompositeScoreByArea(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<CompositeScoreByAreaViewModel>>();
            var allResult = new List<CompositeScoreByAreaViewModel>();
            var appealResult = new List<CompositeScoreByAreaViewModel>();
            var complaintResult = new List<CompositeScoreByAreaViewModel>();
            foreach(var area in ServiceAreaList)
            {
                allResult.Add(AverageOneAreaCompositeScore(area, waveList, AllSurveyTypes));
                appealResult.Add(AverageOneAreaCompositeScore(area, waveList, new[] { "APPEALS" }));
                complaintResult.Add(AverageOneAreaCompositeScore(area, waveList, new[] { "COMPLAINTS" }));
            }
            result.Add("ALL", allResult);
            result.Add("APPEALS", appealResult);
            result.Add("COMPLAINTS", complaintResult);
            return Json(result);
        }


        // return Table VI-1, VI-2, VI-3
        [HttpGet("[action]/{wave}", Name = "GetCompositeScoreFigure")]
        public IActionResult GetCompositeScoreFigure(string wave)
        {
            if (string.IsNullOrEmpty(wave))
            {
                return new BadRequestResult();
            }
            // convert string into list of int
            var waveList = wave.Split(',').Distinct().Select(int.Parse).ToArray();
            var result = new Dictionary<string, List<CompositeScoreFigureViewModel>>();
          
            var appealResult = new List<CompositeScoreFigureViewModel>();
            var complaintResult = new List<CompositeScoreFigureViewModel>();
            foreach (var category in ChartCategoryList)
            {
                appealResult.Add(AverageCompositeScoreFigure(category, waveList, new[] { "APPEALS" }));
                complaintResult.Add(AverageCompositeScoreFigure(category, waveList, new[] { "COMPLAINTS" }));
            }
            result.Add("APPEALS", appealResult);
            result.Add("COMPLAINTS", complaintResult);
            return Json(result);
        }

        [HttpGet("[action]/{wave}", Name = "GetOverallRatingByArea")]
        public IActionResult GetOverallRatingByArea(string wave)
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
                result.Add(AverageOverallRating(area, waveList));
            }
            return Json(result);
        }

        private CompositeScoreFigureViewModel AverageCompositeScoreFigure(string chartCategory, int[] waveList, string[] surveyTypes)
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

                var result = new CompositeScoreFigureViewModel
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

        private CompositeScoreByAreaViewModel AverageOneAreaCompositeScore(string area, int[] waveList, string[] surveyTypes)
        {
            decimal overall = 0.0M;
            decimal communication = 0.0M;
            decimal responsiveness = 0.0M;
            decimal courtesy = 0.0M;

            if (area.Equals("National"))
            {
                overall = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
            }
            else
            {
                overall = _context.MailSurveyResult.Where(m => m.OverallComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode==int.Parse(area)).Select(m => m.OverallComp.Value).DefaultIfEmpty().Average();
                communication = _context.MailSurveyResult.Where(m => m.CommunicationComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CommunicationComp.Value).DefaultIfEmpty().Average();
                responsiveness = _context.MailSurveyResult.Where(m => m.ResponsivenessComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.ResponsivenessComp.Value).DefaultIfEmpty().Average();
                courtesy = _context.MailSurveyResult.Where(m => m.CourtesyComp.HasValue && waveList.Contains(m.SurveyRound) && surveyTypes.Contains(m.SurveyType) && m.RegionCode == int.Parse(area)).Select(m => m.CourtesyComp.Value).DefaultIfEmpty().Average();
            }



            var result =  new CompositeScoreByAreaViewModel
                          {
                              Area = area,
                              Communication = Math.Round(communication, 1, MidpointRounding.AwayFromZero),
                              Overall = Math.Round(overall, 1, MidpointRounding.AwayFromZero),
                              AccessAndResponsiveness = Math.Round(responsiveness, 1, MidpointRounding.AwayFromZero),
                              CourtesyAndRespet = Math.Round(courtesy, 1, MidpointRounding.AwayFromZero)
            };

            return result;
        }

        private OverallRatingViewModel AverageOverallRating(string area, int[] waveList)
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