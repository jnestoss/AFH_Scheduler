
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
    
public partial class Scheduled_Inspections
{

    public long SInspections_Id { get; set; }

    public string SInspections_Date { get; set; }

    public Nullable<long> FK_PHome_ID { get; set; }



    public virtual Provider_Homes Provider_Homes { get; set; }

}

}