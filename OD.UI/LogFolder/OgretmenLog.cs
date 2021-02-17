using OD.Bll.Concrete;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace OD.UI.LogFolder
{
    public class OgretmenLog:ActionFilterAttribute
    {
        OgretmenLogRepository ogretmenlogrepo = new OgretmenLogRepository();
        private Stopwatch _stopwatch;

        public OgretmenLog()
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
            int userId = int.Parse(filterContext.HttpContext.Session["OgretmenID"].ToString());
            
            OD.Model.OgretmenLog l = new OD.Model.OgretmenLog();
            l.Date = DateTime.Now;
            l.ExecutionMs = _stopwatch.ElapsedMilliseconds.ToString(); //çalışma süresi
            l.IPAddress = filterContext.HttpContext.Request.UserHostAddress;
            l.Url = filterContext.HttpContext.Request.RawUrl; //erişilen sayfanın ham url'i
            l.OgretmenID = userId;
            l.ActionName = filterContext.ActionDescriptor.ActionName;
            l.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            ogretmenlogrepo.Add(l);

            base.OnActionExecuted(filterContext);   //işlem devam etsin
        }
    }
}