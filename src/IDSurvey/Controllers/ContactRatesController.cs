using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IDSurvey.Data;
using IDSurvey.Models;

namespace IDSurvey.Controllers
{
    public class ContactRatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactRatesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ContactRates
        public async Task<IActionResult> Index()
        {

            var contactRates= from p in _context.V_Connected_Rates group p by p.QTR into g select new ContactRate { QTR = g.Key, TOTAL = g.Sum(p => p.TOTAL), COMPLETED = g.Sum(p => p.COMPLETED) };
            return View(await contactRates.ToListAsync());
        }

       

        private bool ContactRateExists(string QTR)
        {
            return _context.V_Connected_Rates.Any(e => e.QTR== QTR);
        }
    }
}
