using OD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OD.UI.ViewModel
{
    public class YoklamaGuncellemeViewModel
    {
        public List<OgrenciYoklamaCheckBox> liste { get; set; }
        public int YoklamaID { get; set; }
        public bool islemTamam { get; set; }
    }
}