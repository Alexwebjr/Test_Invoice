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
    public class InvoicesController : Controller
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

        private bool UpdateInvoiceDetailsDB(int invoiceId_)
        {
            try
            {
                var listaDetalles = new List<InvoiceDetail>();
                var invoiceDetails = db.InvoiceDetail.Where(x => x.CustomerId == invoiceId_).ToList();
                invoiceDetails.ForEach(x => {
                    var costo = x.Qty * x.Price;
                    var itbis = costo * (decimal)0.18;
                    x.TotalItbis = itbis;
                        x.SubTotal = costo;
                        x.Total = costo + itbis;
                });
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        // GET: Invoices
        public ActionResult Index()
        {
            var invoice = db.Invoice.Include(i => i.Customers);
            return View(invoice.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "CustName");
            return View();
        }

        // POST: Invoices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,InvoiceDetail")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoice.Add(invoice);
                db.SaveChanges();

                //UpdateDetails
                UpdateInvoice udetails = UpdateInvoiceDetailsDB;
                udetails(invoice.Id);

                //UpdateInvoice
                UpdateInvoice uinvoices = UpdateInvoiceDB;
                bool result = uinvoices(invoice.Id);

                if (result)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "CustName", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "CustName", invoice.CustomerId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,TotalItbis,SubTotal,Total")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "CustName", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoice.Find(id);
            db.Invoice.Remove(invoice);
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
