using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
        
    {
        private readonly string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=DevicesDB;Integrated Security=True";
        [HttpPost]
        [Route("registerUser")]
        public IHttpActionResult RegisterUser(User user)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", Encrypt(user.Password));
                    command.ExecuteNonQuery();

                }
            }
                    return Ok("User registered successfully.");
        }

        private static string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private static string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    

    [HttpPost]
        [Route("loginUser")]
        public IHttpActionResult loginUser(User user)
        {


            int result = ValidateUser(user.Username, user.Password);

            if (result == 1)
            {
                // User authenticated successfully
                return Ok("User authenticated.");
            }
            else
            {
                // Invalid credentials
                return Ok("Unauthorized()");
            }
        }
        private int ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("ValidateUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    var encrypted = Encrypt(password);
                    command.Parameters.AddWithValue("@Password", encrypted);


                    SqlParameter outputParam = new SqlParameter("@IsValid", SqlDbType.Bit);
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);

                    // Execute the command
                    command.ExecuteNonQuery();

                    // Retrieve the output parameter value
                    var result = (bool)command.Parameters["@IsValid"].Value;

                    if (result)
                    {
                        // User authenticated successfully
                        return 1;
                    }

                    return 0;
                }
            }
            }
        }
    }








