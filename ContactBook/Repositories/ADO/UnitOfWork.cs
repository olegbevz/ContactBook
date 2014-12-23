using System.Data.SqlClient;

namespace ContactBook.Repositories.ADO
{
    public class UnitOfWork : ISession
    {
        private readonly SqlConnection connection;

        public UnitOfWork(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
            this.connection.Open();

            ContactRepository = new ContactRepository(connection);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}