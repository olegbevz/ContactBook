using System.Collections.Generic;
using ContactBook.Models;

namespace ContactBook.Repositories.Memory
{
    public class Session : ISession
    {
        public Session(IList<Contact> contacts)
        {
            ContactRepository = new ContactRepository(contacts);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
        }

        public void Dispose()
        {
        }
    }
}