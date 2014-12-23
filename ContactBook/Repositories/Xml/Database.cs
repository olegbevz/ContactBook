using System.IO;
using System.Xml.Serialization;
using ContactBook.Models;

namespace ContactBook.Repositories.Xml
{
    public class Database : IDatabase
    {
        private readonly string fileName;

        public Database(string fileName)
        {
            this.fileName = fileName;
        }

        public void Create()
        {
            using (var fileStream = File.Create(fileName))
            {
                var serializer = new XmlSerializer(typeof(Contact[]));

                serializer.Serialize(fileStream, new Contact[0]);

                fileStream.Flush();
            };
        }

        public void Drop()
        {
            File.Delete(fileName);
        }

        public bool Exist()
        {
            return File.Exists(fileName);
        }

        public ISession OpenSession()
        {
            return new Session(fileName);
        }
    }
}