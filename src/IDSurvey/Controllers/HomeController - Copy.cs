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
    public class HomeController1 : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController1(ApplicationDbContext context)
        {
            _context = context;
          
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


            var totalrates = from p in _context.CompleteRates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            appealsrates = from p in appealsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            complaintsrates = from p in complaintsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR
                                            select m.QTR;

 
            var quarterrates = completerates;
         
            if (!String.IsNullOrEmpty(quarter))
            {
                quarterrates = quarterrates.Where(x => x.QTR == quarter);
            }
            var typerates = from p in quarterrates group p by new {p.TYPE, p.SERVICE_AREA } into g select new CompleteRate { TYPE=g.Key.TYPE,SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            quarterrates = from p in quarterrates group p by p.SERVICE_AREA into g select new CompleteRate {SERVICE_AREA = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
           
            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            rateVM.totalRates = await totalrates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.typeRates= await typerates.ToListAsync();
            rateVM.appealsRates = await appealsrates.ToListAsync();
            rateVM.complaintsRates = await complaintsrates.ToListAsync();
            return View(rateVM);
        }

       
        public async Task<IActionResult> RateByQTR()
        {
            var completerates = from rate in _context.CompleteRates select rate;

            var appealsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "APPEALS") select rate;
            var complaintsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "COMPLAINTS") select rate;

            appealsrates = from p in appealsrates group p by new
            {
                SERVICE_AREA = p.SERVICE_AREA,
                QTR = p.QTR,
            }
            into g orderby g.Key.SERVICE_AREA select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA=g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var appealsQuarter = from p in appealsrates
                                 group p by  p.QTR
                                 into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            
           

            complaintsrates = from p in complaintsrates
                              group p by new
                              {
                                  SERVICE_AREA = p.SERVICE_AREA,
                                  QTR = p.QTR
                              }
            into g orderby g.Key.SERVICE_AREA select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            var complaintsQuarter = from p in complaintsrates
                                    group p by p.QTR
                                    into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };


            var totalrates = from p in completerates
                             group p by new
                             {
                                 SERVICE_AREA = p.SERVICE_AREA,
                                 QTR = p.QTR,
                             }
            into g
                             orderby g.Key.SERVICE_AREA
                             select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };


            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR
                                            select m.QTR;


            var quarterrates = completerates;

        
         

            quarterrates = from p in quarterrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var typerates = from p in completerates
                             group p by new
                             {
                                 SERVICE_AREA = p.SERVICE_AREA,
                                 QTR = p.QTR,
                                 TYPE=p.TYPE
                             }
            into g
                             orderby g.Key.SERVICE_AREA, g.Key.QTR
                             select new CompleteRate { TYPE=g.Key.TYPE,QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            rateVM.totalRates = await totalrates.ToListAsync();
            rateVM.appealsQuarter = await appealsQuarter.ToListAsync();
            rateVM.complaintsQuarter = await complaintsQuarter.ToListAsync();
            rateVM.appealsRates = await appealsrates.ToListAsync();
            rateVM.complaintsRates = await complaintsrates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.typeRates = await typerates.ToListAsync();
            return View(rateVM);
        }


        public async Task<IActionResult> RateByRange(string startQTR, string endQTR)
        {
            var completerates = from rate in _context.CompleteRates select rate;

            var appealsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "APPEALS") select rate;
            var complaintsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "COMPLAINTS") select rate;

            appealsrates = from p in appealsrates
                           group p by new
                           {
                               SERVICE_AREA = p.SERVICE_AREA,
                               QTR = p.QTR,
                           }
            into g
                           orderby g.Key.SERVICE_AREA
                           select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var appealsQuarter = from p in appealsrates
                                 group p by p.QTR
                                 into g
                                 select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };




            complaintsrates = from p in complaintsrates
                              group p by new
                              {
                                  SERVICE_AREA = p.SERVICE_AREA,
                                  QTR = p.QTR
                              }
            into g
                              orderby g.Key.SERVICE_AREA
                              select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            var complaintsQuarter = from p in complaintsrates
                                    group p by p.QTR
                                    into g
                                    select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };


            var totalrates = from p in completerates
                             group p by new
                             {
                                 SERVICE_AREA = p.SERVICE_AREA,
                                 QTR = p.QTR,
                             }
            into g
                             orderby g.Key.SERVICE_AREA
                             select new CompleteRate { QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };


            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR
                                            select m.QTR;


            var quarterrates = completerates;




            quarterrates = from p in quarterrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var typerates = from p in completerates
                            group p by new
                            {
                                SERVICE_AREA = p.SERVICE_AREA,
                                QTR = p.QTR,
                                TYPE = p.TYPE
                            }
            into g
                            orderby g.Key.SERVICE_AREA, g.Key.QTR
                            select new CompleteRate { TYPE = g.Key.TYPE, QTR = g.Key.QTR, SERVICE_AREA = g.Key.SERVICE_AREA, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            rateVM.totalRates = await totalrates.ToListAsync();
            rateVM.appealsQuarter = await appealsQuarter.ToListAsync();
            rateVM.complaintsQuarter = await complaintsQuarter.ToListAsync();
            rateVM.appealsRates = await appealsrates.ToListAsync();
            rateVM.complaintsRates = await complaintsrates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.typeRates = await typerates.ToListAsync();
            return View(rateVM);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
