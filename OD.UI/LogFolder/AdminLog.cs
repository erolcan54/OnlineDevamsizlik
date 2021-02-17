using OD.Bll.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OD.UI.LogFolder
{
    public class AdminLog: ActionFilterAttribute
    {
        AdminLogRepository adminlogrepo = new AdminLogRepository();
        private Stopwatch _stopwatch;

        public AdminLog()
        {
            _stopwatch = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();  //kronometreyi durdur

            //Log classının alanlarını doldur
            int userId = int.Parse(filterContext.HttpContext.Session["AdminID"].ToString());

            OD.Model.AdminLog l = new OD.Model.AdminLog();
            l.Date = DateTime.Now;
            l.ExecutionMs = _stopwatch.ElapsedMilliseconds.ToString(); //çalışma süresi
            l.IPAddress = filterContext.HttpContext.Request.UserHostAddress;
            l.Url = filterContext.HttpContext.Request.RawUrl; //erişilen sayfanın ham url'i
            l.AdminID = userId;
            l.ActionName = filterContext.ActionDescriptor.ActionName;
            l.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            adminlogrepo.Add(l);

            base.OnActionExecuted(filterContext);   //işlem devam etsin
        }
    }
}