using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;
using PagedList;
using System.IO;
namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class ClientsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();

        // GET: /Clients/
        public async Task<ActionResult> Index()
        {
            return View();
        }
        public async Task<JsonResult> GetIndex()
        {

            IEnumerable<Client> clientList = await this.db.Clients.ToListAsync();
            List<ClientsViewModel> model = new List<ClientsViewModel>();
            foreach (Client cmx in clientList)
            {
                model.Add(new ClientsViewModel
                {
                    ClientId = cmx.ClientId,
                    ClientTel = cmx.ClientTel,
                    Mise = (double)cmx.Mise,
                    Name = cmx.Name,
                    Sexe = cmx.Sexe,
                    Solde = (double)cmx.Solde,
                    link = string.Format("<a href='/Clients/Details?ClientId={0}'> <i class='fa fa-info-circle fa-fw fa-2x'></i></a>" +
                                         "<a href='/Clients/Edit?ClientId={0}'> <i class='fa fa-edit fa-fw fa-2x'></i></a>" +
                                         "<a href='/Clients/Transaction?ClientId={0}'> <i class='fa fa-bolt fa-fw fa-2x'></i></a>"
                                        , cmx.ClientId)
                });
            }
            jess jess = new jess();
            jess.data = model;
            return Json(jess, JsonRequestBehavior.AllowGet);
        }
        // GET: /Clients/Details/5
        public async Task<ActionResult> Details(string ClientId)
        {
            if (ClientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await this.db.Clients.Where(c => c.ClientId == ClientId).FirstOrDefaultAsync();
            if (client == null)
            {
                return this.HttpNotFound();
            }
            string Collecteur = null;
            AgentAssignClient agCl = db.AgentAssignClients.Where(c => c.ClientId == ClientId).FirstOrDefault();
            if (agCl == null)
            {
                Collecteur = "Not assigned.......";
            }
            else
            {
                Collecteur = db.Commerciauxes.Where(c => c.AgentId == agCl.AgentId).FirstOrDefault().AgentName;

            }
            EditClientsViewModel edtClient = new EditClientsViewModel
            {
                ClientId = client.ClientId,
                ClientTel = client.ClientTel,
                Mise = (double)client.Mise,
                Name = client.Name,
                Sexe = client.Sexe,
                Solde = (double)client.Solde,
                AgentName = Collecteur
            };
            List<SelectListItem> items = new List<SelectListItem>();
            if (edtClient.Sexe == "Homme")
            {
                items.Add(new SelectListItem { Text = "Homme", Value = "Homme", Selected = true });
                items.Add(new SelectListItem { Text = "Femme", Value = "Femme", Selected = false });
            }
            else
            {
                items.Add(new SelectListItem { Text = "Homme", Value = "Homme", Selected = false });
                items.Add(new SelectListItem { Text = "Femme", Value = "Femme", Selected = true });
            }
            ViewBag.Sexes = items;
            return this.View(edtClient);
        }

        public async Task<ActionResult> Transaction(string ClientId, string startDate, string endDate, int? page)
        {
            if (ClientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await this.db.Clients.Where(c => c.ClientId == ClientId).FirstOrDefaultAsync();
            if (client == null)
            {
                return this.HttpNotFound();
            }
            ViewBag.ClientID = ClientId;
            DateTime date1, date2;

            if (String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                date1 = DateTime.Today.AddMonths(-1);
                date2 = DateTime.Today.AddDays(1);
            }
            else
            {
                date1 = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                date2 = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            List<TransactionsViewModel> lstTrans = new List<TransactionsViewModel>();
            List<TTransaction> trans = new List<TTransaction>();
            trans = await db.TTransactions.Where(c => c.ClientId == ClientId && c.DateOperation >= date1 && c.DateOperation <= date2).ToListAsync();
            if (trans != null)
            {
              foreach (TTransaction tt in trans)
                        {
                            lstTrans.Add(new TransactionsViewModel
                            {
                                AgentId = tt.AgentId,
                                Credit = tt.Credit,
                                Date = tt.DateOperation,
                                Debit = tt.Debit,
                                id = tt.id,
                                Nom_Client =  db.Clients.Where(c=>c.ClientId==tt.ClientId).First().Name,
                                Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                                Solde_Fermeture = tt.SoldeFermeture,
                                Solde_Ouverture = tt.SoldeOuverture

                            });
                        }

            }
          
            return this.View(lstTrans.ToPagedList(page ?? 1, 15));
        }

        // GET: /Clients/Create
        //public ActionResult Create()
        //{
        //    return this.View();
        //}

        // POST: /Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,ClientId,ClientTel,Mise,Solde,Name,Sexe")]
        //                                       Client client)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        this.db.Clients.Add(client);
        //        await this.db.SaveChangesAsync();
        //        return this.RedirectToAction("Index");
        //    }

        //    return this.View(client);
        //}

        // GET: /Clients/Edit/5
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Edit(string ClientId)
        {
            if (ClientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await this.db.Clients.Where(c=>c.ClientId==ClientId).FirstOrDefaultAsync();
            if (client == null)
            {
                //return this.HttpNotFound();
              
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            }
            string Collecteur=null;
            AgentAssignClient agCl = db.AgentAssignClients.Where(c => c.ClientId == ClientId).FirstOrDefault();
           if (agCl == null)
                        {
                            Collecteur="Not assigned.......";
                        }
                     else{
                         Collecteur = db.Commerciauxes.Where(c => c.AgentId == agCl.AgentId).FirstOrDefault().AgentName;

                     }
            EditClientsViewModel edtClient = new EditClientsViewModel {
            ClientId =client.ClientId,
                    ClientTel = client.ClientTel,
                    Mise =(double)client.Mise,
                    Name = client.Name,
                    Sexe = client.Sexe,
                    Solde = (double)client.Solde,
                     AgentName=Collecteur
                    };
            List<SelectListItem> items = new List<SelectListItem>();
            if (edtClient.Sexe == "Homme")
            {
                items.Add(new SelectListItem { Text = "Homme",Value= "Homme",Selected=true });
                items.Add(new SelectListItem { Text = "Femme", Value = "Femme", Selected = false });
            }
            else
            {
                items.Add(new SelectListItem { Text = "Homme", Value = "Homme", Selected =  false});
                items.Add(new SelectListItem { Text = "Femme", Value = "Femme", Selected = true });
            }
            ViewBag.Sexe = items;
            return this.View(edtClient);
        }

        // POST: /Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "ClientTel,Mise,Name,Sexe,ClientId")] EditClientsViewModel client)
        {
            if (this.ModelState.IsValid)
            {
                Client cli = await db.Clients.Where(c => c.ClientId.Equals(client.ClientId)).FirstAsync();
                cli.ClientTel = client.ClientTel;
                cli.Mise = client.Mise;
                cli.Name = client.Name;
                cli.Sexe = client.Sexe;
                await this.db.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            return this.View(client);
        }

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Client client = await db.Clients.FindAsync(id);
        //    if (client == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(client);
        //}

        //// GET: /Clients/Delete/5
        //// POST: /Clients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Client client = await db.Clients.FindAsync(id);
        //    db.Clients.Remove(client);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
       
        public ActionResult IndexOne()
        {
            return View();
        }
        public class jess
        {
            public List<ClientsViewModel> data { get; set; }
        }
        public async Task<JsonResult> GetData()
        {
            
            IEnumerable<Client> clientList = await this.db.Clients.ToListAsync();
            List<ClientsViewModel> model = new List<ClientsViewModel>();
            foreach (Client cmx in clientList)
            {
                model.Add(new ClientsViewModel
                {
                    ClientId = cmx.ClientId,
                    ClientTel = cmx.ClientTel,
                    Mise = (double)cmx.Mise,
                    Name = cmx.Name,
                    Sexe = cmx.Sexe,
                    Solde = (double)cmx.Solde,
                    link = string.Format("<a href='/Clients/Details?ClientId={0}'> <i class='fa fa-info fa-fw fa-2x'></i></a>" +
                                         "<a href='/Clients/Edit?ClientId={0}'> <i class='fa fa-edit fa-fw fa-2x'></i></a>"+
                                         "<a href='/Clients/Transaction?ClientId={0}'> <i class='fa fa-bolt fa-fw fa-2x'></i></a>"
                                        ,cmx.ClientId)
                });
            }
            jess jess = new jess();
            jess.data = model;
            return Json(jess, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
