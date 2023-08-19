using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Description;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";

        [HttpGet]
        [Route("GetAllDevices")]
        public List<Device> GetAllDevices()
        {
            List<Device> devices = new List<Device>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetAllDevices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Device device = new Device
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],

                        };

                        devices.Add(device);

                    }

                }

            }

            return devices;
        }

        [HttpGet]
        [Route("GetFilteredDevices")]

        public List<Device> GetFilteredDevices(string keyword)
        {
            List<Device> devices = new List<Device>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("FilterDevicesByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", keyword);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Device device = new Device
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"]

                        };

                        devices.Add(device);
                    }
                    connection.Close();
                }
            }
            return devices;
        }
        [HttpPost]
        [Route("AddDevice")]

        public bool AddDevice(Device device)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("AddDevice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", device.Name);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }

            }

        }
        [HttpPut]
        [Route("UpdateDevice")]

        public bool UpdateDevice(Device device)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("UpdateDevice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", device.Id);
                    command.Parameters.AddWithValue("@Name", device.Name);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }


            }

        }
    }
}   


