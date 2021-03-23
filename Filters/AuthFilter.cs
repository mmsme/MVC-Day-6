using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Day_6.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["Email"] == null)
            {
                filterContext.Result = new RedirectResult("~/students/login");
            }
        }
    }
}