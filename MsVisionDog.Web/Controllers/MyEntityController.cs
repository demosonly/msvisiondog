using MsVisionDog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MsVisionDog.Web.Controllers
{
    [Authorize]
    public class MyEntityController : Controller
    {
        MyDbContext _db;

        public MyEntityController()
        {
            _db = new MyDbContext();
        }

        public ActionResult Index()
        {
            ViewBag.MyEntities = _db.MyEntities.Where(e => e.UserID == User.Identity.Name).OrderByDescending(e => e.DateCreated).ToList();
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            var myEntity = new MyEntity();
            myEntity.MyEntityID = Guid.NewGuid().ToString();
            myEntity.UserID = User.Identity.Name;
            myEntity.Name = name;
            myEntity.DateCreated = DateTime.UtcNow;
            _db.MyEntities.Add(myEntity);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            ViewBag.MyEntity = _db.MyEntities.Where(e => e.UserID == User.Identity.Name && e.MyEntityID == id).FirstOrDefault();

            var imageIDs = _db.MyEntityImages.Where(ei => ei.UserID == User.Identity.Name && ei.MyEntityID == id).Select(ei => ei.MyImageID).ToList();
            var myImages = _db.MyImages.Where(en => en.UserID == User.Identity.Name && imageIDs.Contains(en.MyImageID)).ToList();

            ViewBag.MyImages = myImages;

            return View();
        }
    }
}