using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcShortLinks.Models;
using MvcShortLinks.App_Start;

namespace MvcShortLinks.Controllers
{
    public class ShortURLController : Controller
    {
        private ShortURLDBContext db = new ShortURLDBContext();



        public JsonResult CreateShortURL(string url)
        {
            JsonResult result = new JsonResult();
            String resultCode = "SUCCESS";
            Boolean resultSuccess = true;

            String returnMessage = "";

            String strShortenURL = "";
            String strNumOfClicks = "0";
            
            if (this.IsURLValid(url) == true)
            {
                ShortURL shortenURL = this.GetDataByURL(url);
                if (shortenURL != null)
                {
                    // Return View and Return shortURL and shortURL Stats
                    returnMessage = "Return View and Return shortURL and shortURL Stats";

                    resultCode = "SUCCESS_BUT_EXISTS";
                    resultSuccess = true;
                    returnMessage = "The URL is already shorten.";
                }
                else
                {
                    // GenerateShortURLID And Add URL to Database
                    returnMessage = "GenerateShortURLID And Add URL to Database";
                    String domainName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    String sUID = GenerateShortURLID();
                    shortenURL = new ShortURL();
                    shortenURL.DateAdded = DateTime.Now;
                    shortenURL.Long_URL = url;
                    shortenURL.Short_URL = domainName + "/" + sUID;
                    shortenURL.Short_URLID = sUID;
                    shortenURL.NumOfVisit = 0;

                    // Save to Database
                    db.ShortURLs.Add(shortenURL);
                    db.SaveChanges();


                    resultCode = "SUCCESS";
                    resultSuccess = true;
                    returnMessage = "URL successfully shorten.";


                }


                strShortenURL = shortenURL.Short_URL;
                strNumOfClicks = shortenURL.NumOfVisit.ToString();
            }
            else
            {
                returnMessage = "Invalid URL.";
                resultSuccess = false;
                resultCode = "INVALID_URL";
            }

            result = new JsonResult
            {
                Data = new
                {
                    success = resultSuccess,
                    message = returnMessage,
                    success_code = resultCode,
                    shorten_url = strShortenURL,
                    num_clicks = strNumOfClicks,
                    
                }
            };


            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }

        private Boolean IsURLValid(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }
        private string GenerateShortURLID()
        {
            String result = "";
            Boolean isResultOK = false;

            while (isResultOK == false)
            {
                Random rnd = new Random();
                int id = rnd.Next();

                result = MyEncryption.Encrypt(id.ToString(), 5);


                var shortUrlId = db.ShortURLs
                                .Where(b => b.Short_URLID == result)
                                .FirstOrDefault();

                if (shortUrlId == null)
                    isResultOK = true;
            }

            return result;
        }

        private Boolean IsURLAlreadyInDatabase(string url)
        {
            var LongURL = db.ShortURLs
                            .Where(b => b.Long_URL == url)
                            .FirstOrDefault();

            if (LongURL != null)
                return true;

            return false;
        }

        private ShortURL GetDataByURL(string url)
        {
            var shortenURL = db.ShortURLs
                            .Where(b => b.Long_URL == url)
                            .FirstOrDefault();

            if (shortenURL != null)
                return shortenURL;

            shortenURL = db.ShortURLs
                        .Where(b => b.Short_URL == url)
                        .FirstOrDefault();

            if (shortenURL != null)
                return shortenURL;


            return null;
        }

        //
        // GET: /ShortURL/

        public ActionResult Index()
        {
            return View(db.ShortURLs.ToList());
        }

        //
        // GET: /ShortURL/Details/5

        public ActionResult Details(int id = 0)
        {
            ShortURL shorturl = db.ShortURLs.Find(id);
            if (shorturl == null)
            {
                return HttpNotFound();
            }
            return View(shorturl);
        }

        //
        // GET: /ShortURL/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShortURL/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShortURL shorturl)
        {
            if (ModelState.IsValid)
            {
                db.ShortURLs.Add(shorturl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shorturl);
        }

        //
        // GET: /ShortURL/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShortURL shorturl = db.ShortURLs.Find(id);
            if (shorturl == null)
            {
                return HttpNotFound();
            }
            return View(shorturl);
        }

        //
        // POST: /ShortURL/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShortURL shorturl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shorturl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shorturl);
        }

        //
        // GET: /ShortURL/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ShortURL shorturl = db.ShortURLs.Find(id);
            if (shorturl == null)
            {
                return HttpNotFound();
            }
            return View(shorturl);
        }

        //
        // POST: /ShortURL/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShortURL shorturl = db.ShortURLs.Find(id);
            db.ShortURLs.Remove(shorturl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}