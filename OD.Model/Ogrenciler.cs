//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ogrenciler
    {
        public int OgrenciID { get; set; }
        public string OgrenciNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public Nullable<int> Sinif { get; set; }
        public string Sube { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
        public Nullable<int> DonemID { get; set; }
        public Nullable<System.DateTime> SilmeTarihi { get; set; }
    
        public virtual Donemler Donemler { get; set; }
        public virtual Okullar Okullar { get; set; }
    }
}
