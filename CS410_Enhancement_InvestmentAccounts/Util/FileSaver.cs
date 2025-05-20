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

namespace CS410_Enhancement_InvestmentAccounts.Util
{
    public class FileSaver
    {
        private const string PATH = ".\\Models\\Info.txt";

       

        public void WriteToDisk(List<UserModel> users, List<AccountModel> accounts)
        {
            (List<AccountModel>, List<UserModel>) items = ReadFromDisk();
            if(users.Count() == 0)
            {
                users = items.Item2;
            }
            if (accounts.Count() == 0)
            {
                accounts = items.Item1;
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateIV();
                aes.GenerateKey();


                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                string acc = CreateAccountStringFromModels(accounts);
                                string use = CreateUserStringFromModels(users);
                                sw.Write(acc);
                                sw.Write(use);
                                sw.Flush();
                            }
                           
                            byte[] final = aes.Key.Concat(aes.IV).Concat(ms.ToArray()).ToArray();
                            File.WriteAllBytes(PATH, final);
                        }
                    }
                }
            }


        }


        public (List<AccountModel>, List<UserModel>) ReadFromDisk()
        {
            if (!Directory.Exists(".\\Models"))
            {
                Directory.CreateDirectory(".\\Models");
            }

            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
                return (new List<AccountModel>(), new List<UserModel>());
            }

            try
            {
                byte[] items = File.ReadAllBytes(PATH);
                if (items.Length == 0)
                {
                    return (new List<AccountModel>(), new List<UserModel>());
                }


                byte[] key = items[0..32];
                byte[] iv = items[32..48];
                // Validate key and IV lengths (e.g., 16 bytes for AES-128)
                if (key.Length != 32 || iv.Length != 16)
                {
                    // Handle invalid key or IV (e.g., log error or return)
                    return (new List<AccountModel>(), new List<UserModel>());
                }

                byte[] items1 = items[48..];
                string itemstring;

                using (Aes aes = Aes.Create())
                using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
                using (MemoryStream ms = new MemoryStream(items1))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    itemstring = sr.ReadToEnd();
                }
                var accounts = GetAccountModelsFromString(itemstring);
                var users = GetUserModelsFromString(itemstring);
                return (accounts, users);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a file or console)
                Console.WriteLine($"Error reading from disk: {ex.Message}");
                return (new List<AccountModel>(), new List<UserModel>());
            }
        }

        private List<UserModel> GetUserModelsFromString(string items)
        {
            List<UserModel> result = new List<UserModel>();
            string[] lines = items.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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
                        UserModel model = new UserModel(itemArray[1], itemArray[2].Trim(), bool.Parse(itemArray[3].Trim()));
                        result.Add(model);
                    }
                }
               
            }
            return result;
        }

        private List<AccountModel> GetAccountModelsFromString(string items)
        {
            List<AccountModel> result = new List<AccountModel>();
            string[] lines = items.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var item in lines)
            {
                string[] itemArray = item.Split('|');
                if (itemArray[0].Equals("2"))
                {
                    if (itemArray.Length == 3)
                    {
                        AccountModel model = new AccountModel(itemArray[1], (Option) Enum.Parse(typeof(Option), itemArray[2].Trim()));
                        result.Add(model);
                    }
                }

            }
            return result;
        }
        private string CreateAccountStringFromModels(List<AccountModel> accounts)
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

        private string CreateUserStringFromModels(List<UserModel> users)
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

        public static string HashString(string input, bool useBase64 = false)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            // Convert string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Create SHA-256 instance
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute hash
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert to string (hex or base64)
                if (useBase64)
                {
                    return Convert.ToBase64String(hashBytes);
                }
                else
                {
                    // Hexadecimal format
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2")); // Lowercase hex
                    }
                    return sb.ToString();
                }
            }
        }

    }
}
