using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ContactBook.Models;

namespace ContactBook.Repositories.EntityFramework
{
    public class ContactRepository : IContactRepository
    {
        public Models.Contact Get(Guid id)
        {
            using (var context = new DomainContainer())
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
            using (var context = new DomainContainer())
            {
                var contactEntity = context.Contacts.Create();
                contactEntity.Id = Guid.NewGuid();
                contactEntity.Name = contact.Name;
                contactEntity.Phone = contact.Phone;
                contactEntity.Address = contact.Address;

                context.Contacts.Add(contactEntity);
                var entry = context.ChangeTracker.Entries<ContactEntity>().FirstOrDefault(x => x.Entity.Equals(contactEntity));
                context.SaveChanges();
            }
        }

        public void Remove(Guid id)
        {
            using (var context = new DomainContainer())
            {
                var contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
                if (contactEntity != null)
                {
                    context.Contacts.Remove(contactEntity);
                    var entry = context.ChangeTracker.Entries<ContactEntity>().FirstOrDefault(x => x.Entity.Equals(contactEntity));
                    context.SaveChanges();
                }
            }
        }

        public void Save(Models.Contact contact)
        {
            using (var context = new DomainContainer())
            {
                var contactEntity = new ContactEntity();
                contactEntity.Id = contact.Id;
                contactEntity.Name = contact.Name;
                contactEntity.Phone = contact.Phone;
                contactEntity.Address = contact.Address;
                context.Contacts.Attach(contactEntity);
                var entry = context.ChangeTracker.Entries<ContactEntity>().FirstOrDefault(x => x.Entity.Equals(contactEntity));
                if (entry != null)
                {
                    entry.State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            using (var context = new DomainContainer())
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}