using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Step>? Steps { get; set; }
        public string Image { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }
}
