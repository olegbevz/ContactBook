using System.Linq;

namespace ContactBook.Repositories.Xml
{
    using Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ContactRepository : IContactRepository
    {
        private readonly List<Contact> contacts; 

        public ContactRepository(List<Contact> contacts)
        {
            this.contacts = contacts;
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

            contacts.Add(contact);
        }

        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="contact">
        /// Удаляемая модель
        /// </param>
        public void Remove(Guid id)
        {
            var contact = contacts.FirstOrDefault(x => x.Id.Equals(id));
            if (contact == null)
            {
                throw new Exception("Удаление несуществующей модели.");
            }

            contacts.Remove(contact);
        }

        /// <summary>
        /// Сохранение модели
        /// </summary>
        /// <param name="contact">
        /// Сохраняемая модель
        /// </param>
        public void Update(Contact contact)
        {
            var existingContact = contacts.FirstOrDefault(x => x.Id.Equals(contact.Id));
            if (existingContact == null)
            {
                throw new Exception("Сохранение несуществующей модели данных.");
            }

            existingContact.Name = contact.Name;
            existingContact.Phone = contact.Phone;
            existingContact.Address = contact.Address;
        }
    }
}