using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ContactBook.Models;

namespace ContactBook.Repositories.ADO
{
    public class ContactRepository : IContactRepository
    {
        private readonly SqlConnection connection;

        public ContactRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public Contact Get(Guid id)
        {
            using (var command = new SqlCommand(
                "SELECT Name, Address, Phone FROM dbo.Contacts WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("Id", id);
                using (SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dataReader.Read())
                    {
                        return new Contact
                        {
                            Id = id,
                            Name = dataReader.GetString(0),
                            Address = dataReader.GetString(1),
                            Phone = dataReader.GetString(2)
                        };
                    }

                    return null;
                }
            }
        }

        public void Add(Contact contact)
        {
            contact.Id = Guid.NewGuid();
            using (var command = new SqlCommand(
                "INSERT INTO dbo.Contacts (Id, Name, Address, Phone) VALUES (@Id, @Name, @Address, @Phone)", connection))
            {
                command.Parameters.AddWithValue("Id", contact.Id);
                command.Parameters.AddWithValue("Name", contact.Name);
                command.Parameters.AddWithValue("Address", contact.Address);
                command.Parameters.AddWithValue("Phone", contact.Phone);
                var rowsChanged = command.ExecuteNonQuery();
            }
        }

        public void Remove(Guid id)
        {
            using (var command = new SqlCommand(
                "DELETE FROM dbo.Contacts WHERE Id=@Id", connection))
            {
                command.Parameters.AddWithValue("Id", id);

                var rowsChanged = command.ExecuteNonQuery();
            }
        }

        public void Update(Contact contact)
        {
            using (var command = new SqlCommand(
                "UPDATE dbo.Contacts SET Name=@Name, Address=@Address, Phone=@Phone WHERE Id=@Id", connection))
            {
                command.Parameters.AddWithValue("Id", contact.Id);
                command.Parameters.AddWithValue("Name", contact.Name);
                command.Parameters.AddWithValue("Address", contact.Address);
                command.Parameters.AddWithValue("Phone", contact.Phone);

                var rowsChanged = command.ExecuteNonQuery();
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            using (var command = new SqlCommand("SELECT Id, Name, Address, Phone FROM dbo.Contacts", connection))
            {
                using (var tableAdapter = new SqlDataAdapter(command))
                {
                    var dataSet = new DataSet();
                    tableAdapter.Fill(dataSet);
                    tableAdapter.Dispose();
                    return dataSet.Tables[0].Rows.Cast<DataRow>()
                        .Select(row => new Contact
                    {
                        Id = row.Field<Guid>("Id"),
                        Name = row.Field<string>("Name"),
                        Address = row.Field<string>("Address"),
                        Phone = row.Field<string>("Phone")
                    }).ToList().GetEnumerator();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}