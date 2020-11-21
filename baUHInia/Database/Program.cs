using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;



namespace baUHInia.Database
{
    class BazaDanych
    {
        private SqlConnection polaczenie = new SqlConnection("Server=tcp:bauhiniaserver.database.windows.net,1433;Initial Catalog=baUHInia;Persist Security Info=False;User ID=qutlet;Password=hdmi007X;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        private SqlCommand komendaSQL= new SqlCommand();
        private SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        //private SqlDataReader sqlDataReader;
        //private int affectedLines = 0;

        private static BazaDanych bazaDanych = new BazaDanych();
        public static BazaDanych GetBazaDanych() { return bazaDanych; }
      
    public bool Polacz()
        {
            try
            {
                polaczenie.Open();
                
            }
            catch (SqlException)
            {
                
                return false;
            }
            return true;
        }

    public bool Rozlacz()
        {
            try
            {
                polaczenie.Close();
                
            }
            catch (SqlException)
            {
                
                return false;
            }
            return true;
        }

    public bool DodajUzytkownika(String nazwa, String haslo, bool is_admim)
        {
            
            try
            {
                if (SprawdzUzytkownika(nazwa))
                {
                    Polacz();
                    int admin = 0;
                    if (is_admim)
                    {
                        admin = 1;
                    }
                sqlDataAdapter.InsertCommand = new SqlCommand("insert into Uzytkownicy (nazwa,haslo,is_admin) values(@nazwa,@haslo,@admin)");
                sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                sqlDataAdapter.InsertCommand.Parameters.Add("@haslo", SqlDbType.VarChar).Value = haslo;
                sqlDataAdapter.InsertCommand.Parameters.Add("@admin", SqlDbType.Bit).Value = admin;
                sqlDataAdapter.InsertCommand.Connection = polaczenie;
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();

            }
                Rozlacz();
            }
            catch (SqlException)
            {
                Rozlacz();
                return false;
            }
            return true;
        }

    public bool UsunUzytkownika()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool SprawdzUzytkownika(String nazwa)
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool DodajMape()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool UsunMape()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool SprawdzMape()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool DodajObiekt()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool UsunObiekt()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool SprawdzObiekt()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool PobierzObiekt()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool PobierzMape()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

        public bool PobierzUzytkownika()
        {
            try
            {
                Polacz();
                Rozlacz();
            }
            catch (SqlException e)
            {
                return false;
            }
            return true;
        }

    }
}
