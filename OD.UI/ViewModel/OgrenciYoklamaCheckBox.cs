using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OD.UI.ViewModel
{
    public class OgrenciYoklamaCheckBox
    {
        public int OgrenciID { get; set; }
        public string OgrenciNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public Nullable<int> Sinif { get; set; }
        public string Sube { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public Nullable<int> DonemID { get; set; }
        public bool secildimi { get; set; }
    }
}