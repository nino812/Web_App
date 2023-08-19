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
using System.Web.UI;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/reservation")]
    public class ReservationController : ApiController
    {
        private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";

        [HttpGet]
        [Route("GetAllReservations")]
        public List<PhoneReservation> GetAllReservations()
        {
            List<PhoneReservation> reservations = new List<PhoneReservation>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetAllPhoneReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PhoneReservation reservation = new PhoneReservation
                        {
                            Id = (int)reader["Id"],
                            PhoneNumber = (int)reader["PhoneNumber"],
                            Client = (int)reader["Client"],
                            EED = reader["EED"] != DBNull.Value ? (DateTime)reader["EED"] : (DateTime?)null,
                            BED = (DateTime)reader["BED"]

                        };

                        reservations.Add(reservation);

                    }

                }

            }

            return reservations;
        }

        [HttpGet]
        [Route("FilterReservations")]
        public List<PhoneReservation> FilterReservations(int? PhoneNumberFilter, int? ClientFilter)
        {
            List<PhoneReservation> reservations = new List<PhoneReservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("FilterPhoneReservations", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Client", ClientFilter);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumberFilter);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        PhoneReservation reservation = new PhoneReservation
                        {
                            Id = (int)reader["Id"],
                            PhoneNumber = (int)reader["PhoneNumber"],
                            Client = (int)reader["Client"],
                            EED = reader["EED"] != DBNull.Value ? (DateTime)reader["EED"] : (DateTime?)null,
                            BED = (DateTime)reader["BED"]
                        };

                        reservations.Add(reservation);
                    }
                }
            }

            return reservations;
        }
        [HttpPost]
        [Route("AddPhoneNumberReservation")]
        public IHttpActionResult AddPhoneNumberReservation(PhoneReservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Invalid reservation data.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("AddReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Client", reservation.Client);
                    command.Parameters.AddWithValue("@PhoneNumber", reservation.PhoneNumber);
                    command.Parameters.AddWithValue("@BED", reservation.BED);
                    command.Parameters.AddWithValue("@EED", reservation.EED);

                    command.ExecuteNonQuery();
                }
            }

            return Ok("Phone number reservation added successfully.");
        }
        [HttpGet]
        [Route("EffectiveReservation")]
        public bool EffectiveReservation(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("EffectiveReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Client", clientId);

                    SqlDataReader reader = command.ExecuteReader();
                    bool hasEffectiveReservation = reader.HasRows;

                    return hasEffectiveReservation;
                }
            }
        }
        [HttpPost]
        [Route("UnReserve")]
        public IHttpActionResult UnReserve(Client client)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UnReserve", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Client", client.Id);

                        command.ExecuteNonQuery();

                        return Ok("Phone registration successfully unreserved.");
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
