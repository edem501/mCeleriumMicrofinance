using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCelerium.Models.Interfaces
{
    interface IAspNetUser
    {
         string Id { get; set; }
         [Required(ErrorMessage = "Le nom d' {0} est obligatoire")]
         [Display(Name = "Utilisateur")]
         string UserName { get; set; }
         string PasswordHash { get; set; }
         string SecurityStamp { get; set; }
         string Discriminator { get; set; }
         [DataType(DataType.EmailAddress, ErrorMessage = "E-mail incorrect")]
         [EmailAddress]
         [Required(ErrorMessage = "L' {0} est obligatoire")]
         [Display(Name = "Addresse E-mail")]
         string Email { get; set; }

         [DataType(DataType.Text)]
         [Required(ErrorMessage = "Le {0} sont obligatoire")]
         [Display(Name = "Nom et prenom")]
         string FullName { get; set; }

          ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
          ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
          ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}
