using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    public class PostingController : Controller
    {
        public readonly SMSServersEntities db = new SMSServersEntities();
        public JsonResult LoadClients(string agentID)
        {
            List<SelectListItem> lstPolling = new List<SelectListItem>();
            
            foreach  (var AgCl in db.AgentAssignClients.Where(a=>a.AgentId.Equals(agentID)).ToList())
            {
                 List<Client> cty = db.Clients.Where(c => c.ClientId.Equals(AgCl.ClientId)).ToList();
                 foreach (Client client in cty)
                 {
                     lstPolling.Add(new SelectListItem { Text = client.Name, Value = client.ClientId.ToString() });
                 }
               
            }
            return Json(lstPolling, JsonRequestBehavior.AllowGet);
        }
        
        //
        //GET: /Manual
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Manual()
        {
            var lstAgent = db.Commerciauxes.ToList();
            List<SelectListItem> AgentsList = new List<SelectListItem>();
            List<SelectListItem> lstClient = new List<SelectListItem>();
            foreach (Commerciaux cmx in lstAgent)
            {
                AgentsList.Add(new SelectListItem { Text = cmx.AgentName, Value = cmx.AgentId.ToString() });
            }
            ViewBag.AgentsList = AgentsList;
            ViewBag.lstClient = lstClient;
            return View();

        }

        //
        // GET: /Posting/
        public async Task<ActionResult> Index(string startDate, string endDate)
        {
            IEnumerable<Commerciaux> cmx = await this.db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            this.ViewBag.agentModel = items;
            DateTime date1, date2;

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
             
            IEnumerable<spToBeValidated_Result> toPostList = this.db.spToBeValidated(date1, date2);
            this.ViewBag.sDate = date1;
            this.ViewBag.eDate = date2;

            return this.View(toPostList);
        }

        public ActionResult Posting()
        {
            return this.View();
        }

        public ActionResult Details(string agentId, DateTime sDate, DateTime eDate)
        {

            PostingDetails pd = new PostingDetails
            {
                agentName = this.db.Commerciauxes.Where(c => c.AgentId == agentId).FirstOrDefault().AgentName,
                Live = this.GetLive(agentId, sDate, eDate),
                Upload = this.GetUploaded(agentId, sDate, eDate)
            };
            var querry = this.db.TTransactions.Where(c => c.AgentId == agentId && c.DateOperation >= sDate && c.DateOperation < eDate);
            List<int> TransIDs = new List<int>();
           
            foreach (TTransaction tt in querry.ToList())
            {
                TransIDs.Add(tt.id);
            }
            this.TempData["TransID"] = TransIDs;
            ViewBag.sDate = sDate;
            ViewBag.aID = agentId;

            return this.View(pd);
        }

        [HttpPost]
        public ActionResult Details()
        {
            PostingDetails model = (PostingDetails)this.TempData["Mod"];
            PostedTansaction pt = new PostedTansaction
            {
                AgentInfo = model.agentName,
                Numtrans = (int)model.Live.NTrans,
                UserID = this.HttpContext.User.Identity.Name,
                PostingStamp = DateTime.Now,
                SCollectes = (decimal)model.Live.SCollet,
                SPaiments = (decimal)model.Live.SPaiement,
                SPayable = (decimal)model.Live.Payable,
                SNouveauClient = (decimal)model.Live.SNouveauClient
            };
            List<int> topostID = (List<int>)this.TempData["TransID"];
            foreach (int id in topostID)
            {
                this.db.spUpdatePosted(id);
            }
            this.db.PostedTansactions.Add(pt);

            this.db.SaveChanges();

            return this.RedirectToAction("Index");
        }

        public PostingViewModel GetLive(string agentId, DateTime sDate, DateTime eDate)
        {
            spToBePosted_Result spLive = this.db.spToBePosted(sDate, eDate).Where(c => c.AgentId == agentId).FirstOrDefault();
            if (spLive != null)
            { 
                PostingViewModel pv = new PostingViewModel
                {
                    AgentId = spLive.AgentId,
                    AgentName = spLive.AgentName,
                    NTrans = spLive.NTrans,
                    SCollet = spLive.SCollet,
                    SPaiement = spLive.SPaiement,
                    Payable = (decimal?)(spLive.SCollet + spLive.SNouveauClient - spLive.SPaiement),
                    SNouveauClient=spLive.SNouveauClient
                    
                };
                return pv;
            }
            else
            {
                PostingViewModel pv = new PostingViewModel
                {
                    AgentId = agentId,
                    AgentName = this.db.Commerciauxes.Where(c => c.AgentId == agentId).FirstOrDefault().AgentName,
                    NTrans = 0,
                    Payable = 0,
                    SCollet = 0,
                    SPaiement = 0,
                    SNouveauClient=0
                }; 
                
                return pv;
            }
        }

        public ActionResult Adjust(DateTime sDate, String agentId)
        {
            List<toRealign_Result> tranIDs = db.toRealign(agentId, sDate).ToList();
            foreach (toRealign_Result tr in tranIDs)
            {
                db.TTransactions.Add(new TTransaction
                {
                    AgentId = tr.AgentId,
                    ClientId = tr.ClientId,
                    Credit = (Double)tr.Credit,
                    DateOperation = sDate,
                    Debit = (Double)tr.Debit,
                    Description = (Double)tr.Description,
                    Posted = tr.Posted,
                    SoldeFermeture = (Double)tr.SoldeFermeture,
                    SoldeOuverture = (Double)tr.SoldeOuverture
                });
                
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public PostingViewModel GetUploaded(string agentId, DateTime? sDate, DateTime? eDate)
        {
            spToBePostedUploaded_Result spUpload = this.db.spToBePostedUploaded(sDate, eDate).Where(c => c.AgentId == agentId).FirstOrDefault();
         
            if (spUpload != null)
            { 
                PostingViewModel pv = new PostingViewModel
                {
                    AgentId = spUpload.AgentId,
                    AgentName = spUpload.AgentName,
                    NTrans = spUpload.NTrans,
                    SCollet = spUpload.SCollet,
                    SPaiement = spUpload.SPaiement,
                    Payable = (decimal?)(spUpload.SCollet + spUpload.SNouveauClient - spUpload.SPaiement),
                    SNouveauClient=spUpload.SNouveauClient
                };
                return pv;
            }
            else
            {
                PostingViewModel pv = new PostingViewModel
                {
                    AgentId = agentId,
                    AgentName = this.db.Commerciauxes.Where(c => c.AgentId == agentId).FirstOrDefault().AgentName + " Veillez transferer les donnees vers le serveur",
                    NTrans = 0,
                    Payable = 0,
                    SCollet = 0,
                    SPaiement = 0,
                    SNouveauClient=0
                }; 
                
                return pv;
            }
        }

        public async Task<ActionResult> ValidationsList(string UserID, DateTime? sDate, DateTime? eDate)
        {
            IEnumerable<AspNetUser> cmx = await this.db.AspNetUsers.Where(c => c.UserName != "Admin").OrderBy(c => c.FullName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "------------", Value = "" });
            foreach (AspNetUser com in cmx)
            {
                items.Add(new SelectListItem { Text = com.FullName.ToUpper(), Value = com.UserName });
            }
            this.ViewBag.UserID = items;
            List<Validations_Result> vr;
            if (UserID == null)
            {
                sDate = DateTime.Today;
                eDate = DateTime.Today.AddDays(1);
                vr = this.db.Validations(sDate, eDate, this.HttpContext.User.Identity.Name).ToList();
            }
            else
            {
                vr = this.db.Validations(sDate, eDate, UserID).ToList();
            }
            this.ViewBag.total = String.Format("{0:c}", vr.Sum(c => c.SPayable));
            List<ValidationViewModel> pv = new List<ValidationViewModel>();

            foreach (Validations_Result result in vr)
            {
                pv.Add(new ValidationViewModel
                {
                    AgentName = result.AgentName,
                    FullName = result.FullName,
                    Numtrans = result.Numtrans,
                    PostingStamp = result.PostingStamp,
                    SCollectes = result.SCollectes,
                    SPaiments = result.SPaiments,
                    SPayable = result.SPayable
                });
            }

            return this.View(pv);
        }
    }
}