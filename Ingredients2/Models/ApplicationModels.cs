using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ingredients2.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        //[MinLength(30)]
        public string Description { get; set; }
        public int TimeToMake { get; set; } = 0;
        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; }
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<IngredientRecipe> Ingredients { get; set; } = new HashSet<IngredientRecipe>();
        [Required]
        public string AddedBy { get; set; }
    }
    public class IngredientRecipe
    {
        [Key, Column(Order = 0)]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
        [Key, Column(Order = 1)]
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public bool Optional { get; set; }
    }

    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(64)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<IngredientRecipe> Recipes { get; set; } = new HashSet<IngredientRecipe>();
    }

    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(64)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}