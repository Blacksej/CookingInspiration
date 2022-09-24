using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookingInspiration.Data;
using CookingInspiration.Models;
using ModelLibrary.Data;
using ModelLibrary.Models;
using System.Text.Json;
using System.Net.Http.Formatting;

namespace CookingInspiration.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        HttpClient client;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            client = new HttpClient();

            using HttpResponseMessage response = await client.GetAsync("https://localhost:7182/api/Ingredients");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(jsonResponse);

            return View(ingredients);

            //var applicationDbContext = _context.Ingredients.Include(i => i.Recipe);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Ingredients/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ingredient = JsonSerializer.Deserialize<Ingredient>(jsonResponse);

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
            client = new HttpClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7182/api/Ingredients", ingredient);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Ingredients/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ingredient = JsonSerializer.Deserialize<Ingredient>(jsonResponse);

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
            try
            {

                client = new HttpClient();

                using HttpResponseMessage response = await client.PutAsJsonAsync($"https://localhost:7182/api/Ingredients/{id}", ingredient);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

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
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", ingredient.RecipeId);
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Ingredients/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var ingredient = JsonSerializer.Deserialize<Ingredient>(jsonResponse);

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7182/api/Ingredients/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        private bool IngredientExists(int id)
        {
            return (_context.Ingredients?.Any(e => e.IngredientId == id)).GetValueOrDefault();
        }
    }
}
