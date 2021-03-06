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
    
    public partial class Okullar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Okullar()
        {
            this.Ogrenciler = new HashSet<Ogrenciler>();
            this.Ogretmenler = new HashSet<Ogretmenler>();
            this.OkulLog = new HashSet<OkulLog>();
            this.Talepler = new HashSet<Talepler>();
            this.Dersler = new HashSet<Dersler>();
            this.Yoklamalar = new HashSet<Yoklamalar>();
        }
    
        public int OkulID { get; set; }
        public string Adi { get; set; }
        public string YetkiliAdSoyad { get; set; }
        public string YetkiliTel { get; set; }
        public string YetkiliEmail { get; set; }
        public Nullable<int> ilID { get; set; }
        public Nullable<int> ilceID { get; set; }
        public string Adres { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<System.DateTime> BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public Nullable<bool> OgrenciListeYuklediMi { get; set; }
    
        public virtual ilceler ilceler { get; set; }
        public virtual iller iller { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ogrenciler> Ogrenciler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ogretmenler> Ogretmenler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OkulLog> OkulLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Talepler> Talepler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dersler> Dersler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yoklamalar> Yoklamalar { get; set; }
    }
}
