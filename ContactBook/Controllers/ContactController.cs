using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactBook.Controllers
{
    using Ninject;

    using ContactBook.Models;
    using ContactBook.Repositories;

    public class ContactController : Controller
    {
        public static DataSourceType DataSourceType { get; set; }

        public ContactController()
        {
            var repositoryFactory = new RepositoryFactory();

            Repository = repositoryFactory.CreateRepository(DataSourceType);
        }

        public IContactRepository Repository { get; set; }        

        public ActionResult Index(DataSourceType dataSourceType = DataSourceType.Memory)
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
            if (Repository != null)
            {
                Repository.Add(contact);
            }            

            return RedirectToAction("Index");
        }

        public ActionResult RemoveContact(Guid id)
        {
            if (Repository != null)
            {
                Repository.Remove(id);
            }            

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
            if (Repository != null)
            {
                Repository.Save(contact);
            }            

            return RedirectToAction("Index");
        }

        public ActionResult ChangeDataSource(DataSourceType dataSourceType)
        {
            DataSourceType = dataSourceType;

            return RedirectToAction("Index");
        }
    }
}
