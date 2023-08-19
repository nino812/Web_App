using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;
using System.Threading;


namespace WebApplication2.Controllers
{
    [RoutePrefix("api/client")]

        public class ClientController : ApiController
        {
            private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";

            [HttpGet]
            [Route("GetAllClients")]
            public List<Client> GetAllClients()
            {
                List<Client> clients = new List<Client>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAllClients", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Client client = new Client
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Type = (ClientType)Enum.Parse(typeof(ClientType), (string)reader["Type"], true),
                                BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : (DateTime?)null
                            };
                            clients.Add(client);
                        }
                    }
                }
                return clients;
            }
      
        [HttpGet]
        [Route("GetFilteredClients")]
        public List<Client> GetFilteredClients(string nameFilter, string typeFilter)
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("FilterClients", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", nameFilter);
                    command.Parameters.AddWithValue("@Type", typeFilter);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Client client = new Client
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Type = (ClientType)Enum.Parse(typeof(ClientType), (string)reader["Type"], true),
                            BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : (DateTime?)null
                        };

                        clients.Add(client);
                    }
                    connection.Close();
                }
            }

            return clients;
        }
        

        [HttpPost]
            [Route("AddClient")]
            public bool AddClient(Client client)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("AddClient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", client.Name);
                        command.Parameters.AddWithValue("@Type", client.Type.ToString());
                        command.Parameters.AddWithValue("@BirthDate", client.Type == ClientType.Organization ? DBNull.Value : (object)client.BirthDate);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }

            [HttpPut]
            [Route("UpdateClient")]
            public bool UpdateClient(Client client)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateClient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", client.Id);
                        command.Parameters.AddWithValue("@Name", client.Name);
                        command.Parameters.AddWithValue("@Type", client.Type.ToString());
                        command.Parameters.AddWithValue("@BirthDate", client.Type == ClientType.Organization ? DBNull.Value : (object)client.BirthDate);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }

        [HttpGet]
        [Route("GetClientsReport")]
        public IHttpActionResult GetClientsReport(string typeFilter)
        {
            List<ClientReport> clientsReport = new List<ClientReport>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ClientReport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Type", typeFilter);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ClientReport report = new ClientReport
                        {
                            Type = (string)reader["Type"],
                            NbOfClients = (int)reader["NbOfClients"]
                        };

                        clientsReport.Add(report);
                    }
                }
            }

            return Ok(clientsReport);
        }
    }
    }
