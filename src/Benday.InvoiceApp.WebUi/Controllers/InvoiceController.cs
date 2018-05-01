using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.DataAccess;
using Benday.InvoiceApp.Api;
using Benday.InvoiceApp.WebUi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Benday.InvoiceApp.WebUi.Controllers
{
    public class InvoiceController : Controller
    {
        private IRepository<Client> _ClientRepository;
        private IRepository<Invoice> _InvoiceRepository;
        public InvoiceController(IRepository<Client> clientRepository, 
            IRepository<Invoice> invoiceRepository)
        {
            if (invoiceRepository == null)
            {
                throw new ArgumentNullException(nameof(invoiceRepository), $"{nameof(invoiceRepository)} is null.");
            }

            if (clientRepository == null)
            {
                throw new ArgumentNullException(nameof(clientRepository), $"{nameof(clientRepository)} is null.");
            }

            _ClientRepository = clientRepository;
            _InvoiceRepository = invoiceRepository;
        }

        // GET: Invoice
        public ActionResult Index()
        {
            var invoices = _InvoiceRepository.GetAll();

            return View(invoices);
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            return RedirectToAction("Edit", new { id = WebUiConstants.ID_FOR_CREATE_NEW_ENTITY });
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var model = new InvoiceViewModel();
            Invoice entity;

            if (id.Value == WebUiConstants.ID_FOR_CREATE_NEW_ENTITY)
            {
                // create new
                entity = new Invoice();

                entity.Id = WebUiConstants.ID_FOR_CREATE_NEW_ENTITY;
                entity.InvoiceDate = DateTime.Now;
                entity.InvoiceLines = new List<InvoiceLine>();

                AddInvoiceLines(entity);

                entity.InvoiceNumber = DateTime.Now.Ticks.ToString();
            }
            else
            {
                entity = _InvoiceRepository.GetById(id.Value);                
            }

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                if (entity.InvoiceLines == null)
                {
                    entity.InvoiceLines = new List<InvoiceLine>();
                }

                model.Invoice = entity;
                model.Id = entity.Id;

                if (entity.OwnerClientIDFK != 0)
                {
                    model.ClientId = entity.OwnerClientIDFK.ToString();
                }
            }

            model.Clients = GetClients();

            return View(model);
        }

        private void AddInvoiceLines(Invoice entity)
        {
            if (entity.InvoiceLines == null)
            {
                entity.InvoiceLines = new List<InvoiceLine>();
            }

            entity.InvoiceLines.Add(CreateInvoiceLine(1));
            entity.InvoiceLines.Add(CreateInvoiceLine(2));
            entity.InvoiceLines.Add(CreateInvoiceLine(3));
            entity.InvoiceLines.Add(CreateInvoiceLine(4));
        }

        private InvoiceLine CreateInvoiceLine(int lineNumber)
        {
            var temp = new InvoiceLine();

            temp.ItemDescription = String.Format("Item Desc #{0}", lineNumber);
            temp.ItemName = String.Format("Item Name #{0}", lineNumber);
            temp.Quantity = lineNumber;
            temp.Value = lineNumber * 100;

            return temp;
        }

        private List<SelectListItem> GetClients()
        {
            var operators = new List<SelectListItem>();

            operators.Add(
                String.Empty,
                WebUiConstants.Message_ChooseAClient,
                true);

            var clients = _ClientRepository.GetAll();

            foreach (var item in clients)
            {
                operators.Add(item.Id.ToString(), item.Name, false);
            }

            return operators;
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isCreateNew = false;

                Invoice fromEntity = model.Invoice;
                Invoice toEntity = null;

                if (model.Id == WebUiConstants.ID_FOR_CREATE_NEW_ENTITY)
                {
                    isCreateNew = true;
                    toEntity = new Invoice();
                }
                else
                {
                    toEntity =
                        _InvoiceRepository.GetById(model.Id);

                    if (toEntity == null)
                    {
                        return new BadRequestObjectResult(
                            String.Format("Unknown client id '{0}'.", model.Id));
                    }
                }

                if (fromEntity == null || toEntity == null)
                {
                    return new BadRequestObjectResult("fromEntity or toEntity was null");
                }
                else
                {
                    toEntity.OwnerClientIDFK = Int32.Parse(model.ClientId);

                    Adapt(fromEntity, toEntity);

                    _InvoiceRepository.Save(toEntity);
                }

                if (isCreateNew == true)
                {
                    model.Id = toEntity.Id;

                    return RedirectToAction("Edit", new { id = toEntity.Id });
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }
            else
            {
                return View(model);
            }
        }
        private void Adapt(Invoice fromValue, Invoice toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentNullException(nameof(fromValue), $"{nameof(fromValue)} is null.");
            }

            if (toValue == null)
            {
                throw new ArgumentNullException(nameof(toValue), $"{nameof(toValue)} is null.");
            }

            toValue.InvoiceDate = fromValue.InvoiceDate;
            toValue.InvoiceNumber = fromValue.InvoiceNumber;

            if (fromValue.InvoiceLines == null)
            {
                fromValue.InvoiceLines = new List<InvoiceLine>();
            }

            if (toValue.InvoiceLines == null)
            {
                toValue.InvoiceLines = new List<InvoiceLine>();
            }

            Adapt(fromValue.InvoiceLines, toValue.InvoiceLines);
        }

        private void Adapt(List<InvoiceLine> fromValue, List<InvoiceLine> toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentException($"{nameof(fromValue)} is null.", nameof(fromValue));
            }

            if (toValue == null)
            {
                throw new ArgumentException($"{nameof(toValue)} is null.", nameof(toValue));
            }

            InvoiceLine toLine;

            foreach (var fromLine in fromValue)
            {
                if (fromLine.Id == 0)
                {
                    toLine = new InvoiceLine();

                    toValue.Add(toLine);
                }
                else
                {
                    toLine = (from temp in toValue
                              where temp.Id == fromLine.Id
                              select temp).FirstOrDefault();

                    if (toLine == null)
                    {
                        throw new InvalidOperationException(String.Format("Invoice line collection did not contain Id '{0}'.", fromLine.Id));
                    }                    
                }

                Adapt(fromLine, toLine);
            }
        }

        private void Adapt(InvoiceLine fromValue, InvoiceLine toValue)
        {
            if (fromValue == null)
            {
                throw new ArgumentNullException(nameof(fromValue), $"{nameof(fromValue)} is null.");
            }

            if (toValue == null)
            {
                throw new ArgumentNullException(nameof(toValue), $"{nameof(toValue)} is null.");
            }

            toValue.ItemName = fromValue.ItemName;
            toValue.ItemDescription = fromValue.ItemDescription;
            toValue.Quantity = fromValue.Quantity;
            toValue.Value = fromValue.Value;
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoice/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}