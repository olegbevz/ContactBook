using System;
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;
using INHSession = NHibernate.ISession;

namespace ContactBook.Repositories.NHibernate
{
    public class Database : IDatabase
    {
        private const string CONFIGURATION_FILE = "ContactBook.Repositories.NHibernate.hibernate.cfg.xml";

        private readonly Configuration masterConfiguration;

        private readonly Configuration configuration;

        public Database(string connectionString, string masterConnectionString)
        {
            var assembly = this.GetType().Assembly;

            this.configuration = new Configuration()
                .Configure(assembly, CONFIGURATION_FILE)
                .SetProperty(Environment.ConnectionString, connectionString);

            this.masterConfiguration = new Configuration()
                .Configure(assembly, CONFIGURATION_FILE)
                .SetProperty(Environment.ConnectionString, masterConnectionString);
        }

        public void Create()
        {
            using (ISessionFactory sessionFactory = masterConfiguration.BuildSessionFactory())
            {
                using (INHSession session = sessionFactory.OpenSession())
                {
                    var query = session.CreateSQLQuery("CREATE DATABASE ContactBook;");
                    var result = query.ExecuteUpdate();
                }
            }

            var schemaExport = new SchemaExport(configuration);
            schemaExport.Create(false, true);
        }

        public void Drop()
        {
            SqlConnection.ClearAllPools();

            using (ISessionFactory sessionFactory = masterConfiguration.BuildSessionFactory())
            {
                using (INHSession session = sessionFactory.OpenSession())
                {
                    var query = session.CreateSQLQuery("DROP DATABASE ContactBook;");
                    var result = query.ExecuteUpdate();
                }
            }
        }

        public bool Exist()
        {
            try
            {
                var schemaValidator = new SchemaValidator(this.configuration);
                schemaValidator.Validate();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ISession OpenSession()
        {
            return new UnitOfWork(configuration);
        }
    }
}