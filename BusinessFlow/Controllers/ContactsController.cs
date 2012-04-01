using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;

namespace BusinessFlow.Controllers
{   
    public class ContactsController : Controller
    {
		private readonly IContactRepository contactRepository;


        public ContactsController(IContactRepository contactRepository)
        {
			this.contactRepository = contactRepository;
        }

        //
        // GET: /Contacts/

        public ViewResult Index()
        {
            return View(contactRepository.AllIncluding(contact => contact.Enquiry));
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(int id)
        {
            return View(contactRepository.Find(id));
        }

        //
        // GET: /Contacts/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Contacts/Create

        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid) {
                contactRepository.InsertOrUpdate(contact);
                contactRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Contacts/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(contactRepository.Find(id));
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid) {
                contactRepository.InsertOrUpdate(contact);
                contactRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Contacts/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(contactRepository.Find(id));
        }

        //
        // POST: /Contacts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            contactRepository.Delete(id);
            contactRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

