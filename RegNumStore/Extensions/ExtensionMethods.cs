using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Domain.Entities;
using RegnumStore.Models;
using RegNumStore.Models;

namespace RegnumStore.Extensions
{
    public static class ExtensionMethods
    {

        public static Category ViewModelToCategory(this CategoryViewModel viewModel)
        {
            Category category = new Category
            {
                CategoryID = viewModel.CategoryID,
                CategoryName = viewModel.CategoryName,
                CreateDate = viewModel.CreateDate,
                UpdateDate = viewModel.UpdateDate,
                Description = viewModel.Description,
                Sequence = viewModel.Sequence,
                ShortName = viewModel.ShortName,
                Snippet = viewModel.Snippet,
                IsActive = viewModel.IsActive,
                KeyWords = viewModel.KeyWords

            };
            return category;
        }

        public static CategoryViewModel CategoryToViewModel(this Category category)
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                CreateDate = category.CreateDate,
                UpdateDate = category.UpdateDate,
                Description = category.Description,
                Sequence = category.Sequence,
                ShortName = category.ShortName,
                Snippet = category.Snippet,
                IsActive = category.IsActive,
                KeyWords = category.KeyWords

            };
            return viewModel;
        }

        public static Product ViewModelToProduct(this ProductViewModel viewModel)
        {
            Product product = new Product()
            {
               // CategoryID = viewModel.SelectedCategoryID,
                ProductID = viewModel.ProductID,
                UserID = viewModel.UserID,
                RegionID = viewModel.SelectedRegionID,
                Price = viewModel.Price,
                Status = viewModel.Status,
                ProductNumber = viewModel.ProductNumber,
                IsForSale = viewModel.IsForSale,
                IsOverbalanceIncluded = viewModel.IsOverbalanceIncluded,
                StartDate = viewModel.StartDate,
                UpdateDate = viewModel.UpdateDate,
                IsChoosen = viewModel.IsChoosen,
                IsDisplay = viewModel.IsDisplay,
                User = viewModel.User,
                Categories = viewModel.Categories.ToList(),
                
                
            };
            return product;
        }


        public static ProductViewModel ProductToViewModel(this Product product)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                //SelectedCategoryID = product.CategoryID,
                SelectedRegionID = product.RegionID,
                UserID = product.UserID,
                ProductID = product.ProductID,
                StartDate = product.StartDate,
                UpdateDate = product.UpdateDate,
                IsChoosen = product.IsChoosen,
                IsDisplay = product.IsDisplay,
                //CategoryName = product.Category.CategoryName,
                RegionNumber = product.Region.RegionNumber,
                RegionName = product.Region.RegionName + " " + product.Region.RegionNumber,
                Region = product.Region,
                IsForSale = product.IsForSale,
                IsOverbalanceIncluded = product.IsOverbalanceIncluded,
                Price = product.Price,
                ProductNumber = product.ProductNumber,
                Status = product.Status,
                Categories = product.Categories,
                User = product.User
            };
            return viewModel;
        }

        public static Product ProductRegNumViewModelToProduct(this ProductRegNumViewModel viewModel )
        {
            //some functional
            return new Product();
        }


        public static ProductRegNumViewModel ProductToProductRegNumViewModel(this Product product )
        {
            ProductRegNumViewModel viewModel = new ProductRegNumViewModel()
            {
                
                        Num1 = product.ProductNumber.Remove(1, product.ProductNumber.Length - 1).ToUpper(),
                        Num2 = product.ProductNumber.Substring(1, 1).ToUpper(),
                        Num3 = product.ProductNumber.Substring(2, 1).ToUpper(),
                        Num4 = product.ProductNumber.Substring(3, 1).ToUpper(),
                        Num5 = product.ProductNumber.Substring(4, 1).ToUpper(),
                        Num6 = product.ProductNumber.Substring(5, 1).ToUpper(),
                        Num7 = product.ProductNumber.Remove(0, 6).ToUpper(),
                        IsOverbalanceIncluded = product.IsOverbalanceIncluded,
                        Price = product.Price,
                        //Description = product."test",
                        ProductID = product.ProductID
            };
            //some functional
            return viewModel;
        }
        //    

        public static User ViewModelToUser(this UserViewModel viewModel)
        {
            User user = new User()
            {
                UserID = viewModel.UserID,
                RoleID = viewModel.SelectedRoleID,
                Login = viewModel.Login,
                Password = viewModel.Password,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                UserName = viewModel.UserName,
                Created = viewModel.Created,
                IsActivated = viewModel.IsActivated,
                NewEmailKey = viewModel.NewEmailKey,
                PasswordSalt = viewModel.PasswordSalt
            };
            return user;
        }

        public static UserViewModel UserToViewModel(this User user)
        {
            UserViewModel viewModel = new UserViewModel()
            {
                UserID = user.UserID,
                Login = user.Login,
                Password = user.Password,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Created = user.Created,
                IsActivated = user.IsActivated,
                NewEmailKey = user.NewEmailKey,
                PasswordSalt = user.PasswordSalt,
                RoleName = user.Role.RoleName,
                SelectedRoleID = user.RoleID
            };
            return viewModel;
        }

        public static CarNumber NumberToCarNumber(this string number)
        {

            return new CarNumber
            {
                Num1 = number.Remove(1, number.Length - 1).ToUpper(),
                Num2 = number.Substring(1, 1).ToUpper(),
                Num3 = number.Substring(2, 1).ToUpper(),
                Num4 = number.Substring(3, 1).ToUpper(),
                Num5 = number.Substring(4, 1).ToUpper(),
                Num6 = number.Substring(5, 1).ToUpper(),
                Num7 = number.Remove(0, 6).ToUpper()
                
            }; 
        }


         public static Order ViewModelToOrder(this OrderViewModel viewModel)
        {
            Order order = new Order()
            {
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                ProductNumber = viewModel.ProductNumber,
                OrderID = viewModel.OrderID,
                Name = viewModel.Name,
                StartDate = viewModel.StartDate,
                Comment = viewModel.Comment,
                IsForSale = viewModel.IsForSale
             };
            return order;
        }

        public static OrderViewModel OrderToViewModel(this Order order)
        {
            CarNumber carNumber = order.ProductNumber.NumberToCarNumber();
            OrderViewModel viewModel = new OrderViewModel()
            {
                Num1 = carNumber.Num1,
                    Num2 = carNumber.Num2,
                    Num3 = carNumber.Num3,
                    Num4 = carNumber.Num4,
                    Num5 = carNumber.Num5,
                    Num6 = carNumber.Num6,
                    Num7 = carNumber.Num7,
                    Comment = order.Comment,
                    Email = order.Email,
                    Name = order.Name,
                    OrderID = order.OrderID,
                    Phone = order.Phone,
                    ProductNumber = order.ProductNumber,
                    StartDate = order.StartDate,
                    IsForSale = order.IsForSale
            };
            return viewModel;
        }



            





        public static MvcHtmlString MenuItem(this HtmlHelper helper,
           string linkText, string actionName, string controllerName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

            var builder = new TagBuilder("li");
            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
                && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("selected");
            builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName).ToHtmlString();
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ActionQueryLink(this HtmlHelper htmlHelper,
            string linkText, string action, object routeValues)
        {
            var newRoute = routeValues == null
                ? htmlHelper.ViewContext.RouteData.Values
                : new RouteValueDictionary(routeValues);

            newRoute = htmlHelper.ViewContext.HttpContext.Request.QueryString
                .ToRouteDic(newRoute);

            return HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext,
                htmlHelper.RouteCollection, linkText, null,
                action, null, newRoute, null).ToMvcHtml();
        }

        public static MvcHtmlString ToMvcHtml(this string content)
        {
            return MvcHtmlString.Create(content);
        }

        public static RouteValueDictionary ToRouteDic(this NameValueCollection collection)
        {
            return collection.ToRouteDic(new RouteValueDictionary());
        }

        public static RouteValueDictionary ToRouteDic(this NameValueCollection collection,
            RouteValueDictionary routeDic)
        {
            foreach (string key in collection.Keys)
            {
                if (!routeDic.ContainsKey(key))
                    routeDic.Add(key, collection[key]);
            }
            return routeDic;
        }
        //------------------

    }

    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                

                TagBuilder tag = new TagBuilder("A");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml =  i.ToString();


                TagBuilder tagLi = new TagBuilder("li");
                if (i == pagingInfo.CurrentPage)
                {
                    tagLi.AddCssClass("active");
                }
                
                tagLi.InnerHtml = tag.ToString();

                /*if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");*/
                
                result.Append(tagLi.ToString());

                /*TagBuilder tag = new TagBuilder("A");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml =  i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");

                result.Append(tag.ToString());*/

            }
            return MvcHtmlString.Create(result.ToString());
        }
    }

    
 


    public static class ActionLinksHelper
    {
        public static IHtmlString SpanActionLink(this AjaxHelper helper, string linkText,
  string classText, /*int num,*/ string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions,
        object htmlAttributes)
        {
            var builder = new TagBuilder("span");
            builder.MergeAttribute("class", classText);
            /*builder.InnerHtml = num.ToString();*/
            var link = helper.ActionLink(linkText + "temp", actionName, routeValues, ajaxOptions,
                                   htmlAttributes).ToHtmlString();
            return new MvcHtmlString(link.Replace("temp", builder.ToString()));
        }

    }



    /*
     Цена", "List", "Product", new { sortOption = "priceDesc", category = Model.CurrentCategory, searcher = searcher }, 
     */
    //public static MvcHtmlString ActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes);
    /*
       public static MvcHtmlString ActionQueryLink(this HtmlHelper htmlHelper,
            string linkText, string action, object routeValues)
        {
            var newRoute = routeValues == null
                ? htmlHelper.ViewContext.RouteData.Values
                : new RouteValueDictionary(routeValues);

            newRoute = htmlHelper.ViewContext.HttpContext.Request.QueryString
                .ToRouteDic(newRoute);

            return HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext,
                htmlHelper.RouteCollection, linkText, null,
                action, null, newRoute, null).ToMvcHtml();
        }
     */

    public static class MyExtensionMethods
    {
        // Изменение размеров картинки
        public static Image Resize(this Image img, int iMaxHeight, int iMaxWidth)
        {
            int iDestHeight = 0;
            int iDestWidth = 0;

            // Определяем новые размеры картинки.
            // Если она меньше максимального размера, то оставляем её без изменения.
            // Если хотя бы по одному измерению больше максимального размера,
            // то уменьшаем её пропорционально до максимального размера.
            if ((iMaxHeight == 0 || iMaxHeight >= img.Height) && (iMaxWidth == 0 || iMaxWidth >= img.Width)) return img;
            else
            {
                if (iMaxHeight == 0 && iMaxWidth > 0)
                {
                    iDestWidth = iMaxWidth;
                    iDestHeight = img.Height*iMaxWidth/img.Width;
                }

                if (iMaxHeight > 0 && iMaxWidth == 0)
                {
                    iDestHeight = iMaxHeight;
                    iDestWidth = img.Width*iMaxHeight/img.Height;
                }

                if (iMaxHeight > 0 && iMaxWidth > 0)
                {
                    iDestWidth = iMaxWidth;
                    iDestHeight = img.Height*iMaxWidth/img.Width;

                    if (iDestHeight > iMaxHeight)
                    {
                        iDestHeight = iMaxHeight;
                        iDestWidth = img.Width*iMaxHeight/img.Height;
                    }
                }


                Image originalImage = img;
                Bitmap newImage = new Bitmap(iDestWidth, iDestHeight);

                using (Graphics g = Graphics.FromImage(newImage))
                {
                    //--Quality Settings Adjust to fit your application
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);
                    return newImage;
                }


                //  return new Bitmap(img, new Size(iDestWidth, iDestHeight));
            }
        }


        // Сохранение картинки на диск одновременно с изменением размеров
        public static void ResizeAndSave(this HttpPostedFileBase imagefile, int iMaxHeight, int iMaxWidth,
            string strSavePath)
        {
            if (imagefile != null)
            {
                ImageFormat format = ImageFormat.Bmp;
                string strExtension = Path.GetExtension(strSavePath);

                switch (strExtension.ToLower())
                {
                    case ".gif":
                        format = ImageFormat.Gif;
                        break;

                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;

                    case ".jpeg":
                        format = ImageFormat.Jpeg;
                        break;

                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }

                //Image.FromStream(imagefile.InputStream).Resize(iMaxHeight, iMaxWidth).Save(strSavePath, format);
                // Image.FromStream(imagefile.InputStream).Resize(iMaxHeight, iMaxWidth).Save(strSavePath, format);



            }
        }

        //Image Resize Helper Method
        public static void ResizeImage(this HttpPostedFileBase imagefile, int maxWidth, int maxHeight,
            string strSavePath)
        {

            if (imagefile != null)
            {
                ImageFormat format = ImageFormat.Bmp;
                string strExtension = Path.GetExtension(strSavePath);

                switch (strExtension.ToLower())
                {
                    case ".gif":
                        format = ImageFormat.Gif;
                        break;

                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;

                    case ".jpeg":
                        format = ImageFormat.Jpeg;
                        break;

                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }

                //using (Image originalImage = Image.FromFile(filename))
                using (Image originalImage = Image.FromStream(imagefile.InputStream))
                {
                    int iDestHeight = 0;
                    int iDestWidth = 0;

                    // Определяем новые размеры картинки.
                    // Если она меньше максимального размера, то оставляем её без изменения.
                    // Если хотя бы по одному измерению больше максимального размера,
                    // то уменьшаем её пропорционально до максимального размера.
                    if ((maxHeight == 0 || maxHeight >= originalImage.Height) &&
                        (maxWidth == 0 || maxWidth >= originalImage.Width))
                    {



                        Bitmap newImage0 = new Bitmap(originalImage.Width, originalImage.Height);

                        using (Graphics g = Graphics.FromImage(newImage0))
                        {
                            //--Quality Settings Adjust to fit your application
                            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            //g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);
                            g.InterpolationMode = InterpolationMode.High;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Color.Transparent);
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            g.DrawImage(originalImage, new Rectangle(0, 0, newImage0.Width, newImage0.Height),
                                        new Rectangle(iDestWidth, iDestHeight, originalImage.Width, originalImage.Height),
                                        GraphicsUnit.Pixel);

                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawImage(originalImage, 0, 0, newImage0.Width, newImage0.Height);

                            newImage0.Save(strSavePath, format);
                        }

                    }
                    else
                    {
                        if (maxHeight == 0 && maxWidth > 0)
                        {
                            iDestWidth = maxHeight;
                            iDestHeight = originalImage.Height*maxHeight/originalImage.Width;
                        }

                        if (maxHeight > 0 && maxWidth == 0)
                        {
                            iDestHeight = maxHeight;
                            iDestWidth = originalImage.Width*maxHeight/originalImage.Height;
                        }

                        if (maxHeight > 0 && maxWidth > 0)
                        {
                            iDestWidth = maxWidth;
                            iDestHeight = originalImage.Height*maxWidth/originalImage.Width;

                            if (iDestHeight > maxHeight)
                            {
                                iDestHeight = maxHeight;
                                iDestWidth = originalImage.Width*maxHeight/originalImage.Height;
                            }
                        }





                        //Caluate new Size
                        //int newWidth = originalImage.Width;
                        //int newHeight = originalImage.Height;
                        //double aspectRatio = (double) originalImage.Width/(double) originalImage.Height;

                        //if (aspectRatio <= 1 && originalImage.Width > maxWidth)
                        //{
                        //    newWidth = maxWidth;
                        //    newHeight = (int) Math.Round(newWidth/aspectRatio);
                        //}
                        //else if (aspectRatio > 1 && originalImage.Height > maxHeight)
                        //{
                        //    newHeight = maxHeight;
                        //    newWidth = (int) Math.Round(newHeight*aspectRatio);
                        //}

                        Bitmap newImage = new Bitmap(iDestWidth, iDestHeight);

                        using (Graphics g = Graphics.FromImage(newImage))
                        {
                            //--Quality Settings Adjust to fit your application
                            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            //g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);
                            g.InterpolationMode = InterpolationMode.High;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Color.Transparent);
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            g.DrawImage(originalImage, new Rectangle(0, 0, newImage.Width, newImage.Height), new Rectangle(iDestWidth, iDestHeight, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);

                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

                            newImage.Save(strSavePath, format);
                            // return newImage;
                        }
                    }
                }



            }
        }
    }

    public static class QueryExtention
    {
        public static SelectList ToSelectList<T>(this IQueryable<T> query, string dataValueField, string dataTextField, object selectedValue)
        {
            return new SelectList(query, dataValueField, dataTextField, selectedValue ?? -1);
        }

    }


    }








    
