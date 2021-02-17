using OD.Bll.Concrete;
using OD.Model;
using OD.UI.Report;
using OD.UI.Utils;
using OD.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.IO;

namespace OD.UI.Controllers
{
    public class OkulController : BaseController.BaseController
    {
        OgretmenRepository ogretmenrepo = new OgretmenRepository();
        OgrenciRepository ogrencirepo = new OgrenciRepository();
        DerslerRepository dersrepo = new DerslerRepository();
        AtananDerslerRepository atanandersrepo = new AtananDerslerRepository();
        DonemlerRepository donemrepo = new DonemlerRepository();
        OkulRepository okulrepo = new OkulRepository();
        YoklamaRepository yoklamarepo = new YoklamaRepository();
        DersGruplariRepository dersgruprepo = new DersGruplariRepository();
        OgretmenLogRepository ogretmenlogrepo = new OgretmenLogRepository();
        Fonksiyon f = new Fonksiyon();

        [LogFolder.OkulLog]
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["OkulID"]);
            ViewBag.OgrenciSayisi = ogrencirepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id).Count();
            ViewBag.OgretmenSayisi = ogretmenrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id).Count();
            ViewBag.DersSayisi = dersrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id).Count();
            List<Yoklamalar> BugunYoklama = (from i in yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id)
                                             where Convert.ToDateTime(i.Tarih).Date == DateTime.Now.Date
                                             orderby i.Sinif, i.Sube, i.DersGrupID
                                             select i).ToList();
            ViewBag.BugunYoklamaSayisi = BugunYoklama.Count();
            ViewBag.ToplamYoklamaSayisi = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id).Count();
            List<OgretmenLogViewModel> OgretmenLogListe = (from i in ogretmenlogrepo.GetByFilterList(a => a.Ogretmenler.OkulID == id)
                                                           where Convert.ToDateTime(i.Date).Date == DateTime.Now.Date
                                                           group i by i.OgretmenID
                                                  into g
                                                           select new OgretmenLogViewModel
                                                           {
                                                               OgretmenID = Convert.ToInt32(g.Key.ToString()),
                                                               AdSoyad = g.Select(n => n.Ogretmenler.AdSoyad).FirstOrDefault(),
                                                               Tarih = g.Select(n => n.Date).FirstOrDefault().ToString(),
                                                           }).ToList();
            ViewData["OgretmenLogListe"] = OgretmenLogListe;
            return View(BugunYoklama);
        }

        public ActionResult CikisYap()
        {
            Session.Remove("OkulID");
            return RedirectToAction("OkulGirisi", "Home");
        }

        [LogFolder.OkulLog]
        public ActionResult Ogretmenler()
        {
            ViewBag.Donem = donemrepo.GetByFilter(a => a.AktifMi == true).DonemAdi;
            int id = Convert.ToInt32(Session["OkulID"]);
            List<Ogretmenler> ogretmenler = ogretmenrepo.GetByFilterList(a => a.OkulID == id && a.AktifMi == true).OrderByDescending(a => a.OgretmenID).ToList();
            return View(ogretmenler);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgretmenEkle(Ogretmenler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                if (f.OgretmenKullaniciAdiVarMi(f.Encrypt(model.KullaniciAdi)) == false)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Aynı kullanıcı adına sahip başka bir öğretmen var. Lütfen kullanıcı adınızı değiştiriniz.";
                }
                else
                {
                    model.KayitTarihi = DateTime.Now;
                    model.AktifMi = true;
                    model.OkulID = Convert.ToInt32(Session["OkulID"]);
                    model.KullaniciAdi = f.Encrypt(model.KullaniciAdi);
                    model.Sifre = f.Encrypt(model.Sifre);
                    model.DonemID = donemrepo.GetByFilter(a => a.AktifMi == true).DonemID;
                    ogretmenrepo.Add(model);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = model.AdSoyad + " isimli öğretmene ait bilgiler başarılı bir şekilde eklendi.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğretmen bilgilerini eklerken bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        public ActionResult Dersler()
        {
            ViewBag.Donem = donemrepo.GetByFilter(a => a.AktifMi == true).DonemAdi;
            int id = Convert.ToInt32(Session["OkulID"]);
            List<Dersler> dersler = dersrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == id).OrderByDescending(a => a.DersAdi).ToList();
            return View(dersler);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult DersEkle(Dersler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {

                model.DonemID = donemrepo.GetByFilter(a => a.AktifMi == true).DonemID;
                model.AktifMi = true;
                model.OkulID = Convert.ToInt32(Session["OkulID"]);
                dersrepo.Add(model);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Ders Ekleme işlemi başarılı.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Ders ekleme işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult DersSil(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Dersler ders = dersrepo.Get(id);
                ders.AktifMi = false;
                dersrepo.Update(ders);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Ders Silme işlemi başarılı.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Ders silme işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DersGuncelle(int id)
        {
            Dersler ders = dersrepo.Get(id);
            return PartialView("_DersGuncelle", ders);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult DersGuncelleislem(Dersler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Dersler ders = dersrepo.Get(model.DersID);
                ders.DersAdi = model.DersAdi;
                dersrepo.Update(ders);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Ders güncelleme işlemi başarılı bir şekilde gerçekleştirildi.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Ders güncelleme işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        public ActionResult _OgretmenDetay(int id)
        {
            Ogretmenler ogretmen = ogretmenrepo.Get(id);
            //ViewData["AtananDersler"] = atanandersrepo.GetByFilterList(a => a.OgretmenID == ogretmen.OgretmenID).OrderBy(a => a.Dersler.DersAdi).ToList();
            return PartialView("_OgretmenDetay", ogretmen);
        }

        [LogFolder.OkulLog]
        public ActionResult _OgretmenGuncelle(int id)
        {
            ViewBag.Donem = donemrepo.GetByFilter(a => a.AktifMi == true).DonemAdi;
            Ogretmenler ogretmen = ogretmenrepo.Get(id);
            return PartialView("_OgretmenGuncelle", ogretmen);
        }

        [LogFolder.OkulLog]
        [HttpPost]
        public JsonResult OgretmenGuncelleislem(Ogretmenler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogretmenler o = ogretmenrepo.GetByFilter(a => a.KullaniciAdi == f.Decrypt(model.KullaniciAdi));
                if (o != null)
                {
                    Ogretmenler ogr = ogretmenrepo.Get(model.OgretmenID);
                    ogr.AdSoyad = model.AdSoyad;
                    ogr.KullaniciAdi = f.Encrypt(model.KullaniciAdi);
                    ogr.Sifre = f.Encrypt(model.Sifre);
                    ogretmenrepo.Update(ogr);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğretmen bilgileri başarılı bir şekilde güncellendi.";
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = model.KullaniciAdi + " kullanıcı adına sahip başka bir öğretmenimiz zaten var. Lütfen başka bir kullanıcı adı belirleyiniz.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğretmen bilgilerini güncelleme işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgretmenSil(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogretmenler ogr = ogretmenrepo.Get(id);
                ogr.AktifMi = false;
                ogretmenrepo.Update(ogr);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Öğretmen bilgileri başarılı bir şekilde silindi.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğretmen bilgilerini silme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgretmenYeniDonemAktar(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogretmenler ogr = ogretmenrepo.Get(id);
                Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
                if (ogr.DonemID == donem.DonemID)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Öğretmenimizin, <strong>" + donem.DonemAdi + "</strong> dönemi için durumu zaten aktif.";
                }
                else
                {
                    ogr.DonemID = donem.DonemID;
                    ogr.AktifMi = true;
                    ogretmenrepo.Update(ogr);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğretmenimizin bilgileri aktif döneme aktarıldı.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "İşlem sırasında bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult DersYeniDonemAktar(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Dersler ders = dersrepo.Get(id);
                Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
                if (ders.DonemID == donem.DonemID)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Ders, <strong>" + donem.DonemAdi + "</strong> dönemi için durumu zaten aktif.";
                }
                else
                {
                    ders.DonemID = donem.DonemID;
                    ders.AktifMi = true;
                    dersrepo.Update(ders);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Ders bilgileri aktif döneme aktarıldı.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "İşlem sırasında bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        public ActionResult DersAtamalari(int Sinif = 0)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            ViewBag.Donem = donem.DonemAdi;
            int okulid = Convert.ToInt32(Session["OkulID"]);
            List<Ogretmenler> ogrliste = ogretmenrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == donem.DonemID).OrderBy(a => a.AdSoyad).ToList();
            ViewData["OgretmenListe"] = ogrliste;
            List<Dersler> dersliste = dersrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == donem.DonemID).OrderBy(a => a.DersAdi).ToList();
            ViewData["DersListe"] = dersliste;
            //var sinifgrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
            //                 group i by i.Sinif into g
            //                 select new SelectListItem
            //                 {
            //                     Text = g.Key.ToString(),
            //                     Value = g.Key.ToString()
            //                 }).ToList();
            //ViewBag.Siniflar = sinifgrup;

            SinifSubeDersGrupView SinifSube = new SinifSubeDersGrupView();
            SinifSube.SinifGrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
                                   orderby i.Sinif
                                   group i by i.Sinif into g
                                   select new SelectListItem
                                   {
                                       Text = g.Key.ToString(),
                                       Value = g.Key.ToString()
                                   }).ToList();

            return View(SinifSube);
        }

        public JsonResult SinifSubeFiltre(int id)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            int okulid = Convert.ToInt32(Session["OkulID"]);
            SinifSubeDersGrupView SinifSube = new SinifSubeDersGrupView();
            SinifSube.SubeGrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true && a.Sinif == id && a.DonemID == donem.DonemID).ToList()
                                  orderby i.Sinif
                                  group i by i.Sube into g
                                  select new SelectListItem
                                  {
                                      Text = g.Key.ToString().ToUpper(),
                                      Value = g.Key.ToString()
                                  }).ToList();

            return Json(SinifSube.SubeGrup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SinifDersFiltre(int id)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            int okulid = Convert.ToInt32(Session["OkulID"]);
            SinifSubeDersGrupView SinifDers = new SinifSubeDersGrupView();
            SinifDers.DersListe = (from i in dersrepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true && a.SinifDuzey == id && a.DonemID == donem.DonemID).ToList()
                                   orderby i.DersAdi
                                   select new SelectListItem
                                   {
                                       Text = i.DersAdi.ToString().ToUpper(),
                                       Value = i.DersID.ToString()
                                   }).ToList();

            return Json(SinifDers.DersListe, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult DersAtamasiYap(AtananDersler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {

                if (model.Sinif == 0 || model.Sinif == null)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Sınıf seçimi yapmadınız. ";
                }
                else if (String.IsNullOrEmpty(model.Sube) || model.Sube == "0")
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Şube seçimi yapmadınız. ";
                }
                else if (model.DersID == 0)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Ders seçimi yapmadınız. ";
                }
                else
                {
                    Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
                    model.DonemID = donem.DonemID;
                    model.AktifMi = true;
                    atanandersrepo.Add(model);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Ders ataması başarılı bir şekilde yapıldı.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Ders atama işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        public ActionResult Ogrenciler()
        {
            int okulid = Convert.ToInt32(Session["OkulID"]);
            Okullar okul = okulrepo.Get(okulid);
            ViewBag.Okul = okul;
            List<Ogrenciler> ogrenciliste = ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true);
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            ViewBag.Donem = donem;
            //var sinifgrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi==true).ToList()
            //                group i by i.Sinif into g
            //                select new SelectListItem
            //                {
            //                    Text = g.Key.ToString(),
            //                    Value=g.Key.ToString()
            //                }).ToList();
            //ViewBag.Siniflar = sinifgrup;
            return View(ogrenciliste);
        }

        [LogFolder.OkulLog]
        [HttpPost]
        public JsonResult OgrenciListeYukle(HttpPostedFileBase excelFile)
        {
            JsonResultModel jmodel = new JsonResultModel();
            //try
            //{
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Lütfen dosya seçimi yapınız.";
            }
            else
            {
                //Dosyanın uzantısı xls ya da xlsx ise;
                if (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))
                {
                    string dosyaadi = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "-" + Session["OkulID"].ToString() + "-" + excelFile.FileName;
                    //Seçilen dosyanın nereye yükleneceği seçiliyor.
                    string path = string.Concat(Server.MapPath("~/UploadExcel/" + dosyaadi));

                    //Dosya kontrol edilir, varsa silinir.
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}

                    //Excel path altına kaydedilir.
                    excelFile.SaveAs(path);

                    FileStream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader;

                    //Gönderdiğim dosya xls'mi xlsx formatında mı? kontrol ediliyor.
                    if (Path.GetExtension(path).ToUpper() == ".XLS")
                    {
                        //Binary üzerinden excel okuması yapılıyor (Excel 97-2003 *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        //Openxml üzerinden excel okuması yapılıyor (Excel 2007 *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);

                    List<Ogrenciler> localList = new List<Ogrenciler>();
                    int counter = 0;
                    while (excelReader.Read())
                    {
                        counter++;
                        if (counter > 1)
                        {
                            Ogrenciler lm = new Ogrenciler();

                            lm.OgrenciNo = Convert.ToString(excelReader.GetDouble(0));
                            lm.Adi = excelReader.GetString(1).ToString();
                            lm.Soyadi = excelReader.GetString(2).ToString();
                            lm.Sinif = Convert.ToInt32(excelReader.GetDouble(3).ToString());
                            lm.Sube = excelReader.GetString(4).ToString();
                            lm.Sube = lm.Sube.ToUpper();
                            lm.OkulID = Convert.ToInt32(Session["OkulID"]);
                            lm.KayitTarihi = DateTime.Now;
                            lm.DonemID = donem.DonemID;
                            lm.AktifMi = true;
                            localList.Add(lm);
                        }
                    }
                    //Okuma bitiriliyor.
                    excelReader.Close();

                    foreach (var item in localList)
                    {
                        ogrencirepo.Add(item);
                    }

                    Okullar okul = okulrepo.Get(Convert.ToInt32(Session["OkulID"]));
                    okul.OgrenciListeYuklediMi = true;
                    okulrepo.Update(okul);

                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğrenci listesi veritabınına başarılı bir şekilde aktarıldı.";
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Dosya tipiniz yanlış, lütfen '.xls' yada '.xlsx' uzantılı dosya yükleyiniz.";
                }
            }
            //}
            //catch (Exception e)
            //{
            //    jmodel.IsSuccess = false;
            //    //jmodel.UserMessage = "Yükleme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            //    jmodel.UserMessage = e.ToString();
            //}
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgrenciEkle(Ogrenciler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogrenciler varmi = ogrencirepo.GetByFilter(a => a.Sinif == model.Sinif && a.Sube == model.Sube && a.OgrenciNo == model.OgrenciNo);
                if (varmi == null)
                {
                    int donemid = donemrepo.GetByFilter(a => a.AktifMi == true).DonemID;
                    Ogrenciler ogr = new Ogrenciler();
                    ogr.OgrenciNo = model.OgrenciNo;
                    ogr.Adi = model.Adi;
                    ogr.AktifMi = true;
                    ogr.DonemID = donemid;
                    ogr.KayitTarihi = DateTime.Now;
                    ogr.OkulID = Convert.ToInt32(Session["OkulID"]);
                    ogr.Sinif = model.Sinif;
                    ogr.Soyadi = model.Soyadi;
                    ogr.Sube = model.Sube;
                    ogrencirepo.Add(ogr);

                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğrenci bilgileri başarılı bir şekilde eklendi.";
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Belirtilen sınıf ve şubede " + model.OgrenciNo + " numaralı öğrenci zaten var.";
                }

            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğrenci bilgilerini eklerken bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgrenciSil(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogrenciler ogr = ogrencirepo.Get(id);
                ogr.AktifMi = false;
                ogr.SilmeTarihi = DateTime.Now;
                ogrencirepo.Update(ogr);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Öğrenci bilgileri başarılı bir şekilde silindi.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğrenci bilgilerini silme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _OgrenciGuncelle(int id)
        {
            //ViewBag.Donem = donemrepo.GetByFilter(a => a.AktifMi == true).DonemAdi;
            Ogrenciler ogrenci = ogrencirepo.Get(id);
            return PartialView("_OgrenciGuncelle", ogrenci);
        }

        [LogFolder.OkulLog]
        [HttpPost]
        public JsonResult OgrenciGuncelle(Ogrenciler model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogrenciler varmi = ogrencirepo.GetByFilter(a => a.Sinif == model.Sinif && a.Sube == model.Sube && a.OgrenciNo == model.OgrenciNo);
                if (varmi == null)
                {
                    Ogrenciler ogr = ogrencirepo.Get(model.OgrenciID);
                    ogr.Adi = model.Adi;
                    ogr.AktifMi = true;
                    ogr.GuncellemeTarihi = DateTime.Now;
                    ogr.OgrenciNo = model.OgrenciNo;
                    ogr.Sinif = model.Sinif;
                    ogr.Soyadi = model.Soyadi;
                    ogr.Sube = model.Sube;
                    ogrencirepo.Update(ogr);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğrenci bilgileri başarılı bir şekilde güncellendi.";
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Belirtilen sınıf ve şubede " + model.OgrenciNo + " numaralı öğrenci zaten var.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğrenci bilgilerini güncelleme işlemi sırasında bir sorunla karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _OgrenciDetay(int id)
        {
            Ogrenciler ogr = ogrencirepo.Get(id);
            return PartialView("_OgrenciDetay", ogr);
        }

        [HttpPost]
        [LogFolder.OkulLog]
        public JsonResult OgrenciYeniDonemAktar(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Ogrenciler ogr = ogrencirepo.Get(id);
                Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
                if (ogr.DonemID == donem.DonemID)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Öğrencimizin, <strong>" + donem.DonemAdi + "</strong> dönemi için durumu zaten aktif.";
                }
                else
                {
                    ogr.DonemID = donem.DonemID;
                    ogr.AktifMi = true;
                    ogrencirepo.Update(ogr);
                    jmodel.IsSuccess = true;
                    jmodel.UserMessage = "Öğrenci bilgileri aktif döneme aktarıldı.";
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "İşlem sırasında bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _OgretmenDersAta(int id)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            ViewBag.Donem = donem.DonemAdi;
            int okulid = Convert.ToInt32(Session["OkulID"]);
            List<Dersler> dersliste = dersrepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == donem.DonemID).OrderBy(a => a.DersAdi).ToList();
            ViewData["DersListe"] = dersliste;
            //var sinifgrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
            //                 group i by i.Sinif into g
            //                 select new SelectListItem
            //                 {
            //                     Text = g.Key.ToString(),
            //                     Value = g.Key.ToString()
            //                 }).ToList();
            //ViewBag.Siniflar = sinifgrup;

            SinifSubeDersGrupView SinifSube = new SinifSubeDersGrupView();
            SinifSube.SinifGrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
                                   orderby i.Sinif
                                   group i by i.Sinif into g
                                   select new SelectListItem
                                   {
                                       Text = g.Key.ToString(),
                                       Value = g.Key.ToString()
                                   }).ToList();
            Ogretmenler ogretmen = ogretmenrepo.Get(id);
            ViewBag.Ogretmen = ogretmen;
            ViewData["OgretmeneAtananDersler"] = atanandersrepo.GetByFilterList(a => a.OgretmenID == id && a.AktifMi == true && a.DonemID == donem.DonemID);
            return PartialView("_OgretmenDersAta", SinifSube);
        }

        [LogFolder.OkulLog]
        public JsonResult OgretmeneAtananDersler(int id)
        {
            Donemler donem = donemrepo.GetByFilter(a => a.AktifMi == true);
            List<AtananDersler> dersler = atanandersrepo.GetByFilterList(a => a.OgretmenID == id && a.AktifMi == true && a.DonemID == donem.DonemID);
            return Json(new
            {
                Result = from k in dersler
                         orderby k.Sinif, k.Sube, k.Dersler.DersAdi
                         select new
                         {
                             ID = k.AtananDersID,
                             Sinif = k.Sinif,
                             Sube = k.Sube.ToUpper(),
                             DersAdi = k.Dersler.DersAdi
                         }
            }, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        [HttpPost]
        public JsonResult AtananDersSil(int id)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                AtananDersler atd = atanandersrepo.Get(id);
                atd.AktifMi = false;
                atanandersrepo.Update(atd);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Öğretmene atanan ders başarılı bir şekilde silindi.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Öğretmene atanan dersi bilgilerini silme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _AtananDerslerView(Ogretmenler model)
        {
            return PartialView("_AtananDerslerView", model);
        }

        [LogFolder.OkulLog]
        public ActionResult BugunYoklama(int Sinif = 0, string Sube = null)
        {
            bool secimyapildimi = false;
            int okulid = Convert.ToInt32(Session["OkulID"]);
            Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
            SinifSubeDersGrupView SinifSube = new SinifSubeDersGrupView();
            SinifSube.SinifGrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
                                   orderby i.Sinif
                                   group i by i.Sinif into g
                                   select new SelectListItem
                                   {
                                       Text = g.Key.ToString(),
                                       Value = g.Key.ToString()
                                   }).ToList();

            List<YoklamaViewModel> YoklamaListe = new List<YoklamaViewModel>();
            //YoklamaListe = null;
            if (Sinif != 0 && !String.IsNullOrEmpty(Sube))
            {
                secimyapildimi = true;
                List<Yoklamalar> liste = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == d.DonemID && a.Sinif == Sinif && a.Sube == Sube);
                liste = (from i in liste
                         where Convert.ToDateTime(i.Tarih).Date == DateTime.Now.Date
                         orderby i.DersGrupID ascending
                         select i).ToList();
                if (liste.Count > 0)
                {
                    foreach (var i in liste)
                    {
                        YoklamaViewModel m = new YoklamaViewModel();
                        m.AktifMi = i.AktifMi;
                        m.DersGrupID = i.DersGrupID;
                        m.DersID = i.DersID;
                        m.DonemID = i.DonemID;
                        if (i.OgrenciIDListe != null)
                        {
                            m.OgrenciIDListe = i.OgrenciIDListe.Split(',');
                        }
                        m.OgretmenID = i.OgretmenID;
                        m.OkulID = i.OkulID;
                        m.Sinif = i.Sinif;
                        m.Sube = i.Sube;
                        m.Tarih = i.Tarih;
                        m.YoklamaID = i.YoklamaID;
                        YoklamaListe.Add(m);
                    }
                }
                ViewData["Yoklama"] = YoklamaListe;
                ViewBag.Sinif = Sinif;
                ViewBag.Sube = Sube;
            }
            ViewBag.SecimVarmi = secimyapildimi;
            return View(SinifSube);
        }

        [LogFolder.OkulLog]
        public ActionResult YoklamaGuncelle(int YoklamaID)
        {
            int okulid = Convert.ToInt32(Session["OkulID"]);
            Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
            YoklamaViewModel YoklamaModel = new YoklamaViewModel();
            Yoklamalar yoklama = yoklamarepo.GetByFilter(a => a.YoklamaID == YoklamaID && a.OkulID == okulid);
            YoklamaGuncellemeViewModel yoklamaliste = new YoklamaGuncellemeViewModel();
            yoklamaliste.islemTamam = false;
            if (yoklama != null)
            {
                if (yoklama.OgrenciIDListe != null)
                {
                    YoklamaModel.OgrenciIDListe = yoklama.OgrenciIDListe.Split(',');
                }

                List<Ogrenciler> ogrencilist = ogrencirepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == d.DonemID && a.Sinif == yoklama.Sinif && a.Sube == yoklama.Sube);
                List<OgrenciYoklamaCheckBox> ogrenciyoklamachecklist = new List<OgrenciYoklamaCheckBox>();
                yoklamaliste.YoklamaID = yoklama.YoklamaID;
                foreach (var item in ogrencilist)
                {
                    OgrenciYoklamaCheckBox m = new OgrenciYoklamaCheckBox();
                    m.Adi = item.Adi;
                    m.AktifMi = item.AktifMi;
                    m.DonemID = item.DonemID;
                    m.OgrenciID = item.OgrenciID;
                    m.OgrenciNo = item.OgrenciNo;
                    m.OkulID = item.OkulID;
                    m.secildimi = false;
                    m.Sinif = item.Sinif;
                    m.Soyadi = item.Soyadi;
                    m.Sube = item.Sube;
                    ogrenciyoklamachecklist.Add(m);
                }

                if (YoklamaModel.OgrenciIDListe != null)
                {
                    for (int i = 0; i < ogrenciyoklamachecklist.Count(); i++)
                    {
                        for (int j = 0; j < YoklamaModel.OgrenciIDListe.Length; j++)
                        {
                            if (ogrenciyoklamachecklist[i].OgrenciID == Convert.ToInt32(YoklamaModel.OgrenciIDListe[j].ToString()))
                            {
                                ogrenciyoklamachecklist[i].secildimi = true;
                            }
                        }
                    }
                }
                yoklamaliste.liste = ogrenciyoklamachecklist;
                yoklamaliste.islemTamam = true;
                ViewBag.Sinif = yoklama.Sinif;
                ViewBag.Sube = yoklama.Sube;
            }
            else
            {
                yoklamaliste.islemTamam = false;
            }
            return View(yoklamaliste);
        }

        [LogFolder.OkulLog]
        [HttpPost]
        public JsonResult YoklamaGuncelle(YoklamaViewModel model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                Yoklamalar y = yoklamarepo.Get(model.YoklamaID);
                y.OgrenciIDListe = null;
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
                yoklamarepo.Update(y);
                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Güncelleme işlemi başarılı bir şekilde gerçekleşti.";
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [LogFolder.OkulLog]
        public ActionResult YoklamaPDFAktar(int Sinif, string Sube, string title, DateTime Tarih)
        {
            YoklamaReport yoklamaReport = new YoklamaReport();
            byte[] abytes = yoklamaReport.PrepareReport(YoklamaPDFAktarListeGetir(Sinif, Sube, Tarih), Sinif, Sube, Tarih);
            return File(abytes, "application/pdf", title + ".pdf");
        }

        public List<YoklamaViewModel> YoklamaPDFAktarListeGetir(int Sinif, string Sube, DateTime Tarih)
        {
            int okulid = Convert.ToInt32(Session["OkulID"]);
            Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
            List<YoklamaViewModel> YoklamaListe = new List<YoklamaViewModel>();
            List<Yoklamalar> liste = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == d.DonemID && a.Sinif == Sinif && a.Sube == Sube);
            liste = (from i in liste
                     where Convert.ToDateTime(i.Tarih).Date == Tarih.Date
                     orderby i.DersGrupID ascending
                     select i).ToList();
            if (liste.Count > 0)
            {
                foreach (var i in liste)
                {
                    YoklamaViewModel m = new YoklamaViewModel();
                    m.AktifMi = i.AktifMi;
                    m.DersGrupID = i.DersGrupID;
                    m.DersID = i.DersID;
                    m.DonemID = i.DonemID;
                    if (i.OgrenciIDListe != null)
                    {
                        m.OgrenciIDListe = i.OgrenciIDListe.Split(',');
                    }
                    m.OgretmenID = i.OgretmenID;
                    m.OkulID = i.OkulID;
                    m.Sinif = i.Sinif;
                    m.Sube = i.Sube;
                    m.Tarih = i.Tarih;
                    m.YoklamaID = i.YoklamaID;
                    YoklamaListe.Add(m);
                }
            }
            return YoklamaListe;
        }

        [LogFolder.OkulLog]
        public ActionResult FiltreYoklama(int Sinif = 0, string Sube = null, string Tarih = null)
        {
            bool secimyapildimi = false;
            int okulid = Convert.ToInt32(Session["OkulID"]);
            Donemler d = donemrepo.GetByFilter(a => a.AktifMi == true);
            SinifSubeDersGrupView SinifSube = new SinifSubeDersGrupView();
            SinifSube.SinifGrup = (from i in ogrencirepo.GetByFilterList(a => a.OkulID == okulid && a.AktifMi == true).ToList()
                                   orderby i.Sinif
                                   group i by i.Sinif into g
                                   select new SelectListItem
                                   {
                                       Text = g.Key.ToString(),
                                       Value = g.Key.ToString()
                                   }).ToList();

            List<YoklamaViewModel> YoklamaListe = new List<YoklamaViewModel>();
            //YoklamaListe = null;
            if (Sinif != 0 && (!String.IsNullOrEmpty(Sube) || Sube != "0") && !String.IsNullOrEmpty(Tarih))
            {
                secimyapildimi = true;
                List<Yoklamalar> liste = yoklamarepo.GetByFilterList(a => a.AktifMi == true && a.OkulID == okulid && a.DonemID == d.DonemID && a.Sinif == Sinif && a.Sube == Sube);
                liste = (from i in liste
                         where Convert.ToDateTime(i.Tarih).Date == Convert.ToDateTime(Tarih).Date
                         orderby i.DersGrupID ascending
                         select i).ToList();
                if (liste.Count > 0)
                {
                    foreach (var i in liste)
                    {
                        YoklamaViewModel m = new YoklamaViewModel();
                        m.AktifMi = i.AktifMi;
                        m.DersGrupID = i.DersGrupID;
                        m.DersID = i.DersID;
                        m.DonemID = i.DonemID;
                        if (i.OgrenciIDListe != null)
                        {
                            m.OgrenciIDListe = i.OgrenciIDListe.Split(',');
                        }
                        m.OgretmenID = i.OgretmenID;
                        m.OkulID = i.OkulID;
                        m.Sinif = i.Sinif;
                        m.Sube = i.Sube;
                        m.Tarih = i.Tarih;
                        m.YoklamaID = i.YoklamaID;
                        YoklamaListe.Add(m);
                    }
                }
                ViewData["Yoklama"] = YoklamaListe;
                ViewBag.Sinif = Sinif;
                ViewBag.Sube = Sube;
                ViewBag.Tarih = Tarih;
            }
            ViewBag.SecimVarmi = secimyapildimi;
            return View(SinifSube);
        }

        public ActionResult IBESayfa()
        {
            return View();
        }
    }
}