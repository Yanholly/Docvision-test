using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLogins.Models;

namespace WebLogins.Repositories
{
    public class LoginRepository
    {
        //Добавление нового пользователя в базу
        public static bool AddUserToDB(MyUser myUser)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                                    + "Integrated Security=SSPI;";

            SqlConnection connection = new SqlConnection(connectionString);

            var query = $"INSERT INTO TestUsers (UserLogin,Password,UserActive) VALUES ('{myUser.Login}','{myUser.Password}','true')";

            query = query.Replace("@UserLogin", myUser.Login)
                         .Replace("@Password", myUser.Password);
            
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        } 
        
        //Поиск пользователя в базе по имени
        public static int FindUser(string UserLogin)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                             + "Integrated Security=SSPI;";

            var query = $"SELECT UserId FROM TestUsers WHERE UserLogin = '{UserLogin}'";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                int answer = 0;
                if (command.ExecuteScalar() != null)
                    answer = (int)command.ExecuteScalar();

                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }
        }
        
        //Сверяет логин/пароль, возвращает айди
        public static int CheckUser(MyUser myUser)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            var query = $"SELECT UserId FROM TestUsers WHERE UserLogin = '{myUser.Login}' AND Password = '{myUser.Password}' AND UserActive = 'true' ";
    
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();

               
                int answer = 0;
                if(command.ExecuteScalar() != null)
                    answer = (int)command.ExecuteScalar();

               
                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }
            
        }

        //Поиск номера тэга по имени
        public static int CheckTag(string tag)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";
            
            var query = $"SELECT TagId FROM TestTags WHERE TagName = '{tag}'";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                int answer = 0;
                if (command.ExecuteScalar() != null)
                    answer = (int)command.ExecuteScalar();

                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Имя по номеру
        public static string GetTagById(int tagId)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            var query = $"SELECT TagName FROM TestTags WHERE TagId = {tagId}";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                string answer = command.ExecuteScalar().ToString();

                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Поиск имени пользователя по айди
        public static string GetUserById(int userId)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            var query = $"SELECT UserLogin FROM TestUsers WHERE UserId = {userId}";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                string answer = command.ExecuteScalar().ToString();

                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Добавление тэга в базу
        public static int AddTagToDB(string tag)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                                    + "Integrated Security=SSPI;";
            
            int answer = 0;
            var query = $"INSERT INTO TestTags (TagName) VALUES ('{tag}'); SELECT SCOPE_IDENTITY()";
             
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                
                answer = int.Parse(command.ExecuteScalar().ToString());
                
                command.Dispose();
                connection.Close();
                
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }
        
        //Добавление связи тэг<->письмо
        public static bool AddTagLetter(int tagId, int letterId)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                                    + "Integrated Security=SSPI;";

            var query = $"INSERT INTO TagLetter (TagId, LetterId) VALUES ({tagId},{letterId})";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Добавление письма в базу
        public static int AddMessageToDB(MyMessage myMessage)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            int addressId= FindUser(myMessage.Address);
            int senderId = FindUser(myMessage.Sender);

            var query = $"INSERT INTO TestLetters (Title, AddressId, SenderId, LetterContent, RegisterDate) VALUES ('{myMessage.Title}',{addressId},{senderId},'{myMessage.Content}','{myMessage.Date}'); SELECT SCOPE_IDENTITY()";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                int answer = int.Parse(command.ExecuteScalar().ToString());
                command.Dispose();
                connection.Close();
                return answer;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Получение списка тэгов по номеру письма
        public static List<string> GetTagsForLetter(int letterId)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";


            var query = $"SELECT TagId FROM TagLetter WHERE LetterId = {letterId}";
            List<string> listOfTags = new List<string>();

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listOfTags.Add(GetTagById(reader.GetInt32(0)));
                }
                command.Dispose();
                connection.Close();
                return listOfTags;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }
     
        //Получение списка всех писем, адресованных конкретному пользователю
        public static List<MyMessage> GetMessagesFromDB(int userLogin)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            var query = $"SELECT * FROM TestLetters WHERE AddressId = {userLogin}";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                List<MyMessage> Messages = new List<MyMessage>();
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Messages.Add(new MyMessage(reader.GetInt32(0),reader.GetString(1),GetTagsForLetter(reader.GetInt32(0)), GetUserById(reader.GetInt32(3)), GetUserById(reader.GetInt32(2)), reader.GetSqlDateTime(5).ToString(), reader.GetString(4)));
                }
                command.Dispose();
                connection.Close();
                return Messages;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }

        //Удаление письма по айди
        public static bool DeleteLetterById(int messageId)
        {
            var connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=KHAMessageDB;"
                              + "Integrated Security=SSPI;";

            var query = $"DELETE FROM TestLetters WHERE LetterId = {messageId}";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                return true;
            }
            catch
            {
                throw new Exception("Ошибка подключения к sql server");
            }

        }


    }
}