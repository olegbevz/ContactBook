using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using ContactBook.Models;

namespace ContactBook.Repositories.ADO
{
    public class ContactRepository : IContactRepository
    {  
        private string connectionString;

        public ContactRepository()
        {
            var connectionStringsSection = WebConfigurationManager
                .GetSection("connectionStrings") as ConnectionStringsSection;

            if (connectionStringsSection == null)
            {
                throw new Exception("Connection strings section not found.");
            }
            
            var connectionStringSettings = connectionStringsSection.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(x => x.Name == "ContactBookConnectionString");

            if (connectionStringSettings == null)
            {
                throw new Exception("Connection string not found");
            }

            this.connectionString = connectionStringSettings.ConnectionString;                   
        }

        public Contact Get(Guid id)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand(
                    "SELECT Name, Address, Phone FROM dbo.Contacts WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("Id", id);
                    connection.Open();
                    using (var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
        }

        public void Add(Contact contact)
        {
            contact.Id = Guid.NewGuid();

            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand(
                    "INSERT INTO dbo.Contacts (Id, Name, Address, Phone) VALUES (@Id, @Name, @Address, @Phone)", connection))
                {
                    command.Parameters.AddWithValue("Id", contact.Id);
                    command.Parameters.AddWithValue("Name", contact.Name);
                    command.Parameters.AddWithValue("Address", contact.Address);
                    command.Parameters.AddWithValue("Phone", contact.Phone);

                    connection.Open();
                    var rowsChanged = command.ExecuteNonQuery();
                }
            }
        }

        public void Remove(Guid id)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand(
                    "DELETE FROM dbo.Contacts WHERE Id=@Id", connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    connection.Open();
                    var rowsChanged = command.ExecuteNonQuery();
                }
            }
        }

        public void Save(Contact contact)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand(
                    "UPDATE dbo.Contacts SET Name=@Name, Address=@Address, Phone=@Phone WHERE Id=@Id", connection))
                {
                    command.Parameters.AddWithValue("Id", contact.Id);
                    command.Parameters.AddWithValue("Name", contact.Name);
                    command.Parameters.AddWithValue("Address", contact.Address);
                    command.Parameters.AddWithValue("Phone", contact.Phone);

                    connection.Open();
                    var rowsChanged = command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                using (var command = new SqlCommand("SELECT Id, Name, Address, Phone FROM dbo.Contacts", connection))
                {
                    using (var tableAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        var dataSet = new DataSet();
                        tableAdapter.Fill(dataSet);
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
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void CreateStorage()
        {
        }


        public void DeleteStorage()
        {
            throw new NotImplementedException();
        }
    }
}