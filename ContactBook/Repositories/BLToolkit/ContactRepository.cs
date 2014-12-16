using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

namespace ContactBook.Repositories.BLToolkit
{
    public class ContactRepository : IContactRepository
    {
        private readonly string connectionString;

        public ContactRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Models.Contact Get(Guid id)
        {
            using (var db = new DbManager(new Sql2012DataProvider(), connectionString))
            {
                var contactEntity = db.GetTable<Contact>().FirstOrDefault(x => x.Id == id);
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
            contact.Id = Guid.NewGuid();

            using (var db = new DbManager(new Sql2012DataProvider(), connectionString))
            {
                db.GetTable<Contact>().InsertWithIdentity(() => new Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Address = contact.Address,
                    Phone = contact.Phone
                });
            }
        }

        public void Remove(Guid id)
        {
            using (var db = new DbManager(new Sql2012DataProvider(), connectionString))
            {
                db.GetTable<Contact>().Delete(contact => contact.Id == id);
            }
        }

        public void Update(Models.Contact contact)
        {
            using (var db = new DbManager(new Sql2012DataProvider(), connectionString))
            {
                db.GetTable<Contact>().Update(
                    contactEntity => contactEntity.Id == contact.Id,
                    contactEntity => new Contact
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Address = contact.Address,
                        Phone = contact.Phone
                    });
            }
        }

        public bool Exist()
        {
            return true;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            using (var db = new DbManager(new Sql2012DataProvider(), connectionString))
            {
                return db.GetTable<Contact>().Select(contact => new Models.Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Address = contact.Address,
                    Phone = contact.Phone
                }).AsEnumerable().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}