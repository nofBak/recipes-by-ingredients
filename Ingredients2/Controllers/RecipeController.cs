using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ingredients2.Controllers
{
    public class RecipeController : Controller
    {
        [AllowAnonymous]
        // GET: Recipe
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Info(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}