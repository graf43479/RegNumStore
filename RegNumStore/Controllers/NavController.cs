using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Domain.Abstract;
using Domain.Entities;

namespace RegnumStore.Controllers
{
    public class NavController : Controller
    {

        
        private ICategoryRepository categoryRepository;

        public NavController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //
        // GET: /Nav/

        public ActionResult Menu(string type)
        {
         
            TempData["mobile"] = type;
            

            return View();
        }

        
        public ActionResult PortfolioMenu(string category = null)
        {
            if (TempData["categoryList"]!=null )
            {
                IEnumerable<Category> categories = TempData["categoryList"] as IEnumerable<Category>;
                ViewBag.SelectedCategory = categories;
                
            }
            //if (TempData["urlCategory2"] != null)
            //{
            //    ViewBag.SelectedCategory = TempData["urlCategory2"] as string; //category;
            //    var m = ViewBag.SelectedCategory;
            //    var s = m;
            //}
            //else
            //{
                
            //}
            var categoryList = categoryRepository.Categories.Where(x => x.IsActive).Where(x => x.Products.Any()).OrderBy(x => x.Sequence).AsNoTracking().ToList();

                return View(categoryList);
            
            
        }




    }
}
