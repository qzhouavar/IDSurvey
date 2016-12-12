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

        private readonly ApplicationDbContext _context;

        public CompositeScoreResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
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