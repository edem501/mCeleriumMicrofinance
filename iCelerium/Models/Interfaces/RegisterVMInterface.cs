using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCelerium.Models.Interfaces
{
    interface RegisterVMInterface
    {
        [Required(ErrorMessage = "Le nom d' {0} est obligatoire")]
        [Display(Name = "Utilisateur")]
         string UserName { get; set; }

        //[Required(ErrorMessage = "Le {0} est obligatoire")]
        [StringLength(100, ErrorMessage = "Le {0} doit etre au moins {2} characteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
         string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation doivent etre le meme.")]
         string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail incorrect")]
        [EmailAddress]
        [Required(ErrorMessage = "L' {0} est obligatoire")]
        [Display(Name = "Addresse E-mail")]
         string Email { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Le {0} sont obligatoire")]
        [Display(Name = "Nom et prenom")]
         string FullName { get; set; }
    }

    
}
