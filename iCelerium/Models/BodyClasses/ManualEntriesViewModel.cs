using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class ManualEntriesViewModel
    {
        [Display(Name = "AgentName", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string AgentID { get; set; }


        [Display(Name = "Member", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string clientID { get; set; }


        [Display(Name = "Amount", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public double Amount { get; set; }
    }
}