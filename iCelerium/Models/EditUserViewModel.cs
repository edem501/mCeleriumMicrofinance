using System.Collections.Generic;
using System.Web.Mvc;
using DotNet.Highcharts;

namespace iCelerium.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}