using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcShortLinks.Models;
using MvcShortLinks.App_Start;
using System.Data;

namespace MvcShortLinks.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private ShortURLDBContext db = new ShortURLDBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult URL(string id)
        {
            ShortURL shortenURL = this.GetDataByURL(id);

            if (shortenURL != null){

                shortenURL.NumOfVisit = shortenURL.NumOfVisit + 1;

                db.Entry(shortenURL).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect(shortenURL.Long_URL);
            }
            else
                return Content("URL is not found.");
        }

        private ShortURL GetDataByURL(string id)
        {
            var shortenURL = db.ShortURLs
                            .Where(b => b.Short_URLID == id)
                            .FirstOrDefault();

            
            if (shortenURL != null)
                return shortenURL;


            return null;
        }
    }
}
