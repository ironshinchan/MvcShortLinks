using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcShortLinks.Models
{
    public class ShortURL
    {
        public int ID { get; set; }
        public string Long_URL { get; set; }
        public DateTime DateAdded { get; set; }
        public string Short_URLID { get; set; }
        public string Short_URL { get; set; }
        public int NumOfVisit { get; set; }


    }
    public class ShortURLDBContext : DbContext
    {
        public DbSet<ShortURL> ShortURLs { get; set; }
    }
}