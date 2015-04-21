using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace iCelerium.Models
{
  
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;
            //Generate an audit
            string ac;
            if(request.RawUrl.Length>50){
                ac = request.RawUrl.Substring(0,50) ;
                }
                else{
                    ac = request.RawUrl;
                }
            AuditRecord audit = new AuditRecord()
            {
                //Your Audit Identifier
                AuditID = Guid.NewGuid(),
                //Our Username (if available)
                UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name :
                "Anonymous",
                //The IP Address of the Request
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                //The URL that was accessed
                AreaAccessed=ac,
                //Creates our Timestamp
                Timestamp = DateTime.UtcNow
            };
            //Stores the Audit in the Database
            SMSServersEntities context = new SMSServersEntities();
            context.AuditRecords.Add(audit);
            context.SaveChanges();
            //Finishes executing the Action as normal
            base.OnActionExecuting(filterContext);
        }
    }
}