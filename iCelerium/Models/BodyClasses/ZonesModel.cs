using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class ZonesModel
    {
        public int Id { get; set; }
         [Display(Name = "ZoneID", ResourceType = typeof(iCelerium.Views.Strings))]
        public string ZoneID { get; set; }
         [Display(Name = "ZoneName", ResourceType = typeof(iCelerium.Views.Strings))]
        public string ZoneName { get; set; }
    }
    public class ClientsList
    {
        public Boolean selected { get; set; }
        public String ClientID { get; set; }
        public String FullName { get; set; }
    }
    public class CreateZoneModel
    {
         [Display(Name = "ZoneName", ResourceType = typeof(iCelerium.Views.Strings))]
         [Required(ErrorMessageResourceType = typeof(iCelerium.Views.Strings),
            ErrorMessageResourceName = "Required")]
        public string ZoneName { get; set; }
    }

}