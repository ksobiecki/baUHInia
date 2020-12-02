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

        private static BazaDanych bazaDanych = new BazaDanych();
        public static BazaDanych GetBazaDanych() { return bazaDanych; }
      
    public int Polacz()
        {
            try
            {
                polaczenie.Open();
            }
            catch (SqlException)
            {
                return 1;
            }
            return 0;
        }

    public int Rozlacz()
        {
            try
            {
                polaczenie.Close();    
            }
            catch (SqlException)
            {
                return 1;
            }
            return 0;
        }

    public int DodajUzytkownika(String nazwa, String haslo, bool is_admim,String pytanie, String odpowiedz)
        {
            int code =0;
            try
            {
                code = CheckUsernameOccupation(nazwa);
                if (code == 0)
                {
                    code = Polacz();
                    int admin = 0;
                    if (is_admim)
                    {
                        admin = 1;
                    }
                    sqlDataAdapter.InsertCommand = new SqlCommand("insert into Uzytkownicy (nazwa,haslo,is_admin,pytanie,odpowiedz) values(@nazwa,@haslo,@admin,@pytanie,@odpowiedz)");
                    sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@haslo", SqlDbType.VarChar).Value = haslo;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@admin", SqlDbType.Bit).Value = admin;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@pytanie",SqlDbType.VarChar).Value = pytanie;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@odpowiedz",SqlDbType.VarChar).Value = odpowiedz;
                    sqlDataAdapter.InsertCommand.Connection = polaczenie;
                    sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                    code = Rozlacz();
                }
            }
            catch (SqlException)
            {
                return code+51;//84 - nazwa uzytkownika zajeta; 51 - blad dodania uzytkownika
            }
            return 0;
        }

        public String PobierzPytanie(String login){
             int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,pytanie from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa){
                        return reader.GetString(1);
                    }
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code+=100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

        public int SprawdzPytanie(String login,String odpowiedz){
         int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,odpowiedz from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                    {
                        if(reader.GetString(1) == odpowiedz)
                            return 200; //odpowiedz prawidolowa
                    }
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code+=100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

  //  public bool UsunUzytkownika()
   //     {
    //        try
    //        {
     //           Polacz();
     //           Rozlacz();
     //       }
      //      catch (SqlException e)
      //      {
      //          return false;
      //      }
      //      return true;
     //   }

        private int CheckUsernameOccupation(String nazwa)
        {
            int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                        return 33; //nazwa uzytkownika zajeta
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code+=100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

        public int CheckUser(String nazwa, String haslo, bool is_admin)
        {
            int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,haslo,is_admin from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa && reader.GetString(1) == haslo)
                    {
                        if(reader.GetBoolean(2) == is_admin)
                        {
                            Rozlacz();
                            return 32; //uzytkownik podal poprawne dane, uzytkownik jest administratorem
                        }
                        else {
                            Rozlacz();
                            return 31; //uzytkownik podal poprawne dane, uzytkownik nie jest administratorem
                        }
                    }
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                return code + 102;//blad 102 = blad pobrania danych uzytkownikow; 103 = blad polaczenia
            }
            return 50; //dane niepoprwane
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

        public String GetGameSerial(int id){   //ewentualnie jakis objekt gry 
            try {
                Polacz();
                String query = "SELECT id,serial from Rozgrywka";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetInt(0) == id)
                        return reader.GetString(1);
                }
                Rozlacz();
            }
            catch(SqlException)
            {
                return 0;
            }
            return  0;
        }
    }
}
