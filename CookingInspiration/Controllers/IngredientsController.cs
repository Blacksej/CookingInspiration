﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookingInspiration.Data;
using CookingInspiration.Models;

namespace CookingInspiration.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ingredients.Include(i => i.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId");
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientId,NameAndAmount,RecipeId")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", ingredient.RecipeId);
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", ingredient.RecipeId);
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredientId,NameAndAmount,RecipeId")] Ingredient ingredient)
        {
            if (id != ingredient.IngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.IngredientId))
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
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", ingredient.RecipeId);
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ingredients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ingredients'  is null.");
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
          return (_context.Ingredients?.Any(e => e.IngredientId == id)).GetValueOrDefault();
        }
    }
}
