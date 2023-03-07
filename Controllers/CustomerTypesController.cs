using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test_Alejandro.Models;

namespace Test_Alejandro.Controllers
{
    public class CustomerTypesController : Controller
    {
        private Test_InvoiceEntities db = new Test_InvoiceEntities();

        // GET: CustomerTypes
        public ActionResult Index()
        {
            return View(db.CustomerTypes.ToList());
        }

        // GET: CustomerTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTypes customerTypes = db.CustomerTypes.Find(id);
            if (customerTypes == null)
            {
                return HttpNotFound();
            }
            return View(customerTypes);
        }

        // GET: CustomerTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] CustomerTypes customerTypes)
        {
            if (ModelState.IsValid)
            {
                db.CustomerTypes.Add(customerTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerTypes);
        }

        // GET: CustomerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTypes customerTypes = db.CustomerTypes.Find(id);
            if (customerTypes == null)
            {
                return HttpNotFound();
            }
            return View(customerTypes);
        }

        // POST: CustomerTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] CustomerTypes customerTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerTypes);
        }

        // GET: CustomerTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTypes customerTypes = db.CustomerTypes.Find(id);
            if (customerTypes == null)
            {
                return HttpNotFound();
            }
            return View(customerTypes);
        }

        // POST: CustomerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerTypes customerTypes = db.CustomerTypes.Find(id);
            db.CustomerTypes.Remove(customerTypes);
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
