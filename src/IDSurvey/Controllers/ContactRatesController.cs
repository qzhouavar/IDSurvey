using IDSurvey.Data;
using IDSurvey.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IDSurvey.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin,Manager,Member")]
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
            var contactRates = from p in _context.V_Connected_Rates
                               group p by p.Description 
                               into g
                               select new ContactRate
                               {   Description = g.Key,
                                   TOTAL = g.Sum(p => p.TOTAL),
                                   COMPLETED = g.Sum(p => p.COMPLETED),
                                   WRONG_NUMBER = g.Sum(P => P.WRONG_NUMBER)
                               };
            return View(await contactRates.ToListAsync());
        }

        private bool ContactRateExists(string QTR)
        {
            return _context.V_Connected_Rates.Any(e => e.Description == QTR);
        }
    }
}