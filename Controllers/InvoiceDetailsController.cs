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
    public class InvoiceDetailsController : Controller
    {
        private Test_InvoiceEntities db = new Test_InvoiceEntities();

        //DELEGATE
        delegate bool UpdateInvoice(int invoiceId);

        private bool UpdateInvoiceDB(int invoiceId_)
        {
            try
            {
                var listInvoiceDetails = db.InvoiceDetail.Where(x => x.CustomerId == invoiceId_).ToList();
                var invoice = db.Invoice.Find(invoiceId_);
                invoice.TotalItbis = listInvoiceDetails.Sum(x => x.TotalItbis);
                invoice.SubTotal = listInvoiceDetails.Sum(x => x.SubTotal);
                invoice.Total = listInvoiceDetails.Sum(x => x.Total);
                var subtotal_ = listInvoiceDetails.Sum(x => x.SubTotal);
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: InvoiceDetails
        public ActionResult Index()
        {
            var invoiceDetail = db.InvoiceDetail.Include(i => i.Invoice);
            return View(invoiceDetail.ToList());
        }

        // GET: InvoiceDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetail.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Invoice, "Id", "Customers.CustName");
            return View();
        }

        // POST: InvoiceDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,Qty,Price,TotalItbis,SubTotal,Total")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceDetail.Add(invoiceDetail);
                db.SaveChanges();
                //update Invoice
                UpdateInvoice uinvoice = UpdateInvoiceDB;
                uinvoice(invoiceDetail.CustomerId);
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Invoice, "Id", "Customers.CustName", invoiceDetail.CustomerId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetail.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Invoice, "Id", "Customers.CustName", invoiceDetail.CustomerId);
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,Qty,Price,TotalItbis,SubTotal,Total")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceDetail).State = EntityState.Modified;
                db.SaveChanges();
                //update Invoice
                UpdateInvoice uinvoice = UpdateInvoiceDB;
                uinvoice(invoiceDetail.CustomerId);
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Invoice, "Id", "Customers.CustName", invoiceDetail.CustomerId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetail.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceDetail invoiceDetail = db.InvoiceDetail.Find(id);
            db.InvoiceDetail.Remove(invoiceDetail);
            db.SaveChanges();
            //update Invoice
            UpdateInvoice uinvoice = UpdateInvoiceDB;
            uinvoice(invoiceDetail.CustomerId);
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
