using System.Collections;
using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ContactBook.Repositories.LinqToXml
{
    public class ContactRepository : IContactRepository
    {
        private string fileName;

        public ContactRepository(string fileName)
        {
            this.fileName = fileName;
        }

        public Contact Get(Guid id)
        {
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var document = XDocument.Load(fileStream);
                    if (document != null && document.Root != null)
                    {
                        var contactNode = document.Root.Elements("Contact")
                            .FirstOrDefault(x => Guid.Parse(x.Attribute("Id").Value).Equals(id));

                        if (contactNode != null)
                        {
                            return new Contact
                            {
                                Id = id,
                                Address = contactNode.Attribute("Address").Value,
                                Name = contactNode.Attribute("Name").Value,
                                Phone = contactNode.Attribute("Phone").Value
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Contact contact)
        {
            XDocument targetDocument = null;

            var contactNode = new XElement("Contact",
                            new XAttribute("Id", Guid.NewGuid().ToString()),
                            new XAttribute("Name", contact.Name),
                            new XAttribute("Address", contact.Address),
                            new XAttribute("Phone", contact.Phone));

            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var document = XDocument.Load(fileStream);
                    if (document != null && document.Root != null)
                    {
                        var sourceElements = document.Root.Elements("Contact").ToList();
                        sourceElements.Add(contactNode);
                        targetDocument = new XDocument(new XElement("ContactBook", sourceElements));
                    }
                }
            }
            else
            {
                targetDocument = new XDocument(new XElement("ContactBook", contactNode));
            }

            if (targetDocument != null)
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    targetDocument.Save(fileStream);
                }
            }        
        }

        public void Remove(Guid id)
        {
            if (!File.Exists(this.fileName))
            {
                return;
            }

            XDocument targetDocument = null;

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var document = XDocument.Load(fileStream);
                if (document != null && document.Root != null)
                {
                    var sourceElements = document.Root.Elements("Contact").ToList();
                    var contactNode = sourceElements
                        .FirstOrDefault(x => Guid.Parse(x.Attribute("Id").Value).Equals(id));
                    if (contactNode != null)
                    {
                        sourceElements.Remove(contactNode);
                    }
                    targetDocument = new XDocument(new XElement("ContactBook", sourceElements));
                }
            }

            if (targetDocument != null)
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var d = targetDocument.ToString();
                    targetDocument.Save(fileStream);
                }
            }  
        }

        public void Save(Contact contact)
        {
            if (!File.Exists(this.fileName))
            {
                return;
            }

            XDocument targetDocument = null;
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var document = XDocument.Load(fileStream);
                if (document != null && document.Root != null)
                {
                    var sourceElements = document.Root.Elements("Contact").ToList();
                    var contactNode = sourceElements
                         .FirstOrDefault(x => Guid.Parse(x.Attribute("Id").Value).Equals(contact.Id));

                    if (contactNode != null)
                    {
                        contactNode.Attribute("Name").Value = contact.Name;
                        contactNode.Attribute("Address").Value = contact.Address;
                        contactNode.Attribute("Phone").Value = contact.Phone;
                    }

                    targetDocument = new XDocument(new XElement("ContactBook", sourceElements));
                }
            }

            if (targetDocument != null)
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    targetDocument.Save(fileStream);
                }
            }
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

        public IEnumerator<Contact> GetEnumerator()
        {
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var document = XDocument.Load(fileStream);
                    if (document != null && document.Root != null)
                    {
                        return document.Root.Elements("Contact").Select(x =>
                           new Contact
                           {
                               Id = Guid.Parse(x.Attribute("Id").Value),
                               Address = x.Attribute("Address").Value,
                               Name = x.Attribute("Name").Value,
                               Phone = x.Attribute("Phone").Value

                           }).GetEnumerator();
                    }
                }
            }

            return new Contact[0].AsEnumerable().GetEnumerator();          
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}