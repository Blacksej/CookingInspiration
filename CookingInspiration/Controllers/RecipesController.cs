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

namespace CookingInspiration.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        HttpClient client;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            client = new HttpClient();

            using HttpResponseMessage response = await client.GetAsync("https://localhost:7182/api/Recipes");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonResponse);

            return View(recipes);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Recipes/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var recipe = JsonSerializer.Deserialize<Recipe>(jsonResponse);

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,Name,Description,Image")] Recipe recipe)
        {
                client = new HttpClient();

                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7182/api/Recipes", recipe);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View();
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Recipes/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var recipe = JsonSerializer.Deserialize<Recipe>(jsonResponse);

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Name,Description,Image")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }
            try
            {

                client = new HttpClient();

                using HttpResponseMessage response = await client.PutAsJsonAsync($"https://localhost:7182/api/Recipes/{id}", recipe);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(recipe.RecipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View();
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.GetAsync($"https://localhost:7182/api/Recipes/{id}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var recipe = JsonSerializer.Deserialize<Recipe>(jsonResponse);

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            client = new HttpClient();

            if (id == null)
            {
                return NotFound();
            }

            using HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7182/api/Recipes/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
