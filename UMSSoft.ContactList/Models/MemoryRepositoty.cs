﻿namespace UMSSoft.ContactList.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class MemoryRepository : IContactRepository
    {
        private readonly IList<Contact> _contacts = new List<Contact>();

        private static MemoryRepository _instance = new MemoryRepository();

        /// <summary>
        /// Gets the instance.
        /// Статический экземпляр сущности
        /// </summary>
        public static MemoryRepository Instance 
        {
            get
            {
                return _instance;
            }
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
            return this._contacts.GetEnumerator();
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
            return this._contacts.FirstOrDefault(x => x.Id.Equals(id));
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

            this._contacts.Add(contact);
        }

        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="id">
        /// Удаляемая модель
        /// </param>
        public void Remove(Guid id)
        {
            this._contacts.Remove(Get(id));
        }

        /// <summary>
        /// Сохранение модели
        /// </summary>
        /// <param name="contact">
        /// Сохраняемая модель
        /// </param>
        public void Save(Contact contact)
        {
            var existingContact = this.Get(contact.Id);

            existingContact.Name = contact.Name;
            existingContact.Adress = contact.Adress;
            existingContact.Phone = contact.Phone;
        }
    }
}