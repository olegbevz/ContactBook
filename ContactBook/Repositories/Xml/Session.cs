using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ContactBook.Models;

namespace ContactBook.Repositories.Xml
{
    public class Session : ISession
    {
        private readonly string fileName;

        private readonly List<Contact> contacts; 

        public Session(string fileName)
        {
            this.fileName = fileName;

            contacts = GetAll().ToList();

            ContactRepository = new ContactRepository(contacts);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
            SaveAll(contacts);
        }

        public void Dispose()
        {
        }

        private IEnumerable<Contact> GetAll()
        {
            if (!File.Exists(fileName))
            {
                return new Contact[0];
            }

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var serializer = new XmlSerializer(typeof(Contact[]));

                return serializer.Deserialize(fileStream) as Contact[];
            }
        }

        private void SaveAll(IEnumerable<Contact> contacts)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var serializer = new XmlSerializer(typeof(Contact[]));

                serializer.Serialize(fileStream, contacts.ToArray());

                fileStream.Flush();
            }
        }
    }
}