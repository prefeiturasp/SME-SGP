using GestaoAcademica.WebApi.Authentication;
using System.Web;
using System.Web.Mvc;

namespace GestaoAcademica.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}