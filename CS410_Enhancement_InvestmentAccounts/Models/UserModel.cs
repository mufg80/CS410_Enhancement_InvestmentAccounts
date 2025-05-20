using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string UserHash { get; set; }
        public bool IsAdmin { get; set; }

        public UserModel(string name, string hash, bool admin)
        {
            UserName = name;
            UserHash = hash;
            IsAdmin = admin;
        }
    }
}
