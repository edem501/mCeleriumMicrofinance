using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iCelerium.Models.Interfaces
{
    public interface ICommerciaux
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        
        [Display(Name = "Nom Collecteur")]
        [Required(ErrorMessage = "Le {0} est obligatoire")]
        string AgentName { get; set; }
         [Display(Name = "Actif?")]
        bool AgentActif { get; set; }
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Le {0} est obligatoire")]
        string AgentTel { get; set; }
        
    }
}