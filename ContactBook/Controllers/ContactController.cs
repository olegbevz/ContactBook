using System;
using System.Linq;
using System.Web.Mvc;

namespace ContactBook.Controllers
{
    using Models;
    using Repositories;

    public class ContactController : Controller
    {
        private static readonly RepositoryFactory repositoryFactory = new RepositoryFactory();

        [HttpGet]
        public ActionResult Index()
        {
            var repository = repositoryFactory.CreateRepository(GetDataSourceType());

            if (!repository.Exist())
            {
                return View(new ContactsViewModel(GetDataSourceType(), false, new Contact[0]));
            }

            using (var stopWatch = new StopWatchCalculator())
            {
                return View(new ContactsViewModel(GetDataSourceType(), true, repository.ToArray(), stopWatch.ElapsedTime));
            }          
        }
        
        [HttpPost]
        public ActionResult CreateRepository(DataSourceType dataSourceType = DataSourceType.Memory)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var repository = repositoryFactory.CreateRepository(dataSourceType);
                repository.Create();
                return View("Index", new ContactsViewModel(dataSourceType, true, new Contact[0], stopWatch.ElapsedTime));
            }
        }

        [HttpPost]
        public ActionResult DropRepository()
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var repository = repositoryFactory.CreateRepository(GetDataSourceType());
                repository.Drop();
                return View("Index", new ContactsViewModel(GetDataSourceType(), false, new Contact[0], stopWatch.ElapsedTime));
            }
        }

        [HttpGet]
        public ActionResult ChangeDataSource(DataSourceType dataSourceType)
        {
            SetDataSourceType(dataSourceType);
            
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
                var repository = repositoryFactory.CreateRepository(GetDataSourceType());
                repository.Add(contact);
                return View("Index", new ContactsViewModel(GetDataSourceType(), true, repository.ToArray(), stopWatch.ElapsedTime));
            }   
        }

        [HttpPost]
        public ActionResult RemoveContact(Guid id)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var repository = repositoryFactory.CreateRepository(GetDataSourceType());
                repository.Remove(id);
                return View("Index", new ContactsViewModel(GetDataSourceType(), true, repository.ToArray(), stopWatch.ElapsedTime));
            }            
        }

        [HttpGet]
        public ActionResult EditContact(Guid id)
        {
            var repository = repositoryFactory.CreateRepository(GetDataSourceType());
            var contact = repository.Get(id);

            return this.View(contact);
        }

        [HttpPost]
        public ActionResult EditContact(Contact contact)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var repository = repositoryFactory.CreateRepository(GetDataSourceType());
                repository.Save(contact);
                return View("Index", new ContactsViewModel(GetDataSourceType(), true, repository.ToArray(), stopWatch.ElapsedTime));
            }
        }

        private void SetDataSourceType(DataSourceType dataSourceType)
        {
            if (HttpContext != null && HttpContext.Session != null)
            {
                HttpContext.Session["DataSourceType"] = dataSourceType;
            }
        }

        private DataSourceType GetDataSourceType()
        {
            if (HttpContext == null || HttpContext.Session == null)
            {
                return DataSourceType.Memory;
            }

            var obj = HttpContext.Session["DataSourceType"];
            if (obj == null)
            {
                return DataSourceType.Memory;;
            }

            return (DataSourceType)obj;
        }
    }
}
