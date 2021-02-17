using OD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OD.UI.ViewModel
{
    public class YoklamaViewModel
    {
        public int YoklamaID { get; set; }
        public Nullable<int> OgretmenID { get; set; }
        public string[] OgrenciIDListe { get; set; }
        public Nullable<int> DersID { get; set; }
        public Nullable<int> DersGrupID { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<int> DonemID { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public Nullable<int> Sinif { get; set; }
        public string Sube { get; set; }
    }
}