using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using wafa.Models;

namespace wafa.Controllers
{
    public class user_clientController : Controller
    {
        private allEntities db = new allEntities();
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User users, Client clients)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();

                clients.user_id = users.id;

                db.Clients.Add(clients);
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
        public ActionResult Edit(User user, Client clients)
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            Client client = db.Clients.Find(id);
            db.Users.Remove(user);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult index()
        {
            var usersWithAdmins = from user in db.Users
                                  join Client in db.Clients on user.id equals Client.user_id into adminGroup
                                  from admin in adminGroup.DefaultIfEmpty()
                                  where admin != null
                                  select user;

            return View(usersWithAdmins.ToList());
        }
    }
}