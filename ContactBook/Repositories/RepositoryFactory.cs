using System.Configuration;
using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    return new ADO.ContactRepository();
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
                    var connectionString = ConfigurationManager.ConnectionStrings["ContactBookConnectionString"].ToString();
                    return new NHibernate.ContactRepository(connectionString);
                case DataSourceType.Xml:
                    return new Xml.ContactRepository(
                        CombineFileName("contactbook-xml.xml"));
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
    }
}