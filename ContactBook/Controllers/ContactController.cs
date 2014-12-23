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
        private static readonly DatabaseFactory repositoryFactory = new DatabaseFactory();

        [HttpGet]
        public ActionResult Index()
        {
            var dataSourceType = GetDataSourceType();

            var database = repositoryFactory.GetDatabase(dataSourceType);

            if (!database.Exist())
            {
                return View(new ContactsViewModel(dataSourceType, false, new Contact[0]));
            }
            
            using (var stopWatch = new StopWatchCalculator())
            {
                using (var session = database.OpenSession())
                {
                    var contacts = session.ContactRepository.ToArray();
                    return View(new ContactsViewModel(dataSourceType, true, contacts, stopWatch.ElapsedTime));
                }
            }
        }
        
        [HttpPost]
        public ActionResult CreateRepository(DatabaseType dataSourceType = DatabaseType.Memory)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var database = repositoryFactory.GetDatabase(dataSourceType);
                database.Create();
                return View("Index", new ContactsViewModel(dataSourceType, true, new Contact[0], stopWatch.ElapsedTime));
            }
        }

        [HttpPost]
        public ActionResult DropRepository()
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var database = repositoryFactory.GetDatabase(GetDataSourceType());
                database.Drop();
                return View("Index", new ContactsViewModel(GetDataSourceType(), false, new Contact[0], stopWatch.ElapsedTime));
            }
        }

        [HttpGet]
        public ActionResult ChangeDataSource(DatabaseType dataSourceType)
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
                var database = repositoryFactory.GetDatabase(GetDataSourceType());
                using (var session = database.OpenSession())
                {
                    session.ContactRepository.Add(contact);
                    session.Commit();
                    return View("Index", new ContactsViewModel(GetDataSourceType(), true, session.ContactRepository.ToArray(), stopWatch.ElapsedTime));
                }
            }   
        }

        [HttpPost]
        public ActionResult RemoveContact(Guid id)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var database = repositoryFactory.GetDatabase(GetDataSourceType());
                using (var session = database.OpenSession())
                {
                    session.ContactRepository.Remove(id);
                    session.Commit();
                    return View("Index", new ContactsViewModel(GetDataSourceType(), true, session.ContactRepository.ToArray(), stopWatch.ElapsedTime));
                }
            }            
        }

        [HttpGet]
        public ActionResult EditContact(Guid id)
        {
            var database = repositoryFactory.GetDatabase(GetDataSourceType());
            using (var session = database.OpenSession())
            {
                var contact = session.ContactRepository.Get(id);
                return this.View(contact);
            }
        }

        [HttpPost]
        public ActionResult EditContact(Contact contact)
        {
            using (var stopWatch = new StopWatchCalculator())
            {
                var database = repositoryFactory.GetDatabase(GetDataSourceType());
                using (var session = database.OpenSession())
                {
                    session.ContactRepository.Update(contact);
                    session.Commit();
                    return View("Index", new ContactsViewModel(GetDataSourceType(), true, session.ContactRepository.ToArray(), stopWatch.ElapsedTime));
                }
            }
        }

        [HttpGet]
        public ActionResult PerformanceTest()
        {
            var performanceCalculator = new PerformanceCalculator();

            var recordsCount = 1000;
            var databaseTypes = new[]
                                    {
                                        DatabaseType.Memory,
                                        DatabaseType.Xml,
                                        DatabaseType.LinqToXml,
                                        DatabaseType.ADO, 
                                        DatabaseType.LinqToSql, 
                                        DatabaseType.EntityFramework,
                                        DatabaseType.BLToolkit, 
                                        DatabaseType.NHibernate
                                    };

            var performanceResult = performanceCalculator.CalculatePerformance(recordsCount, databaseTypes);

            var performanceResultViewModel = new PerformanceResultViewModel
            {
                RecordsCount = recordsCount,
                RepositoriesResults = performanceResult
            };

            return View(performanceResultViewModel);
        }

        private void SetDataSourceType(DatabaseType dataSourceType)
        {
            if (HttpContext != null && HttpContext.Session != null)
            {
                HttpContext.Session["DataSourceType"] = dataSourceType;
            }
        }

        private DatabaseType GetDataSourceType()
        {
            if (HttpContext == null || HttpContext.Session == null)
            {
                return DatabaseType.Memory;
            }

            var obj = HttpContext.Session["DataSourceType"];
            if (obj == null)
            {
                return DatabaseType.Memory;;
            }

            return (DatabaseType)obj;
        }
    }
}
