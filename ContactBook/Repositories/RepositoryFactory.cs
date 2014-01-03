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
                    return new Repositories.ADO.ContactRepository();
                case DataSourceType.EntityFramework:
                    return new Repositories.EntityFramework.ContactRepository();
                case DataSourceType.LinqToSql:
                    return new Repositories.LinqToSql.ContactRepository();
                case DataSourceType.LinqToXml:
                    return new Repositories.LinqToXml.ContactRepository(
                        CombineFileName("contactbook-linqtoxml.xml"));
                case DataSourceType.Memory:
                    return Repositories.Memory.ContactRepository.Instance;
                case DataSourceType.NHibernate:
                    return new Repositories.NHibernate.ContactRepository();
                case DataSourceType.Xml:
                    return new Repositories.Xml.ContactRepository(
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