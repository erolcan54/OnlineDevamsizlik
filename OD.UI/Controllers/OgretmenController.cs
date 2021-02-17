using OD.Bll.Concrete;
using OD.Model;
using OD.UI.LogFolder;
using OD.UI.Utils;
using OD.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OD.UI.Controllers
{

    public class OgretmenController : BaseController.BaseController
    {
        OgretmenRepository ogretmenrepo = new OgretmenRepository();
        OgrenciRepository ogrencirepo = new OgrenciRepository();
        DerslerRepository dersrepo = new DerslerRepository();
        AtananDerslerRepository atanandersrepo = new AtananDerslerRepository();
        DonemlerRepository donemrepo = new DonemlerRepository();
        OkulRepository okulrepo = new OkulRepository();
        DersGruplariRepository dersgruprepo = new DersGruplariRepository();
        YoklamaRepository yoklamarepo = new YoklamaRepository();
        Fonksiyon f = new Fonksiyon();

        [LogFolder.OgretmenLog]
        public ActionResult Index()
        {
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            string okul = ogretmenrepo.Get(OgretmenID).Okullar.Adi;
            ViewBag.Title = okul;
            return View();
        }

        public ActionResult CikisYap()
        {
            Session.Remove("OgretmenID");
            return RedirectToAction("Index", "Home");
        }

        public JsonResult SinifSubeFiltre(int id)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            SinifDersOgrenciListView SinifSube = new SinifDersOgrenciListView();
            SinifSube.SubeGrup = (from i in atanandersrepo.GetByFilterList(a => a.OgretmenID == OgretmenID && a.AktifMi == true && a.Sinif == id && a.DonemID == donem.DonemID).ToList()
                                  orderby i.Sinif
                                  group i by i.Sube into g
                                  select new SelectListItem
                                  {
                                      Text = g.Key.ToString().ToUpper(),
                                      Value = g.Key.ToString()
                                  }).ToList();

            return Json(SinifSube.SubeGrup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SinifDersFiltre(int sinif, string sube)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            SinifDersOgrenciListView SinifDers = new SinifDersOgrenciListView();
            SinifDers.DersListe = (from i in atanandersrepo.GetByFilterList(a => a.OgretmenID == OgretmenID && a.AktifMi == true && a.Sinif == sinif && a.Sube == sube && a.DonemID == donem.DonemID).ToList()
                                   orderby i.Dersler.DersAdi
                                   select new SelectListItem
                                   {
                                       Text = i.Dersler.DersAdi.ToString().ToUpper(),
                                       Value = i.DersID.ToString()
                                   }).ToList();

            return Json(SinifDers.DersListe, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OgretmenLog]
        public ActionResult YoklamaAl(int Sinif = 0, string Sube = null, int DersID = 0, int DersGrupID = 0)
        {
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            SinifDersOgrenciListView model = new SinifDersOgrenciListView();
            //model.SinifGrup = (from i in atanandersrepo.GetByFilterList(a => a.OgretmenID == OgretmenID && a.AktifMi == true).ToList()
            //                   orderby i.Sinif
            //                   group i by i.Sinif into g
            //                   select new SelectListItem
            //                   {
            //                       Text = g.Key.ToString(),
            //                       Value = g.Key.ToString()
            //                   }).ToList();

            //ViewData["DersSaatleri"] = from i in dersgruprepo.GetAll()
            //                           select new SelectListItem
            //                           {
            //                               Value = i.DersGrupID.ToString(),
            //                               Text = i.DersGrupAdi
            //                           };
            Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
            ViewData["YoklamaListe"] = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.DonemID == d.DonemID && a.OgretmenID == OgretmenID).OrderByDescending(a => a.YoklamaID).ToList();

            if (Sinif != 0 && !String.IsNullOrEmpty(Sube) && DersID != 0 && DersGrupID != 0)
            {
                Ogretmenler ogr = ogretmenrepo.Get(OgretmenID);
                model.OgrenciListe = ogrencirepo.GetByFilterList(a => a.Sinif == Sinif && a.Sube == Sube && a.AktifMi == true && a.OkulID == ogr.OkulID).OrderBy(a => a.OgrenciNo).ToList();
                model.DersGrupID = DersGrupID;
                model.DersID = DersID;
                model.Sinif = Sinif;
                model.Sube = Sube;
                model.DersAdi = dersrepo.Get(DersID).DersAdi;
                model.DersGrupAdi = dersgruprepo.Get(DersGrupID).DersGrupAdi;
                model.FiltreUygulandıMı = true;
                model.Tarih = DateTime.Now;
            }

            return View(model);
        }

        [HttpPost]
        [LogFolder.OgretmenLog]
        public JsonResult YoklamaKaydet(YoklamaViewModel model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
                Ogretmenler ogretmen = ogretmenrepo.Get(Convert.ToInt32(Session["OgretmenID"].ToString()));
                List<Yoklamalar> yoklist = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.DersGrupID == model.DersGrupID && a.DonemID == d.DonemID && a.OkulID == ogretmen.OkulID && a.Sinif == model.Sinif && a.Sube == model.Sube).OrderByDescending(a=>a.YoklamaID).ToList();
                Yoklamalar varmi = (from i in yoklist
                                    where Convert.ToDateTime(i.Tarih).Date == Convert.ToDateTime(model.Tarih).Date
                                    select i).FirstOrDefault();
                
                bool durum = false;
                if (varmi != null)
                {
                    DateTime tarih1 = (DateTime)varmi.Tarih;
                    DateTime tarih2 = (DateTime)model.Tarih;
                    if (tarih1.Date == tarih2.Date)
                    { durum = true; }
                    else
                    { durum = false; }
                }
                if (durum == false)
                {
                    Yoklamalar y = new Yoklamalar();
                    string[] ogridliste = model.OgrenciIDListe;
                    if (model.OgrenciIDListe != null)
                    {
                        for (int i = 0; i < ogridliste.Length; i++)
                        {
                            y.OgrenciIDListe += ogridliste[i];
                            if (i + 1 < ogridliste.Length)
                            {
                                y.OgrenciIDListe += ",";
                            }
                        }
                    }
                    y.AktifMi = true;
                    y.DersGrupID = model.DersGrupID;
                    y.DersID = model.DersID;
                    y.DonemID = d.DonemID;
                    y.OgretmenID = ogretmen.OgretmenID;
                    y.OkulID = ogretmen.OkulID;
                    y.Sinif = model.Sinif;
                    y.Sube = model.Sube;
                    y.Tarih = model.Tarih;
                    yoklamarepo.Add(y);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Yoklama alınmıştır.";
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = varmi.Tarih + " tarihinde ," + varmi.Sinif + "/" + varmi.Sube + " sınıfı " + varmi.Dersler.DersAdi + " dersine ait " + varmi.DersGruplari.DersGrupAdi + "  saati için zaten yoklama alınmış.";
                }
            }
            catch (Exception)
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Yoklama alırken bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }

            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _YoklamaFiltreModal()
        {
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            SinifDersOgrenciListView model = new SinifDersOgrenciListView();
            model.SinifGrup = (from i in atanandersrepo.GetByFilterList(a => a.OgretmenID == OgretmenID && a.AktifMi == true).ToList()
                               orderby i.Sinif
                               group i by i.Sinif into g
                               select new SelectListItem
                               {
                                   Text = g.Key.ToString(),
                                   Value = g.Key.ToString()
                               }).ToList();

            ViewData["DersSaatleri"] = from i in dersgruprepo.GetAll()
                                       select new SelectListItem
                                       {
                                           Value = i.DersGrupID.ToString(),
                                           Text = i.DersGrupAdi
                                       };
            model.Sinif = 0;
            model.Sube = "0";
            model.DersGrupID = 0;
            model.DersID = 0;
            return PartialView("_YoklamaFiltreModal", model);
        }

        [LogFolder.OgretmenLog]
        public ActionResult Profil()
        {
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            Ogretmenler ogrtmn = ogretmenrepo.Get(OgretmenID);
            return View(ogrtmn);
        }

        public ActionResult _OgretmenBilgileri()
        {
            int OgretmenID = Convert.ToInt32(Session["OgretmenID"]);
            Ogretmenler ogrtmn = ogretmenrepo.Get(OgretmenID);
            return PartialView("_OgretmenBilgileri", ogrtmn);
        }

        [LogFolder.OgretmenLog]
        [HttpPost]
        public JsonResult OgretmenGuncelle(Ogretmenler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogretmenler o = ogretmenrepo.Get(model.OgretmenID);
                o.AdSoyad = model.AdSoyad;
                o.KullaniciAdi = f.Encrypt(model.KullaniciAdi);
                o.Sifre = f.Encrypt(model.Sifre);
                ogretmenrepo.Update(o);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Güncelleme başarılı bir şekilde yapıldı.";
            }
            catch 
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Güncelleme işlemi sırasında bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyin.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

    }
}