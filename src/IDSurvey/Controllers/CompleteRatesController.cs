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
    public class CompleteRatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompleteRatesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: CompleteRates
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompleteRates.ToListAsync());
        }

        // GET: CompleteRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeRate = await _context.CompleteRates.SingleOrDefaultAsync(m => m.ID == id);
            if (completeRate == null)
            {
                return NotFound();
            }

            return View(completeRate);
        }

        // GET: CompleteRates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompleteRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,COMPLETE,QTR,SERVICE_AREA,TOTAL,TYPE,WAVE")] CompleteRate completeRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(completeRate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(completeRate);
        }

        // GET: CompleteRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeRate = await _context.CompleteRates.SingleOrDefaultAsync(m => m.ID == id);
            if (completeRate == null)
            {
                return NotFound();
            }
            return View(completeRate);
        }

        // POST: CompleteRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,COMPLETE,QTR,SERVICE_AREA,TOTAL,TYPE,WAVE")] CompleteRate completeRate)
        {
            if (id != completeRate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(completeRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompleteRateExists(completeRate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(completeRate);
        }

        // GET: CompleteRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeRate = await _context.CompleteRates.SingleOrDefaultAsync(m => m.ID == id);
            if (completeRate == null)
            {
                return NotFound();
            }

            return View(completeRate);
        }

        // POST: CompleteRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var completeRate = await _context.CompleteRates.SingleOrDefaultAsync(m => m.ID == id);
            _context.CompleteRates.Remove(completeRate);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CompleteRateExists(int id)
        {
            return _context.CompleteRates.Any(e => e.ID == id);
        }
    }
}
