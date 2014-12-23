using System.Collections;
using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ContactBook.Repositories.LinqToXml
{
    public class ContactRepository : IContactRepository
    {
        private readonly XDocument document;

        public ContactRepository(XDocument document)
        {
            this.document = document;
        }

        public Contact Get(Guid id)
        {
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

            return null;
        }

        public void Add(Contact contact)
        {
            var contactNode = new XElement("Contact",
                            new XAttribute("Id", Guid.NewGuid().ToString()),
                            new XAttribute("Name", contact.Name),
                            new XAttribute("Address", contact.Address),
                            new XAttribute("Phone", contact.Phone));


            if (document != null && document.Root != null)
            {
                document.Root.Add(contactNode);
            }  
        }

        public void Remove(Guid id)
        {
            if (document != null && document.Root != null)
            {
                var contactNodes = document.Root.Elements("Contact").ToList();

                var contactNode = contactNodes
                    .FirstOrDefault(x => Guid.Parse(x.Attribute("Id").Value).Equals(id));

                if (contactNode != null)
                {
                    contactNodes.Remove(contactNode);
                    document.Root.RemoveAll();
                    document.Root.Add(contactNodes);
                }
            }
        }

        public void Update(Contact contact)
        {
            if (document != null && document.Root != null)
            {
                var contactNode = document.Root.Elements("Contact")
                    .FirstOrDefault(x => Guid.Parse(x.Attribute("Id").Value).Equals(contact.Id));

                if (contactNode != null)
                {
                    contactNode.Attribute("Name").Value = contact.Name;
                    contactNode.Attribute("Address").Value = contact.Address;
                    contactNode.Attribute("Phone").Value = contact.Phone;
                }
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
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

            return new Contact[0].AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}