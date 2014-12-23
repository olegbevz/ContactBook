namespace ContactBook.Repositories
{
    /// <summary>
    /// Абстрактное понятие базы данных
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Проверка наличия базы данных
        /// </summary>
        /// <returns></returns>
        bool Exist();

        /// <summary>
        /// Создание базы данных
        /// </summary>
        void Create();

        /// <summary>
        /// Удалить базу данных
        /// </summary>
        void Drop();

        /// <summary>
        /// Создание сессии
        /// </summary>
        /// <returns></returns>
        ISession OpenSession();
    }
}