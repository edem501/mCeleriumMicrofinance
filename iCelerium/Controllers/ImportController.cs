using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.Threading.Tasks;
using iCelerium.Models.BodyClasses;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class ImportController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();
        // GET: Import

        public ActionResult IAgent()
        {
            return View();
        }

        public ActionResult Agent()
        {
            if (Request.Files["fileupload1"].FileName.Length > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/UploadedFolder"), Request.Files["FileUpload1"].FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);

                Request.Files["FileUpload1"].SaveAs(path1);
                var listofAgent = Importexcel(path1);

                foreach (var agt in listofAgent)
                {
                    var agent = new CreateAgentViewModels
                    {
                        AgentActif = false,
                        AgentName = agt["Name"],
                        AgentTel = agt["Telephone"]
                    };
                    var text = agt["Zone"];
                    var zone = db.Zones.Where(z => z.ZoneName.Equals(text)).FirstOrDefault().ID;

                    AddNewAgent(agent, zone.ToString());
                }
                return RedirectToAction("Index", "Agents");
            }
            else
            {
                return View();
            }


        }
        public ActionResult IMember()
        {
            IEnumerable<Commerciaux> cmx = db.Commerciauxes.OrderBy(c => c.AgentName).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            ViewBag.Agents = items;
            return View();
        }
        public ActionResult Member(FormCollection frm)
        {
            var agentId = frm["Agents"].ToString();

            if (Request.Files["fileupload1"].FileName.Length > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName);
                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/UploadedFolder"), Request.Files["FileUpload1"].FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);

                Request.Files["FileUpload1"].SaveAs(path1);
                var listofAgent = Importexcel(path1);
               
                foreach (var agt in listofAgent)
                {
                    var client = new ClientsViewModel
                       {
                           ClientTel = agt["Telephone"],
                           Mise = GetMise(agt["Mise"].Value.ToString()),
                           Name = agt["Membre"],
                           Sexe = agt["Sexe"],
                           Solde = GetSolde(agt["Solde"].Value.ToString())
                       };
                    var text = agt["AgentName"];
                    agentId = db.Commerciauxes.Where(c => c.AgentName.Equals(text)).FirstOrDefault().AgentId;
                    AddNewClient(client, agentId);
                }
                return RedirectToAction("Index", "Clients");
            }
            else
            {
                return View();
            }


            return RedirectToAction("Index", "Agents");
        }

        public void AddNewAgent(CreateAgentViewModels agent, string Zones)
        {
            Commerciaux commerciaux = new Commerciaux();
            if (AgentExist(agent.AgentName) == false)
            {
                var id = 0;
                if (db.Commerciauxes.Count() == 0)
                {
                    id = 0;
                }
                else
                {
                    id = db.Commerciauxes.Max(a => a.Id);
                }

                commerciaux.AgentId = string.Format("A{0}", (id + 1).ToString().PadLeft(3, '0'));
                commerciaux.AgentName = agent.AgentName;
                commerciaux.AgentActif = agent.AgentActif;
                commerciaux.AgentTel = agent.AgentTel;
                commerciaux.AgentPass = "1234";
                commerciaux.ZoneID = Convert.ToInt32(Zones);
                this.db.Commerciauxes.Add(commerciaux);
                db.SaveChanges();

                var newId = commerciaux.Id;
                commerciaux.AgentId = string.Format("A{0}", newId.ToString().PadLeft(3, '0'));
                db.SaveChanges();
                this.TempData["Message"] = String.Format("Success!! an account for {0} has been created", commerciaux.AgentName);

            }
            else
            {
                this.TempData["Message"] = String.Format("Error!! Cannot create account. {0} has an account already in iCelerium", agent.AgentName);

            }
        }

        public void AddNewClient(ClientsViewModel client, string AgentID)
        {
            Client newClient = new Client();
            
            if (MemberExist(client.Name) == false)
            {
                var id = 0;
                id = db.Clients.Where(c => c.ClientId.StartsWith(AgentID)).Count();

                newClient.ClientId = string.Format("{0}{1}", AgentID, (id + 1).ToString().PadLeft(4, '0'));
                newClient.Name = client.Name;
                newClient.Sexe = client.Sexe;
                newClient.ClientTel = client.ClientTel;
                newClient.DateCreated = DateTime.Now;
                newClient.Mise = client.Mise;
                newClient.Solde = client.Solde;

                this.db.AgentAssignClients.Add(new AgentAssignClient
                {
                    AgentId=AgentID,
                    ClientId=newClient.ClientId,
                    DateAssignee=DateTime.Now
                });
                this.db.Clients.Add(newClient);
                db.SaveChanges();


                this.TempData["Message"] = String.Format("Success!! an account for {0} has been created", client.Name);

            }
            else
            {
                this.TempData["Message"] = String.Format("Error!! Cannot create account. {0} has an account already in iCelerium", client.Name);

            }
        }

        public List<Row> Importexcel(string path)
        {

            var excel = new ExcelQueryFactory(path);
            
            var listofAgent = excel.Worksheet(0).ToList();
            return listofAgent;

        }
        public Boolean AgentExist(string agentname)
        {
            if (db.Commerciauxes.Where(a => a.AgentName.ToUpper().Equals(agentname.ToUpper())).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean MemberExist(string memberName)
        {
            if (db.Clients.Where(a => a.Name.ToUpper().Equals(memberName.ToUpper())).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double GetMise(string mise)
        {
            Double iMise = 0;
            
            if (!string.IsNullOrEmpty(mise))
            {
                iMise = Convert.ToDouble(mise);
            }
            else
            {
                iMise = 0;
            }
            return iMise;
        }
        public double GetSolde( string solde)
        {
            double iSolde = 0;
            if (!string.IsNullOrEmpty(solde))
            {
                iSolde = Convert.ToDouble(solde);
            }
            else
            {
                iSolde = 0;
            }
            return iSolde;
        }
    }
}