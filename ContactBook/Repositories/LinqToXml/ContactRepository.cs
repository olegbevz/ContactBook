using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBook.Repositories.LinqToXml
{
    public class ContactRepository : IContactRepository
    {
        public Contact Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Contact contact)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}