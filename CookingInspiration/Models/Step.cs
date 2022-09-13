﻿namespace CookingInspiration.Models
{
    public class Step
    {
        public int StepId { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
