using Ingredients2.Dtos;
using Ingredients2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ingredients2.Controllers.Api
{
    public class RecipesController : ApiController
    {
        [HttpGet]
        [Route("Recipes")]
        public IHttpActionResult GetAllRecipes()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var all = db.Recipes.Select(x => new RecipeDto
                    {
                        Id = x.Id,
                        Description = x.Description,
                        TimeToMake = x.TimeToMake,
                        ImageUrl = x.ImageUrl,
                        Name = x.Name,
                        AddedBy = x.AddedBy
                    }).ToList();
                    return Ok(all);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

[HttpGet]
        [Route("Recipes/{id}")]
        public IHttpActionResult GetRecipeById(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var recipe = db.Recipes.Find(id);
                    if (recipe == null)
                    {
                        return NotFound();
                    }
                    var rec = new RecipeDto()
                    {
                       Id = recipe.Id,
                       Name = recipe.Name,
                       DateAdded = recipe.DateAdded,
                       ImageUrl = recipe.ImageUrl,
                       Description = recipe.Description,
                       AddedBy = recipe.AddedBy,
                       TimeToMake = recipe.TimeToMake              
                    };
                 
                    return Ok(rec);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpDelete]
        [Route("Recipes/{id}")]
        public IHttpActionResult DeleteRecipe(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var recipe = db.Recipes.Find(id);
                    if (recipe == null)
                    {
                        return NotFound();
                    }
                    db.Recipes.Remove(recipe);
                    db.SaveChanges();
                    return Ok(recipe);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        [Route("recipes")]
        public IHttpActionResult AddRecipe([FromBody] AddRecipeDto recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Ingredient> mustHaveIngredients;
                    if (recipe.MustHaveIngredients == null)
                        mustHaveIngredients = null;
                    else
                        mustHaveIngredients = db.Ingredients.Where(x => recipe.MustHaveIngredients.Contains(x.Id)).ToList();

                    List<Ingredient> optionalIngredients;
                    if (recipe.OptionalIngredients == null)
                        optionalIngredients = null;
                    else
                        optionalIngredients = db.Ingredients.Where(x => recipe.OptionalIngredients.Contains(x.Id)).ToList();

                    List<Category> categories;
                    if (recipe.Categories == null)
                        categories = null;
                    else
                        categories = db.Categories.Where(x => recipe.Categories.Contains(x.Id)).ToList();
                    var rcp = new Recipe()
                    {
                        Name = recipe.Name,
                        Description = recipe.Description,
                        ImageUrl = recipe.ImageUrl,
                        AddedBy = User.Identity.Name
                    };
                    rcp.TimeToMake = recipe.TimeToMake;
                    if (categories != null) { 
                        foreach (var cat in categories)
                        {
                            rcp.Categories.Add(cat);
                        }
                    }
                    if (mustHaveIngredients != null)
                    {
                        foreach (var ing in mustHaveIngredients)
                        {
                            rcp.Ingredients.Add(new IngredientRecipe()
                            {
                                Ingredient = ing,
                                Recipe = rcp,
                                Optional = false
                            });
                        }
                    }
                    if (optionalIngredients != null)
                    {
                        foreach (var ing in optionalIngredients)
                        {
                            rcp.Ingredients.Add(new IngredientRecipe()
                            {
                                Ingredient = ing,
                                Recipe = rcp,
                                Optional = true,
                            });
                        }
                    }

                    db.Recipes.Add(rcp);
                    db.SaveChanges();
                    recipe.Id = rcp.Id;
                   // recipe.DateAdded = rcp.DateAdded;
                    return Ok(recipe);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
