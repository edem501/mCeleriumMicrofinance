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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //GET: Transferer membre

        public async Task<ActionResult> TransferClient()
        {
            IEnumerable<Commerciaux> lstCom;
           List<SelectListItem> items = new List<SelectListItem>();
           

            using (SMSServersEntities db = new SMSServersEntities()){
                lstCom = db.Commerciauxes.ToList().OrderBy(c=> c.AgentName);
             foreach (Commerciaux com in lstCom)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            }

            ViewBag.Comm = items;
         

            return View();
        }

        public ActionResult _MemberPartial(string Id)
        {
            List<Client> lstClients = new List<Client>();
            using (SMSServersEntities db = new SMSServersEntities())
            {
                var agaCl= db.AgentAssignClients.Where(c=>c.AgentId==Id).ToList();
                foreach (AgentAssignClient ag in agaCl)
            {

                lstClients.Add(db.Clients.Where(c=>c.ClientId==ag.ClientId).First());
            }
            }
            List<ClientsList> cl = new List<ClientsList>();
            foreach (Client client in lstClients)
            {
                cl.Add(new ClientsList { FullName = client.Name, selected = false, ClientID=client.ClientId });
            }
            return PartialView(cl);
        }

       

    }
}