using OD.Bll.Concrete;
using OD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OD.UI.Utils
{
    public class Fonksiyon
    {
        public string hash = "OnlineDevamsızlık.com";

        public string Encrypt(string sifre)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(sifre);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        public string Decrypt(string SifrelenmisDeger)
        {
            byte[] data = Convert.FromBase64String(SifrelenmisDeger);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }

        public bool OkulKullaniciAdiVarMi(string kadi)
        {
            OkulRepository orepo = new OkulRepository();
            Okullar o = orepo.GetByFilter(a => a.KullaniciAdi == kadi);
            if (o == null)
                return true;
            else
                return false;
        }

        public bool OgretmenKullaniciAdiVarMi(string kadi)
        {
            OgretmenRepository ogretmenrepo = new OgretmenRepository();
            Ogretmenler o = ogretmenrepo.GetByFilter(a => a.KullaniciAdi == kadi);
            if (o == null)
                return true;
            else
                return false;
        }

        public Dersler DersGetir(int id)
        {
            DerslerRepository dersrepo = new DerslerRepository();
            Dersler d = dersrepo.Get(id);
            return d;
        }

        public DersGruplari DersSaatiGetir(int id)
        {
            DersGruplariRepository dersgruprepo = new DersGruplariRepository();
            DersGruplari dg = dersgruprepo.Get(id);
            return dg;
        }

        public Ogretmenler OgretmenGetir(int id)
        {
            OgretmenRepository ogretmenrepo = new OgretmenRepository();
            Ogretmenler ogr = ogretmenrepo.Get(id);
            return ogr;
        }

        public Ogrenciler OgrenciGetir(int id)
        {
            OgrenciRepository ogrencirepo = new OgrenciRepository();
            Ogrenciler ogrenci = ogrencirepo.Get(id);
            return ogrenci;
        }
    }
}