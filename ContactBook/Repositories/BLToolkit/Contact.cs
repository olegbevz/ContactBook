using System;
using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Data.Linq;

namespace ContactBook.Repositories.BLToolkit
{
    [TableName("Contacts")]
    public class Contact
    {
        [PrimaryKey, Identity]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}