using ConsoleTables;
using Microsoft.Data.Sqlite;
using PromptCLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web_Wol.Prompts;

namespace Web_Wol.Commands
{
    /// <summary>
    /// Device command for Web-Wol.
    /// </summary>
    internal class DeviceCommand
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

        public DeviceCommand() {  }

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
        /// Add a device in the database.
        /// </summary>
        private void Add()
        {
            // Get the value through the prompt
            Prompt prompt = new Prompt();
            Device.Add result = prompt.Run<Device.Add>();
            string email = null;


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            // Get the email
            SqliteCommand sqliteCommand = new SqliteCommand("SELECT email FROM ww_user WHERE id = @id", connection);
            sqliteCommand.Parameters.AddWithValue("@id", int.Parse(result.IdWwUser));
            SqliteDataReader query = sqliteCommand.ExecuteReader();
            while (query.Read())
                email = query.GetString(0);

            // Insert the device
            sqliteCommand = new SqliteCommand("INSERT INTO ww_device (id_ww_user, name, type, control, interface, mac, remote_type, remote_login, remote_password, status) VALUES(@id_ww_user, @name, @type, @control, @interface, @mac, @remote_type, @remote_login, @remote_password, @status)", connection);
            sqliteCommand.Parameters.AddWithValue("@id_ww_user", int.Parse(result.IdWwUser));
            sqliteCommand.Parameters.AddWithValue("@name", result.Name);
            sqliteCommand.Parameters.AddWithValue("@type", result.Type == "Linux" ? 1 : result.Type == "MacOS" ? 2 : 4);
            sqliteCommand.Parameters.AddWithValue("@control", result.Control == "True" ? 1 : 0);
            sqliteCommand.Parameters.AddWithValue("@interface", result.Interface);
            sqliteCommand.Parameters.AddWithValue("@mac", result.MAC);
            sqliteCommand.Parameters.AddWithValue("@remote_type", result.RemoteType);
            sqliteCommand.Parameters.AddWithValue("@remote_login", result.RemoteLogin);
            sqliteCommand.Parameters.AddWithValue("@remote_password", Secure(result.RemotePassword, email));
            sqliteCommand.Parameters.AddWithValue("@status", result.Control == "True" ? 0 : -1);
            sqliteCommand.ExecuteReader();
            
            connection.Close();
        }

        /// <summary>
        /// List all the device in the database.
        /// </summary>
        private void List()
        {
            ConsoleTable table = new ConsoleTable("id", "id_ww_user", "name", "type", "control", "interface", "mac");


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            SqliteCommand sqliteCommand = new SqliteCommand("SELECT * FROM ww_device", connection);
            SqliteDataReader query = sqliteCommand.ExecuteReader();

            while (query.Read())
                table.AddRow(query.GetInt32(0),
                    query.GetInt32(1),
                    query.GetString(2),
                    query.GetInt32(3),
                    query.GetInt32(4),
                    query.GetString(5),
                    query.GetString(6)
                );


            connection.Close();
            table.Write();
            Console.WriteLine();
        }

        /// <summary>
        /// Delete a device in the database.
        /// </summary>
        private void Delete()
        {
            // Get the value through the prompt
            Prompt prompt = new Prompt();
            Device.Delete result = prompt.Run<Device.Delete>();


            using SqliteConnection connection = new SqliteConnection($"Filename={DatabasePath}");
            connection.Open();

            SqliteCommand sqliteCommand = new SqliteCommand("DELETE FROM ww_device WHERE id = @id", connection);
            sqliteCommand.Parameters.AddWithValue("@id", int.Parse(result.Id));
            sqliteCommand.ExecuteReader();

            connection.Close();
        }

        /// <summary>
        /// Secure a bunch of string data.
        /// </summary>
        /// 
        /// <param name="dataToEncrypt">Data to encrypt as string</param>
        /// <param name="secret">Secret to use to encrypt the data</param>
        /// 
        /// <returns>
        /// Data encrypted
        /// </returns>
        private string Secure(string dataToEncrypt, string secret)
        {
            byte[] encrypted;

            // Create an Aes object with the specified key and IV.
            using Aes aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
            aes.IV = MD5.HashData(Encoding.UTF8.GetBytes(secret));
            aes.Mode = CipherMode.CBC;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for encryption.
            using (MemoryStream ms = new MemoryStream())
            {
                using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (StreamWriter sw = new StreamWriter(cs))
                sw.Write(dataToEncrypt);

                encrypted = ms.ToArray();
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }
    }
}
