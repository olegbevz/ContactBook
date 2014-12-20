using System;
using System.Linq;
using System.Web.Mvc;
using ContactBook.Performance;

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
            var dataSourceType = GetDataSourceType();

            var repository = repositoryFactory.CreateRepository(dataSourceType);

            if (!repository.Exist())
            {
                return View(new ContactsViewModel(dataSourceType, false, new Contact[0]));
            }

            using (var stopWatch = new StopWatchCalculator())
            {
                var contacts = repository.ToArray();

                return View(new ContactsViewModel(dataSourceType, true, contacts, stopWatch.ElapsedTime));
            }          
        }
        
        [HttpPost]
        public ActionResult CreateRepository(RepositoryType dataSourceType = RepositoryType.Memory)
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
        public ActionResult ChangeDataSource(RepositoryType dataSourceType)
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
                repository.Update(contact);
                return View("Index", new ContactsViewModel(GetDataSourceType(), true, repository.ToArray(), stopWatch.ElapsedTime));
            }
        }

        [HttpGet]
        public ActionResult PerformanceTest()
        {
            var reposaitoryTypes = Enum.GetValues(typeof (RepositoryType)).Cast<RepositoryType>().ToArray();

            var performanceCalculator = new PerformanceCalculator();

            var recordsCount = 1000;

            var performanceResult = performanceCalculator.CalculatePerformance(recordsCount, reposaitoryTypes);

            var performanceResultViewModel = new PerformanceResultViewModel
            {
                RecordsCount = recordsCount,
                RepositoriesResults = performanceResult
            };

            return View(performanceResultViewModel);
        }

        private void SetDataSourceType(RepositoryType dataSourceType)
        {
            if (HttpContext != null && HttpContext.Session != null)
            {
                HttpContext.Session["DataSourceType"] = dataSourceType;
            }
        }

        private RepositoryType GetDataSourceType()
        {
            if (HttpContext == null || HttpContext.Session == null)
            {
                return RepositoryType.Memory;
            }

            var obj = HttpContext.Session["DataSourceType"];
            if (obj == null)
            {
                return RepositoryType.Memory;;
            }

            return (RepositoryType)obj;
        }
    }
}
