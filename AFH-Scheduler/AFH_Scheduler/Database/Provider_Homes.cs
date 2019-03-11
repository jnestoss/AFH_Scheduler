//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AFH_Scheduler.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Provider_Homes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Provider_Homes()
        {
            this.Home_History = new HashSet<Home_History>();
            this.Scheduled_Inspections = new HashSet<Scheduled_Inspections>();
        }
    
        public long PHome_ID { get; set; }
        public string PHome_Address { get; set; }
        public string PHome_City { get; set; }
        public string PHome_Zipcode { get; set; }
        public string PHome_Phonenumber { get; set; }
        public Nullable<long> FK_Provider_ID { get; set; }
        public string PHome_Name { get; set; }
        public string PHome_LicenseNumber { get; set; }
        public string PHome_RCSUnit { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Home_History> Home_History { get; set; }
        public virtual Provider Provider { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scheduled_Inspections> Scheduled_Inspections { get; set; }
    }
}
