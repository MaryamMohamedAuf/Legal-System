using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wafa.Models;

namespace wafa.Controllers
{
    public class CourtsController : Controller
    {
        private allEntities db = new allEntities();

        // GET: Courts
        public ActionResult Index()
        {
            return View(db.Courts.ToList());
        }

        // GET: Courts/Details/5


        // GET: Courts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "court_id,name,location,judge_name")] Court court)
        {
            if (ModelState.IsValid)
            {
                db.Courts.Add(court);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(court);
        }

        // GET: Courts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            return View(court);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "court_id,name,location,judge_name")] Court court)
        {
            if (ModelState.IsValid)
            {
                db.Entry(court).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(court);
        }

        // GET: Courts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            return View(court);
        }

        // POST: Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Court court = db.Courts.Find(id);
            db.Courts.Remove(court);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            return View(court);
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
