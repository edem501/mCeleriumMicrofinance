using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class dailyDeliveryViewModel
    {
        public string ID { get; set; }
        public bool Enabled { get; set; }
        [Display(Name="Nom Membre")]
        public string ClientName { get; set; }
        [Display(Name = "Mise")]
        public double mise { get; set; }
        [Display(Name = "Montant du Credit")]
        public double MontantCredit { get; set; }
        [Display(Name = "Solde")]
        public double Solde { get; set; }
        [Display(Name = "Livre?")]
        public bool status { get; set; }
        

    }
    public class echelon
    {
        [Display(Name="Date Echeance")]
        public DateTime DateEcheance { get; set; }
        [Display(Name = "Montant du Credit")]
        public double MontantCredit { get; set; }
        [Display(Name = "Montant Payable")]
        public double MontantPayable { get; set; }
        [Display(Name = "Montant Restant")]
        public double MontantRestant { get; set; }
       

    }
    public class NewContractViewModels
    {
        public string ClientID { get; set; }
        
        public double Amount { get; set; }
        public System.DateTime DateFirstPyt { get; set; }
        public string TypeID { get; set; }
       
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