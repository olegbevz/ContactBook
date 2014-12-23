using System.IO;
using System.Xml.Linq;

namespace ContactBook.Repositories.LinqToXml
{
    public class Session : ISession
    {
        private readonly string fileName;

        private readonly XDocument document;

        public Session(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            this.fileName = fileName;

            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                document = XDocument.Load(fileStream);
            }

            ContactRepository = new ContactRepository(document);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                document.Save(fileStream);
            }
        }

        public void Dispose()
        {
        }
    }
}