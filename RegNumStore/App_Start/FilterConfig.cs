using System.Web.Mvc;

namespace RegnumStore
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
          //  filters.Add(new RequireHttpsAttribute());
        }
    }
}