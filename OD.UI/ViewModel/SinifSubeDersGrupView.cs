using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OD.UI.ViewModel
{
    public class SinifSubeDersGrupView
    {
        public SinifSubeDersGrupView()
        {
            this.SinifGrup = new List<SelectListItem>();
            SinifGrup.Add(new SelectListItem { Text = "Sınıf Seçiniz", Value = "0" });

            this.SubeGrup = new List<SelectListItem>();
            SubeGrup.Add(new SelectListItem { Text = "Şube Seçiniz", Value = "0" });

            this.DersListe = new List<SelectListItem>();
            DersListe.Add(new SelectListItem { Text = "Ders Seçiniz", Value = "0" });
        }

        public int Sinif { get; set; }
        public string Sube { get; set; }
        public int DersID { get; set; }

        public List<SelectListItem> SinifGrup { get; set; }
        public List<SelectListItem> SubeGrup { get; set; }
        public List<SelectListItem> DersListe { get; set; }
    }
}