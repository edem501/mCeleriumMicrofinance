using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCelerium.Models;
using System.Threading.Tasks;
using iCelerium.Models.BodyClasses;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Admin,Super Admin")]
    [Audit]
    public class AdminController : Controller
    {
        private SMSServersEntities dba = new SMSServersEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //GET: Transferer membre

        public async Task<ActionResult> TransferClient(string Id, string AgentID)
        {
            IEnumerable<Commerciaux> lstCom;
            List<SelectListItem> items = new List<SelectListItem>();


            using (SMSServersEntities db = new SMSServersEntities())
            {
                lstCom = db.Commerciauxes.ToList().OrderBy(c => c.AgentName);
                foreach (Commerciaux com in lstCom)
                {
                    items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
                }
            }
            List<ClientsList> cl = new List<ClientsList>();
            List<Client> lstClients = new List<Client>();
            using (SMSServersEntities db = new SMSServersEntities())
            {
                var agaCl = db.AgentAssignClients.Where(c => c.AgentId == Id).ToList();
                foreach (AgentAssignClient ag in agaCl)
                {

                    lstClients.Add(db.Clients.Where(c => c.ClientId == ag.ClientId).First());
                }
            }
            
            foreach (Client client in lstClients)
            {
                cl.Add(new ClientsList { FullName = client.Name, selected = false, ClientID = client.ClientId });
            }
            ViewBag.Id = items;
            ViewBag.AgentID = items;
            return View(cl);
        }

        public ActionResult _MemberPartial(string Id)
        {
            List<Client> lstClients = new List<Client>();
            using (SMSServersEntities db = new SMSServersEntities())
            {
                var agaCl = db.AgentAssignClients.Where(c => c.AgentId == Id).ToList();
                foreach (AgentAssignClient ag in agaCl)
                {

                    lstClients.Add(db.Clients.Where(c => c.ClientId == ag.ClientId).First());
                }
            }
            List<ClientsList> cl = new List<ClientsList>();
            foreach (Client client in lstClients)
            {
                cl.Add(new ClientsList { FullName = client.Name, selected = false, ClientID = client.ClientId });
            }
            return PartialView(cl);
        }
        [HttpPost]
        public ActionResult TransferClient(List<ClientsList> model)
        {
            foreach (ClientsList clientLst in model)
            {
                if (clientLst.selected == true)
                {
                    AgentAssignClient clientAssign = dba.AgentAssignClients.Where(c => c.ClientId.Equals(clientLst.ClientID)).FirstOrDefault();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }

            return View();
        }


    }
}