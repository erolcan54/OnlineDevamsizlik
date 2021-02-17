using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OD.UI.ViewModel;
using OD.Bll.Concrete;
using SRVTextToImage;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using OD.Model;
using OD.UI.Utils;

namespace OD.UI.Controllers
{
    public class HomeController : Controller
    {
        Fonksiyon f = new Fonksiyon();

        ilRepository ilrepo = new ilRepository();
        ilceRepository ilcerepo = new ilceRepository();
        OkulRepository okulrepo = new OkulRepository();
        TalepRepository taleprepo = new TalepRepository();
        AdminRepository adminrepo = new AdminRepository();
        OgretmenRepository ogretmenrepo = new OgretmenRepository();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ilceListeGetir(int id)
        {
            List<SelectListItem> ilceListe = (from i in ilcerepo.GetByFilterList(a => a.sehirid == id).OrderBy(a => a.ilceadi).ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.ilceadi,
                                                  Value = i.id.ToString()
                                              }).ToList();
            return Json(ilceListe, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public FileResult CaptchaImageOlustur()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["GuvenlikKodu"] = CI.GetRandomString(5); //Burda güvenlik kodumuzun 5 karekter olması istedik
            CI.GenerateImage(this.Session["GuvenlikKodu"].ToString(), 300, 75, Color.DarkGray, Color.White);
            //Yukarda Güvenlik resmimizin 300x75 boyutunda olmasını ve yazının koyu gri, arkaplanıda beyaz yaptık
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        public ActionResult TalepOlustur()
        {
            ililceListeView ililcemodel = new ililceListeView();
            ililcemodel.ilListe = (from i in ilrepo.GetAll()
                                   select new SelectListItem
                                   {
                                       Text = i.sehiradi,
                                       Value = i.id.ToString()
                                   }).ToList();
            return View(ililcemodel);
        }

        [HttpPost]
        public JsonResult TalepOlustur(TalepOlusturModel model)
        {
            JsonResultModel jmodel = new JsonResultModel();
            //try
            //{
            if (f.OkulKullaniciAdiVarMi(f.Encrypt(model.KullaniciAdi)) == false)
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Aynı kullanıcı adına sahip başka bir okul var. Lütfen kullanıcı adınızı değiştiriniz.";
            }
            else
            {
                Okullar o = new Okullar();
                Talepler t = new Talepler();
                o.Adi = model.Adi;
                o.Adres = model.Adres;
                o.AktifMi = false;
                o.ilID = model.ilID;
                o.ilceID = model.ilceID;
                o.KayitTarihi = DateTime.Now;
                o.YetkiliAdSoyad = model.YetkiliAdSoyad;
                o.YetkiliEmail = model.YetkiliEmail;
                o.YetkiliTel = model.YetkiliTel;
                o.KullaniciAdi = f.Encrypt(model.KullaniciAdi);
                o.Sifre = f.Encrypt(model.Sifre);
                o.OgrenciListeYuklediMi = false;
                okulrepo.Add(o);

                t.Aciklama = model.Aciklama;
                t.AktifMi = true;
                t.OkulID = o.OkulID;
                t.Tarih = DateTime.Now;
                taleprepo.Add(t);

                jmodel.IsSuccess = true;
                jmodel.UserMessage = "Talebiniz başarılı bir şekilde bize ulaştı. En kısa zamanda belirttiğiniz iletişim bilgileri üzerinden sizinle irtibata geçilecektir.";
            }
            //}
            //catch
            //{
            //    jmodel.IsSuccess = false;
            //    jmodel.UserMessage = "Talep oluşturulurken bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
            //}
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OkulGirisi()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OkulGiris()
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                string kadi = f.Encrypt(Request.Form["KullaniciAdi"].ToString());
                string sifre = f.Encrypt(Request.Form["Sifre"].ToString());
                string guvenlik = Request.Form["GuvenlikKodu"].ToString();

                if (Session["GuvenlikKodu"].ToString() == guvenlik)
                {
                    Okullar okul = okulrepo.GetByFilter(a => a.KullaniciAdi == kadi && a.Sifre == sifre);
                    if (okul == null)
                    {
                        jmodel.IsSuccess = false;
                        jmodel.UserMessage = "Kullanıcı Adınız veya Şifreniz hatalı! Lütfen kontrol ediniz.";
                    }
                    else
                    {
                        Session["OkulID"] = okul.OkulID;
                        jmodel.IsSuccess = true;
                    }
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Güvenlik Kodu yanlış! Lütfen kontrol ediniz.";
                }

            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OgretmenGiris()
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                string kadi = f.Encrypt(Request.Form["KullaniciAdi"].ToString());
                string sifre = f.Encrypt(Request.Form["Sifre"].ToString());
                string guvenlik = Request.Form["GuvenlikKodu"].ToString();

                if (Session["GuvenlikKodu"].ToString() == guvenlik)
                {
                    Ogretmenler ogr = ogretmenrepo.GetByFilter(a => a.KullaniciAdi == kadi && a.Sifre == sifre);
                    if (ogr == null)
                    {
                        jmodel.IsSuccess = false;
                        jmodel.UserMessage = "Kullanıcı Adınız veya Şifreniz hatalı! Lütfen kontrol ediniz.";
                    }
                    else
                    {
                        Session["OgretmenID"] = ogr.OgretmenID;
                        jmodel.IsSuccess = true;
                    }
                }
                else
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Güvenlik Kodu yanlış! Lütfen kontrol ediniz.";
                }

            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AdminGirisi()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AdminGiris()
        {
            JsonResultModel jmodel = new JsonResultModel();
            try
            {
                string kadi = f.Encrypt(Request.Form["KullaniciAdi"].ToString());
                string sifre = f.Encrypt(Request.Form["Sifre"].ToString());

                AdminTBL admin = adminrepo.GetByFilter(a => a.KullaniciAdi == kadi && a.Sifre == sifre);
                if (admin == null)
                {
                    jmodel.IsSuccess = false;
                    jmodel.UserMessage = "Kullanıcı Adınız veya Şifreniz hatalı! Lütfen kontrol ediniz.";
                }
                else
                {
                    Session["AdminID"] = admin.AdminID;
                    jmodel.IsSuccess = true;
                }
            }
            catch
            {
                jmodel.IsSuccess = false;
                jmodel.UserMessage = "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            return Json(jmodel, JsonRequestBehavior.AllowGet);
        }

        public FileResult SablonDownload()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/SablonListe/ogrencilistesablon.xlxs"));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "ogrencilistesablon.xlxs");
        }
    }
}