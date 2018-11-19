using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ingredients2.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeDto> Recipes { get; set; }
    }

    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeToMake { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; }
        public string AddedBy { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<IngredientDto> Ingredients { get; set; }

    }

    public class AddRecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeToMake { get; set; }
        public string ImageUrl { get; set; }
        public int[] Categories { get; set; }
        public int[] MustHaveIngredients { get; set; }
        public int[] OptionalIngredients { get; set; }
    }

    public class RecipesByIngredientsDto
     {
         public int[] Ids { get; set; }
     }

}