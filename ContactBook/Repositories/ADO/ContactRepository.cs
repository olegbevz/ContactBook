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
        private readonly string connectionString;

        private readonly string masterConnectionString;

        public ContactRepository(string connectionString, string masterConnectionString)
        {
            this.masterConnectionString = masterConnectionString;

            this.connectionString = connectionString;                   
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

        public void Create()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"CREATE DATABASE ContactBook;", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(
                    @"CREATE TABLE [dbo].[Contacts] ([Id] uniqueidentifier NOT NULL, [Name] nvarchar(max) NOT NULL, [Address] nvarchar(max) NOT NULL, [Phone] nvarchar(max) NOT NULL);
                    ALTER TABLE [dbo].[Contacts] ADD CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC);", 
                    connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Drop()
        {
            SqlConnection.ClearAllPools();

            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DROP DATABASE ContactBook;", connection))
                {
                    var rowsChanged = command.ExecuteNonQuery();
                }
            }
        }

        public bool Exist()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT count(*) FROM master.sys.databases WHERE name = @database", connection))
                {
                    command.Parameters.AddWithValue("database", "ContactBook");
                    var queryResult = command.ExecuteScalar();
                    return (int)queryResult > 0;
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
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}