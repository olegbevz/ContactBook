using System.IO;
using System.Xml.Linq;

namespace ContactBook.Repositories.LinqToXml
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
            var targetDocument = new XDocument(new XElement("ContactBook"));
            targetDocument.Save(fileName);
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