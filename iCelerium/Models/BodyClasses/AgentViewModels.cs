using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using iCelerium.Models.Interfaces;

namespace iCelerium.Models.BodyClasses
{
    public class CreateAgentViewModels
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }

        
        [Display(Name = "AgentName", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string AgentName { get; set; }

        [Display(Name = "Activated", ResourceType = typeof(iCelerium.Views.Strings))]
      
        public bool AgentActif { get; set; }
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]

        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
             ErrorMessageResourceName = "Required")]
        public string AgentTel { get; set; }
    }
    public class AgentListView
    {
        public int Id { get; set; }
        [Display(Name = "AgentNumber", ResourceType = typeof(iCelerium.Views.Strings))]
        public string AgentId { get; set; }
         [Display(Name = "AgentName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string AgentName { get; set; }
          [Display(Name = "Activated", ResourceType = typeof(iCelerium.Views.Strings))]
   
        public string AgentActif { get; set; }
        [Display(Name = "Telephone")]
        public string AgentTel { get; set; }
        public string link { get; set; }
    }
    public class EditAgentViewModels
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "AgentNumber", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string AgentId { get; set; }

       [Display(Name = "AgentName", ResourceType = typeof(iCelerium.Views.Strings))]
       [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        public string AgentName { get; set; }
       [Display(Name = "Activated", ResourceType = typeof(iCelerium.Views.Strings))]
        public bool AgentActif { get; set; }
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        public string AgentTel { get; set; }

         [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public int ZoneID { get; set; }
    }

    public class AgentClientsModel
    {
       
        public Nullable<double> Mise { get; set; }
        public Nullable<double> Solde { get; set; }
        public string NameID { get; set; }
        public string link { get; set; }

    }



}