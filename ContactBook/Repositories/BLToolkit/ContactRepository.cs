namespace ContactBook.Repositories.BLToolkit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::BLToolkit.Data;
    using global::BLToolkit.Data.Linq;

    public class ContactRepository : IContactRepository
    {
        private readonly DbManager db;

        public ContactRepository(DbManager db)
        {
            this.db = db;
        }

        public Models.Contact Get(Guid id)
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

        public void Add(Models.Contact contact)
        {
            contact.Id = Guid.NewGuid();

                db.GetTable<Contact>().InsertWithIdentity(() => new Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Address = contact.Address,
                    Phone = contact.Phone
                });
        }

        public void Remove(Guid id)
        {
                db.GetTable<Contact>().Delete(contact => contact.Id == id);
        }

        public void Update(Models.Contact contact)
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
                return db.GetTable<Contact>().Select(contact => new Models.Contact
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Address = contact.Address,
                    Phone = contact.Phone
                }).AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}