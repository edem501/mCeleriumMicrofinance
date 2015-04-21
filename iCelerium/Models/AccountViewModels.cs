using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace iCelerium.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class EditAccountViewModel
    {
        public EditAccountViewModel()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    
        public string Id { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "FullName", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
         [Required]
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
    public class ManageUserViewModel
    {
        [Display(Name = "Password", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "nPassword", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "cPassword", ResourceType = typeof(iCelerium.Views.Strings))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(iCelerium.Views.Strings),ErrorMessageResourceName = "Compare")]
        public string ConfirmPassword { get; set; }
    }
   
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string UserName { get; set; }

         [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class RolesViewModel
    {
        //public IEnumerable<SelectListItem> RoleName { get; set; }
        public IEnumerable<string> ListRoleName { get; set; }
    
    }

    
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
                ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string UserName { get; set; }
   
        [Display(Name = "Password", ResourceType = typeof(iCelerium.Views.Strings))]
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "cPassword", ResourceType = typeof(iCelerium.Views.Strings))]
        [Compare("Password", ErrorMessageResourceType = typeof(iCelerium.Views.Strings), ErrorMessageResourceName = "Compare")]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
           ErrorMessageResourceName = "Required")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "FullName", ResourceType = typeof(iCelerium.Views.Strings))]
        [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string FullName { get; set; }

    }
}
