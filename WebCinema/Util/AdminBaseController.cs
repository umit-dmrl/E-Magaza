using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCinema.Util
{
    public class AdminBaseController : System.Web.Mvc.Controller
    {
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (Session["user_admin"] == null)
            {
                filterContext.Result = new RedirectResult("~/Admin");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}