using System.Web.Mvc;

namespace OD.UI.BaseController
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllername = filterContext.RouteData.Values["controller"].ToString();
            string actionname = filterContext.RouteData.Values["action"].ToString();

            if (controllername == "Ogretmen")
            {
                if (Session["OgretmenID"] == null)
                {
                    if (Session["OkulID"] != null)
                    {
                        filterContext.Result = new RedirectResult("~/Okul/Index");
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                }
            }

            if (controllername == "Okul")
            {
                if (Session["OkulID"] == null)
                {
                    if (Session["OgretmenID"] != null)
                    {
                        filterContext.Result = new RedirectResult("~/Ogretmen/Index");
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Home/OkulGirisi");
                        return;
                    }
                }
            }

            if (controllername == "Admin")
            {
                if (Session["AdminID"] == null)
                {
                    if (Session["OkulID"] != null)
                    {
                        filterContext.Result = new RedirectResult("~/Okul/Index");
                        return;
                    }
                    else if (Session["OgretmenID"] != null)
                    {
                        filterContext.Result = new RedirectResult("~/Ogretmen/Index");
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                }
            }
        }
    }
}