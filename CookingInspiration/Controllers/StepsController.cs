using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookingInspiration.Data;
using CookingInspiration.Models;
using System.ComponentModel.DataAnnotations;

namespace CookingInspiration.Controllers
{
    public class StepsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Steps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Steps.Include(s => s.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Steps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Steps == null)
            {
                return NotFound();
            }

            var step = await _context.Steps
                .Include(s => s.Recipe)
                .FirstOrDefaultAsync(m => m.StepId == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // GET: Steps/Create
        public IActionResult Create()
        {
            ViewData["FromRecipe"] = false;
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId");
            return View();
        }

        [Route("Steps/CreateFromRecipe/{id?}")]
        public IActionResult Create(int id)
        {
            ViewData["FromRecipe"] = true;
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", id);
            return View();
        }

        // POST: Steps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepId,Description,RecipeId")] Step step, bool? fromRecipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(step);
                await _context.SaveChangesAsync();
                if(fromRecipe != false)
                {
                    return RedirectToAction("CreateFromRecipe", "Steps", step.RecipeId);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", step.RecipeId);
            return View(step);
        }

        // GET: Steps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Steps == null)
            {
                return NotFound();
            }

            var step = await _context.Steps.FindAsync(id);
            if (step == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", step.RecipeId);
            return View(step);
        }

        // POST: Steps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StepId,Description,RecipeId")] Step step)
        {
            if (id != step.StepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(step);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StepExists(step.StepId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", step.RecipeId);
            return View(step);
        }

        // GET: Steps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Steps == null)
            {
                return NotFound();
            }

            var step = await _context.Steps
                .Include(s => s.Recipe)
                .FirstOrDefaultAsync(m => m.StepId == id);
            if (step == null)
            {
                return NotFound();
            }

            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Steps == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Steps'  is null.");
            }
            var step = await _context.Steps.FindAsync(id);
            if (step != null)
            {
                _context.Steps.Remove(step);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StepExists(int id)
        {
          return (_context.Steps?.Any(e => e.StepId == id)).GetValueOrDefault();
        }
    }
}
