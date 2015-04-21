using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class AgentsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();
        private readonly Cryptographer.CryptorEngine crypto = new Cryptographer.CryptorEngine();


        public async Task<ActionResult> AgentAssign(string agentID)
        {
            Commerciaux commerciaux =await db.Commerciauxes.Where(c => c.AgentId == agentID).FirstOrDefaultAsync();
            if ( commerciaux == null)
            {
                return this.HttpNotFound();
            }
            List<Client> lstClient = new List<Client>();
            List<AgentAssignClient> lstAssigned = await db.AgentAssignClients.Where(c => c.AgentId == agentID).ToListAsync();
            if (lstAssigned != null)
            {
                foreach (AgentAssignClient agAss in lstAssigned)
                {
                    lstClient.Add( await db.Clients.Where(c => c.ClientId == agAss.ClientId).FirstOrDefaultAsync());
                }
            }

            return View(new AgentClientsModel { AgentName=commerciaux.AgentName,Membres = lstClient});

        }

        // GET: /Agents/
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;
            IEnumerable<Commerciaux> agentList;

            if (!String.IsNullOrEmpty(searchString))
            {
                agentList = await this.db.Commerciauxes.Where(c => c.AgentName.ToUpper().Contains(searchString.ToUpper())).ToListAsync();
            }
            else
            {
                agentList = await this.db.Commerciauxes.ToListAsync();
            }

            List<AgentListView> model = new List<AgentListView>();
            foreach (Commerciaux cmx in agentList)
            {
                model.Add(new AgentListView
                {
                    AgentActif = cmx.AgentActif,
                    AgentId = cmx.AgentId,
                    AgentName = cmx.AgentName,
                    AgentTel = cmx.AgentTel,
                    Id = cmx.Id
                });
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return this.View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Test()
        {
            return this.View();
        }

        public async Task<PartialViewResult> Simple(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewBag.CurrentFilter = searchString;
            IEnumerable<Commerciaux> agentList;

            if (!String.IsNullOrEmpty(searchString))
            {
                agentList = await this.db.Commerciauxes.Where(c => c.AgentName.ToUpper().Contains(searchString.ToUpper())).ToListAsync();
            }
            else
            {
                agentList = await this.db.Commerciauxes.ToListAsync();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return this.PartialView("_AgentPartial", agentList.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Agents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commerciaux commerciaux = await this.db.Commerciauxes.FindAsync(id);
            if (commerciaux == null)
            {
                return this.HttpNotFound();
            }
            return this.View(commerciaux);
        }

        // GET: /Agents/Create
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Create()
        {
            IEnumerable<Zone> cmx = await this.db.Zones.OrderBy(c => c.ZoneName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Zone com in cmx)
            {
                items.Add(new SelectListItem { Text = com.ZoneName.ToUpper(), Value = com.ID.ToString() });
            }

            this.ViewBag.Zones = items;
            return this.View();
        }

        // POST: /Agents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Super Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAgentViewModels agent, string Zones)
        {
            Commerciaux commerciaux = new Commerciaux();
            if (this.ModelState.IsValid)
            {
                if (await this.AgentExist(agent.AgentName) == false)
                {
                    var id=0;
                    if (db.Commerciauxes.Count() ==0)
                    {
                        id = 0;
                    }
                    else
                    {
                         id = await this.db.Commerciauxes.MaxAsync(a => a.Id);
                    }
                    
                    commerciaux.AgentId = string.Format("A{0}", (id + 1).ToString().PadLeft(3, '0'));
                    commerciaux.AgentName = agent.AgentName;
                    commerciaux.AgentActif = agent.AgentActif;
                    commerciaux.AgentTel = agent.AgentTel;
                    commerciaux.AgentPass = "1234";
                    commerciaux.ZoneID = Convert.ToInt32(Zones);
                    this.db.Commerciauxes.Add(commerciaux);
                    await this.db.SaveChangesAsync();

                    var newId = commerciaux.Id;
                    commerciaux.AgentId = string.Format("A{0}", newId.ToString().PadLeft(3, '0'));
                    await this.db.SaveChangesAsync();
                    this.TempData["Message"] = String.Format("Success!! an account for {0} has been created", commerciaux.AgentName);
                    return this.RedirectToAction("Index");
                }
                else
                {
                    this.TempData["Message"] = String.Format("Error!! Cannot create account. {0} has an account already in iCelerium", agent.AgentName);
                    return this.RedirectToAction("Index");
                }
            }
            return this.View(agent);
        }

        // GET: /Agents/Edit/5
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commerciaux commerciaux = await this.db.Commerciauxes.FindAsync(id);
       
            if (commerciaux == null)
            {
                return this.HttpNotFound();
            }

            EditAgentViewModels agent = new EditAgentViewModels
            {
                AgentActif = commerciaux.AgentActif,
                AgentId = commerciaux.AgentId,
                AgentName = commerciaux.AgentName,
                AgentTel = commerciaux.AgentTel,
                Id = commerciaux.Id,
                ZoneID = commerciaux.ZoneID
            };
            IEnumerable<Zone> cmx = await this.db.Zones.OrderBy(c => c.ZoneName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Zone com in cmx)
            {
                if (com.ID == commerciaux.ZoneID)
                {
                    items.Add(new SelectListItem { Text = com.ZoneName.ToUpper(), Value = com.ID.ToString(), Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = com.ZoneName.ToUpper(), Value = com.ID.ToString() });
                }
            }

            this.ViewBag.Zones = items;
            return this.View(agent);
        }

        // POST: /Agents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Edit(EditAgentViewModels agent, string Zones)
        {
            Commerciaux commerciaux = await this.db.Commerciauxes.FindAsync(agent.Id);
           
            if (this.ModelState.IsValid)
            {
                commerciaux.AgentName = agent.AgentName;
                commerciaux.AgentActif = agent.AgentActif;
                commerciaux.AgentTel = agent.AgentTel;
                commerciaux.ZoneID = Convert.ToInt32(Zones);
                this.UpdateModel<Commerciaux>(commerciaux);
                await this.db.SaveChangesAsync();
                this.TempData["Message"] = String.Format("Success!! the account for {0} has been modified", commerciaux.AgentName);
                return this.RedirectToAction("Index");
            }
            return this.View(commerciaux);
        }

        // GET: /Agents/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Commerciaux commerciaux = await db.Commerciauxes.FindAsync(id);
        //    if (commerciaux == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(commerciaux);
        //}

        //// POST: /Agents/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Commerciaux commerciaux = await db.Commerciauxes.FindAsync(id);
        //    db.Commerciauxes.Remove(commerciaux);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
        [ChildActionOnly]
        public async Task<Boolean> AgentExist(string agentname)
        {
            if (await this.db.Commerciauxes.Where(a => a.AgentName.ToUpper().Equals(agentname.ToUpper())).CountAsync() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
