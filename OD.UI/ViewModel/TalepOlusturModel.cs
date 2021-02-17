using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OD.UI.ViewModel
{
    public class TalepOlusturModel
    {
        public int TalepID { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public string Aciklama { get; set; }
        public string Adi { get; set; }
        public string YetkiliAdSoyad { get; set; }
        public string YetkiliTel { get; set; }
        public string YetkiliEmail { get; set; }
        public Nullable<int> ilID { get; set; }
        public Nullable<int> ilceID { get; set; }
        public string Adres { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<System.DateTime> BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    }
}