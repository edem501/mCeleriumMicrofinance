using iCelerium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCelerium.Models.BodyClasses;
using System.Net;
namespace iCelerium.Controllers
{
    [Authorize(Roles = "Admin,Super Admin,Utilisateur")]
    [Audit]
    public class ZonesController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();
        // GET: Zones
        public ActionResult Index()
        {
            List<ZonesModel> lstZones = new List<ZonesModel>();
            foreach (Zone zone in db.Zones.ToList())
            {
                lstZones.Add(new ZonesModel { ZoneID = zone.ZoneID, ZoneName = zone.ZoneName, Id=zone.ID });
            }
            return View(lstZones);
        }

        // GET: Zones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Zones/Create
        [Authorize(Roles = "Admin,Super Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Zones/Create
        [HttpPost]
        public ActionResult Create(CreateZoneModel name)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Zones.Where(c => c.ZoneName.Equals(name.ZoneName)).Count() > 0)
                    {
                        throw new HttpException(string.Format("La Zone {0} existe deja dans la base", name.ZoneName));
                    }
                    else
                    {
                        db.Zones.Add(new Zone { ZoneName = name.ZoneName, ZoneID = name.ZoneName.Substring(0, 3) });
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }

        // GET: Zones/Edit/5
        [Authorize(Roles = "Admin,Super Admin")]
        public ActionResult Edit(int Id)
        {
            Zone zone = db.Zones.Find(Id);
            if (zone == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(new ZonesModel { Id=zone.ID,ZoneID=zone.ZoneID,ZoneName=zone.ZoneName});
        }

        // POST: Zones/Edit/5
        [HttpPost]
        public ActionResult Edit(ZonesModel zone)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Zones.Where(c => c.ZoneName.Equals(zone.ZoneName)).Count() > 0)
                    {
                        throw new HttpException(string.Format("La Zone {0} existe deja dans la base", zone.ZoneName));
                    }
                    else
                    {
                        Zone nuZone = db.Zones.Find(zone.Id);
                        nuZone.ZoneName = zone.ZoneName;
                        nuZone.ZoneID = zone.ZoneID;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return View();
        }

        // GET: Zones/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Zones/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
