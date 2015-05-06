using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;
using System.Threading.Tasks;


namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class CreditsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();
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
            List<Echeance> lstech =  db.Echeances.OrderBy(c => c.EcheanceName).ToList();
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
                    Duration=model.Duration,
                    EcheanceID=model.EcheancesID,
                    InterestRate=model.InterestRate,
                    TypeName=model.TypeName,
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
                Client client = db.Clients.Where(c=>c.ClientId.Equals(clientID)).FirstOrDefault();
                if (clientID == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    List<CreditType> lstech = db.CreditTypes.OrderBy(c=>c.TypeName).ToList();
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
                if(i==0)
                {
                    pyt = Math.Round(FirtPay, 2);
                }
                else
                {
                    pyt = SubPay;
                }

               
                lstEche.Add(new echelon
                {
                    DateEcheance=dte.AddDays(i),
                    MontantCredit=Convert.ToDouble(montant),
                    MontantPayable=Convert.ToDouble(pyt),
                    MontantRestant=Convert.ToDouble(montant-pyt)
                });
                //dte = dte.AddDays(1);
                montant = montant - pyt;
            }

            ViewBag.TypeID = Math.Round(TotalPayable,2);
            ViewBag.FirtPay =Math.Round(FirtPay,2);
            ViewBag.SubPay = Math.Round(SubPay,2);
            ViewBag.Amount = Amount;
            ViewBag.DateFirstPyt = DateFirstPyt;

            return PartialView(lstEche);
        }
    }
}