namespace ContactBook.Repositories.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Models;

    public class ContactRepository : IContactRepository
    {
        private readonly DomainContainer context;

        public ContactRepository(DomainContainer domainContainer)
        {
            this.context = domainContainer;
        }

        public Contact Get(Guid id)
        {
            var stringBuilder = new StringBuilder();
            context.Database.Log = generatedSql => stringBuilder.AppendLine(generatedSql);

            var contactEntity = context.Contacts.FirstOrDefault(x => x.Id == id);

            var sqlCode = stringBuilder.ToString();

            if (contactEntity != null)
            {
                sqlCode = stringBuilder.ToString();

                return new Contact
                {
                    Id = contactEntity.Id,
                    Name = contactEntity.Name,
                    Address = contactEntity.Address,
                    Phone = contactEntity.Phone
                };
            }

            return null;
        }

        public void Add(Contact contact)
        {
            var contactEntity = context.Contacts.Create();
            contactEntity.Id = Guid.NewGuid();
            contactEntity.Name = contact.Name;
            contactEntity.Phone = contact.Phone;
            contactEntity.Address = contact.Address;

            context.Contacts.Add(contactEntity);
            var entry = context.ChangeTracker.Entries<ContactEntity>().FirstOrDefault(x => x.Entity.Equals(contactEntity));
        }

        public void Remove(Guid id)
        {
            var contactEntity = context.Contacts.FirstOrDefault(x => x.Id.Equals(id));
            if (contactEntity != null)
            {
                context.Contacts.Remove(contactEntity);
                var entry = context.ChangeTracker.Entries<ContactEntity>().FirstOrDefault(x => x.Entity.Equals(contactEntity));
            }
        }

        public void Update(Contact contact)
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
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            return context.Contacts.Select(contactEntity => new Models.Contact
            {
                Id = contactEntity.Id,
                Name = contactEntity.Name,
                Address = contactEntity.Address,
                Phone = contactEntity.Phone
            }).ToList().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}