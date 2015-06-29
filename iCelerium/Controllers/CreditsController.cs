using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;
using System.Threading.Tasks;
using PagedList;
using System.Net;
using System.Globalization;


namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class CreditsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();
        public class test
        {
           public string valeur { get; set; }
        }
        public ActionResult Checkme(string data)
        {
            test model = new test {valeur=data.ToUpper()};
            return View(model);
        }
        // GET: Credits
        public ActionResult Index()
        {
            var crt = db.CreditTypes.ToList();
            List<CreateCreditTypeViewModel> model = new List<CreateCreditTypeViewModel>();
            foreach (CreditType ct in crt)
            {
                model.Add(new CreateCreditTypeViewModel
                {
                    Duration = ct.Duration,
                    EcheancesID = ct.EcheanceID,
                    InterestRate = ct.InterestRate,
                    TypeName = ct.TypeName,
                    TypeID = ct.TypeID
                });
            }
            return View(model);
        }
        public ActionResult Create()
        {
            List<Echeance> lstech = db.Echeances.OrderBy(c => c.EcheanceName).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Echeance ech in lstech)
            {
                items.Add(new SelectListItem { Text = ech.EcheanceName.ToUpper(), Value = ech.EcheanceID.ToString() });
            }


            ViewBag.EcheancesID = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCreditTypeViewModel model)
        {
            if (ModelState.IsValid)
            {

                int ID = 0;
                if (db.CreditTypes.Count() > 0)
                {
                    ID = db.CreditTypes.Max(c => c.ID);
                }
                db.CreditTypes.Add(new CreditType
                {
                    Duration = model.Duration,
                    EcheanceID = model.EcheancesID,
                    InterestRate = model.InterestRate,
                    TypeName = model.TypeName,
                    TypeID = string.Format("C{0}", (ID + 1).ToString().PadLeft(3, '0'))
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<Echeance> lstech = db.Echeances.OrderBy(c => c.EcheanceName).ToList();
                List<SelectListItem> items = new List<SelectListItem>();
                foreach (Echeance ech in lstech)
                {
                    items.Add(new SelectListItem { Text = ech.EcheanceName.ToUpper(), Value = ech.EcheanceID.ToString() });
                }


                ViewBag.EcheancesID = items;
                return View(model);
            }
        }


        public ActionResult Contract(string clientID)
        {
            if (clientID == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Client client = db.Clients.Where(c => c.ClientId.Equals(clientID)).FirstOrDefault();
                if (clientID == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    List<CreditType> lstech = db.CreditTypes.OrderBy(c => c.TypeName).ToList();
                    List<SelectListItem> items = new List<SelectListItem>();
                    foreach (CreditType ech in lstech)
                    {
                        items.Add(new SelectListItem { Text = ech.TypeName.ToUpper(), Value = ech.TypeID.ToString().Trim() });
                    }
                    IEnumerable<SelectListItem> pop = items.AsEnumerable<SelectListItem>();

                    ViewBag.TypeID = pop;
                    ViewBag.ClientID = client.ClientId;
                    ViewBag.SubTitle = client.Name;
                    return View();
                }

            }

        }

        public ActionResult _Echelonnement(string TypeID, decimal Amount, DateTime DateFirstPyt)
        {
            CreditType creditType = db.CreditTypes.Where(c => c.TypeID.Equals(TypeID)).FirstOrDefault();
            decimal IntRate = Convert.ToDecimal(creditType.InterestRate);
            int Dur = creditType.Duration;
            decimal TotalPayable = Convert.ToDecimal(Amount * (1 + IntRate * Dur * 0.001m));
            decimal rest = Convert.ToDecimal(TotalPayable % Dur);
            decimal SubPay = Convert.ToDecimal((TotalPayable - rest) / Dur);
            decimal FirtPay = Convert.ToDecimal(SubPay + rest);

            string dpart = creditType.Echeance.DPart.Trim();
            List<echelon> lstEche = new List<echelon>();
            decimal montant = TotalPayable;
            DateTime dte = DateFirstPyt;

            for (int i = 0; i < Dur; i++)
            {
                decimal pyt = 0;
                ;
                if (i == 0)
                {
                    pyt = Math.Round(FirtPay, 2);
                }
                else
                {
                    pyt = SubPay;
                }


                lstEche.Add(new echelon
                {
                    DateEcheance = dte.AddDays(i),
                    MontantCredit = Convert.ToDouble(montant),
                    MontantPayable = Convert.ToDouble(pyt),
                    MontantRestant = Convert.ToDouble(montant - pyt)
                });
                //dte = dte.AddDays(1);
                montant = montant - pyt;
            }

            ViewBag.TypeID = Math.Round(TotalPayable, 2);
            ViewBag.FirtPay = Math.Round(FirtPay, 2);
            ViewBag.SubPay = Math.Round(SubPay, 2);
            ViewBag.Amount = Amount;
            ViewBag.DateFirstPyt = DateFirstPyt;

            return PartialView(lstEche);
        }

        public ActionResult _EchelonnementBis(string ClientID)
        {
            Client client = db.Clients.Where(c => c.ClientId.Equals(ClientID)).FirstOrDefault();
            Credit credit = db.Credits.Where(c => c.ClientID.Equals(ClientID)).OrderByDescending(d => d.DateCreated).First();

            int Dur = 31;
            decimal TotalPayable = Convert.ToDecimal(31 * client.Mise.Value);
            decimal rest = Convert.ToDecimal(TotalPayable % Dur);
            decimal SubPay = Convert.ToDecimal((TotalPayable - rest) / Dur);
            decimal FirtPay = Convert.ToDecimal(SubPay + rest);

            List<echelon> lstEche = new List<echelon>();
            decimal montant = TotalPayable;
            DateTime dte = credit.DateFirstPyt;

            for (int i = 0; i < Dur; i++)
            {
                decimal pyt = 0;
                ;
                if (i == 0)
                {
                    pyt = Math.Round(FirtPay, 2);
                }
                else
                {
                    pyt = SubPay;
                }


                lstEche.Add(new echelon
                {
                    DateEcheance = dte.AddDays(i),
                    MontantCredit = Convert.ToDouble(montant),
                    MontantPayable = Convert.ToDouble(pyt),
                    MontantRestant = Convert.ToDouble(montant - pyt)
                });
                //dte = dte.AddDays(1);
                montant = montant - pyt;
            }

            ViewBag.TypeID = Math.Round(TotalPayable, 2);
            ViewBag.FirtPay = Math.Round(FirtPay, 2);
            ViewBag.SubPay = Math.Round(SubPay, 2);
            ViewBag.Amount = credit.Amount;
            ViewBag.DateFirstPyt = credit.DateFirstPyt;
            ViewBag.Member = client.Name;

            return PartialView(lstEche);
        }

        public async Task<ActionResult> DailyDelivery(string AgentID)
        {
            IEnumerable<Commerciaux> cmx = await db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            ViewBag.AgentID = items;

            DateTime date = new DateTime();
            date = DateTime.Today.AddDays(-10);
            List<Credit> lstCredits = db.Credits.Where(c => c.status == false && c.DateCreated <= date).ToList();
            List<dailyDeliveryViewModel> lstmodel = new List<dailyDeliveryViewModel>();
            foreach (Credit dvml in lstCredits)
            {
                Client client = db.Clients.Where(c => c.ClientId.Equals(dvml.ClientID)).FirstOrDefault();
                if (string.IsNullOrEmpty(AgentID))
                {

                    bool enaable;
                    if (client.Solde >= client.Mise.Value * 10)
                    {
                        enaable = false;
                    }
                    else
                    {
                        enaable = true;
                    }
                    //lstmodel.Add(new dailyDeliveryViewModel
                    //{
                    //    ClientName = client.Name,
                    //    mise = client.Mise.Value,
                    //    MontantCredit = dvml.Amount,
                    //    Solde = client.Solde.Value,
                    //    status = dvml.status,
                    //    ID = dvml.ClientID,
                    //    Enabled = enaable
                    //});
                    
                }

                else
                {
                    int cnt = db.AgentAssignClients.Where(a => a.AgentId.Equals(AgentID) && a.ClientId.Equals(client.ClientId)).ToList().Count();
                    if (cnt >= 1)
                    {

                        bool enaable;
                        if (client.Solde >= client.Mise.Value * 10)
                        {
                            enaable = false;
                        }
                        else
                        {
                            enaable = true;
                        }
                        lstmodel.Add(new dailyDeliveryViewModel
                        {
                            ClientName = client.Name,
                            mise = client.Mise.Value,
                            MontantCredit = dvml.Amount,
                            Solde = client.Solde.Value,
                            status = dvml.status,
                            ID = dvml.ClientID,
                            Enabled = enaable
                        });
                       
                    }

                }
            }
           
            return View(lstmodel.OrderBy(c=>c.ClientName).ToList());
        }
        [HttpPost]
        public ActionResult DailyDelivery(List<dailyDeliveryViewModel> model)
        {
            if (ModelState.IsValid)
            {

                foreach (dailyDeliveryViewModel dvml in model)
                {
                    if (dvml.status == true)
                    {
                        Credit cr = db.Credits.Where(c => c.ClientID.Equals(dvml.ID)).FirstOrDefault();
                        cr.DateDisbursed = DateTime.Now;
                        cr.status = true;

                        Client client = db.Clients.Where(c => c.ClientId.Equals(cr.ClientID)).FirstOrDefault();


                        TTransaction trans = new TTransaction
                        {
                            AgentId = db.Commerciauxes.Where(c => c.AgentName.ToLower().Equals("systeme")).FirstOrDefault().AgentId,
                            ClientId = client.ClientId,
                            Credit = 0,
                            Debit = client.Mise.Value * 31,
                            Description = 0,
                            Posted = false,
                            SoldeFermeture = client.Solde.Value - client.Mise.Value * 31,
                            SoldeOuverture = client.Solde.Value,
                            DateOperation = DateTime.Now

                        };

                        client.Solde = client.Solde.Value - client.Mise.Value * 31;

                        db.TTransactions.Add(trans);
                        db.SaveChanges();
                    }
                }

            }
            return RedirectToAction("DailyDelivery");
        }

        public ActionResult ListOfCredit(int? page)
        {
            List<Client> lstClient = db.Clients.Where(c => c.Solde < 0).ToList();
            List<ClientsViewModelCredit> model = new List<ClientsViewModelCredit>();

            foreach (Client cl in lstClient)
            {
                model.Add(new ClientsViewModelCredit
                {
                    ClientId = cl.ClientId,
                    ClientTel = cl.ClientTel,
                    Mise = cl.Mise.Value,
                    Name = cl.Name,
                    Sexe = cl.Sexe,
                    Solde = string.Format("({0})", cl.Solde.Value * (-1))
                });
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListOfSavings(int? page)
        {
            List<Client> lstClient = db.Clients.Where(c => c.Solde > 0).ToList();
            List<ClientsViewModel> model = new List<ClientsViewModel>();

            foreach (Client cl in lstClient)
            {
                model.Add(new ClientsViewModel
                {
                    ClientId = cl.ClientId,
                    ClientTel = cl.ClientTel,
                    Mise = cl.Mise.Value,
                    Name = cl.Name,
                    Sexe = cl.Sexe,
                    Solde = cl.Solde.Value
                });
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Recouvrements(string agentModel, string startDate, string endDate, int? page)
        {
            IEnumerable<Commerciaux> cmx = await db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            ViewBag.agentModel = items;
            List<TransactionsViewModel> lstTrans = new List<TransactionsViewModel>();
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
            if (agentModel == null)
            {
                List<TTransaction> nulltrans = db.TTransactions.Where(c => c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 2).ToList();
                if (nulltrans != null)
                {
                    foreach (TTransaction tt in nulltrans)
                    {
                        lstTrans.Add(new TransactionsViewModel
                        {
                            AgentId = tt.AgentId,
                            Credit = tt.Credit,
                            Date = tt.DateOperation,
                            Debit = tt.Debit,
                            id = tt.id,
                            Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                            Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                            Solde_Fermeture = tt.SoldeFermeture,
                            Solde_Ouverture = tt.SoldeOuverture

                        });
                    }

                }
                ViewBag.sumC = String.Format("{0:c}", nulltrans.Sum(t => t.Credit));
                return this.View(lstTrans.ToPagedList(page ?? 1, 15));
            }
            Commerciaux Agent = await this.db.Commerciauxes.Where(c => c.AgentId == agentModel).FirstOrDefaultAsync();
            if (Agent == null)
            {
                List<TTransaction> nulltrans = db.TTransactions.Where(c => c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 2).ToList();
                if (nulltrans != null)
                {
                    foreach (TTransaction tt in nulltrans)
                    {
                        lstTrans.Add(new TransactionsViewModel
                        {
                            AgentId = tt.AgentId,
                            Credit = tt.Credit,
                            Date = tt.DateOperation,
                            Debit = tt.Debit,
                            id = tt.id,
                            Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                            Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                            Solde_Fermeture = tt.SoldeFermeture,
                            Solde_Ouverture = tt.SoldeOuverture

                        });
                    }

                }
                ViewBag.sumC = String.Format("{0:c}", nulltrans.Sum(t => t.Credit));
                return this.View(lstTrans.ToPagedList(page ?? 1, 15));
            }
            ViewBag.AgentID = agentModel;



            List<TTransaction> trans = new List<TTransaction>();
            trans = await db.TTransactions.Where(c => c.AgentId == agentModel && c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 2).ToListAsync();
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
                        Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                        Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                        Solde_Fermeture = tt.SoldeFermeture,
                        Solde_Ouverture = tt.SoldeOuverture

                    });
                }

            }
            ViewBag.sumC = String.Format("{0:c}", trans.Sum(t => t.Credit));
            return this.View(lstTrans.ToPagedList(page ?? 1, 15));
        }

        public async Task<ActionResult> Collettes(string agentModel, string startDate, string endDate, int? page)
        {
            IEnumerable<Commerciaux> cmx = await db.Commerciauxes.OrderBy(c => c.AgentName).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Commerciaux com in cmx)
            {
                items.Add(new SelectListItem { Text = com.AgentName.ToUpper(), Value = com.AgentId });
            }
            ViewBag.agentModel = items;
            List<TransactionsViewModel> lstTrans = new List<TransactionsViewModel>();
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
            if (agentModel == null)
            {
                List<TTransaction> nulltrans = db.TTransactions.Where(c => c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 0).ToList();
                if (nulltrans != null)
                {
                    foreach (TTransaction tt in nulltrans)
                    {
                        lstTrans.Add(new TransactionsViewModel
                        {
                            AgentId = tt.AgentId,
                            Credit = tt.Credit,
                            Date = tt.DateOperation,
                            Debit = tt.Debit,
                            id = tt.id,
                            Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                            Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                            Solde_Fermeture = tt.SoldeFermeture,
                            Solde_Ouverture = tt.SoldeOuverture

                        });
                    }

                }
                ViewBag.sumC = String.Format("{0:c}", nulltrans.Sum(t => t.Credit));
                return this.View(lstTrans.ToPagedList(page ?? 1, 15));
            }
            Commerciaux Agent = await this.db.Commerciauxes.Where(c => c.AgentId == agentModel).FirstOrDefaultAsync();
            if (Agent == null)
            {
                List<TTransaction> nulltrans = db.TTransactions.Where(c => c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 0).ToList();
                if (nulltrans != null)
                {
                    foreach (TTransaction tt in nulltrans)
                    {
                        lstTrans.Add(new TransactionsViewModel
                        {
                            AgentId = tt.AgentId,
                            Credit = tt.Credit,
                            Date = tt.DateOperation,
                            Debit = tt.Debit,
                            id = tt.id,
                            Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                            Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                            Solde_Fermeture = tt.SoldeFermeture,
                            Solde_Ouverture = tt.SoldeOuverture

                        });
                    }

                }
                ViewBag.sumC = String.Format("{0:c}", nulltrans.Sum(t => t.Credit));
                return this.View(lstTrans.ToPagedList(page ?? 1, 15));
            }
            ViewBag.AgentID = agentModel;



            List<TTransaction> trans = new List<TTransaction>();
            trans = await db.TTransactions.Where(c => c.AgentId == agentModel && c.DateOperation >= date1 && c.DateOperation <= date2 && c.Description == 0).ToListAsync();
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
                        Nom_Client = db.Clients.Where(c => c.ClientId == tt.ClientId).First().Name,
                        Nom_Commercial = db.Commerciauxes.Where(c => c.AgentId == tt.AgentId).First().AgentName,
                        Solde_Fermeture = tt.SoldeFermeture,
                        Solde_Ouverture = tt.SoldeOuverture

                    });
                }

            }
            ViewBag.sumC = String.Format("{0:c}", trans.Sum(t => t.Credit));
            return this.View(lstTrans.ToPagedList(page ?? 1, 15));
        }
        [HttpPost]
        public ActionResult testme(List<string> json )
        {
            if (json != null)
            {
                Console.WriteLine("Working...");
            }
            return View();
        }
    }
}