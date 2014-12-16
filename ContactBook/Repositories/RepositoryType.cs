using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBook.Repositories
{
    public enum DataSourceType
    {
        Memory,
        Xml,
        ADO,
        LinqToXml,
        LinqToSql,
        EntityFramework,
        NHibernate,
        BLToolkit
    }
}