//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using WebApplication2.Models;

//namespace WebApplication2.Controllers
//{
//    [RoutePrefix("api/client")]
//    public class ClientReportController : ApiController
//    {

//        [HttpGet]
//        [Route("GetClientsReport")]
//        public List<Client> GetClientsReport()
//        {
//            List<Client> clients = new List<Client>();
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                using (SqlCommand command = new SqlCommand("ClientReport", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    SqlDataReader reader = command.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        Client client = new Client
//                        {
//                            Id = (int)reader["Id"],
//                            Type = (ClientType)Enum.Parse(typeof(ClientType), (string)reader["Type"], true),
//                        };
//                        clients.Add(client);
//                    }
//                }
//            }
//            return clients;
//        }
//        [HttpGet]
//        [Route("GetFilteredClients")]
//        public List<Client> GetFilteredClients(string typeFilter)
//        {
//        private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";

//        List<Client> clients = new List<Client>();

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                using (SqlCommand command = new SqlCommand("FilterClients", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
                  
//                    command.Parameters.AddWithValue("@Type", typeFilter);

//                    SqlDataReader reader = command.ExecuteReader();

//                    while (reader.Read())
//                    {
//                        Client client = new Client
//                        {
//                            Id = (int)reader["Id"],
//                            Name = (string)reader["Name"],
//                            Type = (ClientType)Enum.Parse(typeof(ClientType), (string)reader["Type"], true),
//                            BirthDate = reader["BirthDate"] != DBNull.Value ? (DateTime)reader["BirthDate"] : (DateTime?)null
//                        };

//                        clients.Add(client);
//                    }
//                    connection.Close();
//                }
//            }

//            return clients;
//        }

//    }
//}
