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
    
    public partial class Dersler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dersler()
        {
            this.AtananDersler = new HashSet<AtananDersler>();
            this.Yoklamalar = new HashSet<Yoklamalar>();
        }
    
        public int DersID { get; set; }
        public string DersAdi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> DonemID { get; set; }
        public Nullable<int> SinifDuzey { get; set; }
    
        public virtual Donemler Donemler { get; set; }
        public virtual Okullar Okullar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AtananDersler> AtananDersler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yoklamalar> Yoklamalar { get; set; }
    }
}
