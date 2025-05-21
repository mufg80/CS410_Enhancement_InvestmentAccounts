using CS410_Enhancement_InvestmentAccounts.Enums;
using CS410_Enhancement_InvestmentAccounts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CS410_Enhancement_InvestmentAccounts.Util
{
    /// <summary>
    /// This class is used to save and load the data from the disk. It uses AES encryption to encrypt the data before saving it to the disk.
    /// It also has a hash method to utilize SHA256 encryption on passwords so that hashes are saved instead of the passwords.
    /// </summary>
    public class FileSaver
    {
        /// <summary>Constant path to encrypted file.</summary>
        private const string PATH = ".\\Models\\Info.txt";


        /// <summary>
        /// Method to write the data to disk. It takes a list of users and a list of accounts and saves them to the disk.
        /// It also saves its encryption key and IV to the disk so that it can be decrypted later.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="accounts"></param>
        public static void WriteToDisk(List<UserModel> users, List<AccountModel> accounts)
        {
            // Method can be called for either case, which ever parameter is empty will be filled with the data from disk. Caller simply 
            // supplies an empty list for the parameter they are not working with. Example Usermodel calls with filled userlist and empty account list.
            (List<AccountModel>, List<UserModel>) items = ReadFromDisk();
            if(users.Count == 0)
            {
                users = items.Item2;
            }
            if (accounts.Count == 0)
            {
                accounts = items.Item1;
            }

            // Perform encryption using AES library.
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateIV();
            aes.GenerateKey();

            using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs))
            {
                // Two helper methods that convert list of models to string. The string is then written to the stream.
                string acc = CreateAccountStringFromModels(accounts);
                string use = CreateUserStringFromModels(users);
                sw.Write(acc);
                sw.Write(use);
                sw.Flush();
            }
            // Finally, the byte array written to the disk is a concatenation of the key, IV and the encrypted data.
            byte[] final = [.. aes.Key, .. aes.IV, .. ms.ToArray()];
            File.WriteAllBytes(PATH, final);


        }

        /// <summary>
        /// Method to read the data from disk. It reads the encrypted file and decrypts it using the AES library.
        /// </summary>
        /// <returns>type="(List<AccountModel>, List<UserModel>)"</returns>
        public static (List<AccountModel>, List<UserModel>) ReadFromDisk()
        {
            // Validate path before beginning.
            if (!Directory.Exists(".\\Models"))
            {
                Directory.CreateDirectory(".\\Models");
            }

            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
                return ([], []);
            }
            // Read bytes and begin performing checks, return empty list if any checks fail.
            try
            {
                byte[] items = File.ReadAllBytes(PATH);
                if (items.Length == 0)
                {
                    return ([], []);
                }

                // Pull out the key and iv.
                byte[] key = items[0..32];
                byte[] iv = items[32..48];
                // Validate key and IV lengths (e.g., 16 bytes for AES-128)
                if (key.Length != 32 || iv.Length != 16)
                {
                    // Handle invalid key or IV (e.g., log error or return)
                    return ([], []);
                }
                // Actual data savaed to items1.
                byte[] items1 = items[48..];
                string itemstring;

                // Perform decryption by saving array into memory stream and using the AES library to decrypt it.
                using (Aes aes = Aes.Create())
                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
                using (MemoryStream ms = new(items1))
                using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new(cs))
                {
                    itemstring = sr.ReadToEnd();
                }

                // Split the string into lines and process each line to create models.Each helper
                // method only processes the lines that are relevant to it.
                var accounts = GetAccountModelsFromString(itemstring);
                var users = GetUserModelsFromString(itemstring);
                return (accounts, users);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a file or console)
                Console.WriteLine($"Error reading from disk: {ex.Message}");
                return ([], []);
            }
        }
        /// <summary>
        /// Helper method to convert the string from the file into a list of user models. The string is split into lines and each line is processed.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>List<UserModel></returns>
        private static List<UserModel> GetUserModelsFromString(string items)
        {
            List<UserModel> result = [];
            string[] lines = items.Split([Environment.NewLine], StringSplitOptions.None);
            foreach (var item in lines)
            {
                string[] itemArray = item.Split('|');
                if (itemArray[0].Equals("1"))
                {
                    if (itemArray.Length == 4)
                    {
                        if(itemArray[1].Trim() == string.Empty || itemArray[2].Trim() == string.Empty || itemArray[3].Trim() == string.Empty)
                        {
                            continue;
                        }
                        UserModel model = new(itemArray[1], itemArray[2].Trim(), bool.Parse(itemArray[3].Trim()));
                        result.Add(model);
                    }
                }
               
            }
            return result;
        }

         
        /// <summary>
        /// Helper method to convert the string from the file into a list of usaccount models. The string is split into lines and each line is processed.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>List<UserModel></returns>
        private static List<AccountModel> GetAccountModelsFromString(string items)
        {
            List<AccountModel> result = [];
            string[] lines = items.Split([Environment.NewLine], StringSplitOptions.None);
            foreach (var item in lines)
            {
                string[] itemArray = item.Split('|');
                if (itemArray[0].Equals("2"))
                {
                    if (itemArray.Length == 3)
                    {
                        AccountModel model = new(itemArray[1], (Option) Enum.Parse(typeof(Option), itemArray[2].Trim()));
                        result.Add(model);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Creates string from list of accounts. String positions 2 at beginning to signal account model.
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns>string</returns>
        private static string CreateAccountStringFromModels(List<AccountModel> accounts)
        {
            if(accounts.Count == 0)
            {
                return string.Empty;
            }
            string result = string.Empty;
            foreach (var item in accounts)
            {
                result += $"2|{item.Name}|{item.Option}" + Environment.NewLine;
            }
            return result;
        }

        /// <summary>
        /// Creates string from list of users. String positions 1 at beginning to signal user model.
        /// </summary>
        /// <param name="users"></param>
        /// <returns>string</returns>
        private static string CreateUserStringFromModels(List<UserModel> users)
        {
            if (users.Count == 0)
            {
                return string.Empty;
            }
            string result = string.Empty;
            foreach (var item in users)
            {
                result += $"1|{item.UserName}|{item.UserHash}|{item.IsAdmin}" + Environment.NewLine;
            }
            return result;
        }

        /// <summary>
        /// Method to hash a string using SHA256. The method takes a string and returns a hashed string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="useBase64"></param>
        /// <returns>string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string HashString(string input, bool useBase64 = false)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            // Convert string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Create SHA-256 instance
            // Compute hash
            //byte[] hashBytes = sha256.ComputeHash(inputBytes);

            byte[] hashBytes = SHA256.HashData(inputBytes);

            // Convert to string (hex or base64)
            if (useBase64)
            {
                return Convert.ToBase64String(hashBytes);
            }
            else
            {
                // Hexadecimal format
                StringBuilder sb = new();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Lowercase hex
                }
                return sb.ToString();
            }
        }

        /// <summary> Method to validate strings in one location to be used in Loginmodel and
        /// UsersModel. The method checks if the strings are not null or empty, if the password and confirm password match,
        /// if the name is unique, if the name and password are longer than 8 characters,
        /// if the name contains only letters and spaces, and if the password contains only letters and numbers.</summary>
        /// <param name="name">The name to check.</param>
        /// <param name="password">The password to check.</param>
        /// <param name="password2">The confirm password to check.</param>
        /// <returns>Boolean</returns>
        public static bool ValidateStrings(string name, string password, string password2)
        {
            if (!string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(password) &&
                !string.IsNullOrEmpty(password2) &&
                password.Equals(password2) && IsUnique(name) &&
                name.Length > 8 &&
                password.Length > 8 &&
                name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)) &&
                password.All(c => char.IsLetter(c) || char.IsNumber(c)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary> Helper method to check uniquness of username so that duplicates are not added to system.</summary>
        /// <param name="name">The name to check for uniqueness.</param>
        private static bool IsUnique(string name)
        {
            FileSaver saver = new();
            var items = FileSaver.ReadFromDisk();

            bool isInDatabase = items.Item2.Any(x => x.UserName.Equals(name));
            if (isInDatabase)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
