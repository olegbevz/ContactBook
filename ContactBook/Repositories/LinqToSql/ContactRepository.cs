using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContactBook.Repositories.LinqToSql
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataClassesDataContext context;

        public ContactRepository(DataClassesDataContext dataContext)
        {
            this.context = dataContext;
        }

        public Models.Contact Get(Guid id)
        {
            var stringBuilder = new StringBuilder();
            context.Log = new StringWriter(stringBuilder);

            Contact contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
            if (contactEntity != null)
            {
                var sqlQuery = stringBuilder.ToString();

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
            var stringBuilder = new StringBuilder();
            context.Log = new StringWriter(stringBuilder);

            Contact contactEntity = new Contact();
            contactEntity.Id = Guid.NewGuid();
            contactEntity.Name = contact.Name;
            contactEntity.Phone = contact.Phone;
            contactEntity.Address = contact.Address;

            context.Contacts.InsertOnSubmit(contactEntity);

            var sqlQuery = stringBuilder.ToString();
        }

        public void Remove(Guid id)
        {
            Contact contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
            if (contactEntity != null)
            {
                context.Contacts.DeleteOnSubmit(contactEntity);
            }
        }

        public void Update(Models.Contact contact)
        {
            Contact contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(contact.Id));
            contactEntity.Name = contact.Name;
            contactEntity.Phone = contact.Phone;
            contactEntity.Address = contact.Address;
        }

        public IEnumerator<Models.Contact> GetEnumerator()
        {
            return context.Contacts.Select(contactEntity => new Models.Contact
            {
                Id = contactEntity.Id,
                Name = contactEntity.Name,
                Address = contactEntity.Address,
                Phone = contactEntity.Phone
            }).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}