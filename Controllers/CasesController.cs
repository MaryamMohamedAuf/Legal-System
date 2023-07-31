using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using wafa.Models;

namespace wafa.Controllers
{
    public class CasesController : Controller
    {
        private allEntities db = new allEntities();
        public ActionResult Create()
        {
            ViewBag.admin_id = new SelectList(db.Admins, "user_id", "user_id");
            ViewBag.client_id = new SelectList(db.Clients, "user_id", "user_id");
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name");
            ViewBag.lawyer_id = new SelectList(db.Lawyers, "user_id", "user_id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "case_no,issuance_date,court_id,admin_id,client_id,lawyer_id")] Case @case, HttpPostedFileBase documents)
        {
            if (ModelState.IsValid)
            {
                if (documents != null && documents.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(documents.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Documents"), fileName);
                    documents.SaveAs(path); 
                    @case.documents = path;
                }

                db.Cases.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.court_id = new SelectList(db.Courts, "court_id", "court_name", @case.court_id);
            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name", @case.admin_id);
            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_name", @case.client_id);
            ViewBag.lawyer_id = new SelectList(db.Lawyers, "lawyer_id", "lawyer_name", @case.lawyer_id);
            return View(@case);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            ViewBag.admin_id = new SelectList(db.Admins, "user_id", "user_id", @case.admin_id);
            ViewBag.client_id = new SelectList(db.Clients, "user_id", "user_id", @case.client_id);
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name", @case.court_id);
            ViewBag.lawyer_id = new SelectList(db.Lawyers, "user_id", "licence", @case.lawyer_id);
            return View(@case);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "case_id,case_no,issuance_date,court_id,admin_id,client_id,lawyer_id,documents")] Case @case, HttpPostedFileBase documents)
        {
            if (ModelState.IsValid)
            {
                if (documents != null )
                {
                    var fileName = Path.GetFileName(documents.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/Documents"), fileName);
                    documents.SaveAs(path); 
                    @case.documents = path; 

                }

                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.court_id = new SelectList(db.Courts, "court_id", "court_name", @case.court_id);
            ViewBag.admin_id = new SelectList(db.Admins, "admin_id", "admin_name", @case.admin_id);
            ViewBag.client_id = new SelectList(db.Clients, "client_id", "client_name", @case.client_id);
            ViewBag.lawyer_id = new SelectList(db.Lawyers, "lawyer_id", "lawyer_name", @case.lawyer_id);
            return View(@case);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            var sessions = db.sessions.Where(s => s.case_id == id);
            foreach (var session in sessions)
            {
                db.sessions.Remove(session);
            }
            db.Cases.Remove(@case);
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

        public ActionResult find_related_sessions(int id)
        {
              session sessions = db.sessions.Find(id);
            if (id != null) {
                var idd = from session in db.sessions
                          where session.case_id==id
                          select session;

                
                return View(idd.ToList());

            }
           return View(sessions);
        }
       public ActionResult index()
        {
       
            var query = from Case in db.Cases
                        join Court in db.Courts on Case.court_id equals Court.court_id into courtGroup
                        from Court in courtGroup.DefaultIfEmpty()

                        join Lawyer in db.Lawyers on Case.lawyer_id equals Lawyer.user_id into lawyerGroup
                        from lawyer in lawyerGroup.DefaultIfEmpty()
                       

        join Client in db.Clients on Case.case_id equals Client.user_id into clientGroup
        from client in clientGroup.DefaultIfEmpty()
        join Admin in db.Admins on Case.admin_id equals Admin.user_id into adminGroup
        from admin in adminGroup.DefaultIfEmpty()

         select Case;
            return View(query.ToList());
        }
        public ActionResult GetLawyerDetails(int id)
        {
            int? lawyerId = db.Cases.Where(c => c.case_id == id).Select(c => c.lawyer_id).FirstOrDefault();
            if (lawyerId != null)
            {
                return RedirectToAction("Details", "user_lawyer", new { id = lawyerId });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetClientDetails(int? id)
        {
            int? clientId = db.Cases.Where(c => c.case_id == id).Select(c => c.client_id).FirstOrDefault();
            if (clientId != null)
            {
                return RedirectToAction("Details", "user_client", new { id = clientId });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetAdminDetails(int id)
        {
            int? adminId = db.Cases.Where(c => c.case_id == id).Select(c => c.admin_id).FirstOrDefault();
            if (adminId != null)
            {
                return RedirectToAction("Details", "user_admin", new { id = adminId });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetCourtDetails(int id)
        {
            int? courtId = db.Cases.Where(c => c.case_id == id).Select(c => c.court_id).FirstOrDefault();
            if (courtId != null)
            {
                return RedirectToAction("Details", "Courts", new { id = courtId });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        int x;
        public ActionResult CreateForCase(int id)
        {
            x = id;
            ViewBag.case_id = id;
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForCase( session session)
        {
            if (ModelState.IsValid)
            {
                db.sessions.Add(session);               
                session.case_id = x;

                db.SaveChanges();
                return RedirectToAction("index", "Cases");
            }

            ViewBag.case_id = session.case_id;
            ViewBag.court_id = new SelectList(db.Courts, "court_id", "name", session.court_id);

            return View(session);
        }
       
    }
}
