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

            using (var stopWatch = new StopWatchCalculator())
            {
                return View(new ContactsViewModel(DataSourceType, true, repository.ToArray(), stopWatch.ElapsedTime));
            }          
        }
        
        [HttpPost]
        public ActionResult CreateRepository(DataSourceType dataSourceType = DataSourceType.Memory)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var repositoryFactory = new RepositoryFactory();
                var temporaryRepository = repositoryFactory.CreateRepository(dataSourceType);
                temporaryRepository.Create();
                return View("Index", new ContactsViewModel(dataSourceType, true, new Contact[0], stopWatch.ElapsedTime));
            }
        }

        [HttpPost]
        public ActionResult DropRepository()
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                repository.Drop();
                return View("Index", new ContactsViewModel(DataSourceType, false, new Contact[0], stopWatch.ElapsedTime));
            }
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
            using (var stopWatch = new StopWatchCalculator())
            {
                repository.Add(contact);
                return View("Index", new ContactsViewModel(DataSourceType, true, repository.ToArray(), stopWatch.ElapsedTime));
            }   
        }

        [HttpPost]
        public ActionResult RemoveContact(Guid id)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                repository.Remove(id);
                return View("Index", new ContactsViewModel(DataSourceType, true, repository.ToArray(), stopWatch.ElapsedTime));
            }            
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
            using (var stopWatch = new StopWatchCalculator())
            {
                repository.Save(contact);
                return View("Index", new ContactsViewModel(DataSourceType, true, repository.ToArray(), stopWatch.ElapsedTime));
            }
        }
    }
}
