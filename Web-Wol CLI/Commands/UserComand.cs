using ConsoleTables;
using Microsoft.Data.Sqlite;
using PromptCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web_Wol.Prompts;

namespace Web_Wol.Commands
{
    /// <summary>
    /// User command for Web-Wol.
    /// </summary>
    internal class UserCommand
    {
        private const int Success = 0;
        private const int Failure = 1;


        /// <summary>
        /// Database path
        /// </summary>
        public string DatabasePath { get; set; }
        /// <summary>
        /// Add switch
        /// </summary>
        public bool IsAdd { get; set; }
        /// <summary>
        /// Delete switch
        /// </summary>
        public bool IsDelete { get; set; }

        public UserCommand() {  }

        /// <summary>
        /// Run the command.
        /// </summary>
        /// 
        /// <returns>
        /// Success code or error code.
        /// </returns>
        public int Run()
        {
            try
            {
                if (IsAdd)
                    Add();
                else if (IsDelete)
                    Delete();
                else
                    List();


                return Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                return Failure;
            }
        }


        /// <summary>
        /// Add a user in the database.
        /// </summary>
        private void Add()
        {
            // Get the value through the prompt
            Prompt prompt = new Prompt();
            User.Add result = prompt.Run<User.Add>();


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            SqliteCommand sqliteCommand = new SqliteCommand("INSERT INTO ww_user (email, password, name) VALUES(@email, @password, @name)", connection);
            sqliteCommand.Parameters.AddWithValue("@email", result.Email);
            sqliteCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(result.Password));
            sqliteCommand.Parameters.AddWithValue("@name", result.Name);
            sqliteCommand.ExecuteReader();
            
            connection.Close();
        }

        /// <summary>
        /// List all the user in the database.
        /// </summary>
        private void List()
        {
            ConsoleTable table = new ConsoleTable("id", "email", "password", "name");


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            SqliteCommand sqliteCommand = new SqliteCommand("SELECT * FROM ww_user", connection);
            SqliteDataReader query = sqliteCommand.ExecuteReader();

            while (query.Read())
                table.AddRow(query.GetInt32(0), query.GetString(1), query.GetString(2), query.GetString(3));


            connection.Close();
            table.Write();
            Console.WriteLine();
        }

        /// <summary>
        /// Delete a user in the database.
        /// </summary>
        private void Delete()
        {
            // Get the value through the prompt
            Prompt prompt = new Prompt();
            User.Delete result = prompt.Run<User.Delete>();


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            SqliteCommand sqliteCommand = new SqliteCommand("DELETE FROM ww_user WHERE id = @id", connection);
            sqliteCommand.Parameters.AddWithValue("@id", int.Parse(result.Id));
            sqliteCommand.ExecuteReader();

            connection.Close();
        }
    }
}
