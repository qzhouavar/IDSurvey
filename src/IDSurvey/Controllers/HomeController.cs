using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDSurvey.Data;
using IDSurvey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace IDSurvey.Controllers
{
    [Authorize(Roles = "Admin,Manager,Member")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
          
        }

       
        public async Task<IActionResult> Index( string quarter)
        {
            var completerates = from rate in _context.CompleteRates select rate;
            var overalltotal = completerates.Select(v => v.TOTAL).Sum();
            ViewData["OverallTotal"] = overalltotal;
            var overalcomplete = completerates.Select(v => v.COMPLETE).Sum();
            ViewData["OverallComplete"] = overalcomplete;
            ViewData["AveRate"] = 100.0 * overalcomplete / overalltotal;

            var appealsrates = from rate in completerates.Where(x => x.TYPE == "APPEALS") select rate;
            var appealstotal = appealsrates.Select(v => v.TOTAL).Sum();
            var appealscomplete = appealsrates.Select(v => v.COMPLETE).Sum();
            ViewData["AppealsRate"] = 100.0 * appealscomplete / appealstotal;

            var complaintsrates = from rate in completerates.Where(x => x.TYPE == "COMPLAINTS") select rate;
            var complaintstotal = complaintsrates.Select(v => v.TOTAL).Sum();
            var complaintscomplete = complaintsrates.Select(v => v.COMPLETE).Sum();
            ViewData["ComplaintsRate"] = 100.0 * complaintscomplete/ complaintstotal;


            var totalrates = from p in completerates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
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
            rateVM.totalRates = await totalrates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.typeRates= await typerates.ToListAsync();
            rateVM.appealsRates = await appealsrates.ToListAsync();
            rateVM.complaintsRates = await complaintsrates.ToListAsync();
            return View(rateVM);
        }

       
        public async Task<IActionResult> RateByQTR()
        {
            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR ascending
                                            select m.QTR;

            
            var appealsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "APPEALS") orderby rate.SERVICE_AREA select rate;
            var complaintsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "COMPLAINTS") orderby rate.SERVICE_AREA select rate;

            var typerates = from c in appealsrates
                            from p in complaintsrates
                            where c.QTR.Equals(p.QTR)&&c.SERVICE_AREA.Equals(p.SERVICE_AREA)
                      
                            select new CompleteRate { ID= c.TOTAL, WAVE = c.COMPLETE,QTR = c.QTR, SERVICE_AREA = c.SERVICE_AREA, TOTAL = p.TOTAL, COMPLETE = p.COMPLETE };


            var appealsquarterrates = from p in appealsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            var complaintsquarterrates = from p in complaintsrates group p by p.QTR into g select new CompleteRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var quarterrates = from c in appealsquarterrates
                               from p in complaintsquarterrates
                               where c.QTR.Equals(p.QTR)
                               select new CompleteRate { ID = c.TOTAL, WAVE = c.COMPLETE, QTR = c.QTR, TOTAL = p.TOTAL, COMPLETE = p.COMPLETE };

            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            rateVM.typeRates = await typerates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();

            return View(rateVM);
        }

        public async Task<IActionResult> RateByRange(string startQTR, string endQTR)
        {
            IQueryable<string> genreQuery = from m in _context.CompleteRates
                                            orderby m.QTR
                                            select m.QTR;

            var appealsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "APPEALS") select rate;
            var complaintsrates = from rate in _context.CompleteRates.Where(x => x.TYPE == "COMPLAINTS") select rate;

            if (!String.IsNullOrEmpty(startQTR)){
                appealsrates = appealsrates.Where(x => x.QTR.CompareTo(startQTR)>=0);
                complaintsrates = complaintsrates.Where(x => x.QTR.CompareTo(startQTR) >= 0);
            }
            else
            {
                startQTR = genreQuery.Distinct().ToList().First();
            }
            if (!String.IsNullOrEmpty(endQTR))
            {
                appealsrates = appealsrates.Where(x => x.QTR.CompareTo(endQTR) <= 0);
                complaintsrates = complaintsrates.Where(x => x.QTR.CompareTo(endQTR) <= 0);

            }
            else
            {
                endQTR = genreQuery.Distinct().ToList().Last();
            }
            var typerates = from c in appealsrates
                            from p in complaintsrates
                            where c.QTR.Equals(p.QTR) && c.SERVICE_AREA.Equals(p.SERVICE_AREA)
                            select new CompleteRate { ID = c.TOTAL, WAVE = c.COMPLETE, QTR = c.QTR, SERVICE_AREA = c.SERVICE_AREA, TOTAL = p.TOTAL, COMPLETE = p.COMPLETE };

            typerates = from p in typerates group p by p.SERVICE_AREA
             into g select new CompleteRate { SERVICE_AREA = g.Key,ID = g.Sum(p => p.ID), WAVE = g.Sum(p => p.WAVE), TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };

            var appealsquarterrates = from p in appealsrates group p by p.TYPE into g select new CompleteRate { TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };
            var complaintsquarterrates = from p in complaintsrates group p by p.TYPE into g select new CompleteRate {TOTAL = g.Sum(p => p.TOTAL), COMPLETE = g.Sum(p => p.COMPLETE) };



            var quarterrates = from c in appealsquarterrates
                               from p in complaintsquarterrates
                               select new CompleteRate { ID = c.TOTAL, WAVE = c.COMPLETE, TOTAL = p.TOTAL, COMPLETE = p.COMPLETE };
          

            var rateVM = new RateViewModel();
            rateVM.quarters = new SelectList(await genreQuery.Distinct().ToListAsync());
            rateVM.typeRates = await typerates.ToListAsync();
            rateVM.quarterRates = await quarterrates.ToListAsync();
            rateVM.startQTR = startQTR;
            rateVM.endQTR = endQTR;
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
