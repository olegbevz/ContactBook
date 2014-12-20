using System;
using ContactBook.Repositories;

namespace ContactBook.Models
{
    public class ContactsViewModel
    {
        public ContactsViewModel(RepositoryType dataSourceType, bool dataSourceExists, Contact[] contacts)
            : this(dataSourceType, dataSourceExists, contacts, TimeSpan.Zero)
        {
        }

        public ContactsViewModel(RepositoryType dataSourceType, bool dataSourceExists, Contact[] contacts, TimeSpan requestTime)
        {
            DataSourceType = dataSourceType;
            DataSourceExists = dataSourceExists;
            Contacts = contacts;
            RequestTime = requestTime;
        }

        public RepositoryType DataSourceType { get; private set; }

        public bool DataSourceExists { get; private set; }

        public TimeSpan RequestTime { get; private set; }

        public Contact[] Contacts { get; private set; }
    }
}