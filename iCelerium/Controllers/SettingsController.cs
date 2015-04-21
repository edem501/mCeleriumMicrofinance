using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    [Authorize(Users = "Admin")]
    [Audit]
    public class SettingsController : Controller
    {
        private readonly SMSServersEntities adb = new SMSServersEntities();

        //
        // GET: /Settings/
        public ActionResult Index()
        {
            return this.View();
        }
        //
        // GET: /Settings/AuditTrail
        public async Task<ActionResult> AuditTrail(string startDate, string endDate, int? page)
        {
            DateTime date1, date2;

            this.ViewBag.startDate = startDate;
            this.ViewBag.endDate = endDate;

            if (String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                date1 = DateTime.Today;
                date2 = DateTime.Today.AddDays(1);
            }
            else
            {
                date1 = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date2 = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            IEnumerable<AuditRecord> querry = await this.adb.AuditRecords.Where(x => x.Timestamp >= date1 && x.Timestamp < date2).OrderBy(x => x.Timestamp).ToListAsync();
            List<AuditViewModel> Vm = new List<AuditViewModel>();
            if (querry != null && querry.Count() > 0)
            {
                foreach (AuditRecord ar in querry)
                {
                    Vm.Add(new AuditViewModel
                    {
                        AreaAccessed = ar.AreaAccessed,
                        AuditID = ar.AuditID,
                        IPAddress = ar.IPAddress,
                        Timestamp = ar.Timestamp,
                        UserName = ar.UserName
                    });
                }
            }

            return this.View(Vm.ToPagedList(page ?? 1, 15));
        }

        public async Task<ActionResult> AssigneAgent()
        {
            IEnumerable<Commerciaux> cmx = await adb.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            ViewBag.agentModel = items;
            return View();
        }
    }
}