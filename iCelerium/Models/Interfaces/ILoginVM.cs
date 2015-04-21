using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCelerium.Models.Interfaces
{
   interface ILoginViewModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire")]
        [Display(Name = "Utilisateur")]
        string UserName { get; set; }

        [Required(ErrorMessage = "Le {0} est obligatoire")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        string Password { get; set; }

        [Display(Name = "Rester connecte?")]
        bool RememberMe { get; set; }
    }
}
