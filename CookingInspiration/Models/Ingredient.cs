namespace CookingInspiration.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string NameAndAmount { get; set; }
        public int? RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
