using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using INHSession = NHibernate.ISession;

namespace ContactBook.Repositories.NHibernate
{
    public class ContactRepository : IContactRepository
    {
        private readonly INHSession session;

        public ContactRepository(INHSession session)
        {
            this.session = session;
        }

        public Models.Contact Get(Guid id)
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

        public void Add(Models.Contact contact)
        {

            var contactEntity = new Contact
            {
                Id = contact.Id,
                Name = contact.Name,
                Address = contact.Address,
                Phone = contact.Phone
            };

            session.Save(contactEntity);
        }

        public void Remove(Guid id)
        {
            Contact contactEntity = session.Get<Contact>(id);
            if (contactEntity != null)
            {
                session.Delete(contactEntity);
            }
        }

        public void Update(Models.Contact contact)
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
        }

        public IEnumerator<Models.Contact> GetEnumerator()
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}