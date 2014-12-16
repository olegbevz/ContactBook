using System.Configuration;
using System;
using System.IO;
using System.Web;

namespace ContactBook.Repositories
{
    public class RepositoryFactory
    {
        public IContactRepository CreateRepository(DataSourceType dataSourceType)
        {
            switch (dataSourceType)
            {
                case DataSourceType.ADO:
                    return new ADO.ContactRepository(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case DataSourceType.EntityFramework:
                    return new EntityFramework.ContactRepository();
                case DataSourceType.LinqToSql:
                    return new LinqToSql.ContactRepository();
                case DataSourceType.LinqToXml:
                    return new LinqToXml.ContactRepository(
                        CombineFileName("contactbook-linqtoxml.xml"));
                case DataSourceType.Memory:
                    return Memory.ContactRepository.Instance;
                case DataSourceType.NHibernate:
                    return new NHibernate.ContactRepository(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case DataSourceType.Xml:
                    return new Xml.ContactRepository(
                        CombineFileName("contactbook-xml.xml"));
                case DataSourceType.BLToolkit:
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