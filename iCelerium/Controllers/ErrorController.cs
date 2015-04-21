using System;
using System.Linq;
using System.Web.Mvc;

namespace iCelerium.Controllers
{
   [Authorize]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult NotFound()
        {

            return View();
        }
    }
}