using ContactBook.Repositories;

namespace ContactBook.Models
{
    public class ContactsViewModel
    {
        public ContactsViewModel(DataSourceType dataSourceType, bool dataSourceExists, Contact[] contacts)
        {
            DataSourceType = dataSourceType;
            DataSourceExists = dataSourceExists;
            Contacts = contacts;
        }

        public DataSourceType DataSourceType { get; private set; }

        public bool DataSourceExists { get; private set; }

        public Contact[] Contacts { get; private set; }
    }
}