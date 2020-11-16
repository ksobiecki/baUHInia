using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Authorisation
{
    class LoginData
    {
        private LoginData() { }
        private static LoginData _instance;

        public int UserID { get; set; }
        public String name { get; set; }
        public String hash { get; set; }
        public Boolean isAdmin { get; set; }


        public static LoginData GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoginData();
            }
            return _instance;
        }
    }
}
