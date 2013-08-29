namespace UMSSoft.ContactList.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Модель котактных данных
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the id.
        /// Идентификатор модели 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// Полное имя пользователя
        /// </summary>
        [DisplayName("Полное имя")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the adress.
        /// Адресс контактных данных
        /// </summary>
        [DisplayName("Адрес")]
        public string Adress { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// Телефонный номер
        /// </summary>
        [DisplayName("Телефон")]
        public string Phone { get; set; }
    }
}