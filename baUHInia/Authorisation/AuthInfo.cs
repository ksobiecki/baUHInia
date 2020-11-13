using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Authorisation
{
    class AuthInfo
    {
        private AuthInfo() { }
        private static AuthInfo _instance;

        public int UserID { get; set; }
        public String name { get; set; }
        public String hash { get; set; }
        public Boolean isAdmin { get; set; }


        public static AuthInfo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AuthInfo();
            }
            return _instance;
        }
    }
}
