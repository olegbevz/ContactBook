using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using ContactBook.Models;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace ContactBook.Repositories.NHibernate
{
    public class ContactRepository : IContactRepository
    {
        private const string CONFIGURATION_FILE = "ContactBook.Repositories.NHibernate.hibernate.cfg.xml";

        private readonly Configuration masterConfiguration;

        private readonly Configuration configuration;

        public ContactRepository(string connectionString, string masterConnectionString)
        {
            var assembly = this.GetType().Assembly;

            this.configuration = new Configuration()
                .Configure(assembly, CONFIGURATION_FILE)
                .SetProperty(Environment.ConnectionString, connectionString);

            this.masterConfiguration = new Configuration()
                .Configure(assembly, CONFIGURATION_FILE)
                .SetProperty(Environment.ConnectionString, masterConnectionString);
        }

        public Models.Contact Get(Guid id)
        {
            using (ISessionFactory sessionFactory = configuration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    Contact contactEntity = session.Get<Contact>(id);
                    if (contactEntity != null)
                    {
                        return new Models.Contact
                        {
                            Id = contactEntity.Id,
                            Name = contactEntity.Name,
                            Address = contactEntity.Address,
                            Phone = contactEntity.Phone
                        };
                    }

                    return null;
                }
            }
        }

        public void Add(Models.Contact contact)
        {
            using (ISessionFactory sessionFactory = configuration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    var contactEntity = new Contact
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Address = contact.Address,
                        Phone = contact.Phone
                    };

                    session.Save(contactEntity);
                    session.Flush();
                }
            }
        }

        public void Remove(Guid id)
        {
            using (ISessionFactory sessionFactory = configuration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    Contact contactEntity = session.Get<Contact>(id);
                    if (contactEntity != null)
                    {
                        session.Delete(contactEntity);
                    }

                    session.Flush();
                }
            }
        }

        public void Update(Models.Contact contact)
        {
            using (ISessionFactory sessionFactory = configuration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    Contact contactEntity = new Contact
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Address = contact.Address,
                        Phone = contact.Phone
                    };

                    var mergedContactEntity = session.Merge(contactEntity);
                    session.Update(mergedContactEntity);
                    session.Flush();
                }
            }
        }

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            using (ISessionFactory sessionFactory = configuration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    var contacts = session.QueryOver<Contact>().List().Select(contactEntity => new Models.Contact
                    {
                        Id = contactEntity.Id,
                        Name = contactEntity.Name,
                        Address = contactEntity.Address,
                        Phone = contactEntity.Phone
                    });

                    return contacts.GetEnumerator();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Create()
        {
            using (ISessionFactory sessionFactory = masterConfiguration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    var query = session.CreateSQLQuery("CREATE DATABASE ContactBook;");
                    var result = query.ExecuteUpdate();
                }
            }

            var schemaExport = new SchemaExport(configuration);
            schemaExport.Create(false, true);
        }

        public void Drop()
        {
            SqlConnection.ClearAllPools();

            using (ISessionFactory sessionFactory = masterConfiguration.BuildSessionFactory())
            {
                using (ISession session = sessionFactory.OpenSession())
                {
                    var query = session.CreateSQLQuery("DROP DATABASE ContactBook;");
                    var result = query.ExecuteUpdate();
                }
            }
        }

        public bool Exist()
        {
            try
            {
                var schemaValidator = new SchemaValidator(this.configuration);
                schemaValidator.Validate();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}