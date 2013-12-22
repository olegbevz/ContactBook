using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactBook.Repositories.NHibernate
{
    public class Contact
    {
        public virtual System.Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phone { get; set; }
    }
}