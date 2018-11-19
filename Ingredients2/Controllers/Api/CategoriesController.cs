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
    public class CategoriesController : ApiController
    {
        [HttpGet]
        [Route("category")]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var all = db.Categories.Select(x => new CategoryDto
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
        [Route("Category/{id}")]
        public IHttpActionResult GetCategoryById(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var category = db.Categories.Find(id);
                    if (category == null)
                    {
                        return NotFound();
                    }

                    return Ok(category);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpDelete]
        [Route("Category/{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var category = db.Categories.Find(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    return Ok(category);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }



        [HttpPost]
        [Route("category")]
        public IHttpActionResult AddCategory([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("validation failed");
            }
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (db.Categories.Where(x => x.Name.Equals(category.Name)).Any())
                    {
                        return BadRequest("הקטגוריה כבר קיימת");
                    }
                    var categor = new Category()
                    {
                        Description = category.Description,
                        Name = category.Name,
                    };

                    db.Categories.Add(categor);
                    db.SaveChanges();
                    category.Id = categor.Id;
                    return Ok(category);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
