using System;

namespace ContactBook.Repositories.BLToolkit
{
    public class Database : IDatabase
    {
        private readonly string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Database()
        {
            throw new NotImplementedException();
        }

        public void Create()
        {
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public bool Exist()
        {
            return true;
        }

        public ISession OpenSession()
        {
            return new UnitOfWork(connectionString);
        }
    }
}