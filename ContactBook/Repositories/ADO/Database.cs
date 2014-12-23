using System.Data.SqlClient;

namespace ContactBook.Repositories.ADO
{
    public class Database : IDatabase
    {
        private readonly string connectionString;

        private readonly string masterConnectionString;

        public Database(string connectionString, string masterConnectionString)
        {
            this.masterConnectionString = masterConnectionString;

            this.connectionString = connectionString;
        }

        public void Create()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"CREATE DATABASE ContactBook;", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"CREATE TABLE [dbo].[Contacts] ([Id] uniqueidentifier NOT NULL, [Name] nvarchar(max) NOT NULL, [Address] nvarchar(max) NOT NULL, [Phone] nvarchar(max) NOT NULL);
                    ALTER TABLE [dbo].[Contacts] ADD CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC);",
                    connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Drop()
        {
            SqlConnection.ClearAllPools();

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DROP DATABASE ContactBook;", connection))
                {
                    var rowsChanged = command.ExecuteNonQuery();
                }
            }
        }

        public bool Exist()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT count(*) FROM master.sys.databases WHERE name = @database", connection))
                {
                    command.Parameters.AddWithValue("database", "ContactBook");
                    var queryResult = command.ExecuteScalar();
                    return (int)queryResult > 0;
                }
            }
        }

        public ISession OpenSession()
        {
            return new UnitOfWork(connectionString);
        }
    }
}