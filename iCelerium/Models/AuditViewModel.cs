using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models
{
    public class AuditViewModel
    {
        public System.Guid AuditID { get; set; }
        [Display(Name = "Utilisateur")]
        public string UserName { get; set; }
        [Display(Name = "Addresse IP")]
        public string IPAddress { get; set; }
        [Display(Name = "Log d'acces")]
        public string AreaAccessed { get; set; }
        [Display(Name = "Timestamp")]
        public System.DateTime Timestamp { get; set; }
    }
}