using System.Collections.Generic;
using ContactBook.Models;

namespace ContactBook.Repositories.Memory
{
    public class Database : IDatabase
    {
        private IList<Contact> contacts;

        static Database()
        {
            Instance = new Database();
        }

        /// <summary>
        /// Gets the instance.
        /// Статический экземпляр сущности
        /// </summary>
        public static Database Instance { get; private set; }

        public void Create()
        {
            contacts = new List<Contact>();
        }

        public void Drop()
        {
            contacts = null;
        }

        public bool Exist()
        {
            return contacts != null;
        }

        public ISession OpenSession()
        {
            return new Session(contacts);
        }
    }
}