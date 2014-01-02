using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactBook.Controllers
{
    using Ninject;

    using ContactBook.Models;

    public class ContactController : Controller
    {
        public IContactRepository Repository { get; set; }

        public ContactController()
        {
            using (var kernel = new StandardKernel(new NinjectModule()))
            {
                Repository = kernel.Get<IContactRepository>();
            }
        }

        public ActionResult Index()
        {
            try
            {
                return View(Repository.ToArray() ?? new Contact[0]);
            }
            catch
            {
                return View(new Contact[0]);
            }            
        }

        public ActionResult CreateContact()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult CreateContact(Contact contact)
        {
            Repository.Add(contact);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveContact(Guid id)
        {
            Repository.Remove(id);

            return RedirectToAction("Index");
        }

        public ActionResult EditContact(Guid id)
        {
            var contact = Repository.Get(id);

            return this.View(contact);
        }

        [HttpPost]
        public ActionResult EditContact(Contact contact)
        {
            Repository.Save(contact);

            return RedirectToAction("Index");
        }
    }
}
