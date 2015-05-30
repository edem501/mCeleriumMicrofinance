using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class TransactionsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();

        public async Task<ActionResult> ExportToExcel(string searchDate, string agentModel)
        {
            IEnumerable<vTransaction> tran;
            if (!String.IsNullOrEmpty(searchDate))
            {
                var chk = DateTime.ParseExact(searchDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
                if (!String.IsNullOrEmpty(agentModel))
                {
                    ViewBag.VBagentModel = agentModel;
                    tran = await db.vTransactions.Where(t => t.Date.Day == chk.Day && t.Date.Month == chk.Month && t.Date.Year == chk.Year && t.AgentId == agentModel).ToListAsync();

                }
                else
                {
                    ViewBag.VBagentModel = null;
                    tran = await db.vTransactions.Where(t => t.Date.Day == chk.Day && t.Date.Month == chk.Month && t.Date.Year == chk.Year).ToListAsync();

                }

            }
            else
            {
                ViewBag.VBsearchDate = null;
                if (!String.IsNullOrEmpty(agentModel))
                {
                    ViewBag.VBagentModel = agentModel;
                    tran = await db.vTransactions.Where(t => t.AgentId == agentModel).ToListAsync();

                }
                else
                {
                    ViewBag.VBagentModel = null;
                    tran = await db.vTransactions.ToListAsync();

                }
            }
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
            String fileNa;
            if (agentModel == null)
            {
                fileNa = "AGENCE" + searchDate.ToString();
            }
            else
            {
                fileNa = agentModel + searchDate.ToString();
            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.xls",fileNa));
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());

            Response.End();
            return View();
        }
        // GET: /Transactions/
        public async Task<ActionResult> Index(string searchDate, string agentModel, int? page)
        {
            
            IEnumerable<Commerciaux> cmx = await db.Commerciauxes.OrderBy(c=>c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach(Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId});
            }
            if (String.IsNullOrEmpty(searchDate))
            {
                searchDate = DateTime.Now.ToString("dd-MM-yyyy");
            }
            ViewBag.agentModel = items;
            IEnumerable<vTransaction> tran ;
            if (!String.IsNullOrEmpty(searchDate))
            {
                var chk = DateTime.ParseExact(searchDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                ViewBag.VBsearchDate = searchDate;
                //var chk = searchDate;
                if (!String.IsNullOrEmpty(agentModel))
                {
                    ViewBag.VBagentModel = agentModel;
                    tran = await db.vTransactions.Where(t => t.Date.Day == chk.Day && t.Date.Month == chk.Month && t.Date.Year == chk.Year && t.AgentId == agentModel).ToListAsync();
                    
                }
                else
                {
                    ViewBag.VBagentModel = null;
                    tran = await db.vTransactions.Where(t => t.Date.Day == chk.Day && t.Date.Month == chk.Month && t.Date.Year == chk.Year).ToListAsync();
                   
                }
                
            }
            else
            {
                ViewBag.VBsearchDate = null;
                if ( !String.IsNullOrEmpty(agentModel))
                {
                    ViewBag.VBagentModel = agentModel;
                    tran = await db.vTransactions.Where(t => t.AgentId == agentModel).ToListAsync();
                  
                }
                else
                {
                    ViewBag.VBagentModel = null;
                    tran=await db.vTransactions.ToListAsync();
                  
                }
            }
            List<TransactionsViewModel> lstran = new List<TransactionsViewModel>();
            foreach (vTransaction tr in tran)
            {
                lstran.Add(new TransactionsViewModel
                {
                    AgentId = tr.AgentId,
                    Credit = tr.Credit,
                    Date = tr.Date,
                    Debit = tr.Debit,
                    id = tr.id,
                    Nom_Client = tr.Nom_Client,
                    Nom_Commercial = tr.Nom_Commercial,
                    Solde_Fermeture = tr.Solde_Fermeture,
                    Solde_Ouverture = tr.Solde_Ouverture
                });
            }
            lstran = lstran.OrderBy(c=>c.Date).ToList();
            ViewBag.Export = tran;
            ViewBag.sumC = String.Format("{0:c}", tran.Sum(t => t.Credit));
            ViewBag.sumD = String.Format("{0:c}", tran.Sum(t => t.Debit));
            return View(lstran.ToPagedList(page ?? 1, 15));
        }

        // GET: /Transactions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vTransaction vtransaction = await db.vTransactions.FindAsync(id);
            if (vtransaction == null)
            {
                return HttpNotFound();
            }
            return View(vtransaction);
        }

        // GET: /Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Date,Nom_Commercial,Nom_Client,Solde_Ouverture,Debit,Credit,Solde_Fermeture,id,AgentId")] vTransaction vtransaction)
        {
            if (ModelState.IsValid)
            {
                db.vTransactions.Add(vtransaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vtransaction);
        }

        // GET: /Transactions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vTransaction vtransaction = await db.vTransactions.FindAsync(id);
            if (vtransaction == null)
            {
                return HttpNotFound();
            }
            return View(vtransaction);
        }

        // POST: /Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Date,Nom_Commercial,Nom_Client,Solde_Ouverture,Debit,Credit,Solde_Fermeture,id,AgentId")] vTransaction vtransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vtransaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vtransaction);
        }

        // GET: /Transactions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vTransaction vtransaction = await db.vTransactions.FindAsync(id);
            if (vtransaction == null)
            {
                return HttpNotFound();
            }
            return View(vtransaction);
        }

        // POST: /Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            vTransaction vtransaction = await db.vTransactions.FindAsync(id);
            db.vTransactions.Remove(vtransaction);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ViewPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vTransaction vtransaction = await db.vTransactions.FindAsync(id);
            if (vtransaction == null)
            {
                return HttpNotFound();
            }
            return View(vtransaction);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
