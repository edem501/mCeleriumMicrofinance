using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class ReportsController : Controller
    {
        public readonly SMSServersEntities db = new SMSServersEntities();

        public ActionResult Index()
        {
            return this.View();
        }

        //Get
        [HttpGet]
        public ActionResult _PartialOne()
        {
            //"_PartialOne"
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _PartialOne(string searchDate, string searchDate1)
        {
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
               
                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0).ToListAsync();

                this.GenerateExcel(tran);
            }
            else
            {
                return this.PartialView();
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> _PartialTwo()
        {
            IEnumerable<Commerciaux> cmx = await this.db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            this.ViewBag.searchAgent = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _PartialTwo(string searchDate, string searchDate1, string searchAgent)
        {
            this.ViewBag.searchAgent = searchAgent;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(this.ViewBag.searchAgent))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;

                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.AgentId.Equals(searchAgent)).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }
               
            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> _Partial3()
        {
            IEnumerable<Client> cmx = await this.db.Clients.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Client com in cmx)
            {
                items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id.ToString() });
            }
            this.ViewBag.searchClient = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial3(string searchDate, string searchDate1, int searchClient)
        {
            this.ViewBag.searchClient = searchClient;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && this.ViewBag.searchClient != null)
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
                var myId = this.db.Clients.Where(c => c.Id == searchClient).FirstOrDefault().Name;
                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.Nom_Client.Equals(myId)).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult _Partial4()
        {
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial4(string searchDate, string searchDate1)
        {
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;

                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.Debit == 0).ToListAsync();

                this.GenerateExcel(tran);
            }
            else
            {
                return this.PartialView();
            }

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> _Partial5()
        {
            IEnumerable<Commerciaux> cmx = await this.db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            this.ViewBag.searchAgent = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial5(string searchDate, string searchDate1, string searchAgent)
        {
            this.ViewBag.searchAgent = searchAgent;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(this.ViewBag.searchAgent))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;

                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.AgentId.Equals(searchAgent) && t.Debit == 0).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> _Partial6()
        {
            IEnumerable<Client> cmx = await this.db.Clients.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Client com in cmx)
            {
                items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id.ToString() });
            }
            this.ViewBag.searchClient = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial6(string searchDate, string searchDate1, int searchClient)
        {
            this.ViewBag.searchClient = searchClient;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && this.ViewBag.searchClient != null)
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
                var myId = this.db.Clients.Where(c => c.Id == searchClient).FirstOrDefault().Name;
                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.Nom_Client.Equals(myId) && t.Debit == 0).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult _Partial7()
        {
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial7(string searchDate, string searchDate1)
        {
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;

                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.Credit == 0).ToListAsync();

                this.GenerateExcel(tran);
            }
            else
            {
                return this.PartialView();
            }

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> _Partial8()
        {
            IEnumerable<Commerciaux> cmx = await this.db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            this.ViewBag.searchAgent = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial8(string searchDate, string searchDate1, string searchAgent)
        {
            this.ViewBag.searchAgent = searchAgent;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && !String.IsNullOrEmpty(this.ViewBag.searchAgent))
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;

                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.AgentId.Equals(searchAgent) && t.Credit == 0).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> _Partial9()
        {
            IEnumerable<Client> cmx = await this.db.Clients.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Client com in cmx)
            {
                items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id.ToString() });
            }
            this.ViewBag.searchClient = items;
            return this.PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> _Partial9(string searchDate, string searchDate1, int searchClient)
        {
            this.ViewBag.searchClient = searchClient;
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate) && !String.IsNullOrEmpty(searchDate1) && this.ViewBag.searchClient != null)
            {
                var chk = DateTime.ParseExact(searchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var chk1 = DateTime.ParseExact(searchDate1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
                var myId = this.db.Clients.Where(c => c.Id == searchClient).FirstOrDefault().Name;
                tran = await this.db.vTransactions.Where(t => DbFunctions.DiffDays(t.Date, chk) <= 0 && DbFunctions.DiffDays(t.Date, chk1) >= 0 && t.Nom_Client.Equals(myId) && t.Credit == 0).ToListAsync();
                if (tran.Count() > 0)
                {
                    this.GenerateExcel(tran);
                }
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult _Partial10()
        {
            return this.PartialView();
        }

        public async Task<ActionResult> _Partial11()
        {
            IEnumerable<Commerciaux> cmx = await this.db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            this.ViewBag.searchAgent = items;
            return this.PartialView();
        }

        public async Task<ActionResult> _Partial12()
        {
            IEnumerable<Client> cmx = await this.db.Clients.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Client com in cmx)
            {
                items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id.ToString() });
            }
            this.ViewBag.searchClient = items;
            return this.PartialView();
        }

        public ActionResult Chart()
        {
            return this.View();
        }

        private void GenerateExcel(IEnumerable<vTransaction> tran)
        {
            var grid = new System.Web.UI.WebControls.GridView();
            //itran.Add(new vTransaction { id = 1, AgentId="Ag001",Nom_Client="Abalo Akua",Date=DateTime.Today,Solde_Ouverture=100,Solde_Fermeture=1000,Debit=0,Credit=900});
            grid.DataSource = from d in tran.ToList()
                              select new
                              {
                                  DateOperation = d.Date,
                                  NumeroComm = d.AgentId,
                                  Client = d.Nom_Client,
                                  SoldeOverture = d.Solde_Ouverture,
                                  Debit = d.Debit,
                                  Credit = d.Credit,
                                  SoldeFermeture = d.Solde_Fermeture
                              };

            grid.DataBind();
            String fileNa = "Rapport";

            this.Response.ClearContent();
            this.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.xls", fileNa));
            this.Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            this.Response.Write(sw.ToString());
        
            this.Response.End();
        }
    }
}