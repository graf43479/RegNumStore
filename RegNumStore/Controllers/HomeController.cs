using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Domain;
using Domain.Abstract;
using Domain.Entities;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RegnumStore.Extensions;
using RegNumStore.Models;
using RegnumStore.Models;


namespace RegnumStore.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private IUserRepository userRepository;
        private IRegionRepository regionRepository;
        private IOrderRepository orderRepository;
        private IArticleRepository articleRepository;
        private ICommentRepository commentRepository;
        private ISeoAttributeRepository seoAttributeRepository;

        //private ISeoArticleBaseRepository seoArticleBaseRepository;
        //private ISeoRepository seoRepository;
        //private IArticleDerivedRepository articleDerivedRepository;
 

        private IDeliveryProcessor deliveryProcessor;

        public HomeController(
            IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            IUserRepository userRepository, 
            IRegionRepository regionRepository,  
            IOrderRepository orderRepository,
            IArticleRepository articleRepository, 
            ICommentRepository commentRepository, 
            ISeoAttributeRepository seoAttributeRepository,

            //ISeoArticleBaseRepository seoArticleBaseRepository,
            //ISeoRepository seoRepository,
            //IArticleDerivedRepository articleDerivedRepository,

            IDeliveryProcessor deliveryProcessor)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.regionRepository = regionRepository;
            this.orderRepository = orderRepository;
            this.commentRepository = commentRepository;
            this.deliveryProcessor = deliveryProcessor;
            this.articleRepository = articleRepository;
            this.seoAttributeRepository = seoAttributeRepository;

            //this.seoArticleBaseRepository = seoArticleBaseRepository;
            //this.seoRepository = seoRepository;
            //this.articleDerivedRepository = articleDerivedRepository;
        }

        public async Task<ActionResult> Index()
        {
            string header = "Index";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            //throw new HttpException(404, "облом");
            
         //   ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            string header = "About";
            TempData["nav-message"] = header;
            GetSeoHeaders(header);

            //Article article = articleRepository.Articles.AsNoTracking().FirstOrDefault(x => x.ArticleID == 1);
            ArticleViewModel article = GetArticleByTag(header);
            return View(article);
        }


      


        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> Contact()
        {
            
            string header = "Contact";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);

            return View(new Message());
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Contact(Message message)
      //  public ActionResult Contact(string Name, string Email, string Text)
        {
          
            TempData["nav-message"] = "Contact";
            if (ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    try
                    {
                        deliveryProcessor.FeedBackRequest(message);
                        return Content("true");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Возникла ошибка при отправке сообщения!");
                        return PartialView("ContactPartialView", message);   
                    }

                }
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Некорректно заполнение полей!");
                return PartialView("ContactPartialView",message);    
            }
            
        }

        public async Task<ActionResult> Price()
        {
            string header = "Price";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            ArticleViewModel article = GetArticleByTag(header);
            return View(article);
            //return View();
        }

        public async Task<ActionResult> Comments()
        {
            string header = "Comments";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            IEnumerable<Comment> comments = await commentRepository.Comments.Where(x=>x.IsAccept==true).OrderByDescending(x=>x.CreateDate).AsNoTracking().ToListAsync();

            return View(comments);
        }


        public async Task<ActionResult> QuickSale()
        {
            string header = "QuickSale";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);

            OrderViewModel viewModel = new OrderViewModel
            {
                Regions = regionRepository.Regions.OrderBy(x => x.Sequence).AsNoTracking(),
                Categories = categoryRepository.Categories.AsNoTracking()
            };
            //if (User.Identity.IsAuthenticated)
            //{
            //    RedirectToAction("EditRegNum", "Account");
            //}

            return View(viewModel);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> QuickSale(OrderViewModel viewModel)
        {
            viewModel.StartDate = DateTime.Now;

            // viewModel.ProductNumber = viewModel.Num1 + viewModel.Num2 + viewModel.Num3 + viewModel.Num4 + viewModel.Num5 +
            //         viewModel.Num6 + viewModel.Num7;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
                string m1 = "";
                foreach (var error in errors)
                {
                    m1 = m1 + error;
                }
                ModelState.AddModelError("", m1);
                // ModelState.AddModelError("", "Некорректно заполнение полей!");
                viewModel.Regions = regionRepository.Regions.OrderBy(x => x.Sequence);
                viewModel.Categories = categoryRepository.Categories;
                return View(viewModel);
            }
            string number = viewModel.ProductNumber;


            if (!number.Contains("*") && productRepository.Products.AsNoTracking().Any(x => x.ProductNumber == number))
            {
                ModelState.AddModelError("", "В базе уже существует номер " + number);
                return View(viewModel);
            }

            //if (number.Split('*').Count() > 2)
            //{
            //    ModelState.AddModelError("", "В номере содержится слишком много неопределенности");
            //    return View(viewModel);
            //}
           

                Order order = new Order()
                {

                    StartDate = DateTime.Now,
                    Comment = viewModel.Comment,
                    Email = viewModel.Email,
                    Name = viewModel.Name,
                    Phone = viewModel.Phone,
                    RegionID =
                        regionRepository.Regions.FirstOrDefault(x => x.RegionNumber == viewModel.Num7)
                                   .RegionID,
                    ProductNumber = number,
                    IsForSale = true
                };

                //CarNumber carNumber = new CarNumber()
                //{
                //    Num1 = viewModel.Num1,
                //    Num2 = viewModel.Num2,
                //    Num3 = viewModel.Num3,
                //    Num4 = viewModel.Num4,
                //    Num5 = viewModel.Num5,
                //    Num6 = viewModel.Num6,
                //    Num7 = viewModel.Num7
                //};

                //CarNumber carNum = order.ProductNumber.NumberToCarNumber();
                await orderRepository.SaveOrderAsync(order);
            deliveryProcessor.OrderQuickSale(order, "quickSale");

                
                //Переопределить метод 
                // Response.Redirect("~/account/RegNumImage/?productId=100");
                // RegNumImage(product.ProductID, carNumber);


                //else
                //{
                //    Product product =
                //        dataManager.Products.Products.FirstOrDefault(x => x.ProductID == viewModel.ProductID);

                //    product.Price = viewModel.Price;
                //    product.IsOverbalanceIncluded = viewModel.IsOverbalanceIncluded;
                //    product.UpdateDate = DateTime.Now;
                //    dataManager.Products.SaveProduct(product);
                //}


                return View("Success");
           

          //  return View();
        }

        public async Task<ActionResult> Order()
        {
            string header = "Order";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            //if (User.Identity.IsAuthenticated)
            //{
            //    RedirectToAction("EditRegNum", "Account");
            //}
            OrderViewModel viewModel = new OrderViewModel
            {
                Regions = regionRepository.Regions.OrderBy(x => x.Sequence).AsNoTracking(),
                Categories = categoryRepository.Categories.AsNoTracking()
            };

            return View(viewModel);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Order(OrderViewModel viewModel)
        {
            viewModel.StartDate=DateTime.Now;
           
           // viewModel.ProductNumber = viewModel.Num1 + viewModel.Num2 + viewModel.Num3 + viewModel.Num4 + viewModel.Num5 +
                             //         viewModel.Num6 + viewModel.Num7;
            if (!ModelState.IsValid)
            {
               var errors =  ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
                string m1 = "";
                foreach (var error in errors)
                {
                    m1 = m1 + error;
                }
               ModelState.AddModelError("", m1);
              // ModelState.AddModelError("", "Некорректно заполнение полей!");
               viewModel.Regions = regionRepository.Regions.OrderBy(x => x.Sequence).AsNoTracking();
               viewModel.Categories = categoryRepository.Categories.AsNoTracking();
                return View(viewModel);  
            }
            string number = viewModel.ProductNumber;
            

            if (!number.Contains("*") && productRepository.Products.AsNoTracking().Any(x => x.ProductNumber == number))
            {
                ModelState.AddModelError("", "В базе уже существует номер " + number);
                return View(viewModel);
            }

            //if (number.Split('*').Count() > 2)
            //{
            //    ModelState.AddModelError("", "В номере содержится слишком много неопределенности");
            //    return View(viewModel);
            //}
            else
            {
                
                    Order order = new Order()
                    {
                        
                        StartDate = DateTime.Now,
                        Comment = viewModel.Comment,
                        Email = viewModel.Email,
                        Name = viewModel.Name,
                        Phone = viewModel.Phone,
                        RegionID =
                            regionRepository.Regions.FirstOrDefault(x => x.RegionNumber == viewModel.Num7)
                                       .RegionID,
                        ProductNumber = number,
                        IsForSale = false
                    };

                    //CarNumber carNumber = new CarNumber()
                    //{
                    //    Num1 = viewModel.Num1,
                    //    Num2 = viewModel.Num2,
                    //    Num3 = viewModel.Num3,
                    //    Num4 = viewModel.Num4,
                    //    Num5 = viewModel.Num5,
                    //    Num6 = viewModel.Num6,
                    //    Num7 = viewModel.Num7
                    //};

              //      CarNumber carNum = order.ProductNumber.NumberToCarNumber();
                    await orderRepository.SaveOrderAsync(order);
                deliveryProcessor.OrderQuickSale(order, "order");
                    //Переопределить метод 
                   // Response.Redirect("~/account/RegNumImage/?productId=100");
                   // RegNumImage(product.ProductID, carNumber);

                
                //else
                //{
                //    Product product =
                //        dataManager.Products.Products.FirstOrDefault(x => x.ProductID == viewModel.ProductID);

                //    product.Price = viewModel.Price;
                //    product.IsOverbalanceIncluded = viewModel.IsOverbalanceIncluded;
                //    product.UpdateDate = DateTime.Now;
                //    dataManager.Products.SaveProduct(product);
                //}


                return View("Success");
            }

            return View();
        }

        //[System.Web.Mvc.HttpGet]
       // public ActionResult Portfolio(bool? ajax, string carNumber, string category2, int? minCost, int? maxCost, int page = 1) //(int? categoryId)
        public async Task<ActionResult> Portfolio(bool? ajax, string carNumber, string category2, int? minCost, int? maxCost, int page = 1) //(int? categoryId)
        {


          
            //TempData["url"] =
            //List<string> lst = new List<string>();
            TempData["urlCarNumber"]= HttpContext.Request.Params.Get("carNumber");
            TempData["urlCategory2"] = HttpContext.Request.Params.Get("category2");
            TempData["urlCarMinCost"] = HttpContext.Request.Params.Get("minCost");
            TempData["urlCarMaxCost"] = HttpContext.Request.Params.Get("maxCost");
            


            
            
            CarNumber car= new CarNumber();
          
            if (carNumber!=null)
            {
                var jss = new JavaScriptSerializer();
                 car = jss.Deserialize<CarNumber>(carNumber);
            }
            
            string num = car.Num1 + car.Num2 + car.Num3 + car.Num4 + car.Num5 + car.Num6 + car.Num7;
            TempData["SerializedNum"] = car;
            

            int pageSize = Constants.PRODUCT_PAGE_SIZE;

            //IEnumerable<Product> products = productRepository.Products.ToList();


            IEnumerable<Product> products = await productRepository.Products.AsNoTracking().ToListAsync();
           
            
           
            
            if (products.Any())
            {
                ViewBag.MinVal = (int)products.Min(x => x.Price);
                ViewBag.MaxVal = (int)products.Max(x => x.Price);    
            }


            if (category2 != null && String.IsNullOrWhiteSpace(category2) != true)
            {
                
                    var jss = new JavaScriptSerializer();
                    var st = jss.Deserialize<string[]>(category2);
                    if (st.Any())
                    {
                    IEnumerable<Category> categories = from c in categoryRepository.Categories.AsNoTracking().ToList()
                                                       join s in st on c.ShortName equals s
                                                       select c;
                    TempData["categoryList"] = categories;

                        products = categories.SelectMany(x => x.Products).Distinct();

                        var userList = userRepository.UsersInfo.ToList();

                        foreach (var product in products)
                        {
                            product.User = userList.FirstOrDefault(x => x.UserID == product.UserID);
                        }
                        //from p in products
                        //       where p.Categories == categories
                        //       //join c in categories on p.CategoryID equals c.CategoryID
                        //       select p;    
                    }
            }
            else
            {
                TempData["categoryList"] = null;
            }

            if (minCost!=null)
            {
                products = products.Where(x => x.Price >= minCost && x.Price <= maxCost);
            }



           
            if (!String.IsNullOrEmpty(num))
            {
                //List<string> tmp = new List<string>();
                //foreach (var p in products.ToList())
                //{
                //    tmp.Add(p.ProductNumber.Remove(0, p.ProductNumber.Length - 3));
                //}

                num = num.Replace("*", ".");
               

                if (car.Num7.Length==2)
                {
                    products = from p in products
                               let l = p.ProductNumber.Substring(p.ProductNumber.Length - 3, 1).ToUpper()
                               where l == "А" || l == "В" || l == "С" || l == "Е" || l == "Н" || l == "К" || l == "М" || l == "О" || l == "Р" || l == "Т" || l == "Х" || l == "У"     
                        select p;
                    //АВСЕНКМОРТХУ
                    //products = products.TakeWhile(x => x.ProductNumber.Substring(x.ProductNumber.Length - 3, 1) == "А"
                        //products = products.Where(x => x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "А" + car.Num7 
                      //                             ||  x.ProductNumber.Substring(x.ProductNumber.Length - 3, 1) == "С"); 
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "С" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Е" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Н" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "К" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "М" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "О" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Р" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Т" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Х" + car.Num7
                    //   | x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "У" + car.Num7);

                    //products = products.Where(x => x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "А" + car.Num7 
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "В" + car.Num7 
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "С" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Е" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Н" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "К" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "М" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "О" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Р" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Т" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "Х" + car.Num7
                    //    || x.ProductNumber.Remove(0, x.ProductNumber.Length - 3) == "У" + car.Num7);
                }
                //else if (car.Num7 == "*")
                //{
                //    products = from p in products
                //        let l = p.ProductNumber.Substring(p.ProductNumber.Length - 3, 1).ToUpper()
                //        where
                //            l == "А" || l == "В" || l == "С" || l == "Е" || l == "Н" || l == "К" || l == "М" || l == "О" ||
                //            l == "Р" || l == "Т" || l == "Х" || l == "У"
                //        select p;
                //}
                //АВСЕНКМОРТХУ

                Regex regex = new Regex(num.ToUpper());
                Regex regex2;
                Regex regex3;
                if (car.Num7=="*")
                {
                    regex2 = new Regex(num.ToUpper() + ".");
                    regex3 = new Regex(num.ToUpper() + "..");

                    products = from p in products
                               let l = p.ProductNumber.Substring(6, ((p.ProductNumber.Length == 8) ? 2 : 3)).ToUpper()
                               where ((l.Length == 2) ? regex2.IsMatch(p.ProductNumber.ToUpper()) : regex3.IsMatch(p.ProductNumber.ToUpper()))
                               select p;

                }
                else
                {
                    products = from p in products
                               //let l = p.ProductNumber.Substring(6, ((p.ProductNumber.Length == 8) ? 2 : 3)).ToUpper()
                               //where ((l.Length == 2) ? regex.IsMatch(p.ProductNumber.ToUpper()) : regex2.IsMatch(p.ProductNumber.ToUpper()))
                               where regex.IsMatch(p.ProductNumber.ToUpper()) 
                               select p;
                }

                //= new Regex(num.Substring(6, num.Length).ToUpper());
                //products = products.Where(x => regex.IsMatch(x.ProductNumber.ToUpper()));

                //products = from p in products
                //   let l = p.ProductNumber.Substring(6, ((p.ProductNumber.Length == 8) ? 2 : 3)).ToUpper()
                //    where ((l.Length==2) ?  regex.IsMatch(p.ProductNumber.ToUpper()) : regex2.IsMatch(p.ProductNumber.ToUpper()) )
                //    select p;


            }

       
            ProductPagedListViewModel viewModel = new ProductPagedListViewModel
                {
                    PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = pageSize,
                            TotalItems = products.Count()
                        },
                    Products =  products.OrderByDescending(p => p.StartDate).Skip((page - 1) * pageSize).Take(pageSize)
                };
            
                
            

            
            if (Request.IsAjaxRequest())
            {
                if (ajax==true)
                {
                    var jsonModel = new JsonModel();
                    jsonModel.NoMoreData = viewModel.Products.Count() < pageSize;
                    jsonModel.HTMLString = RenderPartialViewToString("PortfolioAjaxPartialView", viewModel);

                    return Json(jsonModel);    
                }
                else
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    return PartialView("PortfolioPartialView", viewModel);
                }
                
            }

            string header = "Portfolio";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            return View(viewModel);
            
        }

        public async Task<ActionResult> NumInfo(int productId)
        {
            Product product = await productRepository.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductID == productId);
            if (Request.IsAjaxRequest())
            {
               // Thread.Sleep(4000);
                return PartialView(product);    
            }
            string header = "NumInfo";
            TempData["nav-message"] = header;
            await GetSeoHeadersAsync(header);
            return PartialView(product);    
        }


        [System.Web.Mvc.HttpGet]
        public PartialViewResult SendComment()
        {
            //CommentViewModel viewModel = new CommentViewModel();
            return PartialView("SendCommentPartialView",new Comment(){AnswerText = "Ответ ещё не готов"});
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SendComment(Comment comment)
        {
            //Thread.Sleep(2000);
            if (ModelState.IsValid)
            {
                comment.IsAccept = false;
                comment.CreateDate = DateTime.Now;
                commentRepository.SaveComment(comment);

                
                //return Content("success");
                return Json(new {success = "true"});
                //return PartialView(new CommentViewModel());
            }
            else
            {
                var errors = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));

                return Json(new {success = "false", message = errors}); //Content(errors);
            }
            //CommentViewModel viewModel = new CommentViewModel();
           // return null;  //PartialView("SendCommentPartialView",viewModel);
        }


        public string GetUserPhoneNumber(int userId)
        {
          //  Thread.Sleep(4000);
            //return userRepository.UsersInfo.AsNoTracking().FirstOrDefault(x => x.UserID == userId).Phone;
            return userRepository.UsersInfo.AsNoTracking().FirstOrDefault(x => x.UserID == userId).Phone;
        }


        public async Task<ActionResult> SimilarNumbers(int productid)
        {
           // Thread.Sleep(4000);
            Product prod = await productRepository.Products.FirstOrDefaultAsync(x => x.ProductID == productid);

            var ct = prod.Categories;
            var m = ct.Select(x=>x.Products).ToList();

            List<Product> products = new List<Product>();

            foreach (var v in m)
            {
                products.AddRange(v);
            }

            products = products.Where(x=>x.ProductID!=productid).Distinct().Take(16).ToList();

            

            
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            
            return PartialView(products);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult ImageGalery()
        {
            IEnumerable<Product> imageList = productRepository.Products.Where(x => x.IsChoosen).AsNoTracking();
            List<string> list = new List<string>();
            foreach (var image in imageList)
            {
                list.Add("Content/img/Image" + image.ProductID + ".jpg");
            }
            return Json(list);    
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> GetNumAttributeList(string carNumber, string category2, int? minCost, int? maxCost)
        {
            if ( TempData["urlCategory2"] !=null)
            {
                category2 = (string)TempData["urlCategory2"];
            }
            
            CarNumber car = new CarNumber();
            if (carNumber != null)
            {
                var jss = new JavaScriptSerializer();
                car = jss.Deserialize<CarNumber>(carNumber);
            }

            string numOrigin = car.Num1 + car.Num2 + car.Num3 + car.Num4 + car.Num5 + car.Num6 + car.Num7;

            numOrigin = numOrigin.Replace("*", ".");

            
            Regex regex = new Regex(numOrigin.ToUpper());


            IEnumerable<Product> numList1 = await productRepository.Products.AsNoTracking().ToListAsync();



            if (car.Num7.Length == 2 )
            {
                numList1 = from z in numList1
                           let l = z.ProductNumber.Substring(z.ProductNumber.Length - 3, 1).ToUpper()
                           where
                               l == "А" || l == "В" || l == "С" || l == "Е" || l == "Н" || l == "К" || l == "М" || l == "О" ||
                               l == "Р" || l == "Т" || l == "Х" || l == "У"
                           select z;
            }

            else if (car.Num7 == "*")
            {
                Regex regex2;
                Regex regex3;
                regex2 = new Regex(numOrigin.ToUpper() + ".");
                regex3 = new Regex(numOrigin.ToUpper() + "..");

                numList1 = from p1 in numList1
                           let l = p1.ProductNumber.Substring(6, ((p1.ProductNumber.Length == 8) ? 2 : 3)).ToUpper()
                           where ((l.Length == 2) ? regex2.IsMatch(p1.ProductNumber.ToUpper()) : regex3.IsMatch(p1.ProductNumber.ToUpper()))
                           select p1;
            }
            

            
          

            //-----------------------------------------
            if (numList1.Any())
            {
                ViewBag.MinVal = (int)numList1.Min(x => x.Price);
                ViewBag.MaxVal = (int)numList1.Max(x => x.Price);
            }


            if (category2 != null)
            {

                var jss = new JavaScriptSerializer();
                var st = jss.Deserialize<string[]>(category2);
                if (st.Any())
                {
                    IEnumerable<Category> categories = from c in categoryRepository.Categories.AsNoTracking().ToList()
                                                       join s in st on c.ShortName equals s
                                                       select c;
                    numList1 = categories.SelectMany(x => x.Products).Distinct();
                    //from p in products
                    //       where p.Categories == categories
                    //       //join c in categories on p.CategoryID equals c.CategoryID
                    //       select p;    
                }
            }

            if (minCost != null)
            {
                numList1 = numList1.Where(x => x.Price >= minCost && x.Price <= maxCost);
            }

            //-----------------------------------------
            
            
            //List<string> numList = new List<string>();
            //if (!String.IsNullOrEmpty(numOrigin))
            //{
            //    //products = from p in products
            //    //           where SqlMethods.Like(p.ProductNumber, "%") 
            //    //           select p;
            //    products = products.Where(x => regex.IsMatch(x.ProductNumber.ToUpper()));
            //}

            
            var numList = numList1.Where(x => regex.IsMatch(x.ProductNumber.ToUpper())).Select(x=>x.ProductNumber.ToUpper());

           
            //List<string> n1 = new List<string>();
            //foreach (string num in numList)
            //{
            //    n1.Add(num.Remove(1,num.Length-1));
            //}
            //n1 = n1.Distinct().ToList();

            
            //return Json(n1);

            List<string> n1 = new List<string>();
            List<string> n2 = new List<string>();
            List<string> n3 = new List<string>();
            List<string> n4 = new List<string>();
            List<string> n5 = new List<string>();
            List<string> n6 = new List<string>();
            List<string> n7 = new List<string>();
            foreach (string num in numList)
            {
                n1.Add(num.Remove(1, num.Length - 1));
                n2.Add(num.Substring(1, 1));
                n3.Add(num.Substring(2, 1));
                n4.Add(num.Substring(3, 1));
                n5.Add(num.Substring(4, 1));
                n6.Add(num.Substring(5, 1));
                n7.Add(num.Remove(0, 6));
            }
            n1 = n1.Distinct().Where(x=>x!="*").ToList();
            n2 = n2.Distinct().Where(x => x != "*").OrderBy(x => x).ToList();
            n3 = n3.Distinct().Where(x => x != "*").OrderBy(x => x).ToList();
            n4 = n4.Distinct().Where(x => x != "*").OrderBy(x => x).ToList();
            n5 = n5.Distinct().Where(x => x != "*").ToList();
            n6 = n6.Distinct().Where(x => x != "*").ToList();
            n7 = n7.Where(x => x != "*").OrderByDescending(x => x.Count()).Distinct().ToList();

            //foreach (var h in n7)
            //{
            //    if (h.Length==2)
            //    {
            //        //n7.Remove(h);
            //    }
            //}
            

            //n7 = from t in numList1
            //     let l = t.ProductNumber.Substring(t.ProductNumber.Length - 3, 1).ToUpper()
            //     where l == "А" || l == "В" || l == "С" || l == "Е" || l == "Н" || l == "К" || l == "М" || l == "О" || l == "Р" || l == "Т" || l == "Х" || l == "У"
            //     select t; 


            IEnumerable<List<string>> numArray = new List<string>[] { n1, n2, n3, n4, n5, n6, n7 };
            var p = new JavaScriptSerializer();
            
            //IEnumerable<List<string>> numArray = new List<string>[]{ n1, n2, n3, n4, n5, n6, n7 };
            //Thread.Sleep(4000);
            return Json(numArray.ToList());
           // return Json(p.Serialize(numArray));
        }



        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private void GetSeoHeaders(string header)
        {
            SimpleSeoViewModel seo = (from p in seoAttributeRepository.SeoAttributes
                where p.TagID == header
                select new SimpleSeoViewModel
                {
                    TagID = p.TagID,
                    Keywords = p.Keywords,
                    Snippet = p.Snippet,
                    Robots = p.Robots,
                    Tittle = p.Tittle
                }).AsNoTracking().FirstOrDefault();
                //seoAttributeRepository.SeoAttributes.AsNoTracking().FirstOrDefault(x => x.TagID == header).;
            ViewBag.SEO = seo;

           
            //SimpleSeoViewModel view = (from p in seoRepository.Seos
            //    where p.TagID == header
            //    select new SimpleSeoViewModel 
            //    {
            //        p.Keywords,
            //        p.Robots,
            //        p.Snippet,
            //        p.Tittle
            //    }

            //    ).AsNoTracking() ;

            //SimpleSeoViewModel view =
            //    seoRepository.Seos.AsNoTracking().FirstOrDefault(x => x.TagID == header).Select(x => new SimpleSeoViewModel
            //    {
            //        Keywords = x.Keywords,
            //        Tittle = x.Tittle,
            //        Robots = x.Robots,
            //        Snippet = x.Snippet
            //    }) as SimpleSeoViewModel;

            // IEnumerable<Seo> view2 = seoRepository.Seos.Select(x=>new Seo{TagID = x.TagID, Keywords = x.Keywords});


            //var view2 = ((from p in seoRepository.Seos
            //    select new
            //    {
            //        p.Keywords,
            //        p.Robots,
            //        p.Snippet,
            //        p.Tittle,
            //        p.TagID
            //    }

            //    ).AsNoTracking().ToList());

            //where p.TagID == header 
            //select p.Keywords;

            //var view3 = from p in view2
            //                where p.TagID==header
            //            select new SimpleSeoViewModel()
            //            {
            //                Keywords = p.Keywords,
            //                Snippet = p.Snippet,
            //                Tittle = p.Tittle,
            //                Robots = p.Robots
            //            }
            //                ; //.Where(x=>x.TagID==header);
            //var view4 = view3;


            //SimpleSeoViewModel view = (from p in seoRepository.Seos.AsNoTracking()
            //    where p.TagID == header
            //    select new SimpleSeoViewModel
            //    {
            //        Keywords = p.Keywords,
            //        Tittle = p.Tittle,
            //        Robots = p.Robots,
            //        Snippet = p.Snippet
            //    }).First();



            //ViewBag.SEO = view;
        }

        private async Task GetSeoHeadersAsync(string header)
        {

            SimpleSeoViewModel seo = await (from p in seoAttributeRepository.SeoAttributes
                                      where p.TagID == header
                                      select new SimpleSeoViewModel
                                      {
                                          Keywords = p.Keywords,
                                          Snippet = p.Snippet,
                                          Robots = p.Robots,
                                          Tittle = p.Tittle
                                      }).AsNoTracking().FirstOrDefaultAsync();
            //seoAttributeRepository.SeoAttributes.AsNoTracking().FirstOrDefault(x => x.TagID == header).;
            ViewBag.SEO = seo;

            //ViewBag.SEO = await seoAttributeRepository.SeoAttributes.AsNoTracking().FirstOrDefaultAsync(x => x.Tag == header);
            
            
            //.Select(x => new { x.Keywords, x.Robots, x.Snippet, x.Tittle}
            //ViewBag.SEO = await seoRepository.Seos.AsNoTracking().FirstOrDefaultAsync(x => x.TagID == header).Result(x=> new {x.Keywords, x.Robots, x.Snippet, x.Tittle});  //seoAttributeRepository.SeoAttributes.AsNoTracking().FirstOrDefaultAsync(x => x.Tag == header);
            //ViewBag.SEO = await ((from p in seoRepository.Seos
            //    where p.TagID == header
            //    select new 
            //    {
            //        p.Keywords,
            //        p.Robots,
            //        p.Snippet,
            //        p.Tittle
            //    }

            //    ).AsNoTracking().ToListAsync());

        }

        public ArticleViewModel GetArticleByTag(string header)
        {
            return (from p in seoAttributeRepository.SeoAttributes
                    where p.TagID == header
                    select new ArticleViewModel
                    {
                        TagID = p.TagID,
                        UserID = p.UserID,
                        ShortLink = p.ShortLink,
                        ArticlePreview = p.ArticlePreview,
                        ArticleText = p.ArticleText,
                        Tag = p.Tag,
                        Header = p.Header,
                        User = p.User
                    }).AsNoTracking().FirstOrDefault();
        }

        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }

    

        public ActionResult Sitemap()
        {
            string host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            Response.ContentType = "text/xml";
            IEnumerable<Category> categoryList = categoryRepository.Categories.OrderBy(x => x.ShortName).Where(x => x.IsActive).AsNoTracking();
            //IEnumerable<Product> productList = dataManager.Products.Products.OrderBy(x => x.Category.ShortName).Where(x => x.IsDeleted != true);
            
            List<SitemapAttributes> nodes = new List<SitemapAttributes>();

            //url.Add();

            string path = "http://" + host + ":" + port + "/";

            foreach (var category in categoryList)
            {
                nodes.Add(new SitemapAttributes
                {
                    Loc = path + category.ShortName + "\n",
                    Lastmod = category.UpdateDate.ToString("yyyy-MM-dd"),
                    Changefreq = "daily",
                    Priority = "0.8"
                });
            }

            IEnumerable<LastModifiedViewModel> seo = (from p in seoAttributeRepository.SeoAttributes
                    select new LastModifiedViewModel()
                    {
                        TagID = p.TagID,
                        CreateDate = p.CreateDate,
                        UpdateDate = p.UpdateDate
                    }).AsNoTracking().ToList();

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Contact" + "\n",
                Lastmod = (!seo.Any(x => x.TagID == "Contact")) ? DateTime.Now.ToString("yyyy-MM-dd") : seo.FirstOrDefault(x => x.TagID == "Contact").UpdateDate.ToString("yyyy-MM-dd"),
                Changefreq = "monthly",
                Priority = "0.5"
            });

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Portfolio" + "\n",
                Lastmod = (!seo.Any(x => x.TagID == "Portfolio")) ? DateTime.Now.ToString("yyyy-MM-dd") : seo.FirstOrDefault(x => x.TagID == "Portfolio").UpdateDate.ToString("yyyy-MM-dd"),
                Changefreq = "monthly",
                Priority = "0.3"
            });

            nodes.Add(new SitemapAttributes
            {
                Loc = path + "Price" + "\n",
                Lastmod = (!seo.Any(x => x.TagID == "Price")) ? DateTime.Now.ToString("yyyy-MM-dd") : seo.FirstOrDefault(x => x.TagID == "Price").UpdateDate.ToString("yyyy-MM-dd"),
            Changefreq = "daily",
                Priority = "0.5"
            });
            
            nodes.Add(new SitemapAttributes
            {
                Loc = path + "About" + "\n",
                Lastmod = (!seo.Any(x => x.TagID == "About")) ? DateTime.Now.ToString("yyyy-MM-dd") : seo.FirstOrDefault(x => x.TagID == "About").UpdateDate.ToString("yyyy-MM-dd"),
                Changefreq = "daily",
                Priority = "0.5"
            });

        

            XElement data = new XElement("urlset", nodes.Select(x => new XElement("url",
                                                                                  new XElement("loc", x.Loc),
                                                                                  new XElement("lastmod", x.Lastmod),
                                                                                  new XElement("changefreq", x.Changefreq),
                                                                                  new XElement("priority", x.Priority))));
            data.Add(new XAttribute("id", "1"));
            return Content(data.ToString(), "text/xml");
        }

        
        public class JsonModel
        {
            public string HTMLString { get; set; }
            public bool NoMoreData { get; set; }
        }




        public class SitemapAttributes
        {
            public string Loc { get; set; }
            public string Lastmod { get; set; }
            public string Changefreq { get; set; }
            public string Priority { get; set; }

        }


       
    }
}
