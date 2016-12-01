using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDSurvey.Data;
using IDSurvey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IDSurvey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
          
        }

        [HttpGet("[action]", Name ="GetChartDataTotalRates")]
        public IActionResult GetChartDataTotalRates() {
            var totalratesList = (from p in _context.CompleteRates
                                  group p by p.QTR into g
                                  select new CompleteRate
                                  {
                                      QTR = g.Key,
                                      TOTAL = g.Sum(p => p.TOTAL),
                                      COMPLETE = g.Sum(p => p.COMPLETE)
                                  }).ToList();
            var totalratesChartList = new List<ChartDataViewModel>();
            foreach (var t in totalratesList)
            {
                totalratesChartList.Add(new ChartDataViewModel
                {
                    name = t.QTR,
                    value = (t.COMPLETE * 100.0) / t.TOTAL
                });
            }
            return Json(totalratesChartList);
        }
     
        public async Task<IActionResult> Index( string quarter)
        {
            var completerates = from rate in _context.CompleteRates select rate;
            var overalltotal = completerates.Select(v => v.TOTAL).ToArray().Sum();
            ViewData["OverallTotal"] = overalltotal;
            var overalcomplete = completerates.Select(v => v.COMPLETE).ToArray().Sum();
            ViewData["OverallComplete"] = overalcomplete;
            ViewData["AveRate"] = 100.0 * overalcomplete / overalltotal;

            var appealsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "APPEALS") select rate;
            var appealstotal = appealsrates.Select(v => v.TOTAL).ToArray().Sum();
            var appealscomplete = appealsrates.Select(v => v.COMPLETE).ToArray().Sum();
            ViewData["AppealsRate"] = 100.0 * appealscomplete / appealstotal;

            var complaintsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "COMPLAINTS") select rate;
            var complaintstotal = complaintsrates.Select(v => v.TOTAL).ToArray().Sum();
            var complaintscomplete = complaintsrates.Select(v => v.COMPLETE).ToArray().Sum();
            ViewData["ComplaintsRate"] = 100.0 * complaintscomplete/ complaintstotal;


            //var totalratesList = (from p in _context.CompleteRates
            //                  group p by p.QTR into g
            //                  select new CompleteRate
            //                  {
            //                      QTR = g.Key,
            //                      TOTAL = g.Sum(p => p.TOTAL),
            //                      COMPLETE = g.Sum(p => p.COMPLETE)
            //                  }).ToList();
            //var totalratesChartList = new List<ChartDataViewModel>();
            //foreach( var t in totalratesList)
            //{
            //    totalratesChartList.Add(new ChartDataViewModel
            //    {
            //        name = t.QTR,
            //        value = (t.COMPLETE * 100.0 )/ t.TOTAL
            //    });
            //}

            appealsrates = from p in appealsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            complaintsrates = from p in complaintsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR
                                            select m.QTR;

 
            var quarterrates = completerates;
         
            if (!string.IsNullOrEmpty(quarter))
            {
                quarterrates = quarterrates.Where(x => x.QTR == quarter);
            }
            var typerates = from p in quarterrates group p by new {p.TYPE, p.SERVICE_AREA } into g select new CompleteRate { TYPE=g.Key.TYPE,SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            quarterrates = from p in quarterrates group p by p.SERVICE_AREA into g select new CompleteRate {SERVICE_AREA = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            

            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            //rateVM.totalRates = totalrates;
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.typeRates= await typerates.ToListAsync();
            rateVM.appealsRates = await appealsrates.ToListAsync();
            rateVM.complaintsRates = await complaintsrates.ToListAsync();

            ViewData["Title"] = "Complete Rates";

            return View(rateVM);
        }

       
        public async Task<IActionResult> Rate()
        {
            var completerates = from rate in _context.CompleteRates select rate;

            var overalltotal = completerates.Select(v => v.TOTAL).ToArray().Sum();

            ViewData["OverallTotal"] = overalltotal;
            var overalcomplete = completerates.Select(v => v.COMPLETE).ToArray().Sum();
            ViewData["OverallComplete"] = overalcomplete;

            ViewData["AveRate"] = 100.0 * overalcomplete / overalltotal;





            var rates = from p in _context.CompleteRates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            return View(await rates.ToListAsync());
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
