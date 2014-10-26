using System;
using System.Linq;
using System.Web.Mvc;

namespace ContactBook.Controllers
{
    using Models;
    using Repositories;

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
            if (!Repository.Exist())
            {
                return RedirectToAction("CreateRepository");
            }

            using (new StopWatchCalculator(StopWatchAction))
            {
                return View(Repository.ToArray());
            }          
        }

        [HttpGet]
        public ActionResult CreateRepository()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateRepository(DataSourceType dataSourceType = DataSourceType.Memory)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                Repository.Create();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DropRepository()
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                Repository.Drop();
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateContact(Contact contact)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                Repository.Add(contact);
            }   

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveContact(Guid id)
        {
            using (new StopWatchCalculator(StopWatchAction))
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
            using (new StopWatchCalculator(StopWatchAction))
            {
                Repository.Save(contact);
            }            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeDataSource(DataSourceType dataSourceType)
        {
            DataSourceType = dataSourceType;

            return RedirectToAction("Index");
        }

        private void StopWatchAction(TimeSpan timeSpan)
        {
            this.ViewBag.ElapsedTime = timeSpan;
        }
    }
}
