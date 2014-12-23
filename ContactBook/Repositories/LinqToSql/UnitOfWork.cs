namespace ContactBook.Repositories.LinqToSql
{
    public class UnitOfWork : ISession
    {
        private readonly DataClassesDataContext dataContext;

        public UnitOfWork()
        {
            this.dataContext = new DataClassesDataContext();
            this.ContactRepository = new ContactRepository(dataContext);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
            dataContext.SubmitChanges();
        }

        public void Dispose()
        {
            dataContext.Dispose();
        }
    }
}