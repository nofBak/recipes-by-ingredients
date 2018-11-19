using Ingredients2.Dtos;
using Ingredients2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ingredients2.Controllers.Api
{
    public class IngredientsController : ApiController
    {
        [HttpGet]
        [Route("ingredient")]
        public IHttpActionResult GetAllIngredients()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var all = db.Ingredients.Select(x => new IngredientDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
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
        [Route("ingredient/{id}")]
        public IHttpActionResult GetIngredientById(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var ingr = db.Ingredients.Find(id);
                    if (ingr == null)
                    {
                        return NotFound();
                    }

                    return Ok(ingr);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [HttpDelete]
        [Route("ingredient/{id}")]
        public IHttpActionResult DeleteIngredient(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var ingr = db.Ingredients.Find(id);
                    if (ingr == null)
                    {
                        return NotFound();
                    }
                    db.Ingredients.Remove(ingr);
                    db.SaveChanges();
                    return Ok(ingr); // make it simple dto
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        [Route("ingredient")]
        public IHttpActionResult AddIngredient([FromBody] IngredientDto ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("validation failed");
            }
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (db.Ingredients.Where(x => x.Name.Equals(ingredient.Name)).Any())
                    {
                        return BadRequest("המצרך כבר קיים במערכת");
                    }
                    var ingr = new Ingredient()
                    {
                        Description = ingredient.Description,
                        Name = ingredient.Name,
                    };

                    db.Ingredients.Add(ingr);
                    db.SaveChanges();
                    ingredient.Id = ingr.Id;
                    return Ok(ingredient);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        [Route("RecipesByIngredients")]
        public IHttpActionResult GetRecipesByIngredients([FromBody] RecipesByIngredientsDto ingredients)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var user_wish_list_ingredients = new List<Ingredient>();

                    foreach (var id_ingeredient in ingredients.Ids)
                    {
                        var found = db.Ingredients.Find(id_ingeredient);

                        if (found == null)
                        {
                            return BadRequest("Ingredient does not exist");
                        }
                        else
                        {
                            user_wish_list_ingredients.Add(found);
                        }
                    }
                    //creat a liset with all the recpes that contain one or more ingredients from the user list 
                    var allRecipes = db.Recipes.ToList();
                    var recipes_result = new List<Recipe>();

                    foreach (var rec in allRecipes)
                    {
                        var recipe_ingredients = new List<IngredientRecipe>();
                        recipe_ingredients = rec.Ingredients.ToList(); //list of ingredins per recipe
                        foreach (var ing in user_wish_list_ingredients)
                        {
                            if (recipe_ingredients.Where(x => x.IngredientId.Equals(ing.Id)).Any())
                            {
                                recipes_result.Add(rec);
                                break;
                            }
                        }
                    }
                    //remove all the recipes that contain inmportant ingredient that not in the user_wish_list_ingredients      
                    var recipe_final = new List<Recipe>();
                    //    Boolean flag;
                    //    foreach (var rec in recipes_result)
                    //    {
                    //        flag = false;
                    //        var recipe_ingredients = new List<IngredientRecipe>();
                    //        recipe_ingredients = rec.Ingredients.ToList(); //list of ingredins per recipe
                    //        foreach (var ing in recipe_ingredients)
                    //        {
                    //            if (ing.Optional == true)
                    //            {
                    //                if (user_wish_list_ingredients.Where(x => x.Id.Equals(ing.IngredientId)).Any())
                    //                {
                    //                    continue;
                    //                }
                    //                else
                    //                {
                    //                    flag = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        if(flag == false)
                    //        {
                    //            recipe_final.Add(rec);
                    //        }
                    //    }
                    var all = recipes_result.Select(x => new RecipeDto
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
    }
}
