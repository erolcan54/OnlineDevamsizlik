using OD.Bll.Concrete;
using OD.Model;
using OD.UI.LogFolder;
using OD.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OD.UI.Controllers
{
    public class AdminController : BaseController.BaseController
    {
        Fonksiyon f = new Fonksiyon();

        AdminRepository adminrepo = new AdminRepository();
        TaleplerRepository taleprepo = new TaleplerRepository();
        DonemlerRepository donemrepo = new DonemlerRepository();
        OkulRepository okulrepo = new OkulRepository();
        AdminLogRepository adminlogrepo = new AdminLogRepository();
        OgretmenLogRepository ogretmenlogrepo = new OgretmenLogRepository();
        OkulLogRepository okullogrepo = new OkulLogRepository();

        //[LogFolder.AdminLog]
        public ActionResult Index()
        {
            return View();
        }

        //[LogFolder.AdminLog]
        public ActionResult Profil()
        {
            return View();
        }

        //[LogFolder.AdminLog]
        [HttpPost]
        public JsonResult ProfilGuncelle(AdminTBL model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                int id = Convert.ToInt32(Session["AdminID"]);
                AdminTBL admin = adminrepo.Get(id);
                admin.KullaniciAdi = f.Encrypt(model.KullaniciAdi);
                admin.Sifre = f.Encrypt(model.Sifre);
                adminrepo.Update(admin);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Güncelleme işlemi başarılı bir şekilde gerçekleştirildi.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Güncelleme işlemi sırasında bir hata oluştu.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CikisYap()
        {
            Session.Remove("AdminID");
            return RedirectToAction("AdminGirisi", "Home");
        }

        //[LogFolder.AdminLog]
        public ActionResult AktifTalepler()
        {
            List<Talepler> aktiftalep = taleprepo.GetByFilterList(a => a.AktifMi == true);
            return View(aktiftalep);
        }

        //[LogFolder.AdminLog]
        public ActionResult PasifTalepler()
        {
            List<Talepler> pasiftalep = taleprepo.GetByFilterList(a => a.AktifMi == false);
            return View(pasiftalep);
        }

        //[LogFolder.AdminLog]
        public ActionResult Donemler()
        {
            List<Donemler> donemliste = donemrepo.GetAll().OrderByDescending(a => a.DonemID).ToList();
            return View(donemliste);
        }

        //[LogFolder.AdminLog]
        public JsonResult DonemEkle(Donemler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                foreach (var item in donemrepo.GetAll())
                {
                    item.AktifMi = false;
                    donemrepo.Update(item);
                }
                foreach (var i in okulrepo.GetByFilterList(a=>a.AktifMi==true&& a.BitisTarihi>DateTime.Now))
                {
                    i.OgrenciListeYuklediMi = false;
                    okulrepo.Update(i);
                }
                model.Baslama = Convert.ToDateTime(model.Baslama);
                model.Bitis = Convert.ToDateTime(model.Bitis);
                model.AktifMi = true;
                donemrepo.Add(model);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Dönem ekleme işlemi başarılı bir şekilde gerçekleştirildi.";
        }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Dönem ekleme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DonemGuncelleme(int id)
        {
            Donemler donem = donemrepo.Get(id);
            return PartialView("_DonemGuncelle", donem);
        }

        //[LogFolder.AdminLog]
        [HttpPost]
        public JsonResult DonemGuncelleislem(Donemler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Donemler donem = donemrepo.Get(model.DonemID);
                donem.Baslama = Convert.ToDateTime(model.Baslama);
                donem.Bitis = Convert.ToDateTime(model.Bitis);
                donem.DonemAdi = model.DonemAdi;
                donemrepo.Update(donem);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Dönem Güncelleme işlemi başarılı bir şekilde yapıldı.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Güncelleme işlemi sırasında bir hata oluştu.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Talepislem(int id)
        {
            Talepler talep = taleprepo.Get(id);
            return PartialView("_Talepislem", talep);
        }

        //[LogFolder.AdminLog]
        [HttpPost]
        public JsonResult TalepislemYap(int TalepID, int sure)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Talepler talep = taleprepo.Get(TalepID);
                Okullar okul = okulrepo.Get((int)talep.OkulID);
                okul.BaslangicTarihi = DateTime.Now;
                okul.BitisTarihi = DateTime.Now.Date.AddMonths(sure);

                okul.AktifMi = true;
                talep.AktifMi = false;

                okulrepo.Update(okul);
                taleprepo.Update(talep);

                jmodel.IsSuccess = true;
                jmodel.UserMessage = "İşlem başarılı bir şekilde gerçekleştirildi.";
            }
            catch (Exception)
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Güncelleme işlemi sırasında bir hata oluştu.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _OkulDetay(int id)
        {
            Talepler talep = taleprepo.Get(id);
            Okullar okul = okulrepo.Get((int)talep.OkulID);
            return PartialView("_OkulDetay", okul);
        }

        //[LogFolder.AdminLog]
        public ActionResult OkulListesi()
        {
            return View(okulrepo.GetAll().OrderByDescending(a => a.OkulID).ToList());
        }

        //[LogFolder.AdminLog]
        public ActionResult AdminLog()
        {
            return View(adminlogrepo.GetAll());
        }

        //[LogFolder.AdminLog]
        public ActionResult OgretmenLog()
        {
            return View(ogretmenlogrepo.GetAll());
        }

        //[LogFolder.AdminLog]
        public ActionResult OkulLog()
        {
            return View(okullogrepo.GetAll());
        }
    }
}