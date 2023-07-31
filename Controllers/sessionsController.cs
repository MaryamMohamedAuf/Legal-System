using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using wafa.Models;
using static System.Collections.Specialized.BitVector32;

namespace wafa.Controllers
{
    public class sessionsController : Controller
    {
        private allEntities db = new allEntities();

        public ActionResult Index()
        {
            var sessions = db.sessions.Include(s => s.Case).Include(s => s.Court);
            return View(sessions.ToList());
        }



 public ActionResult CreateForCase(int? id)
        {
            ViewBag.case_id = id;           
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name");

            return View();
        }

       
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "session_id,session_data,session_number,case_id,court_id")] session session)
        {
            if (ModelState.IsValid)
            {
                db.sessions.Add(session);
               
                db.SaveChanges();
                return RedirectToAction("index", "cases");
            }

            ViewBag.case_id = new SelectList(db.Cases, "case_id", "case_id", session.case_id);
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name", session.court_id);

            return RedirectToAction("index", "cases");

        }


        // GET: sessions/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            session session = db.sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.case_id = new SelectList(db.Cases, "case_id", "case_no", session.case_id);
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name", session.court_id);
            return View(session);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "session_id,session_data,session_number,case_id,court_id")] session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.case_id = new SelectList(db.Cases, "case_id", "case_no", session.case_id);
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name", session.court_id);
            return View(session);
        }

        // GET: sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            session session = db.sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            session session = db.sessions.Find(id);
            db.sessions.Remove(session);
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
