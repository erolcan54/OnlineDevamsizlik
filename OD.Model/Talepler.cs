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
    
    public partial class Talepler
    {
        public int TalepID { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<bool> AktifMi { get; set; }
        public Nullable<int> OkulID { get; set; }
        public string Aciklama { get; set; }
    
        public virtual Okullar Okullar { get; set; }
    }
}
