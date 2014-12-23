using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace ContactBook.Repositories.BLToolkit
{
    public class UnitOfWork : ISession
    {
        private readonly DbManager databaseManager;

        public UnitOfWork(string connectionString)
        {
            databaseManager = new DbManager(new Sql2012DataProvider(), connectionString);
            ContactRepository = new ContactRepository(databaseManager);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
        }

        public void Dispose()
        {
            databaseManager.Dispose();
        }
    }
}