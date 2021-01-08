using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Authorisation
{
    public class LoginData
    {
        public LoginData() { }
        private static LoginData _instance;

        public int UserID { get; set; }
        //bardzo prosiłbym o nie posługiwanie się UserID bo nie ufam tej wartości XD
        //lepiej bierzcie po loginie niżej.
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
