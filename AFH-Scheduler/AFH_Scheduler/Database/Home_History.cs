
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
    
public partial class Home_History
{

    public long HHistory_ID { get; set; }

    public string HHistory_Date { get; set; }

    public Nullable<long> FK_PHome_ID { get; set; }

    public string FK_IOutcome_Code { get; set; }



    public virtual Inspection_Outcome Inspection_Outcome { get; set; }

    public virtual Provider_Homes Provider_Homes { get; set; }

}

}
