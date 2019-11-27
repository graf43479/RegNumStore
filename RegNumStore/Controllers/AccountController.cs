using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Domain;
using Domain.Entities;
using Domain.Infrasructure.Abstract;
using RegnumStore.Extensions;
using RegNumStore.Models;
using RegnumStore.Models;


namespace RegnumStore.Controllers
{
    [Authorize]
  //  [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private IAuthProvider auth;
        private DataManager dataManager;

        public AccountController(IAuthProvider auth, DataManager dataManager)
        {
            this.auth = auth;
            this.dataManager = dataManager;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
       // [RequireHttps]
        public ActionResult Login(string returnUrl)
        {
            //if (User.Identity.IsAuthenticated)
            //{
              
            //        var p1 = Roles.IsUserInRole(User.Identity.Name, "admin");
            //        var p2 = Roles.IsUserInRole(User.Identity.Name, "User");
            //        var p3 = Roles.IsUserInRole(User.Identity.Name, "ContentManager");
              
            //    Roles.IsUserInRole(User.Identity.Name);
            //    bool t = User.IsInRole("admin");
            //    bool t2 = User.IsInRole("User");
            //}
            TempData["nav-message"] = "Login";
            ViewBag.ReturnUrl = returnUrl;

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("UserEnterPartialView", new LoginModel() { });
    
            //}
            
            return View();
        }


        [AllowAnonymous]
        // [RequireHttps]
        public ActionResult LoginUser(string returnUrl)
        {
            TempData["nav-message"] = "Login";
            ViewBag.ReturnUrl = returnUrl;

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("UserEnterPartialView", new LoginModel() { });

            //}

            return PartialView("UserEnterPartialView");
        }



        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult LoginUser(LoginModel model)
        {
            if (Session["attempt"] == null)
            {
                Session["attempt"] = 0;
            }

            if ((int)Session["attempt"] > 3)
            {
               // return View("Login", model);
                return JavaScript("alert('!!!'); parent.$('a.lightcase-icon-close').trigger('click'); document.location.href = 'http://localhost:58920/Account/Login';");
                //string s = "$('.wrappAdd').css('display', 'none');" +
                //           "$('a.lightcase-icon-close').click();" +


                //           " parent.$('a.lightcase-icon-close').trigger('click');"+
                //           "document.location.href = 'http://localhost:58920/Account/Login';";

                //return JavaScript(s);
                //return Json(new { message = "redirect" });
            }

            //validate captcha
           
            
            if (ModelState.IsValid)
            {
                try
                {
                    User user = dataManager.UsersRepository.UsersInfo.AsNoTracking().FirstOrDefault(x => x.Login == model.UserName);

                    if (dataManager.Provider.ValidateUser(model.UserName,
                                                          CreatePasswordHash(model.Password, user.PasswordSalt)))
                    {

                        if (user.IsActivated != true)
                        {
                            ModelState.AddModelError("", "Учетная запись не активирована");
                              var errors = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
                            return Json(new {success = "false", message = errors});


                        }
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        
                        dataManager.UsersRepository.GetMembershipUserByName(model.UserName);
                        Session["attempt"] = 0;

                        return Json(new { success = "true" });
                    }
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                    var errors = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = "false", message = errors });
                }
            }
            var errors2 = ModelState.Select(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage));
            return Json(new { success = "false", message = errors2 });

        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            

            if (Session["attempt"] == null)
            {
                Session["attempt"] = 0;
            }


            //validate captcha
            if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) &&
                (int)Session["attempt"] > 3)
            {
                ModelState.AddModelError("Captcha", "Сумма введена неверно! Пожалуйста, повторите ещё раз!");
                Session["attempt"] = (int)Session["attempt"] + 1;

                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                   User user = dataManager.UsersRepository.UsersInfo.AsNoTracking().FirstOrDefault(x => x.Login == model.UserName);

                    if (dataManager.Provider.ValidateUser(model.UserName,
                                                          CreatePasswordHash(model.Password, user.PasswordSalt)))
                    {

                        if (user.IsActivated != true)
                        {
                            ModelState.AddModelError("", "Учетная запись не активирована");
                            return View(model);
                        }

                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);


                        dataManager.UsersRepository.GetMembershipUserByName(model.UserName);
                        Session["attempt"] = 0;
                        
                        return RedirectToAction("About", "Home");
                    }
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                    return View(model);
                }
            }
            return View(model);
           
        }

        //
        // POST: /Account/LogOff

      //  [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut();
            //Session["UserName"] = null;
            //Session["UserID"] = null;
            HttpContext.Session.Clear();
            

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            TempData["nav-message"] = "Registrate";
            return View(new RegisterModel());
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
              
                model.PasswordSalt = CreateSalt();
                string pwd = CreatePasswordHash(model.Password, model.PasswordSalt);

                

                int userRoleId = dataManager.UsersRepository.Roles.FirstOrDefault(x => x.RoleName.ToLower() == "user").RoleID;

                MembershipCreateStatus status = dataManager.Provider.CreateUser(model.Login, pwd,
                                                                                    model.Email, model.Phone, model.UserName,
                                                                                    model.IsActivated, 
                                                                                    model.PasswordSalt,
                                                                                    userRoleId);
                    
                    if (status == MembershipCreateStatus.Success)
                    {
                        //FormsAuthentication.SetAuthCookie(model.Login, false);

                        dataManager.UsersRepository.GetMembershipUserByName(model.Login);

                        User userInfo = dataManager.UsersRepository.UsersInfo
                                                   .FirstOrDefault(p => p.Login == model.Login);

                        userInfo.Password = model.Password;


                        // userInfo.Password = originPassword; //ViewBag.originPassword;
                        string host = Request.Url.Host;
                        dataManager.DeliveryProcessor.EmailActivation(userInfo, host);

                      //  return RedirectToAction("UserRole", new { login = model.Login });
                        // dataManager.UsersRepository.AddUserToRole(model.Login, "User");

                        return RedirectToAction("Index","Home");

                        
                    }
                    else if (status == MembershipCreateStatus.DuplicateEmail)
                    {
                        RedirectToAction("Login", "Account");
                    }

                    ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
                }
            TempData["nav-message"] = "Login";
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult AdminSetup()
        {
            if (dataManager.UsersRepository.AdminExists())
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdminSetup(RegisterModel model)
        {
            TempData["nav-message"] = "Login";
            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(model.Password);
              //  string originPassword = sb.ToString();
                string salt = CreateSalt();
                string pwd = CreatePasswordHash(model.Password, model.PasswordSalt);
                //model.Password = CreatePasswordHash(model.Password, model.PasswordSalt);
                    dataManager.UsersRepository.CreateRole("admin");

                int adminRoleId = dataManager.UsersRepository.Roles.FirstOrDefault(x => x.RoleName.ToLower() == "admin").RoleID;


                MembershipCreateStatus status = dataManager.Provider.CreateUser(model.Login, 
                                                                                pwd, model.Email, model.Phone, model.UserName,
                                                                                model.IsActivated, salt, adminRoleId);

                if (status == MembershipCreateStatus.Success)
                {

                    dataManager.UsersRepository.GetMembershipUserByName(model.Login);

                    User userInfo = dataManager.UsersRepository.UsersInfo
                                               .FirstOrDefault(p => p.Login == model.Login);

                    userInfo.Password = model.Password;
                    string host = Request.Url.Host;

                    dataManager.DeliveryProcessor.EmailActivation(userInfo, host);
                    

                    //dataManager.UsersRepository.AddUserToRole(model.Login, "Admin");
                    
                    return View("Success", model);
                }
                ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            TempData["nav-message"] = "Login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgottenPassword(User user)
        {
            TempData["nav-message"] = "UserInfo";
            if (dataManager.UsersRepository.GetUserNameByEmail(user.Email) != "")
            {

                User userInfo = dataManager.UsersRepository.UsersInfo.FirstOrDefault(
                    x => x.Email == user.Email);
                string pwdOrigin = CreatePassword(6);
                userInfo.Password = CreatePasswordHash(pwdOrigin, userInfo.PasswordSalt);
                dataManager.UsersRepository.SaveUser(userInfo);
                userInfo.Password = pwdOrigin;
                string host = Request.Url.Host;
                dataManager.DeliveryProcessor.EmailRecovery(userInfo, host);


                ViewBag.UserInfo = "На указанный адрес был выслан пароль";
                return RedirectToAction("Login","Account");
                //return Content(Boolean.TrueString);
                //return Json(JsonStandardResponse.SuccessResponse("На указанный адрес был выслан пароль"),
                //            JsonRequestBehavior.DenyGet);
            }

            else
            {
                if (user.Email == null)
                {
                    Session["UserEmail"] = "";
                    ModelState.AddModelError("", "Неудачная попытка изменения пароля");
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                                //JsonRequestBehavior.DenyGet);
                    //return Content("Пожалуйста проверьте форму");
                    return View(user);
                }
                else
                {
                    ModelState.AddModelError("", "Неудачная попытка изменения пароля");
                    //Session["UserEmail"] = "Пользователя с таким email не существует!";
                    //return Content("Пользователя с таким email не существует!");
                    //return Json(JsonStandardResponse.ErrorResponse("Пользователя с таким email не существует!"),
                                //JsonRequestBehavior.DenyGet);
                    return View(user);
                }


            }
        }

        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {

                User user = dataManager.UsersRepository.UsersInfo.FirstOrDefault(p => p.Login == User.Identity.Name);


                RegisterModel viewModel = new RegisterModel()
                {
                    Login = user.Login,
                    Password = "", //user.Password,
                    ConfirmPassword = "", // user.Password,
                    Email = user.Email,
                    UserID = user.UserID,
                    IsActivated = user.IsActivated,
                    UserName = user.UserName,
                    Created = user.Created,
                    PasswordSalt = user.PasswordSalt,
                    Phone = user.Phone
                };
                
                TempData["nav-message"] = "UserInfo";
                return View(viewModel);

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(RegisterModel model)
        {
            if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
            {
                User userInfo = dataManager.UsersRepository.UsersInfo.FirstOrDefault(p => p.UserID == model.UserID);
                string tmp = CreatePasswordHash(model.OldPassword, userInfo.PasswordSalt);
                if (userInfo.Password != CreatePasswordHash(model.OldPassword, userInfo.PasswordSalt) &&
                    (userInfo.Password.Length > 35))
                {
                    //return PartialView(model); 
                    ModelState.AddModelError("", "Неудачная попытка изменения учетной записи");
                    //return Content("Пожалуйста проверьте форму");
                    return View(model); 
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                    //            JsonRequestBehavior.DenyGet);
                }

                if (userInfo.Password != model.OldPassword && userInfo.Password.Length < 35)
                {
                    //return PartialView(model);
                    ModelState.AddModelError("", "Неудачная попытка изменения учетной записи");
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                    //            JsonRequestBehavior.DenyGet);
                    return View(model); 
                }

                userInfo.Password = CreatePasswordHash(model.Password, userInfo.PasswordSalt);
                FormsAuthentication.SetAuthCookie(model.Login, false);

                dataManager.UsersRepository.SaveUser(userInfo);
                TempData["Message"] = string.Format("Пароль для учетной записи {0} изменен", model.Login);
                TempData["messageType"] = "information-msg";
                //Session["UserName"] = model.UserName;
                //return RedirectToAction("List", "Product");
                //return Content(Boolean.TrueString); ; //JavaScript("window.location.replace('http://localhost:57600/Account/UserAccountEdit');");
                //return Json(JsonStandardResponse.SuccessResponse(true), JsonRequestBehavior.DenyGet);
                return RedirectToAction("Login", "Account");
            }
            else
                ModelState.AddModelError("", "Неудачная попытка изменения пароля");
            //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"), JsonRequestBehavior.DenyGet);
            //return Content("Пожалуйста проверьте форму"); //Content("Пожалуйста проверьте форму");
             return View(model);
            //  return PartialView(model);
        }

        public ActionResult UserAccountChange()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = dataManager.UsersRepository.UsersInfo.FirstOrDefault(p => p.Login == User.Identity.Name);

                EditUserModel viewModel = new EditUserModel()
                {
                    Login = user.Login,
                    Email = user.Email,
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Phone = user.Phone
                    
                };
                TempData["nav-message"] = "UserInfo";
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult UserAccountChange(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<User> userlist = dataManager.UsersRepository.UsersInfo;
                User userInfo = userlist.FirstOrDefault(p => p.UserID == model.UserID);


                User userSearched = userlist.Where(x=>x.UserID!=model.UserID).FirstOrDefault(x => x.Email == model.Email);
                if (userSearched!=null)
                {
                    ModelState.AddModelError("", "Учетная запись с заданным email уже существует");
                    return View(model);
                }
                else
                {
                    userInfo.UserName = model.UserName;
                    userInfo.Phone = model.Phone;
                    userInfo.Email = model.Email;    
                }

                //if (userlist.Where(x=>x.Email==model.Email).Count()==0)
                //{
                    
                //}
                //else
                //{
                    
                //}
                
                
                FormsAuthentication.SetAuthCookie(model.Login, false);

                dataManager.UsersRepository.SaveUser(userInfo);
                TempData["Message"] = string.Format("Данные учетной записи {0} изменены", model.Login);
                TempData["messageType"] = "information-msg";
                //Session["UserName"] = model.UserName;
                //return RedirectToAction("List", "Product");
                //return Content(Boolean.TrueString); ; //JavaScript("window.location.replace('http://localhost:57600/Account/UserAccountEdit');");
                //return Json(JsonStandardResponse.SuccessResponse(true), JsonRequestBehavior.DenyGet);
                return RedirectToAction("Login", "Account");
            }
            else
                ModelState.AddModelError("", "Неудачная попытка изменения учетной записи");
            //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"), JsonRequestBehavior.DenyGet);
            //return Content("Пожалуйста проверьте форму"); //Content("Пожалуйста проверьте форму");
            return View(model);
            //  return PartialView(model);
        }

       

        #region Вспомогательные методы
        [AllowAnonymous]
        public ActionResult Activate(string username, string key)
        {

            if (dataManager.UsersRepository.ActivateUser(username, key) == false)
            {
                TempData["message"] =
                    string.Format("При активации произошла проблема. Возможно вы уже активировались ранее!");
                TempData["messageType"] = "warning-msg";
                return RedirectToAction("Register", "Account");
            }
            else
            {
                TempData["message"] =
                    string.Format(
                        "Поздравляем! Активация прошла успешно! Введите свой логин и пароль, чтобы авторизироваться на сайте! ");
                TempData["messageType"] = "confirmation-msg";
                //TempData.Keep("message");
                //logger.Info("Пользователь " + username + " активировался");
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult UserRole(string login)
        {
            dataManager.UsersRepository.AddUserToRole(login, "User");
            return View("Success");

        }


        public string GetMembershipCreateStatusResultText(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.DuplicateUserName)
                return "Пользователь с таким логином уже существует";
            if (status == MembershipCreateStatus.DuplicateEmail)
                return "Пользователь с таким email уже существует";
            return "Неизвестная ошибка";
        }

        //генерация хэша
        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        //Генерация соли
        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        //генерация пароля
        public string CreatePassword(int length)
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        //Капча
        [AllowAnonymous]
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }


        [AllowAnonymous]
        public ActionResult GetUserPhoneNumber(int userId)
        {
            
            var captcha = dataManager.UsersRepository.UsersInfo.FirstOrDefault(x => x.UserID == userId).Phone;

            //image stream
            FileContentResult img = null;
            
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                SolidBrush background = new SolidBrush(Color.FromArgb(45, 52, 62));
                SolidBrush font = new SolidBrush(Color.FromArgb(236, 97, 21));
                gfx.FillRectangle(background, new Rectangle(0, 0, bmp.Width, bmp.Height));
            
                gfx.DrawString(captcha, new Font("Tahoma", 15), font, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }
            return img;
        }


        public ActionResult UserInfo()
        {
            ViewBag.User = dataManager.UsersRepository.UsersInfo.FirstOrDefault(x => x.Login == User.Identity.Name);
            IEnumerable<Product> products = dataManager.Products.Products.Where(x => x.User.Login == User.Identity.Name).OrderByDescending(x=>x.StartDate);
            TempData["nav-message"] = "UserInfo";
            return View(products);
        }

        [HttpGet]
        public async Task<ActionResult> EditRegNum()
        {
            ProductRegNumViewModel viewModel = new ProductRegNumViewModel
                {Regions = await dataManager.RegionRepository.Regions.OrderBy(x => x.Sequence).ToListAsync(),
                Categories = await  dataManager.Categories.Categories.ToListAsync()};
            TempData["nav-message"] = "UserInfo";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditRegNum(ProductRegNumViewModel viewModel)
        {
            
            string number = viewModel.Num1 + viewModel.Num2 + viewModel.Num3 + viewModel.Num4 + viewModel.Num5 +
                            viewModel.Num6 + viewModel.Num7;
            viewModel.Regions = dataManager.RegionRepository.Regions.OrderBy(x => x.Sequence);
            viewModel.Categories = dataManager.Categories.Categories;
            if (viewModel.Price<30000)
            {
                ModelState.AddModelError("", "Указана заниженная стоимость");
                return View(viewModel);
            }
            if (viewModel.Price > 10000000)
            {
                ModelState.AddModelError("", "Указана слишком завышенная стоимость");
                return View(viewModel);
            }
            if (viewModel.Num7=="*")
            {
                ModelState.AddModelError("", "Номер региона обязателен к заполнению");
                return View(viewModel);
            }


            if (!number.Contains("*") && dataManager.Products.Products.Any(x=>x.ProductNumber==number))
            {
                ModelState.AddModelError("", "В базе уже существует номер " + number);
                return View(viewModel);
            }


            if (number.Split('*').Count()>2)
            {
                    ModelState.AddModelError("", "В номере содержится слишком много неопределенности");
                    return View(viewModel);
            }
            else
            {
                if (viewModel.ProductID==0)
                {
                    Product product = new Product()
                    {
                        //   CategoryID = viewModel.CategoryID,
                        UserID = dataManager.UsersRepository.GetUserByName(User.Identity.Name).UserID,
                        StartDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsChoosen = false,
                        IsForSale = true,
                        IsOverbalanceIncluded = viewModel.IsOverbalanceIncluded,
                        IsDisplay = true,
                        Price = viewModel.Price,
                        Status = "",
                        RegionID =
                            dataManager.RegionRepository.Regions.FirstOrDefault(x => x.RegionNumber == viewModel.Num7)
                                       .RegionID,
                        ProductNumber = number
                    };

                    CarNumber carNumber = new CarNumber()
                    {
                        Num1 = viewModel.Num1,
                        Num2 = viewModel.Num2,
                        Num3 = viewModel.Num3,
                        Num4 = viewModel.Num4,
                        Num5 = viewModel.Num5,
                        Num6 = viewModel.Num6,
                        Num7 = viewModel.Num7
                    };

                    CarNumber carNum = product.ProductNumber.NumberToCarNumber();

                List<int> assignedIDs = new List<int>();

                //if (carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6)
                //{
                //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые буквы");
                //    if (firstOrDefault != null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}
                //if (carNum.Num2 == carNum.Num3 && carNum.Num3 == carNum.Num4)
                //{
                //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые цифры");
                //    if (firstOrDefault != null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}

                //if ((carNum.Num2 + carNum.Num3 + carNum.Num4 == carNum.Num7) || (carNum.Num2 == "0" && carNum.Num2 + carNum.Num3 + carNum.Num4 == "0" + carNum.Num7))
                //{
                //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Номер-регион");
                //    if (firstOrDefault !=
                //        null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}

                //if (carNum.Num2 == "0" && carNum.Num3 == "0")
                //{
                //    var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Первая десятка");
                //    if (firstOrDefault !=
                //        null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}

                //if (carNum.Num3 == "0" && carNum.Num4 == "0")
                //{
                //    var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Сотые");
                //    if (firstOrDefault != null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}

                //if (carNum.Num2 == carNum.Num4)
                //{
                //    var firstOrDefault = repositoryCategory.Categories.FirstOrDefault(x => x.CategoryName == "Зеркальные");
                //    if (firstOrDefault != null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}

                //if (!assignedIDs.Any())
                //{
                //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Другое");
                //    if (firstOrDefault != null)
                //        assignedIDs.Add(firstOrDefault.CategoryID);
                //}
                //else
                //{

                //}



                var CategoryIDs = product.Categories.Select(x => x.CategoryID);
                var courseIDs = CategoryIDs as int[] ?? CategoryIDs.ToArray();

                var coursesToDeleteIDs = courseIDs.Where(id => !assignedIDs.Contains(id)).ToList();

                // Delete removed courses
                foreach (var id in coursesToDeleteIDs)
                {
                    product.Categories.Remove(dataManager.Products.FindID(id));
                }

                // Add courses that user doesn't already have
                foreach (var id in assignedIDs)
                {
                    if (!courseIDs.Contains(id))
                    {
                        product.Categories.Add(dataManager.Products.FindID(id));
                    }
                }
                    
                //dataManager.Products.SaveProduct(product);

                await dataManager.Products.SaveProductAsync(product);
                //dataManager.Products.SaveProductAsync(product);
                DefineAllCategories(product);
                    RegNumImage(product.ProductID, carNumber);
    
                }
                else
                {
                    Product product =
                        dataManager.Products.Products.FirstOrDefault(x => x.ProductID == viewModel.ProductID);

                    product.Price = viewModel.Price;
                    product.IsOverbalanceIncluded = viewModel.IsOverbalanceIncluded;
                    product.UpdateDate = DateTime.Now;
                    dataManager.Products.SaveProduct(product);
                    //dataManager.Products.SaveProductAsync(product);
                    
                }
                
                
                return RedirectToAction("UserInfo");
            }
            
            return View();
        }

        public ActionResult UpdateRegNum(int productId)
        {

            IEnumerable<Category> categories = dataManager.Categories.Categories.ToList();

            //IEnumerable<Category> categories = from c in categoryRepository.Categories.ToList()
            //                                   join s in st on c.ShortName equals s
            //                                   select c;
            //var products = categories.SelectMany(x => x.Products).Distinct();

            var products = categories.SelectMany(x => x.Products).Distinct().Where(x=>x.ProductID==productId);

           // var m2 = products;
            Product product = dataManager.Products.Products.FirstOrDefault(x => x.ProductID == productId);
            ProductRegNumViewModel viewModel = product.ProductToProductRegNumViewModel();
            viewModel.Regions = dataManager.RegionRepository.Regions.ToList();
            viewModel.Categories = products.SelectMany(x=>x.Categories);
            TempData["nav-message"] = "UserInfo";
            return View("EditRegNum", viewModel);
        }



        public ActionResult DeleteRegNum(int productId)
        {
            Product product = dataManager.Products.Products.FirstOrDefault(x => x.ProductID == productId);
            DeleteProductRelationships(product);


            if (product.Categories!= null)
            {
                foreach (var cat in product.Categories.ToList())
                {
                    product.Categories.Remove(cat);
                }
            }

            dataManager.Products.DeleteProduct(product);
            
            

            return RedirectToAction("UserInfo");
        }



        public void DeleteProductRelationships(Product product)
        {
            //if ((productId == null) && (resubmit!=null)) 
            if (product != null)
            {
                try
                {
                    string strSaveFileName = product.ProductID.ToString() + ".jpg";

                    string strSaveFullPath = System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                        Constants.PRODUCT_IMAGE_FOLDER,
                        strSaveFileName);

                    if (System.IO.File.Exists(strSaveFullPath))
                    {
                        System.IO.File.Delete(strSaveFullPath);
                    }
                    else
                    {
                        Exception ex;
                    }

                    //TempData["message"] = string.Format("Фотографии были удалены");
                    //TempData["messageType"] = "warning-msg";
                }
                catch (Exception)
                {
                    TempData["message"] = string.Format("Что-то пошло не так при удалении файлов");
                    TempData["messageType"] = "warning-msg";
                }

                //TempData["message"] = string.Format("Фотографии были удалены");
                //TempData["messageType"] = "warning-msg";
            }
        }



        public void DefineAllCategories(Product product)
        {
            CarNumber carNum = product.ProductNumber.NumberToCarNumber();

            List<int> assignedIDs = new List<int>();

            if (carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6)
            {
                var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые буквы");
                if (firstOrDefault != null)
                    assignedIDs.Add(firstOrDefault.CategoryID);
            }
            if (carNum.Num2 == carNum.Num3 && carNum.Num3 == carNum.Num4)
            {
                var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Одинаковые цифры");
                if (firstOrDefault != null)
                    assignedIDs.Add(firstOrDefault.CategoryID);
            }

            if ((carNum.Num2 + carNum.Num3 + carNum.Num4 == carNum.Num7) || (carNum.Num2 == "0" && carNum.Num2 + carNum.Num3 + carNum.Num4 == "0" + carNum.Num7))
            {
                var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Номер-регион");
                if (firstOrDefault !=
                    null)
                    assignedIDs.Add(firstOrDefault.CategoryID);
            }

            if (((carNum.Num2 == carNum.Num3 && carNum.Num3 == carNum.Num4) && (carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6)) || ((carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6) && ((carNum.Num2 + carNum.Num3 + carNum.Num4 == carNum.Num7) || (carNum.Num2 == "0" && carNum.Num2 + carNum.Num3 + carNum.Num4 == "0" + carNum.Num7)))
                || ((carNum.Num1 == carNum.Num5 && carNum.Num5 == carNum.Num6) && (carNum.Num2==carNum.Num3 && carNum.Num2=="0")))
            {
                var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Эксклюзив");
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
                || (carNum.Num1 == "С" && carNum.Num5 == "С" && carNum.Num6 == "С" && (carNum.Num7 == "77" ||carNum.Num7 == "97" ||carNum.Num7 == "99"))
               || ((carNum.Num1 == "А"||carNum.Num1 == "В"||carNum.Num1 == "К"||carNum.Num1 == "С"||carNum.Num1 == "О"
                ||carNum.Num1 == "М"||carNum.Num1 == "Т"||carNum.Num1 == "Н"
                ||carNum.Num1 == "У"||carNum.Num1 == "Х") && carNum.Num5 == "М" && carNum.Num6 == "О" && carNum.Num7 == "50")
                || ((carNum.Num1 == "А" || carNum.Num1 == "М") && carNum.Num5 == "М" && carNum.Num6 == "М" && (carNum.Num7 == "50" || carNum.Num7 == "90"))
                )
                {
                    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Спецсерия");
                    if (firstOrDefault !=
                        null)
                        assignedIDs.Add(firstOrDefault.CategoryID);
                }




            //if (carNum.Num2 == "0" && carNum.Num3 == "0")
            //{
            //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Первая десятка");
            //    if (firstOrDefault !=
            //        null)
            //        assignedIDs.Add(firstOrDefault.CategoryID);
            //}

            //if (carNum.Num3 == "0" && carNum.Num4 == "0")
            //{
            //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Сотые");
            //    if (firstOrDefault != null)
            //        assignedIDs.Add(firstOrDefault.CategoryID);
            //}

            //if (carNum.Num2 == carNum.Num4)
            //{
            //    var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Зеркальные");
            //    if (firstOrDefault != null)
            //        assignedIDs.Add(firstOrDefault.CategoryID);
            //}

            if (!assignedIDs.Any())
            {
                var firstOrDefault = dataManager.Categories.Categories.FirstOrDefault(x => x.CategoryName == "Другое");
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
                product.Categories.Remove(dataManager.Products.FindID(id));
            }

            // Add courses that user doesn't already have
            foreach (var id in assignedIDs)
            {
                if (!courseIDs.Contains(id))
                {
                    product.Categories.Add(dataManager.Products.FindID(id));
                }
            }
            dataManager.Products.SaveProduct(product);
        }


        public void Temp()
        {
            IEnumerable<Product> productList = dataManager.Products.Products.ToList();
            foreach (Product product in productList)
            {
                DefineAllCategories(product);
            }
        }

        [AllowAnonymous]
        public void RegNumImage(int productId, CarNumber num, bool noisy = true)
        {
            string[] number = { num.Num1, num.Num2 + num.Num3 + num.Num4, num.Num5 + num.Num6, num.Num7 };

            int h = 40;
            int w = 170;
            //FileContentResult img = null;

            try
            {
                using (var bmp = new Bitmap(w, h))
                {
                    using (var gfx = Graphics.FromImage((Image)bmp))
                    {
                        //gfx.Clear(Color.Red);
                        //gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        gfx.SmoothingMode = SmoothingMode.AntiAlias;

                        Image image = new Bitmap(System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                            "images",
                            "template.jpg")); // new Bitmap("~/Content/images/template.jpg");
                        gfx.DrawImage(image, 0, 0, w, h);
                        if (number[0] == "М")
                        {
                            gfx.DrawString(number[0], new Font("Tahoma", 15), Brushes.Black, 12, 8);
                            gfx.DrawString(number[1], new Font("Tahoma", 20), Brushes.Black, 27, 2);
                            gfx.DrawString(number[2], new Font("Tahoma", 16), Brushes.Black, 71, 8);
                        }
                        else
                        {
                            gfx.DrawString(number[0], new Font("Tahoma", 16), Brushes.Black, 13, 8);
                            gfx.DrawString(number[1], new Font("Tahoma", 20), Brushes.Black, 27, 2);
                            gfx.DrawString(number[2], new Font("Tahoma", 16), Brushes.Black, 74, 8);
                        }

                        //gfx.DrawString(number[0], new Font("Tahoma", 16), Brushes.Black, 12, 10);

                        gfx.DrawString(number[3], new Font("Tahoma", 15), Brushes.Black, number[3].Length == 2 ? 120 : 115,
                            2);
                        string p = System.IO.Path.Combine(
                            Server.MapPath(Url.Content("~/Content")), "img", productId + ".jpg");
                        bmp.Save(p, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bmp.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
           
        }


        public async Task<string> RegImgs()
        {
            var products = await dataManager.Products.Products.AsNoTracking().ToListAsync();
            foreach (var product in products)
            {
                

                //CarNumber cars = new CarNumber()
                //    {
                //        Num1 = product.ProductNumber.Remove(1, product.ProductNumber.Length - 1).ToUpper(),
                //        Num2 = product.ProductNumber.Substring(1, 1).ToUpper(),
                //        Num3 = product.ProductNumber.Substring(2, 1).ToUpper(),
                //        Num4 = product.ProductNumber.Substring(3, 1).ToUpper(),
                //        Num5 = product.ProductNumber.Substring(4, 1).ToUpper(),
                //        Num6 = product.ProductNumber.Substring(5, 1).ToUpper(),
                //        Num7 = product.ProductNumber.Remove(0, 6).ToUpper()
                //    };
                try
                {
                    CarNumber cars = product.ProductNumber.NumberToCarNumber();
                    RegNumImage(product.ProductID, cars);
                    
                }
                catch (Exception exception)
                {
                    Session["exception"] = exception.Message;
                    TempData["exception"] = exception.Message;
                    return exception.Message;
                }
            }

            return "Success";
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion
    }
}
