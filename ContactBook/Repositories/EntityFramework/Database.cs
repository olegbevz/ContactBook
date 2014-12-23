namespace ContactBook.Repositories.EntityFramework
{
    public class Database : IDatabase
    {
        public void Create()
        {
            using (var context = new DomainContainer())
            {
                context.Database.Create();
            }
        }

        public void Drop()
        {
            using (var context = new DomainContainer())
            {
                context.Database.Delete();
            }
        }

        public bool Exist()
        {
            using (var context = new DomainContainer())
            {
                return context.Database.Exists();
            }
        }

        public ISession OpenSession()
        {
            return new UnitOfWork();
        }
    }
}