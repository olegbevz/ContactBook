using System.Configuration;
using System;
using System.IO;
using System.Web;

namespace ContactBook.Repositories
{
    public class RepositoryFactory
    {
        public IContactRepository CreateRepository(RepositoryType dataSourceType)
        {
            switch (dataSourceType)
            {
                case RepositoryType.ADO:
                    return new ADO.ContactRepository(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case RepositoryType.EntityFramework:
                    return new EntityFramework.ContactRepository();
                case RepositoryType.LinqToSql:
                    return new LinqToSql.ContactRepository();
                case RepositoryType.LinqToXml:
                    return new LinqToXml.ContactRepository(
                        CombineFileName("contactbook-linqtoxml.xml"));
                case RepositoryType.Memory:
                    return Memory.ContactRepository.Instance;
                case RepositoryType.NHibernate:
                    return new NHibernate.ContactRepository(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case RepositoryType.Xml:
                    return new Xml.ContactRepository(
                        CombineFileName("contactbook-xml.xml"));
                case RepositoryType.BLToolkit:
                    return new BLToolkit.ContactRepository(GetConnectionString("ContactBookConnectionString"));
                default:
                    throw new NotImplementedException();
            }
        }

        private string CombineFileName(string fileName)
        {
            var directory = HttpContext.Current.Server.MapPath("~/App_Data");

            if (string.IsNullOrEmpty(directory))
            {
                throw new DirectoryNotFoundException();
            }

            return Path.Combine(directory, fileName);
        }

        private string GetConnectionString(string connectionStringName)
        {
            var connectionStringsSection = ConfigurationManager.ConnectionStrings;

            if (connectionStringsSection == null)
            {
                throw new Exception("Connection strings section not found.");
            }

            var connectionStringSettings = connectionStringsSection[connectionStringName];
            if (connectionStringSettings == null)
            {
                throw new Exception("Connection string not found");
            }

            return connectionStringSettings.ConnectionString;
        }
    }
}