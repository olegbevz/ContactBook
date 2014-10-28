using System.Collections;
using System.Linq;
using ContactBook.Models;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace ContactBook.Repositories.NHibernate
{
    public class ContactRepository : IContactRepository
    {
        private readonly Configuration configuration;

        public ContactRepository(string connectionString)
        {
            var assembly = this.GetType().Assembly;

            this.configuration = new Configuration()
                .Configure(assembly, "ContactBook.Repositories.NHibernate.hibernate.cfg.xml")
                .SetProperty(Environment.ConnectionString, connectionString);;
        }

        public Models.Contact Get(Guid id)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                var contactEntity = session.Get<Contact>(id);
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

        public void Add(Models.Contact contact)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
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

        public void Remove(Guid id)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                var contactEntity = session.Get<Contact>(id);
                if (contactEntity != null)
                {
                    session.Delete(contactEntity);
                }
                
                session.Flush();
            }
        }

        public void Save(Models.Contact contact)
        {
            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                var contactEntity = new Contact
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

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            var sessionFactory = configuration.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                return session.QueryOver<Contact>().List().Select(contactEntity => new Models.Contact
                    {
                        Id = contactEntity.Id,
                        Name = contactEntity.Name,
                        Address = contactEntity.Address,
                        Phone = contactEntity.Phone
                    }).GetEnumerator();
            }            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Create()
        {
            var schemaExport = new SchemaExport(configuration);

            schemaExport.Create(false, true);
        }

        public void Drop()
        {
            var schemaExport = new SchemaExport(configuration);

            schemaExport.Drop(false, true);
        }

        public bool Exist()
        {
            try
            {
                var myvalidator = new SchemaValidator(this.configuration);

                myvalidator.Validate();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}