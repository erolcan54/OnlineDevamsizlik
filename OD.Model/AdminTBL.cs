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
    
    public partial class AdminTBL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdminTBL()
        {
            this.AdminLog = new HashSet<AdminLog>();
        }
    
        public int AdminID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdminLog> AdminLog { get; set; }
    }
}