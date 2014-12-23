using System.Configuration;
using System;
using System.IO;
using System.Web;

namespace ContactBook.Repositories
{
    public class DatabaseFactory
    {
        public IDatabase GetDatabase(DatabaseType dataSourceType)
        {
            switch (dataSourceType)
            {
                case DatabaseType.ADO:
                    return new ADO.Database(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case DatabaseType.EntityFramework:
                    return new EntityFramework.Database();
                case DatabaseType.LinqToSql:
                    return new LinqToSql.Database();
                case DatabaseType.LinqToXml:
                    return new LinqToXml.Database(CombineFileName("contactbook-linqtoxml.xml"));
                case DatabaseType.Memory:
                    return Memory.Database.Instance;
                case DatabaseType.NHibernate:
                    return new NHibernate.Database(GetConnectionString("ContactBookConnectionString"), GetConnectionString("MasterConnectionString"));
                case DatabaseType.Xml:
                    return new Xml.Database(CombineFileName("contactbook-xml.xml"));
                case DatabaseType.BLToolkit:
                    return new BLToolkit.Database(GetConnectionString("ContactBookConnectionString"));
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