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
    
    public partial class Commerciaux
    {
        public Commerciaux()
        {
            this.TerminalAssigneds = new HashSet<TerminalAssigned>();
        }
    
        public int Id { get; set; }
        public string AgentId { get; set; }
        public string AgentPass { get; set; }
        public string AgentName { get; set; }
        public bool AgentActif { get; set; }
        public string AgentTel { get; set; }
        public int ZoneID { get; set; }
    
        public virtual ICollection<TerminalAssigned> TerminalAssigneds { get; set; }
    }
}
