using System;

namespace ContactBook.Repositories
{
    /// <summary>
    /// Абтрактная сессия. 
    /// В случае работы с ORM под сессией понимается реализация шаблона UnitOfWork. 
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Gets or sets the contact repository.
        /// </summary>
        IContactRepository ContactRepository { get; }

        /// <summary>
        /// Применение выполненных изменений к базе данных
        /// </summary>
        void Commit();
    }
}