namespace ContactBook.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    public class XmlRepository : IContactRepository
    {
        private string _filePath = string.Empty;

        public XmlRepository(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Contact> GetEnumerator()
        {
            var contacts = this.GetAll();

            return contacts.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, который осуществляет перебор элементов коллекции.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Collections.IEnumerator"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Получение модели по идентификатору
        /// </summary>
        /// <param name="id">
        /// Идентификатор модели
        /// </param>
        /// <returns>
        /// The <see cref="Contact"/>.
        /// </returns>
        public Contact Get(Guid id)
        {
            var contacts = this.GetAll();

            return contacts.FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Добавление модели 
        /// </summary>
        /// <param name="contact">
        /// Добавляемая модель
        /// </param>
        public void Add(Contact contact)
        {
            contact.Id = Guid.NewGuid();

            var contacts = this.GetAll().ToList();

            contacts.Add(contact);

            this.SaveAll(contacts);
        }

        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="contact">
        /// Удаляемая модель
        /// </param>
        public void Remove(Guid id)
        {
            var contacts = this.GetAll().ToList();

            var contact = contacts.FirstOrDefault(x => x.Id.Equals(id));

            if (contact == null)
            {
                throw new Exception("Удаление несуществующей модели.");
            }

            contacts.Remove(contact);

            this.SaveAll(contacts);
        }

        /// <summary>
        /// Сохранение модели
        /// </summary>
        /// <param name="contact">
        /// Сохраняемая модель
        /// </param>
        public void Save(Contact contact)
        {
            var contacts = this.GetAll();

            var existingContact = contacts.FirstOrDefault(x => x.Id.Equals(contact.Id));

            if (existingContact == null)
            {
                throw new Exception("Сохранение несуществующей модели данных.");
            }

            existingContact.Name = contact.Name;
            existingContact.Phone = contact.Phone;
            existingContact.Address = contact.Address;

            this.SaveAll(contacts);
        }

        private IEnumerable<Contact> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new Contact[0];
            }

            using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var serializer = new XmlSerializer(typeof(Contact[]));

                return serializer.Deserialize(fileStream) as Contact[];
            }
        }

        private void SaveAll(IEnumerable<Contact> contacts)
        {
            using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var serializer = new XmlSerializer(typeof(Contact[]));

                serializer.Serialize(fileStream, contacts.ToArray());

                fileStream.Flush();
            }
        }
    }
}