using ContactBook.Models;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Tool.hbm2ddl;

namespace ContactBook.Repositories.NHibernate
{
    public class ContactRepository : IContactRepository
    {
        private ISessionFactory sessionFactory;

        public ContactRepository()
        {
            var assembly = this.GetType().Assembly;

            var ress = assembly.GetManifestResourceNames();

            this.sessionFactory = new Configuration()
                .Configure(assembly, "ContactBook.Repositories.NHibernate.hibernate.cfg.xml")
                .BuildSessionFactory();
        }

        public Models.Contact Get(Guid id)
        {
            using (var session = this.sessionFactory.OpenSession())
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
            using (var session = this.sessionFactory.OpenSession())
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
            using (var session = this.sessionFactory.OpenSession())
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
            using (var session = this.sessionFactory.OpenSession())
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
            using (var session = this.sessionFactory.OpenSession())
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public void CreateStorage()
        {
            Configuration cfg = new Configuration().Configure("hibernate.cfg.xml");

            //add assembly in which the hbm.xml mappings are embedded (assuming Product class is in this assembly)
            cfg.AddAssembly(typeof(Contact).Assembly);

            //this will generate the SQL schema file in the executable folder
            new SchemaExport(cfg).SetOutputFile("schema.sql").Execute(true, false, false);
        }


        public void DeleteStorage()
        {
            throw new NotImplementedException();
        }
    }
}