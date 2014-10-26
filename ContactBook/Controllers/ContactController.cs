using System;
using System.Linq;
using System.Web.Mvc;

namespace ContactBook.Controllers
{
    using Models;
    using Repositories;

    public class ContactController : Controller
    {
        public static DataSourceType DataSourceType { get; private set; }

        private readonly IContactRepository repository;   

        public ContactController()
        {
            var repositoryFactory = new RepositoryFactory();

            repository = repositoryFactory.CreateRepository(DataSourceType);
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!repository.Exist())
            {
                return View(new ContactsViewModel(DataSourceType, false, new Contact[0]));
            }

            using (new StopWatchCalculator(StopWatchAction))
            {
                return View(new ContactsViewModel(DataSourceType, true, repository.ToArray()));
            }          
        }
        
        [HttpPost]
        public ActionResult CreateRepository(DataSourceType dataSourceType = DataSourceType.Memory)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                var repositoryFactory = new RepositoryFactory();
                var temporaryRepository = repositoryFactory.CreateRepository(dataSourceType);
                temporaryRepository.Create();
            }

            return RedirectToAction("Index", new { dataSourceType });
        }

        [HttpPost]
        public ActionResult DropRepository()
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                repository.Drop();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeDataSource(DataSourceType dataSourceType)
        {
            DataSourceType = dataSourceType;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateContact(Contact contact)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                repository.Add(contact);
            }   

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveContact(Guid id)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                repository.Remove(id);
            }            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditContact(Guid id)
        {
            var contact = repository.Get(id);

            return this.View(contact);
        }

        [HttpPost]
        public ActionResult EditContact(Contact contact)
        {
            using (new StopWatchCalculator(StopWatchAction))
            {
                repository.Save(contact);
            }            

            return RedirectToAction("Index");
        }

        private void StopWatchAction(TimeSpan timeSpan)
        {
            this.ViewBag.ElapsedTime = timeSpan;
        }
    }
}
