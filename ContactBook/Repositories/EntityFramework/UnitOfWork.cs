namespace ContactBook.Repositories.EntityFramework
{
    public class UnitOfWork : ISession
    {
        private readonly DomainContainer dataContext;

        public UnitOfWork()
        {
            this.dataContext = new DomainContainer();
            this.dataContext.Configuration.AutoDetectChangesEnabled = false;
            this.ContactRepository = new ContactRepository(dataContext);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
            this.dataContext.ChangeTracker.DetectChanges();
            dataContext.SaveChanges();
        }

        public void Dispose()
        {
            dataContext.Dispose();
        }
    }
}