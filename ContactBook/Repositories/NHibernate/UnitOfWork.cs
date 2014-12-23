using NHibernate;
using NHibernate.Cfg;

using INHSession = NHibernate.ISession;

namespace ContactBook.Repositories.NHibernate
{
    public class UnitOfWork : ISession
    {
        private readonly ISessionFactory sessionFactory;

        private readonly INHSession session;

        public UnitOfWork(Configuration configuration)
        {
            this.sessionFactory = configuration.BuildSessionFactory();
            this.session = sessionFactory.OpenSession();
            this.ContactRepository = new ContactRepository(session);
        }

        public IContactRepository ContactRepository { get; private set; }

        public void Commit()
        {
            session.Flush();
        }

        public void Dispose()
        {
            session.Dispose();
            sessionFactory.Dispose();
        }
    }
}