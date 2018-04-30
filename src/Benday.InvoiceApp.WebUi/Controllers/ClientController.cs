using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Benday.DataAccess;
using Benday.InvoiceApp.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Benday.InvoiceApp.WebUi.Controllers
{
    public class ClientController : Controller
    {
        private IRepository<Client> _Repository;
        public ClientController(IRepository<Client> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository), $"{nameof(repository)} is null.");
            }

            _Repository = repository;
        }

        // GET: Client
        public ActionResult Index()
        {
            var clients = _Repository.GetAll();

            return View(clients);
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            return RedirectToAction("Edit", new { id = WebUiConstants.ID_FOR_CREATE_NEW_ENTITY });
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            return RedirectToAction("Edit", new { id = WebUiConstants.ID_FOR_CREATE_NEW_ENTITY });
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            Client model;

            if (id.Value == WebUiConstants.ID_FOR_CREATE_NEW_ENTITY)
            {
                // create new
                model = new Client();

                model.Id = WebUiConstants.ID_FOR_CREATE_NEW_ENTITY;
                model.Name = String.Empty;
            }
            else
            {
                model = _Repository.GetById(id.Value);
            }

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client model)
        {
            if (ModelState.IsValid)
            {
                bool isCreateNew = false;

                if (model.Id == WebUiConstants.ID_FOR_CREATE_NEW_ENTITY)
                {
                    isCreateNew = true;
                    model.Id = 0;
                    _Repository.Save(model);
                }
                else
                {
                    Client toValue =
                        _Repository.GetById(model.Id);

                    if (toValue == null)
                    {
                        return new BadRequestObjectResult(
                            String.Format("Unknown client id '{0}'.", model.Id));
                    }

                    toValue.Name = model.Name;

                    _Repository.Save(toValue);
                }
                
                if (isCreateNew == true)
                {
                    return RedirectToAction("Edit", new { id = model.Id });
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

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
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