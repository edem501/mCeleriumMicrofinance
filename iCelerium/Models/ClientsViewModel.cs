using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models
{
    public class ClientsViewModel
    {
        [Display(Name = "ClientID", ResourceType = typeof(iCelerium.Views.Strings))]
        public string ClientId { get; set; }

        [Display(Name = "Telephone")]
        public string ClientTel { get; set; }
        [Display(Name = "Daily", ResourceType = typeof(iCelerium.Views.Strings))]
        public double Mise { get; set; }
        [Display(Name = "cBalance", ResourceType = typeof(iCelerium.Views.Strings))]
        public double Solde { get; set; }
       [Display(Name = "FullName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string Name { get; set; }
        [Display(Name = "Sex", ResourceType = typeof(iCelerium.Views.Strings))]
        public string Sexe { get; set; }

        public string link { get; set; }
    }

    public class ClientsViewModelCredit
    {
        [Display(Name = "ClientID", ResourceType = typeof(iCelerium.Views.Strings))]
        public string ClientId { get; set; }

        [Display(Name = "Telephone")]
        public string ClientTel { get; set; }
        [Display(Name = "Daily", ResourceType = typeof(iCelerium.Views.Strings))]
        public double Mise { get; set; }
        [Display(Name = "Reste a Payer")]
        public string Solde { get; set; }
        [Display(Name = "FullName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string Name { get; set; }
        [Display(Name = "Sex", ResourceType = typeof(iCelerium.Views.Strings))]
        public string Sexe { get; set; }
    }


    public class EditClientsViewModel
    {
          [Display(Name = "ClientID", ResourceType = typeof(iCelerium.Views.Strings))]
        
        public string ClientId { get; set; }
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        public string ClientTel { get; set; }
         [Display(Name = "Daily", ResourceType = typeof(iCelerium.Views.Strings))]
         [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        public double Mise { get; set; }
         [Display(Name = "cBalance", ResourceType = typeof(iCelerium.Views.Strings))]

        public double Solde { get; set; }
       [Display(Name = "FullName", ResourceType = typeof(iCelerium.Views.Strings))]
       [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        [Display(Name = "Sex", ResourceType = typeof(iCelerium.Views.Strings))]
        public string Sexe { get; set; }
        [Display(Name = "AgentName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string AgentName { get; set; }


    }
}