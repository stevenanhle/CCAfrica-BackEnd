using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Login_Registration.Models
{
    public class Profile
    {
        public string profileID { get; set; }
        public string email { get; set; }
        public string activity { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string attendedEvent { get; set; }
        public string phone { get; set; }
        public string country { get; set; }

        public void Retrieve(String UserId)
        {
         
            MySqlConnection connection = new MySqlConnection("server=localhost; user id=root; database=demo; password=1234");
            connection.Open();
            String text = "Select * from profile where email=@loginUser";
            MySqlCommand command = new MySqlCommand(text, connection);
            string loginUser =UserId;
            command.Parameters.AddWithValue("@loginUser", loginUser);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    email= reader["email"].ToString();
                    activity = reader["activity"].ToString();
                    address1 = reader["address1"].ToString();
                    address2 = reader["address2"].ToString();
                    country = reader["country"].ToString();
                    phone = reader["phone"].ToString();
                    attendedEvent = reader["attendedEvent"].ToString();

                }
            }

            connection.Close();
             
        }


        public bool Edit(String UserId)
        {
                bool flag = false;
                email = UserId;
                MySqlConnection connection = new MySqlConnection("server=localhost; user id=root; database=demo; password=1234");
                connection.Open();
                String text = "UPDATE profile SET activity=@activity, address1=@address1, address2=@address2, country=@country, phone=@phone, attendedEvent=@attendedEvent where email=@email";
                MySqlCommand command = new MySqlCommand(text, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@activity", activity);
                command.Parameters.AddWithValue("@address1", address1);
                command.Parameters.AddWithValue("@address2", address2);
                command.Parameters.AddWithValue("@country", country);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@attendedEvent", attendedEvent);
                command.ExecuteNonQuery();
                connection.Close();
                flag = true;
                return flag;
          
        }
    }
}
