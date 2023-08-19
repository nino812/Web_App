using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Vanrise_task3.Models
{
    public class Helper
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connec"].ConnectionString);
        public static List<T> GetSPItems<T>(string spName, Func<SqlDataReader, T> mapper, params object[] vars)
        {

            var result = new List<T>();
            con.Open();
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < vars.Length; i++)
            {
                cmd.Parameters.AddWithValue($"Prm{i}", vars[i]);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(mapper(reader));
            };
            con.Close();
            return result;
        }
        public static int ExecuteNonQuerySP(string spName, params object[] vars)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < vars.Length; i++)
            {
                cmd.Parameters.AddWithValue($"Prm{i}", vars[i]);
            }
            var exec = cmd.ExecuteNonQuery();
            con.Close();
            return exec;
        }
    }
}