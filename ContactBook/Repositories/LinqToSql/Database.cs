namespace ContactBook.Repositories.LinqToSql
{
    public class Database : IDatabase
    {
        public void Create()
        {
            using (var context = new DataClassesDataContext())
            {
                context.CreateDatabase();
            }
        }

        public void Drop()
        {
            using (var context = new DataClassesDataContext())
            {
                context.DeleteDatabase();
            }
        }

        public bool Exist()
        {
            using (var context = new DataClassesDataContext())
            {
                return context.DatabaseExists();
            }
        }

        public ISession OpenSession()
        {
            return new UnitOfWork();
        }
    }
}