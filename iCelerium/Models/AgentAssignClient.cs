//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iCelerium.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AgentAssignClient
    {
        public int id { get; set; }
        public string AgentId { get; set; }
        public string ClientId { get; set; }
        public System.DateTime DateAssignee { get; set; }
    
        public virtual AgentAssignClient AgentAssignClient1 { get; set; }
        public virtual AgentAssignClient AgentAssignClient2 { get; set; }
    }
}
