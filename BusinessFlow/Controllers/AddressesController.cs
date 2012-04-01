using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;

namespace BusinessFlow.Controllers
{   
    public class AddressesController : Controller
    {
		private readonly IAddressRepository addressRepository;

	
        public AddressesController(IAddressRepository addressRepository)
        {
			this.addressRepository = addressRepository;
        }

        //
        // GET: /Addresses/

        public ViewResult Index()
        {
            return View(addressRepository.AllIncluding(address => address.Enquiry));
        }

        //
        // GET: /Addresses/Details/5

        public ViewResult Details(int id)
        {
            return View(addressRepository.Find(id));
        }

        //
        // GET: /Addresses/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Addresses/Create

        [HttpPost]
        public ActionResult Create(Address address)
        {
            if (ModelState.IsValid) {
                addressRepository.InsertOrUpdate(address);
                addressRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Addresses/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(addressRepository.Find(id));
        }

        //
        // POST: /Addresses/Edit/5

        [HttpPost]
        public ActionResult Edit(Address address)
        {
            if (ModelState.IsValid) {
                addressRepository.InsertOrUpdate(address);
                addressRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Addresses/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(addressRepository.Find(id));
        }

        //
        // POST: /Addresses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            addressRepository.Delete(id);
            addressRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

