namespace ContactBook.Repositories.Memory
{
    using global::BLToolkit.Data;
    using global::BLToolkit.Data.DataProvider;

    using Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ContactRepository : IContactRepository
    {
        private readonly IList<Contact> contacts;

        public ContactRepository(IList<Contact> contacts)
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
            return this.contacts.GetEnumerator();
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
            return this.contacts.FirstOrDefault(x => x.Id.Equals(id));
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

            this.contacts.Add(contact);
        }

        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="id">
        /// Удаляемая модель
        /// </param>
        public void Remove(Guid id)
        {
            this.contacts.Remove(Get(id));
        }

        /// <summary>
        /// Сохранение модели
        /// </summary>
        /// <param name="contact">
        /// Сохраняемая модель
        /// </param>
        public void Update(Contact contact)
        {
            var existingContact = this.Get(contact.Id);

            existingContact.Name = contact.Name;
            existingContact.Address = contact.Address;
            existingContact.Phone = contact.Phone;
        }
    }
}