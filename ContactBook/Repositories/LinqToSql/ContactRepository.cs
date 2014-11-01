using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ContactBook.Models;

namespace ContactBook.Repositories.LinqToSql
{
    public class ContactRepository : IContactRepository
    {
        public Models.Contact Get(Guid id)
        {
            using (var context = new DataClassesDataContext())
            {
                var contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
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
            using (var context = new DataClassesDataContext())
            {
                var contactEntity = new Contact();
                contactEntity.Id = Guid.NewGuid();
                contactEntity.Name = contact.Name;
                contactEntity.Phone = contact.Phone;
                contactEntity.Address = contact.Address;

                context.Contacts.InsertOnSubmit(contactEntity);
                context.SubmitChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new DataClassesDataContext())
            {
                var contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
                if (contactEntity != null)
                {
                    context.Contacts.DeleteOnSubmit(contactEntity);
                    context.SubmitChanges();
                }
            }
        }

        public void Save(Models.Contact contact)
        {
            using (var context = new DataClassesDataContext())
            {
                var contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(contact.Id));
                contactEntity.Name = contact.Name;
                contactEntity.Phone = contact.Phone;
                contactEntity.Address = contact.Address;
                context.SubmitChanges();
            }
        }

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            using (var context = new DataClassesDataContext())
            {
                return context.Contacts.Select(contactEntity => new Models.Contact
                {
                    Id = contactEntity.Id,
                    Name = contactEntity.Name,
                    Address = contactEntity.Address,
                    Phone = contactEntity.Phone
                }).ToList().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public void Create()
        {
            using (var context = new DataClassesDataContext())
            {
                context.CreateDatabase();
            }
        }

        public void Drop()
        {
            using (var context = new DataClassesDataContext())
            {
                context.DeleteDatabase();
            }
        }

        public bool Exist()
        {
            using (var context = new DataClassesDataContext())
            {
                return context.DatabaseExists();
            }
        }
    }
}