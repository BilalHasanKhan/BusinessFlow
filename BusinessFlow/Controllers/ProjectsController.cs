using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessFlow.Models;

namespace BusinessFlow.Controllers
{   
    public class ProjectsController : Controller
    {
        private BusinessFlowContext context = new BusinessFlowContext();

        //
        // GET: /Projects/

        public ViewResult Index()
        {
            return View(context.Projects.ToList());
        }

        //
        // GET: /Projects/Details/5

        public ViewResult Details(int id)
        {
            Project project = context.Projects.Single(x => x.ProjectID == id);
            return View(project);
        }

        //
        // GET: /Projects/Create

        public ActionResult Create()
        {
            ViewBag.PossibleEnquiries = context.Enquiries;
            return View();
        } 

        //
        // POST: /Projects/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.ConfirmDate = DateTime.Now.Date;
                context.Projects.Add(project);
                context.SaveChanges();
               // return RedirectToAction("Index");  
            }

            ViewBag.PossibleEnquiries = context.Enquiries;
            return View(project);
        }
        
        //
        // GET: /Projects/Edit/5
 
        public ActionResult Edit(int id)
        {
            Project project = context.Projects.Single(x => x.ProjectID == id);
            ViewBag.PossibleEnquiries = context.Enquiries;
            return View(project);
        }

        //
        // POST: /Projects/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                context.Entry(project).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleEnquiries = context.Enquiries;
            return View(project);
        }

        //
        // GET: /Projects/Delete/5
 
        public ActionResult Delete(int id)
        {
            Project project = context.Projects.Single(x => x.ProjectID == id);
            return View(project);
        }

        //
        // POST: /Projects/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = context.Projects.Single(x => x.ProjectID == id);
            context.Projects.Remove(project);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}