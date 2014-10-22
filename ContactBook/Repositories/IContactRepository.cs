using System.Collections.Generic;

namespace ContactBook.Models
{
    using System;

    public interface IContactRepository : IEnumerable<Contact>
    {
        /// <summary>
        /// Получение модели по идентификатору
        /// </summary>
        /// <param name="id">
        /// Идентификатор модели
        /// </param>
        /// <returns>
        /// The <see cref="Contact"/>.
        /// </returns>
        Contact Get(Guid id);

        /// <summary>
        /// Добавление модели 
        /// </summary>
        /// <param name="contact">
        /// Добавляемая модель
        /// </param>
        void Add(Contact contact);

        /// <summary>
        /// Удаление модели
        /// </summary>
        /// <param name="id">
        /// Удаляемая модель
        /// </param>
        void Remove(Guid id);

        /// <summary>
        /// Сохранение модели
        /// </summary>
        /// <param name="contact">
        /// Сохраняемая модель
        /// </param>
        void Save(Contact contact);

        /// <summary>
        /// Создание базы данных
        /// </summary>
        void CreateStorage();

        /// <summary>
        /// Удалить базу данных
        /// </summary>
        void DeleteStorage();
    }
}
