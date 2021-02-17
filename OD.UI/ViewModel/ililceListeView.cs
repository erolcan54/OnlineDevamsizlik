using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OD.UI.ViewModel
{
    public class ililceListeView
    {
        public ililceListeView()
        {
            this.ilListe = new List<SelectListItem>();
            ilListe.Add(new SelectListItem { Text = "Şehir Seçiniz", Value = "0" });

            this.ilceListe = new List<SelectListItem>();
            ilceListe.Add(new SelectListItem { Text = "İlçe Seçiniz", Value = "0" });
        }

        public int ilID { get; set; }
        public int ilceID { get; set; }
        public List<SelectListItem> ilListe { get; set; }
        public List<SelectListItem> ilceListe { get; set; }
    }
}