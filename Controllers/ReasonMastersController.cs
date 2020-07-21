using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrInfra.Models;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class ReasonMastersController : Controller
    {
        private GInfraEntities db = new GInfraEntities();

        // GET: ReasonMasters
        public ActionResult Index()
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            return View(db.ReasonMasters.ToList());
        }

        // GET: ReasonMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReasonMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Reasons,isactive")] ReasonMaster reasonMaster)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            if (ModelState.IsValid)
            {
                db.ReasonMasters.Add(reasonMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reasonMaster);
        }

        // GET: ReasonMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReasonMaster reasonMaster = db.ReasonMasters.Find(id);
            if (reasonMaster == null)
            {
                return HttpNotFound();
            }
            return View(reasonMaster);
        }

        // POST: ReasonMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Reasons,isactive")] ReasonMaster reasonMaster)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            if (ModelState.IsValid)
            {
                db.Entry(reasonMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reasonMaster);
        }

        public ActionResult Delete(int id)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            ReasonMaster hr = db.ReasonMasters.Find(id);
            db.ReasonMasters.Remove(hr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
