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

        public UserModel(string name, string hash)
        {
            UserName = name;
            UserHash = hash;
        }
    }
}
