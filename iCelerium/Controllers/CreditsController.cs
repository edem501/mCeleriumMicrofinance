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


        public ActionResult Contract()
        {
            return View();
        }
    }
}