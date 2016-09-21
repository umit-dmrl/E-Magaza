using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCinema.Util
{
    public class BaseController : System.Web.Mvc.Controller
    {
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            //string controllerName = filterContext.RouteData.Values["controller"].ToString();
            //Bu kısımda gelen controller ismini elde ettik eğer ileride bir controller ' a oturum kontrolü yaptırıp
            //diğer kontroller ın erişimini  direk açmak istiyorsak if şartları ile şu kontroller da oturumu kontrol et bunda etme
            // gibi bir mantık kurabilirsin.

            if (Session["username"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}