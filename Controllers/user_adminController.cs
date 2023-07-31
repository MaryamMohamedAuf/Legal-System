using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wafa.Models;
using System.Web.Security;
using System.Configuration;
using Microsoft.Ajax.Utilities;

namespace wafa.Controllers
{
    public class user_adminController : Controller
    {
        private allEntities db = new allEntities();
        public ActionResult AdminOptions()
        {
            return View();
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
        public ActionResult signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signin(User users, Admin admins)
        {
            var result = db.Users.Where(x => x.email.Equals(users.email) && x.password.Equals(users.password)).FirstOrDefault();
            if (result != null && admins.user_id == users.id)
            {
               
                var mail = db.Users.Where(x => x.email.Equals(users.email)).FirstOrDefault().email;
                var id = db.Users.Where(x => x.email.Equals(mail)).FirstOrDefault().id;
                bool admin = db.Admins.Any(x => x.user_id.Equals(id));

                

                if (id != null && admin)
                {
                    FormsAuthentication.SetAuthCookie(users.email, true);

                    return RedirectToAction("AdminOptions");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return RedirectToAction("signin");
                }

            }     
            return RedirectToAction("index");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User users, Admin admins)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();

                admins.user_id = users.id;

                db.Admins.Add(admins);
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
            User admins = db.Users.Find(id);
            if (admins == null)
            {
                return HttpNotFound();
            }
            return View(admins);

        }

        [HttpPost]
        public ActionResult Edit(User user, Admin admins)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
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
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            Admin admin = db.Admins.Find(id);
            db.Users.Remove(user);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult index()
        {
            var usersWithAdmins = from user in db.Users
                                  join admin in db.Admins on user.id equals admin.user_id into adminGroup
                                  from admin in adminGroup.DefaultIfEmpty()
                                  where admin != null
                                  select user;

            return View(usersWithAdmins.ToList());
        }

    }


}
