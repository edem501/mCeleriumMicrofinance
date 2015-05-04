using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class CreditsViewModels
    {
    }

    public class CreateCreditTypeViewModel
    {
        
        public string TypeID { get; set; }


        [Display(Name = "CreditType", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string TypeName { get; set; }
        [Display(Name = "Echeance", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public int EcheancesID { get; set; }

        [Display(Name = "InterestRate", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public double InterestRate { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public int Duration { get; set; }

        public virtual Echeance Echeance { get; set; }
    }
}