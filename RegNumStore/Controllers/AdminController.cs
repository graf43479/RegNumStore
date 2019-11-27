using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using RegNumStore.Models;
using RegnumStore.Models;
using RegnumStore.Extensions;

namespace RegnumStore.Controllers
{
     [Authorize(Roles = "admin, contentManager, Admin, ContentManager, SEO")]
    public class AdminController : Controller
    {
        
        private IProductRepository repositoryProduct;
        private ICategoryRepository repositoryCategory;
        private IUserRepository repositoryUser;
        private IOrderRepository repositoryOrder;
         private IRegionTypeRepository repositoryRegionType;
        private IRegionRepository repositoryRegion;
        private ICommentRepository repositoryComment;
     //   private IArticleRepository repositoryArticle;region

        private ISeoAttributeRepository repositorySeoAttribute;
        private IMailingSettingsRepository repositoryMailingSetting;
        
        

        public int PageSize = 5;


        //-----------------------------------------------------
         public AdminController(
             IProductRepository repoProduct, 
             ICategoryRepository repoCategory,
             IUserRepository repoUser, 
             IOrderRepository repoOrder, 
             IRegionTypeRepository repoRegionType, 
             IRegionRepository repoRegion, 
             ICommentRepository repoComment, /*IArticleRepository repoArticle,*/ 
             ISeoAttributeRepository repoSeoAttribute, 
             IMailingSettingsRepository reposMailingSetting)
         {
             repositoryProduct = repoProduct;
             repositoryCategory = repoCategory;
             repositoryUser = repoUser;
             repositoryOrder = repoOrder;
             repositoryRegionType = repoRegionType;
             repositoryRegion = repoRegion;
             repositoryComment = repoComment;
             repositorySeoAttribute = repoSeoAttribute;
            // repositoryArticle = repoArticle;
             repositoryMailingSetting = reposMailingSetting;

         }


         public ActionResult Index()
        {
            return View();
        }

#region Category
         public async Task<ActionResult> Categories(string searchWord, GridSortOptions gridSortOptions, int? page, int? categoryId, string actionType)
        {

         
            int pageItemsCount = 20;

         
            IEnumerable<Category> categoryType = await repositoryCategory.Categories.ToListAsync();

             if (categoryId!=null && String.IsNullOrEmpty(actionType)!=true)
             {
                 try
                    {
                        Exception ex = new Exception();
                        int[] sequence =
                            categoryType.Select(x => x.Sequence).ToArray();
                            //repositoryCategory.Categories.Select(x => x.Sequence).ToArray();
                        Array.Sort(sequence);
                        //sequence.OrderBy(x => x.ToString());

                        for (int i = 0; i < sequence.Count(); i++)
                        {
                            if (sequence[i] == i + 1)
                            {

                            }
                            else
                            {
                                //logger.Error(User.Identity.Name + ". Проблема обновления списка " + ex.Message);
                                throw (ex);
                            }
                        }

                        repositoryCategory.CategorySequence((int)categoryId, actionType);
                    }

                    catch (Exception ex)
                    {
                        TempData["message"] = string.Format("Нарушена последовательность! Список был пересчитан!");
                        TempData["messageType"] = "error-msg";
                      //  logger.Error(User.Identity.Name + ". Нарушена последовательность в категориях! Список был пересчитан!" + ex.Message);
                        repositoryCategory.UpdateCategorySequence();
                        repositoryCategory.CategorySequence((int)categoryId, actionType);
                    }
             }
         
            var query = from a in categoryType
                        select new CategoryViewModel()
                        {
                            CategoryID = a.CategoryID,
                            Sequence = a.Sequence,
                            ShortName = a.ShortName,
                            Description = a.Description,
                            CategoryName = a.CategoryName,
                            CreateDate = a.CreateDate,
                            UpdateDate = a.UpdateDate,
                            Snippet = a.Snippet,
                            IsActive = a.IsActive,
                            KeyWords = a.KeyWords,
                            //PhotoCount = productsList.Count(x => x.CategoryID==a.CategoryID),
                            PhotoCount = categoryType.Where(x=>x.CategoryID==a.CategoryID).SelectMany(x=>x.Products).Count()
                          //  ChoosenCount = productsList.Where(x=>x.IsChoosen).Count(x => x.CategoryID == a.CategoryID),
                           // VisibleCount = productsList.Where(x => x.IsDisplay==false).Count(x => x.CategoryID == a.CategoryID)

                        };
         
            var pagedViewModel = new PagedViewModel<CategoryViewModel>
            {
                ViewData = ViewData,
                Query = query.AsQueryable(),//categoryType.AsQueryable(),
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Sequence",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            }
                .AddFilter("searchWord", searchWord,
                           a => a.CategoryName.ToLower().Contains(searchWord.ToLower()) /*|| a.ShortName.Contains(searchWord)*/)
                .Setup();


            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("CategoryGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);

        }


         public ActionResult DefineProductsCategories()
         {
             IEnumerable<Product> productList = repositoryProduct.Products.ToList();
             foreach (Product product in productList)
             {
                 DefineAllCategories(product);
             }
             return RedirectToAction("Categories");
         }

         public void DefineAllCategories2(Product product)
         {
             var controllers = (from t in typeof(HomeController).Assembly.GetExportedTypes()
                                where
                                    t != null &&
                                    t.IsPublic &&
                                    t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                                    !t.IsAbstract &&
                                    typeof(IController).IsAssignableFrom(t)
                                select t).ToList();
             var m = controllers.First();

            var l1 = m.GetMethods().First(x => x.Name == "DefineAllCategories");
       //     l1.Invoke(new AccountController(), new object[] {  product });
             //var mapMvcAttributeRoutesMethod = typeof(RouteCollectionAttributeRoutingExtensions)
             //    .GetMethod(
             //        "MapMvcAttributeRoutes",
             //        BindingFlags.NonPublic | BindingFlags.Static,
             //        null,
             //        new Type[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
             //        null);

             //mapMvcAttributeRoutesMethod.Invoke(null, new object[] { routes, controllers });
             //controller.DefineAllCategories(product);
         }

         public void DefineAllCategories(Product product)
         {
             CarNumber carNum = product.ProductNumber.NumberToCarNumber();

             List<int> assignedIDs = new List<int>();

             if (carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6)
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые буквы");
                 if (firstOrDefault != null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }
             if (carNum.Num2 == carNum.Num3 && carNum.Num3 == carNum.Num4)
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые цифры");
                 if (firstOrDefault != null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }

             if ((carNum.Num2 + carNum.Num3 + carNum.Num4 == carNum.Num7) || (carNum.Num2 == "0" && carNum.Num2 + carNum.Num3 + carNum.Num4 == "0" + carNum.Num7))
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Номер-регион");
                 if (firstOrDefault !=
                     null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }

             if (((carNum.Num2 == carNum.Num3 && carNum.Num3 == carNum.Num4) && (carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6)) || ((carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6) && ((carNum.Num2 + carNum.Num3 + carNum.Num4 == carNum.Num7) || (carNum.Num2 == "0" && carNum.Num2 + carNum.Num3 + carNum.Num4 == "0" + carNum.Num7)))
           || ((carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6) && (carNum.Num2 == carNum.Num3 && carNum.Num2 == "0")))
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Эксклюзив");
                 if (firstOrDefault !=
                     null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }


             if ((carNum.Num5 == "М" && (carNum.Num6 == "Р" || carNum.Num6 == "М") && carNum.Num7 == "77")
                 || (carNum.Num1 == "Е" && carNum.Num5 == "К" && carNum.Num6 == "Х" && (carNum.Num7 == "77" || carNum.Num7 == "99"))
                 || (carNum.Num1 == "Х" && carNum.Num5 == "К" && carNum.Num6 == "Х" && carNum.Num7 == "77")
                 || (carNum.Num1 == "С" && carNum.Num5 == "А" && carNum.Num6 == "С" && carNum.Num7 == "77")
                 || (carNum.Num1 == "О" && carNum.Num5 == "О" && carNum.Num6 == "О" && (carNum.Num7 == "77" || carNum.Num7 == "99" || carNum.Num7 == "97"))
                 || (carNum.Num1 == "К" && carNum.Num5 == "К" && carNum.Num6 == "К" && carNum.Num7 == "99")
                 || (carNum.Num1 == "С" && carNum.Num5 == "С" && carNum.Num6 == "С" && (carNum.Num7 == "77" || carNum.Num7 == "97" || carNum.Num7 == "99"))
                || ((carNum.Num1 == "А" || carNum.Num1 == "В" || carNum.Num1 == "К" || carNum.Num1 == "С" || carNum.Num1 == "О"
                 || carNum.Num1 == "М" || carNum.Num1 == "Т" || carNum.Num1 == "Н"
                 || carNum.Num1 == "У" || carNum.Num1 == "Х") && carNum.Num5 == "М" && carNum.Num6 == "О" && carNum.Num7 == "50")
                 || ((carNum.Num1 == "А" || carNum.Num1 == "М") && carNum.Num5 == "М" && carNum.Num6 == "М" && (carNum.Num7 == "50" || carNum.Num7 == "90"))
                 )
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Спецсерия");
                 if (firstOrDefault !=
                     null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }

            
             if (!assignedIDs.Any())
             {
                 var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Другое");
                 if (firstOrDefault != null)
                     assignedIDs.Add(firstOrDefault.CategoryID);
             }
             else
             {

             }


             var CategoryIDs = product.Categories.Select(x => x.CategoryID);
             var courseIDs = CategoryIDs as int[] ?? CategoryIDs.ToArray();

             var coursesToDeleteIDs = courseIDs.Where(id => !assignedIDs.Contains(id)).ToList();

             // Delete removed courses
             foreach (var id in coursesToDeleteIDs)
             {
                 product.Categories.Remove(repositoryProduct.FindID(id));
             }

             // Add courses that user doesn't already have
             foreach (var id in assignedIDs)
             {
                 if (!courseIDs.Contains(id))
                 {
                     product.Categories.Add(repositoryProduct.FindID(id));
                 }
             }
             repositoryProduct.SaveProduct(product);
         }


         public ActionResult CreateCategories()
         {
             var catList = repositoryCategory.Categories.ToList();
             if (repositoryCategory.Categories.Any())
             {
                 foreach (var cat in catList)
                 {
                     repositoryCategory.DeleteCategory(cat);
                 }
             }
             var categories = new List<Category>
                {
                    //new Category { CategoryName = "Золотые буквы", Description = "Все буквы номера одинаковые", ShortName = "cat1",CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 1},
                    //new Category { CategoryName = "Золотые цифры", Description = "Все цифры в номере одинаковые", ShortName = "cat2", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 2},
                    //new Category { CategoryName = "Региональный номер", Description = "Цифры номера совпадают с его регионом",ShortName = "cat3", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 3},
                    //new Category { CategoryName = "Первая десятка", Description = "Первые десять номеров серии",ShortName = "cat4", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 4},
                    //new Category {CategoryName = "Сотые", Description = "Номера вида \"*100**\",  \"*200**\" и т.д.",ShortName = "cat5", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 5},
                    //new Category { CategoryName = "Зеркальные", Description = "Номера вида  \"*050**\",  \"*101**\" и т.д.",ShortName = "cat6", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 6},
                    //new Category { CategoryName = "Спецномер", Description = "",ShortName = "cat7", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 7},
                    // new Category { CategoryName = "Другое", Description = "",ShortName = "cat8", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 8}
               
                  new Category {CategoryName = "Одинаковые буквы", Description = "Все буквы номера одинаковые", ShortName = "cat1",CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 1},
                    new Category {CategoryName = "Одинаковые цифры", Description = "Все цифры в номере одинаковые", ShortName = "cat2", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 2},
                    new Category {CategoryName = "Номер-регион", Description = "Цифры номера совпадают с его регионом",ShortName = "cat3", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 3},
                    new Category {CategoryName = "Спецсерия", Description = "",ShortName = "cat4", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 4},
                     new Category {CategoryName = "Эксклюзив", Description = "",ShortName = "cat5", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 5},
                       new Category {CategoryName = "Другое", Description = "",ShortName = "cat6", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true, Sequence = 6}
              
                };

             categories.ForEach(x => repositoryCategory.SaveCategory(x));
             

             return RedirectToAction("Categories");
         }


       

        //[HttpPost]
        //[Authorize(Roles = "Admin, ContentManager")]
        //public ActionResult DeleteCategory(int categoryId)
        //{
        //    Category category = repositoryCategory.Categories.FirstOrDefault(p => p.CategoryID == categoryId);

        //    if (category != null)
        //    {
        //            repositoryCategory.DeleteCategory(category);
        //            TempData["message"] = string.Format("Категория '{0}' была удалена", category.CategoryName);
        //            TempData["messageType"] = "warning-msg";
        //    }
        //    return RedirectToAction("Categories");
        //}
         
        public ActionResult RefreshAllShortNamesInCategories()
        {
            repositoryCategory.RefreshAllShortNames();
            return RedirectToAction("Categories");
        }


#endregion Category


        #region Product
        public ActionResult Products(string searchWord, GridSortOptions gridSortOptions, int? categoryId, int? regionId, int? page)
        {
            
            int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

            List<ProductViewModel> products = new List<ProductViewModel>();
            var plist = repositoryProduct.Products.ToList();
            var regions = repositoryRegion.Regions.OrderBy(x => x.Sequence);
            var existRegions = (from p in plist
                               join r in regions on p.RegionID equals r.RegionID
                               select new RegionViewModel()
                                   {
                                       RegionID = r.RegionID,
                                       RegionName = r.RegionName,
                                       RegionNumber = r.RegionNumber,
                                       Sequence = r.Sequence,
                                       RegionFullName = r.RegionNumber + ": " + r.RegionName
                                   }).GroupBy(x=>x.RegionFullName).Select(m=>m.First()).OrderByDescending(x=>x.Sequence).ThenBy(l=>l.RegionFullName);

            //existRegions = existRegions.Distinct();
            
            

            foreach (var product in plist)      
            {
                products.Add(product.ProductToViewModel());
            }

            //if (String.IsNullOrEmpty(regionName))
            //{
            //    regionName = "";
            //}
            

            var pagedViewModel = new PagedViewModel<ProductViewModel>
            {
                ViewData = ViewData,
                Query = products.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "UpdateDate",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            };
            

            pagedViewModel
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.ProductNumber.Contains(searchWord)
                           || a.Region.RegionName.ToLower().Contains(searchWord.ToLower()))
                //           || a.CategoryName.Contains(searchWord))
                .AddFilter("categoryId", categoryId, a => a.Categories.Any(m => m.CategoryID == categoryId), //a.SelectedCategoryID == categoryId,
                           repositoryCategory.Categories.OrderBy(x => x.CategoryName), "CategoryName")
                //.AddFilter("regionId", regionId, a => a.SelectedRegionID == regionId,
                //           repositoryRegion.Regions.OrderBy(x => x.Sequence), "RegionNumber")
                //.AddFilter("regionId", regionId, a => a.SelectedRegionID == regionId,
                //            existRegions.AsQueryable(), "RegionNumber")
               .AddFilter("regionId", regionId, a => a.SelectedRegionID == regionId,
                            existRegions.AsQueryable(), "RegionFullName")
                            
                            //.AddFilter("regionName", regionName, a => a.RegionName == regionName,
                //           existRegions.AsQueryable(), "RegionName")
                .Setup();
            //x => x.Categories.Any()
            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("ProductGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
            //return View(repositoryProduct.Products);

        }


        public ActionResult EditProduct(int? productId)
        {
            if (productId==null)
            {
                return RedirectToAction("Products","Admin");
            }
            ViewBag.Users = repositoryUser.UsersInfo.ToList();

            IEnumerable<Category> categoryList = repositoryCategory.Categories.OrderBy(x => x.CategoryName);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            Product product = repositoryProduct.Products.FirstOrDefault(p => p.ProductID == productId);

            ProductViewModel productViewModel = product.ProductToViewModel();
            productViewModel.Categories = categoryList;

            //List<ProductViewModel> products = new List<ProductViewModel>();
            //var plist = repositoryProduct.Products.ToList();
            //foreach (var product in plist)
            //{

            //    products.Add(product.ProductToViewModel());
            //}



            return View(productViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProduct(ProductViewModel productViewModel)
        {
            //int selectedCategory = productViewModel.SelectedCategoryID;
            ViewBag.Users = repositoryUser.UsersInfo.ToList();
            if (ModelState.IsValid)
            {
                productViewModel.UpdateDate = DateTime.Now;

                if (productViewModel.ProductID == 0)
                {
                    productViewModel.StartDate = DateTime.Now;
                    repositoryProduct.SaveProduct(productViewModel.ViewModelToProduct());


                    TempData["message"] = string.Format("{0}-{1}.{2} сохранен", productViewModel.CategoryName, productViewModel.ProductID, ".jpg");
                    TempData["messageType"] = "confirmation-msg";

                    return RedirectToAction("EditProduct", "Admin", new { productId = repositoryProduct.Products.Max(x => x.ProductID) });
                }
                else
                {


                    //Product product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == productViewModel.ProductID);
                    //Product productOriginal = repositoryProduct.GetProductOrigin(product);
                    productViewModel.UpdateDate = DateTime.Now;
                    repositoryProduct.SaveProduct(productViewModel.ViewModelToProduct());

                    
                    return RedirectToAction("EditProduct", "Admin", new { productId = productViewModel.ProductID });
                }
            }
            else
            {
                productViewModel.Categories = repositoryCategory.Categories.ToList();

                return View(productViewModel);
            }
        }

     

        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteProduct(int[] resubmit)
        {
            DeleteProductRelationships(resubmit);
           
            return RedirectToAction("Products");
        }




        public void DeleteProductRelationships(int[] resubmit)
        {
            //if ((productId == null) && (resubmit!=null)) 
            if (resubmit != null)
            {
                try
                {
                    foreach (var p in resubmit)
                    {

                        //var productAsync = repositoryProduct.GetProductByIDAsync(p);
                        var product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == p);//GetProductByIDAsync(p);


                        if (product != null)
                        {
                            string strSaveFileName = "Image" + product.ProductID.ToString();// +product.ImgExt;
                           
                            string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                            Constants.PRODUCT_IMAGE_FOLDER,
                                                                            strSaveFileName);
                            string strSavePreviewFullPath =
                                System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                       Constants.PRODUCT_IMAGE_FOLDER,
                                                       Constants.PRODUCT_IMAGE_PREVIEW_FOLDER,
                                                       strSaveFileName);
                            if (System.IO.File.Exists(strSaveFullPath))
                            {
                                System.IO.File.Delete(strSaveFullPath);
                            }
                            else
                            {
                                Exception ex;
                            }
                            if (System.IO.File.Exists(strSavePreviewFullPath))
                            {
                                System.IO.File.Delete(strSavePreviewFullPath);
                            }

                            repositoryProduct.DeleteProduct(product);

                        }

                    }
                    TempData["message"] = string.Format("Фотографии были удалены");
                    TempData["messageType"] = "warning-msg";
                }
                catch (Exception)
                {
                    TempData["message"] = string.Format("Что-то пошло не так при удалении файлов");
                    TempData["messageType"] = "warning-msg";    
                }
    
                TempData["message"] = string.Format("Фотографии были удалены");
                TempData["messageType"] = "warning-msg";
            }
            
            
        }



        //public ActionResult ProductSequenceView(int categoryId)
        //{
        
        //    GridSortOptions gridSortOptions = new GridSortOptions()
        //    {
        //        Direction = SortDirection.Ascending,
        //        Column = "Sequence"
        //    };

        //    var pagedViewModel = new PagedViewModel<Product>
        //    {
        //        ViewData = ViewData,
        //        Query = repositoryProduct.Products.Where(x => x.CategoryID == categoryId),
        //        GridSortOptions = gridSortOptions,
        //        DefaultSortColumn = "Sequence",
        //        Page = 1,
        //        PageSize = 100,
        //    }
        //        .Setup();

        //    return View("ProductSequence", pagedViewModel);
        //}


        
            //---Определение пустых категорий 
            /*    IEnumerable<string> productCategories = from c in repositoryCategory.Categories.ToList()
                                                    join p in repositoryProduct.Products.ToList() on c.CategoryID
                                                        equals
                                                        p.CategoryID
                                                    group c by new { c.CategoryID, c.ShortName }
                                                        into tmp
                                                        select tmp.Key.ShortName;

            IEnumerable<string> categoriesExists = from c in repositoryCategory.Categories.ToList()
                                                   select c.ShortName;


            IQueryable<string> difference = categoriesExists.Except(productCategories).AsQueryable();



            var z = from j in repositoryCategory.Categories.ToList()
                    where j.ShortName == difference.FirstOrDefault(x => x.ToString() == j.ShortName)
                    select j.CategoryID;

            ViewBag.EmptyCategories = z.ToArray();  */
            //--------------------------



         //public ActionResult UploadPhoto(int categoryId)
         //{
         //    Category category = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryID == categoryId); 
         //    return View(category);
         //}

         //    public ActionResult Upload(int categoryId)
         //{
         //    for (int i = 0; i < Request.Files.Count; i++)
         //    {
                 
                 
         //        var file = Request.Files[i];
         //        //file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "Uploads/" + file.FileName);

         //        //int productId;
         //        //try
         //        //{
         //        //    productId = repositoryProduct.Products.Max(x => x.ProductID) + 1;
         //        //}
         //        //catch (Exception)
         //        //{
         //        //    productId = 1;
         //        //} 

                
                 
         //        //int sequence;
         //        //try
         //        //{
         //        //    sequence = ((repositoryProduct.Products.Where(x => x.CategoryID == categoryId)
         //        //                                 .Select(x => x.Sequence)
         //        //                                 .Max()) == 0
         //        //             ? 1
         //        //             : repositoryProduct.Products.Where(x => x.CategoryID == categoryId)
         //        //                                      .Select(x => x.Sequence)
         //        //                                      .Max() + 1);
         //        //}
         //        //catch (Exception)
         //        //{
                     
         //        //    sequence = 1;
         //        //}

         //        string strExtension = System.IO.Path.GetExtension(file.FileName);
         //        string path = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
         //                                             "img");
         //        Product product = new Product()
         //            {
         //                CategoryID = categoryId,
         //                Path = path,
         //                ImgExt = strExtension,
         //                IsChoosen = false,
         //                IsDisplay = true,
         //                StartDate = DateTime.Now,
         //                UpdateDate = DateTime.Now,
         //                Sequence = sequence
         //            };
         //        repositoryProduct.SaveProduct(product);


                 
         //        string strSaveFileName = "Image" + product.ProductID + strExtension;
                 
         //        string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
         //                                                        "img",
         //                                                        strSaveFileName);

         //        string strSavePreviewFullPath = System.IO.Path.Combine(
         //                   Server.MapPath(Url.Content("~/Content")), "img",
         //                   "cache", strSaveFileName);


         //        // Если файл с таким названием имеется, удаляем его.
         //        if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);
         //        if (System.IO.File.Exists(strSavePreviewFullPath)) System.IO.File.Delete(strSavePreviewFullPath);
         //        file.ResizeImage(Constants.PRODUCT_IMAGE_HEIGHT, Constants.PRODUCT_IMAGE_WIDTH, strSaveFullPath);
         //        file.ResizeImage(Constants.PRODUCT_IMAGE_PREVIEW_HEIGHT, Constants.PRODUCT_IMAGE_PREVIEW_WIDTH, strSavePreviewFullPath);

         //    }
         //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
         //}

         

#endregion Product


#region User
        public ActionResult UsersView(string searchWord, GridSortOptions gridSortOptions, int? page,int? roleId,
                                     string userActivity = "Активные")
        {
            var userList = repositoryUser.UsersInfo.ToList();
            bool IsActive;

            int pageItemsCount = Domain.Constants.ADMIN_PAGE_SIZE;

            List<UserViewModel> users = new List<UserViewModel>();

            foreach (var user in userList)
            {
                users.Add(user.UserToViewModel());
            }

            var pagedViewModel = new PagedViewModel<UserViewModel>
            {
                ViewData = ViewData,
                Query = users.AsQueryable(), 
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "Login",
                Page = page,
                PageSize = pageItemsCount
            }
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.Login.ToLower().Contains(searchWord.ToLower()) || a.Email.ToLower().Contains(searchWord.ToLower()))
                
                .AddFilter("userActivity", (userActivity == "Активные") ? IsActive = true : IsActive = false,
                           a => a.IsActivated == IsActive) //,  _service.GetGenres(), "Name")
                 .AddFilter("roleId", roleId, a => a.SelectedRoleID == roleId,
                           repositoryUser.Roles.OrderBy(x => x.RoleName), "RoleName")
                .Setup();
            //  ViewBag.ServName = Server.MachineName;

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("UserGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
            //return View(repositoryUser.UsersInfo);
        }



        [Authorize(Roles = "Admin, ContentManager")]
        public ViewResult EditUser(int userId)
        {
            /*
             IEnumerable<Category> categoryList = repositoryCategory.Categories.OrderBy(x => x.CategoryName);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            Product product = repositoryProduct.Products.FirstOrDefault(p => p.ProductID == productId);

            ProductViewModel productViewModel = product.ProductToViewModel();
            productViewModel.Categories = categoryList;

            //List<ProductViewModel> products = new List<ProductViewModel>();
            //var plist = repositoryProduct.Products.ToList();
            //foreach (var product in plist)
            //{

            //    products.Add(product.ProductToViewModel());
            //}



            return View(productViewModel);
             */
            IEnumerable<Role> roleList = repositoryUser.Roles.OrderBy(x => x.RoleName);
            User user = repositoryUser.UsersInfo.FirstOrDefault(p => p.UserID == userId);
            UserViewModel userViewModel = user.UserToViewModel();
            userViewModel.Roles = roleList;
            return View(userViewModel);

        }


        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult EditUser(UserViewModel viewModel)
        {

            viewModel.Email = viewModel.Email.TrimEnd();
            viewModel.Login = viewModel.Login.TrimEnd();
            viewModel.Password = viewModel.Password.TrimEnd();
            

            if (ModelState.IsValid)
            {
                repositoryUser.SaveUser(viewModel.ViewModelToUser());

                // return the user to the list

                if (viewModel.UserID == 0)
                {
                    // add a message to the viewbag
                    TempData["Message"] = string.Format("Пользователь '{0}' создан", viewModel.Login);
                    TempData["messageType"] = "confirmation-msg";
                }
                else
                {
                    TempData["Message"] = string.Format("Пользователь '{0}' изменен", viewModel.Login);
                    TempData["messageType"] = "information-msg";
                }

                return RedirectToAction("UsersView");
            }
            else
            {
                // there is something wrong with the data values
                return View(viewModel);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteUser(int?[] resubmit)
        {
            
            if (resubmit != null)
            {
                foreach (var p in resubmit)
                {
                    User user = repositoryUser.UsersInfo.FirstOrDefault(x => x.UserID == p);
                    if (user != null)
                    {
                        try
                        {
                            repositoryUser.DeleteUser(user);
                            TempData["Message"] = string.Format("Пользователь '{0}' был удален", user.Login);
                            TempData["messageType"] = "warning-msg";
                        }
                        catch (Exception)
                        {
                            
                            //repositoryUser.SaveUser(user);
                            TempData["Message"] = string.Format("Произошла ошибка удаления '{0}' ", user.Login);
                            TempData["messageType"] = "warning-msg";
                            //logger.Warn(User.Identity.Name + ". Пользователь " + user.Login + " деактивирован ");
                        }

                    }
                }
            }

            return RedirectToAction("UsersView");
        }

#endregion User

        #region Comment

        public ActionResult Comments(string searchWord, GridSortOptions gridSortOptions, int? categoryId, int? page)
         {
             //IEnumerable<Comment> comments = repositoryComment.Comments.ToList();


             int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

             List<Comment> comments = new List<Comment>();
             var clist = repositoryComment.Comments.OrderByDescending(x=>x.CreateDate).ToList();
             foreach (var comment in clist)
             {

                 comments.Add(comment);
             }

             var pagedViewModel = new PagedViewModel<Comment>
             {
                 ViewData = ViewData,
                 Query = comments.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                 GridSortOptions = gridSortOptions,
                 DefaultSortColumn = "CreateDate",
                 Page = page,
                 PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
             };

             pagedViewModel
                 .AddFilter("searchWord", searchWord,
                            a =>
                            a.QuestionText.Contains(searchWord)
                            || a.AnswerText.Contains(searchWord)
                            || a.Tittle.Contains(searchWord))
                 .Setup();

             if (Request.IsAjaxRequest())
             {
                 Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                 return PartialView("CommentGridPartial", pagedViewModel);
             }

             return View(pagedViewModel);
         }

        public ViewResult EditComment(int commentId)
        {
            Comment comment = repositoryComment.Comments.FirstOrDefault(x=>x.CommentID==commentId);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            return View(comment);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
             
                    repositoryComment.SaveComment(comment);
                    
                    return RedirectToAction("Comments", "Admin");
            }
            else
            {
                return View(comment);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, ContentManager")]
        public ActionResult DeleteComment(int[] resubmit)
        {
             if (resubmit != null)
               {
                foreach (var p in resubmit)
                {
                    var comment = repositoryComment.Comments.FirstOrDefault(x => x.CommentID == p);//GetProductByIDAsync(p);

                    
                    if (comment != null){
                        repositoryComment.DeleteComment(comment);
                    }

                }
                TempData["message"] = string.Format("Нежелательные комментарии были удалены");
                TempData["messageType"] = "warning-msg";
            }
            

            return RedirectToAction("Comments");
        }
        

         #endregion Comment


        #region Order
        public ActionResult Orders(string searchWord, GridSortOptions gridSortOptions, bool? isForSale, int? regionId, int? page)
        {

            int pageItemsCount = Constants.ADMIN_PAGE_SIZE;

            List<OrderViewModel> orders = new List<OrderViewModel>();
            var plist = repositoryOrder.Orders.ToList();
            //var regions = repositoryRegion.Regions.OrderBy(x => x.Sequence);
          //existRegions = existRegions.Distinct();



            foreach (var order in plist)
            {
                orders.Add(order.OrderToViewModel());
            }

            var pagedViewModel = new PagedViewModel<OrderViewModel>
            {
                ViewData = ViewData,
                Query = orders.AsQueryable(),//query, //repositoryProduct.Products, //repositoryProduct.Products,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "StartDate",
                Page = page,
                PageSize = (pageItemsCount == 0) ? Domain.Constants.ADMIN_PAGE_SIZE : pageItemsCount,
            };


            pagedViewModel
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.ProductNumber.Contains(searchWord))
                           .AddFilter("isForSale", isForSale, a=> (!isForSale.HasValue) ? a.IsForSale == true || a.IsForSale==false : ((isForSale==true) ? a.IsForSale==true : a.IsForSale==false) )
                           
            
                .Setup();
         
            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("OrderGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);
            //return View(repositoryProduct.Products);

        }


        public ActionResult EditOrder(int? orderId)
        {
            if (orderId == null)
            {
                return RedirectToAction("Orders", "Admin");
            }
          //  ViewBag.Users = repositoryUser.UsersInfo.ToList();

        //    IEnumerable<Category> categoryList = repositoryCategory.Categories.OrderBy(x => x.CategoryName);
            //IEnumerable<ProductImage> productImagesList = repositoryProductImages.ProductImages;
            Order order = repositoryOrder.Orders.FirstOrDefault(p => p.OrderID== orderId);

           // OrderViewModel orderViewModel = order.OrderToViewModel();
         //   productViewModel.Categories = categoryList;

            //List<ProductViewModel> products = new List<ProductViewModel>();
            //var plist = repositoryProduct.Products.ToList();
            //foreach (var product in plist)
            //{

            //    products.Add(product.ProductToViewModel());
            //}



            return View(order);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditOrder(Order order)
        {
            //int selectedCategory = productViewModel.SelectedCategoryID;
           // ViewBag.Users = repositoryUser.UsersInfo.ToList();
            if (ModelState.IsValid)
            {
                //orderViewModel.UpdateDate = DateTime.Now;
                    //Product product = repositoryProduct.Products.FirstOrDefault(x => x.ProductID == productViewModel.ProductID);
                    //Product productOriginal = repositoryProduct.GetProductOrigin(product);
                    repositoryOrder.SaveOrder(order);


                    return RedirectToAction("EditOrder", "Admin", new { orderId = order.OrderID});
                
            }
            else
            {
              //  orderViewModel.Categories = repositoryCategory.Categories.ToList();

                return View(order);
            }
        }



        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteOrder(int[] resubmit)
        {
            if (resubmit != null)
            {
                foreach (var p in resubmit)
                {
                    var order = repositoryOrder.Orders.FirstOrDefault(x => x.OrderID == p); //GetProductByIDAsync(p);


                    if (order != null)
                    {
                        repositoryOrder.DeleteOrder(order);
                    }

                }
                TempData["message"] = string.Format("Нежелательные заказы были удалены");
                TempData["messageType"] = "warning-msg";
            }
            return RedirectToAction("Orders");
        }
#endregion Order


        #region SeoAttribute
        public ActionResult SeoAttributes(string searchWord, GridSortOptions gridSortOptions, int? page)
        {
          

            var pagedViewModel = new PagedViewModel<SeoAttribute>
            {
                ViewData = ViewData,
                Query = repositorySeoAttribute.SeoAttributes,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "UpdateDate",
                Page = page,
                PageSize = Domain.Constants.ADMIN_PAGE_SIZE,
            }
                /*.AddFilter("searchWord", searchWord,
                           a =>
                           a.OrderNumber == sw
                               ||
                           a.Phone.Contains(searchWord) || a.UserName.Contains(searchWord) ||
                           a.UserAddress.Contains(searchWord))
                .AddFilter("startDate", Convert.ToDateTime(startDate), a => a.TransactionDate >= dStart)
                .AddFilter("endDate", Convert.ToDateTime(endDate), a => a.TransactionDate <= dEnd)
                .AddFilter("isActive", isActive, a => a.IsActive == isActive)*/
    .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("SeoAttributesGridPartial", pagedViewModel);
            }
            return View(pagedViewModel);
           
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateSeoAttributes(string seoAttributesId = null)
        {
            if (seoAttributesId == null)
            {
                SeoAttribute seoAttribute = new SeoAttribute()
                {
                    TagID = "",
                    Keywords = "",
                    Robots = "",
                    Snippet = "",
                    Tag = "",
                    Tittle = ""
                };

                //seoAttribute.ArticleDate = DateTime.Now;
                return PartialView(seoAttribute);
            }
            else
            {
                SeoAttribute seoAttributes= repositorySeoAttribute.SeoAttributes.FirstOrDefault(x => x.TagID== seoAttributesId);
                return PartialView(seoAttributes);
            }

        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateSeoAttributes(SeoAttribute seoAttribute)
        {
            var user = repositoryUser.UsersInfo.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (user!=null)
            {
                seoAttribute.UserID = user.UserID;
            }
            if (ModelState.IsValid)
            {
                var seoAttributesExist = repositorySeoAttribute.SeoAttributes.FirstOrDefault(x => x.TagID == seoAttribute.TagID);

                if (seoAttributesExist==null)
                {
                    seoAttribute.CreateDate = seoAttribute.UpdateDate = DateTime.Now;
                    repositorySeoAttribute.SaveSeoAttributes(seoAttribute);    
                }
                else
                {
                   seoAttributesExist.UpdateDate=DateTime.Now;
                    seoAttributesExist.Keywords = seoAttribute.Keywords;
                    seoAttributesExist.Robots = seoAttribute.Robots;
                    seoAttributesExist.Snippet= seoAttribute.Snippet;
                    seoAttributesExist.Tittle= seoAttribute.Tittle;
                    seoAttributesExist.UserID = seoAttribute.UserID;
                    repositorySeoAttribute.SaveSeoAttributes(seoAttributesExist);
                }

                
                return View("SeoAttributes");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("CreateSeoAttributes", seoAttribute);
                }
                return RedirectToAction("SeoAttributes");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteSeoAttribute(string SeoAttributeID = null)
        {
            if (SeoAttributeID == null)
            {
                RedirectToAction("SeoAttributes");
            }
            SeoAttribute seoAttribute = repositorySeoAttribute.SeoAttributes.FirstOrDefault(x => x.TagID== SeoAttributeID);
            if (seoAttribute == null)
            {
                return RedirectToAction("SeoAttributes");
            }
            //if (seoAttribute.SeoAttributeID == 0)
            //{
            //    return RedirectToAction("SeoAttributes");
            //}
            repositorySeoAttribute.DeleteSeoAttributes(seoAttribute);

            return RedirectToAction("SeoAttributes");
        }
#endregion SeoAttribute

        #region Article

        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult ArticleList(string searchWord, GridSortOptions gridSortOptions, int? page)
        {
            var pagedViewModel = new PagedViewModel<SeoAttribute>
            {
                ViewData = ViewData,
                Query = repositorySeoAttribute.SeoAttributes,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "CreateDate",
                Page = page,
                PageSize = Domain.Constants.ADMIN_PAGE_SIZE,
            }
                /*.AddFilter("searchWord", searchWord,
                           a =>
                           a.OrderNumber == sw
                               ||
                           a.Phone.Contains(searchWord) || a.UserName.Contains(searchWord) ||
                           a.UserAddress.Contains(searchWord))
                .AddFilter("startDate", Convert.ToDateTime(startDate), a => a.TransactionDate >= dStart)
                .AddFilter("endDate", Convert.ToDateTime(endDate), a => a.TransactionDate <= dEnd)
                .AddFilter("isActive", isActive, a => a.IsActive == isActive)*/
    .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("ArticleListGridPartial", pagedViewModel);
            }

            return View(pagedViewModel);

            /*IEnumerable<NewsTape> allNews = repositoryNewsTape.NewsTapes.OrderByDescending(x => x.NewsDate);
            return PartialView(allNews);*/
        }

        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult CreateArticle(string tagId = null)
        {
            if (tagId == null)
            {
                var user = repositoryUser.UsersInfo.FirstOrDefault(x => x.Login == User.Identity.Name);

                SeoAttribute article = new SeoAttribute();

               
                article.CreateDate = DateTime.Now;
                article.ArticleText =
                    "<h1></h1><div class='page-content'><p><a class='modal' href='upload-files/i.jpg'><img style='margin-left: 10px; margin-right: 10px; float: right;' src='~/Content/images/' alt='' width='200' height='301' /></a></p><p></p><p></p></div>";
                return PartialView(article);
            }
            else
            {
                SeoAttribute article = repositorySeoAttribute.SeoAttributes.FirstOrDefault(x => x.TagID == tagId);
                return PartialView(article);
            }

        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult CreateArticle(SeoAttribute seoAttribute)
        {
            if (ModelState.IsValid)
            {
                seoAttribute.UpdateDate = DateTime.Now;
                var firstOrDefault = repositoryUser.UsersInfo.FirstOrDefault(x => x.Login == User.Identity.Name);
                if (firstOrDefault != null)
                    seoAttribute.UserID = firstOrDefault.UserID;
                repositorySeoAttribute.SaveSeoAttributes(seoAttribute);
                return View("ArticleList");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateArticle", seoAttribute);
            }
            return RedirectToAction("ArticleList");
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteArticle(string tagId=null)
        {
            if (tagId == null)
            {
                RedirectToAction("ArticleList");
            }
            SeoAttribute article = repositorySeoAttribute.SeoAttributes.FirstOrDefault(x => x.TagID == tagId);
            if (article == null)
            {
                return RedirectToAction("ArticleList");
            }
            //if (article.ArticleID == 0)
            //{
            //    return RedirectToAction("ArticleList");
            //}
            repositorySeoAttribute.DeleteSeoAttributes(article);
            
            return RedirectToAction("ArticleList");
        }


       



        #endregion

         #region Settings

              //public ActionResult DimSettings()
              //  {
              //   //   IEnumerable<DimSettingType> ds = repositoryDimSettingType.DimSettingTypes;
              //      return PartialView();
              //  }



        public ActionResult MailSettings()
        {
            var settings = repositoryMailingSetting.MailSettingses.ToList();
            return View(settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult MailSettings(MailSettings setting)
        {
            MailSettings ds = repositoryMailingSetting.MailSettingses.Single(x => x.MailSettingsID == setting.MailSettingsID);
            ds.SettingsValue = setting.SettingsValue;
            repositoryMailingSetting.SaveDimSetting(ds, false);
            return RedirectToAction("MailSettings");
        }


        public void ResetEmailOptions()
        {
             var mailSattings = new List<MailSettings>
                {
                    new MailSettings() {MailSettingsID = "MAIL_TO_ADDRESS", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Email адресата", SettingsValue = Constants.MAIL_TO_ADDRESS},
                    new MailSettings() {MailSettingsID = "MAIL_FROM_ADDRESS", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Email сервера отправки", SettingsValue = Constants.MAIL_FROM_ADDRESS},
                    new MailSettings() {MailSettingsID = "MAIL_USE_SSL", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Использовать SSL", SettingsValue = Constants.USE_SSL.ToString()},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_USER_NAME", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Логин почты", SettingsValue = Constants.USERNAME},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_PASSWORD", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Пароль", SettingsValue = Constants.PASSWORD},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_NAME", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Имя сервера", SettingsValue = Constants.SERVERNAME},
                    new MailSettings() {MailSettingsID = "MAIL_WRITE_AS_FILE", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Записывать как файл", SettingsValue = Constants.WRITE_AS_FILE.ToString()},
                    new MailSettings() {MailSettingsID = "MAIL_FILE_LOCATION", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Местонахождение файла", SettingsValue = Constants.FILE_LOCATION},
                    new MailSettings() {MailSettingsID = "MAIL_SERVER_PORT", SettingsType = "ADMIN_EMAIL_SETTINGS", SettingsDesc = "Порт", SettingsValue = Constants.SERVER_PORT.ToString()},
                    
                };

             foreach (var mailSetting in repositoryMailingSetting.MailSettingses)
             {
                 repositoryMailingSetting.DeleteMailSettings(mailSetting);
             }

            foreach (var mailSetting in mailSattings)
            {
                repositoryMailingSetting.SaveDimSetting(mailSetting, true);
            }



        }

        #endregion Settings

        #region RegionType

        public ActionResult RegionTypes(string searchWord, GridSortOptions gridSortOptions, int? page)
        {


            var pagedViewModel = new PagedViewModel<RegionType>
            {
                ViewData = ViewData,
                Query = repositoryRegionType.RegionTypes,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "RegionTypeID",
                Page = page,
                PageSize = Domain.Constants.ADMIN_PAGE_SIZE,
            }
                /*.AddFilter("searchWord", searchWord,
                           a =>
                           a.OrderNumber == sw
                               ||
                           a.Phone.Contains(searchWord) || a.UserName.Contains(searchWord) ||
                           a.UserAddress.Contains(searchWord))
                .AddFilter("startDate", Convert.ToDateTime(startDate), a => a.TransactionDate >= dStart)
                .AddFilter("endDate", Convert.ToDateTime(endDate), a => a.TransactionDate <= dEnd)
                .AddFilter("isActive", isActive, a => a.IsActive == isActive)*/
    .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("RegionTypesGridPartial", pagedViewModel);
            }
            return View(pagedViewModel);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateRegionType(int? regionTypeId)
        {
            if (regionTypeId == null || regionTypeId == 0)
            {
                RegionType regionType = new RegionType()
                {

                };

                //seoAttribute.ArticleDate = DateTime.Now;
                return PartialView(regionType);
            }
            else
            {
                RegionType regionType = repositoryRegionType.RegionTypes.FirstOrDefault(x => x.RegionTypeID == regionTypeId);
                return PartialView(regionType);
            }

        }


        [HttpPost]
        //  [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRegionType(RegionType regionType)
        {
            if (ModelState.IsValid)
            {

                repositoryRegionType.SaveRegionType(regionType);


                return View("RegionTypes");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("CreateRegionType", regionType);
                }
                return RedirectToAction("RegionTypes");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteRegionType(int? RegionTypeID = null)
        {
            if (RegionTypeID == null)
            {
                RedirectToAction("RegionTypes");
            }
            var regionType = repositoryRegionType.RegionTypes.FirstOrDefault(x => x.RegionTypeID == RegionTypeID);
            if (regionType == null)
            {
                return RedirectToAction("RegionTypes");
            }
            //if (seoAttribute.SeoAttributeID == 0)
            //{
            //    return RedirectToAction("SeoAttributes");
            //}
            repositoryRegionType.DeleteRegionType(regionType);

            return RedirectToAction("RegionTypes");
        }


        #endregion RegionType




        #region Region

        public ActionResult Regions(string searchWord, GridSortOptions gridSortOptions, int? regionTypeId, int? page)
        {
            

            //var regionList = from p in repositoryRegion.Regions 
            //                select new RegionViewModel
            //                {
            //                  RegionID  = p.RegionID,
            //                  //RegionFullName = p.RegionName,
            //                  RegionName = p.RegionName,
            //                  IsActive = p.IsActive,
            //                  RegionNumber = p.RegionNumber,
            //                  RegionTypeID = p.RegionTypeID,
            //                  Sequence = p.Sequence,
            //                  //RegionTypeDesc = p.RegionType.RegionTypeDesc,
            //                 // RegionTypes = types
                              
            //                };

           
            //var rtlist = repositoryRegionType.RegionTypes;
            //var regions = repositoryRegion.Regions.OrderBy(x => x.Sequence);
            var existRegions = repositoryRegionType.RegionTypes;
            //var existRegions = (from p in rtlist
            //                    join r in regions on p.RegionTypeID equals r.RegionTypeID
            //                    select new RegionViewModel()
            //                    {
            //                        RegionID = r.RegionID,
            //                        RegionName = r.RegionName,
            //                        RegionNumber = r.RegionNumber,
            //                        Sequence = r.Sequence,
            //                        RegionFullName = r.RegionNumber + ": " + r.RegionName,
            //                        IsActive = r.IsActive,
            //                        //RegionTypeDesc = p.RegionTypeDesc,
            //                        RegionTypeID = r.RegionTypeID
                                    
            //                    }); //.GroupBy(x => x.RegionFullName).Select(m => m.First()).OrderByDescending(x => x.Sequence).ThenBy(l => l.RegionFullName);
            var pagedViewModel = new PagedViewModel<Region>
            {
                ViewData = ViewData,
                Query = repositoryRegion.Regions,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "RegionID",
                Page = page,
                PageSize = Domain.Constants.ADMIN_PAGE_SIZE,
            }
                .AddFilter("searchWord", searchWord,
                           a =>
                           a.RegionName.ToUpper().Contains(searchWord.ToUpper()) || a.RegionNumber.ToUpper().Contains(searchWord.ToUpper()))
                //.AddFilter("startDate", Convert.ToDateTime(startDate), a => a.TransactionDate >= dStart)
                //.AddFilter("endDate", Convert.ToDateTime(endDate), a => a.TransactionDate <= dEnd)
                //.AddFilter("isActive", isActive, a => a.IsActive == isActive)

                 .AddFilter("regionTypeId", regionTypeId, a => a.RegionTypeID == regionTypeId,
                            existRegions.AsQueryable(), "RegionTypeDesc")
    .Setup();

            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("RegionsGridPartial", pagedViewModel);
            }
            return View(pagedViewModel);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateRegion(int? regionId)
        //{
        //    if (regionId == null || regionId == 0)
        //    {
        //        Region region = new Region()
        //        {

        //        };

        //        //seoAttribute.ArticleDate = DateTime.Now;
        //        return PartialView(region);
        //    }
        //    else
        //    {
        //        Region region = repositoryRegion.Regions.FirstOrDefault(x => x.RegionID == regionId);
        //        return PartialView(region);
        //    }

        {
            if (regionId == null || regionId == 0)
            {
                RegionViewModel region = new RegionViewModel()
                {
                    RegionTypes = repositoryRegionType.RegionTypes.ToList()
                };

                //seoAttribute.ArticleDate = DateTime.Now;
                return PartialView(region);
            }
            else
            {
                Region p =  repositoryRegion.Regions.First(x => x.RegionID == regionId);

                RegionViewModel region = new RegionViewModel()
                {
                    RegionID = p.RegionID,
                    //RegionFullName = p.RegionName,
                    RegionName = p.RegionName,
                    IsActive = p.IsActive,
                    RegionNumber = p.RegionNumber,
                    RegionTypeID = p.RegionTypeID,
                    Sequence = p.Sequence,
                    RegionTypeDesc = p.RegionType.RegionTypeDesc,
                    RegionTypes = repositoryRegionType.RegionTypes.ToList()
                };
                    
                    
               
                return PartialView(region);
            }


        }


        [HttpPost]
        //  [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRegion(RegionViewModel regionViewModel)
        {
            if (ModelState.IsValid)
            {
                Region region = new Region()
                {
                    RegionID = regionViewModel.RegionID,
                    IsActive = regionViewModel.IsActive,
                    RegionName = regionViewModel.RegionName,
                    //RegionType = regionViewModel.RegionType,
                    RegionNumber = regionViewModel.RegionNumber,
                    RegionTypeID = regionViewModel.RegionTypeID,
                    Sequence = regionViewModel.Sequence
                };

                repositoryRegion.SaveRegion(region);


                return View("Regions");
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("CreateRegion", regionViewModel);
                }
                return RedirectToAction("Regions");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin, ContentManager")]
        public ActionResult DeleteRegion(int? RegionID = null)
        {
            if (RegionID == null)
            {
                RedirectToAction("Regions");
            }
            var region = repositoryRegion.Regions.FirstOrDefault(x => x.RegionID == RegionID);
            if (region == null)
            {
                return RedirectToAction("Regions");
            }
            //if (seoAttribute.SeoAttributeID == 0)
            //{
            //    return RedirectToAction("SeoAttributes");
            //}
            repositoryRegion.DeleteRegion(region);

            return RedirectToAction("Regions");
        }


        #endregion Region

        #region Galery
        public ActionResult Galery()
        {
            if (Request.IsAjaxRequest())
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                return PartialView("GaleryPartial", GaleryImageList());
            }

            /* var files = from f in System.IO.Directory.GetFiles(
                                    Server.MapPath("~/Content/Galery/"),
                                    "*.*",
                                    SearchOption.TopDirectoryOnly)
                         select System.IO.Path.GetFileName(f);*/

            return View(GaleryImageList());



            /*string strSaveFileName = productId.ToString() + "_" + productImage.ProductImageID.ToString() + productImage.ImageExt;
              
            string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                            Constants.GALERY_IMAGES_FOLDER, strSaveFileName);
                            string strSavePreviewFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                                   Constants.PRODUCT_IMAGE_FOLDER,
                                                                                   Constants.PRODUCT_IMAGE_PREVIEW_FOLDER,
                                                                                   strSaveFileName);
                            if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);
                            if (System.IO.File.Exists(strSavePreviewFullPath)) System.IO.File.Delete(strSavePreviewFullPath);
            */
            //      files3 = files3;
            //    return View(files);

        }


        //[HttpPost]
        //public ActionResult UploadInGalery(HttpPostedFileBase imagefile)
        //{
        //    // Получаем объект, для которого загружаем картинку
        //    if (imagefile == null)
        //    {
        //        TempData["message"] = string.Format("Изображение не было загружено");
        //        TempData["messageType"] = "error-msg";
        //        return RedirectToAction("Galery");
        //    }

        //    try
        //    {
        //        // Определяем название конечного графического файла вместе с полным путём.
        //        // Название файла должно быть такое же, как ID объекта. Это гарантирует уникальность названия.
        //        // Расширение должно быть такое же, как расширение у исходного графического файла.
        //        string strExtension = System.IO.Path.GetExtension(imagefile.FileName);
        //        string strSaveFileName = imagefile.FileName; //+ strExtension;
        //        string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
        //                                                        Constants.GALERY_IMAGES_FOLDER,
        //                                                        strSaveFileName);

        //        // Если файл с таким названием имеется, удаляем его.
        //        if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);

        //        // Сохраняем картинку, изменив её размеры.
        //        imagefile.ResizeAndSave(Constants.GALERY_IMAGES_HEIGHT, Constants.GALERY_IMAGES_WIDTH,
        //                                strSaveFullPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        string strErrorMessage = ex.Message;
        //        if (ex.InnerException != null)
        //            strErrorMessage = string.Format("{0} --- {1}", strErrorMessage, ex.InnerException.Message);
        //        ViewBag.ErrorMessage = strErrorMessage;
        //        TempData["message"] = string.Format("Изображение не было загружено");
        //        TempData["messageType"] = "information-msg";
        //    }
        //    return RedirectToAction("Galery");
        //}


        // [HttpPost]
        //public ActionResult UploadManyInGalery(IEnumerable<HttpPostedFileBase> imagefiles)
        //{
        //    IEnumerable<HttpPostedFileBase> imagefiles2 = imagefiles.Where(x => x != null);
        //    if (imagefiles.Count() == 0)
        //    {
        //        TempData["message"] = string.Format("Изображение не было загружено");
        //        TempData["messageType"] = "information-msg";
        //        return RedirectToAction("Galery");
        //    }

        //    // Получаем объект, для которого загружаем картинку
        //    foreach (var imagefile in imagefiles2)
        //    {
        //        try
        //        {
        //            // Определяем название конечного графического файла вместе с полным путём.
        //            // Название файла должно быть такое же, как ID объекта. Это гарантирует уникальность названия.
        //            // Расширение должно быть такое же, как расширение у исходного графического файла.
        //            string strExtension = System.IO.Path.GetExtension(imagefile.FileName);
        //            string strSaveFileName = imagefile.FileName; //+ strExtension;
        //            string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
        //                                                            Constants.GALERY_IMAGES_FOLDER,
        //                                                            strSaveFileName);

        //            // Если файл с таким названием имеется, удаляем его.
        //            if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);

        //            // Сохраняем картинку, изменив её размеры.
        //            imagefile.ResizeAndSave(Constants.GALERY_IMAGES_HEIGHT, Constants.GALERY_IMAGES_WIDTH,
        //                                    strSaveFullPath);

        //            if (Request.IsAjaxRequest())
        //            {
        //                return PartialView("GaleryPartial", GaleryImageList());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string strErrorMessage = ex.Message;
        //            if (ex.InnerException != null)
        //                strErrorMessage = string.Format("{0} --- {1}", strErrorMessage, ex.InnerException.Message);
        //            ViewBag.ErrorMessage = strErrorMessage;
        //            TempData["message"] = string.Format("Изображение не было загружено");
        //            TempData["messageType"] = "information-msg";
        //        }
        //    }

        //    return RedirectToAction("Galery");

        //}


        public ActionResult UploadPhoto()
        {
            return View();
        }

        public ActionResult Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                string strExtension = System.IO.Path.GetExtension(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                     Constants.GALERY_IMAGES_FOLDER);

                string strSaveFileName = "Image-" + System.Guid.NewGuid().ToString("N") + strExtension;

                string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                Constants.GALERY_IMAGES_FOLDER,
                                                                strSaveFileName);

               
                // Если файл с таким названием имеется, удаляем его.
                if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);
                //if (System.IO.File.Exists(strSavePreviewFullPath)) System.IO.File.Delete(strSavePreviewFullPath);
                file.ResizeImage(Constants.PRODUCT_IMAGE_HEIGHT, Constants.PRODUCT_IMAGE_WIDTH, strSaveFullPath);
                //file.ResizeImage(Constants.PRODUCT_IMAGE_PREVIEW_HEIGHT, Constants.PRODUCT_IMAGE_PREVIEW_WIDTH, strSavePreviewFullPath);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeleteGaleryImage(string filename)
        {
            try
            {
                string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                   Constants.GALERY_IMAGES_FOLDER,
                                                                   filename);

                // Если файл с таким названием имеется, удаляем его.
                if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Delete(strSaveFullPath);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("GaleryPartial", GaleryImageList());
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Galery");
        }

        [HttpPost]
        public ActionResult GaleryRenameImage(string oldfilename, string newfilename)
        {
            try
            {
                string strFolderPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                   Constants.GALERY_IMAGES_FOLDER);

                string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                   Constants.GALERY_IMAGES_FOLDER,
                                                                   oldfilename);

                string extension = oldfilename.Substring(oldfilename.LastIndexOf('.'));

                // Если файл с таким названием имеется, удаляем его.
                if (System.IO.File.Exists(strSaveFullPath)) System.IO.File.Move(strFolderPath + '/' + oldfilename, strFolderPath + '/' + newfilename + extension);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("GaleryPartial", GaleryImageList());
                }
            }
            catch (Exception)
            {
                TempData["message"] = string.Format("Вероятно, что файл с таким именем уже существует");
                TempData["messageType"] = "error-msg";
                return RedirectToAction("Galery");
            }
            return RedirectToAction("Galery");
        }

        public IEnumerable<string> GaleryImageList()
        {
            var files = from f in System.IO.Directory.GetFiles(
                                   Server.MapPath("~/Content/galery/"),
                                   "*.*",
                                   SearchOption.TopDirectoryOnly)
                        select System.IO.Path.GetFileName(f);
            return files.OrderBy(x => x.ToString()).ToList();
        }
        #endregion Galery
    }

}
