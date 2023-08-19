using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;
using System.Security.Policy;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/phone")]
    public class PhoneController : ApiController
    {
        private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";

        [HttpGet]
        [Route("GetAllPhones")]
        public List<phone> GetAllPhones()
        {
            List<phone> phones = new List<phone>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetAllPhoneNumbers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        phone phone = new phone
                        {
                            Id = (int)reader["Id"],
                            Number = (string)reader["Number"],
                            DeviceId = (int)reader["DeviceId"]

                        };

                        phones.Add(phone);

                    }

                }

            }

            return phones;
        }

        [HttpPost]
        [Route("AddPhone")]
        public bool AddPhone(phone phone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("AddPhone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Number", phone.Number);
                    command.Parameters.AddWithValue("@DeviceId", phone.DeviceId);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

           
        }
        [HttpPut]
        [Route("UpdatePhone")]
        public bool UpdatePhone(phone phone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdatePhone", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", phone.Id);
                    command.Parameters.AddWithValue("@Number", phone.Number);
                    command.Parameters.AddWithValue("@DeviceId", phone.DeviceId);


                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

           
        }

        [HttpGet]
        [Route("FilterPhones")]
        public List<phone> FilterPhones(string numberFilter,int? deviceFilter)
        {
            List<phone> phones = new List<phone>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("FilterPhones", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Number", numberFilter);
                    command.Parameters.AddWithValue("@DeviceId", deviceFilter);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        phone phone = new phone
                        {
                            Id = (int)reader["Id"],
                            Number = (string)reader["Number"],
                            DeviceId = (int)reader["DeviceId"]
                        };

                        phones.Add(phone);
                    }
                }
            }

            return phones;
        }
        [HttpGet]
        [Route("GetPhoneReport")]
        public List<ReportPhone> GetPhoneReport(int? deviceFilter, string registrationFilter)
        {
            List<ReportPhone> reportPhone = new List<ReportPhone>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ReportPhones", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                   command.Parameters.AddWithValue("@Status", registrationFilter);
                    command.Parameters.AddWithValue("@DeviceId", deviceFilter);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ReportPhone phone = new ReportPhone
                        {
                            
                            Status = (string)reader["Status"],
                            DeviceId = (int)reader["DeviceId"],
                            NbOfPhoneNumbers = (int)reader["NbOfPhoneNumbers"]
                        };

                        reportPhone.Add(phone);
                    }
                }
            }

            return reportPhone;
        }
    }
}



