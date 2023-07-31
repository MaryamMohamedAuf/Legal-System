using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using wafa.Models;

namespace test.Controllers
{
    public class user_lawyerController : Controller
    {
        private allEntities db = new allEntities();


        // GET: test

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User users, Lawyer lawyers)
        {

            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                lawyers.user_id = users.id;
                db.Lawyers.Add(lawyers);
                db.SaveChanges();
            }

            return RedirectToAction("index");


        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }
            User user = db.Users.Find(id);
            Lawyer lawyer = db.Lawyers.Find(id);
            if (user == null && lawyer == null)
            {
                return HttpNotFound();
            }
            return View(user);


        }

        [HttpPost]
        public ActionResult Edit(User user, Lawyer lawyer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;

                lawyer.user_id = user.id;

                db.Entry(lawyer).State = EntityState.Modified;

                db.SaveChanges();
            }
            return RedirectToAction("index");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            User user = db.Users.Find(id);
            Lawyer lawyer = db.Lawyers.Find(id);
            db.Users.Remove(user);
            db.Lawyers.Remove(lawyer);
            db.SaveChanges();
            return RedirectToAction("index");
        }


        public ActionResult index()
        {
            var lawyers = from user in db.Users
                          join Lawyer in db.Lawyers on user.id equals Lawyer.user_id into lawyerGroup
                          from lawyer in lawyerGroup.DefaultIfEmpty()
                          where lawyer != null
                          select user;

            return View(lawyers.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User users = db.Users.Find(id);
                if (users == null)
            {
                return HttpNotFound();
            }
                 return View(users);
        }

    }
}