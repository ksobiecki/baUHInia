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



        public int DodajUzytkownika(String nazwa, String haslo, bool is_admim, String pytanie, String odpowiedz)
        {
            int code = 0;
            try
            {
                code = CheckUsernameOccupation(nazwa);
                if (code == 0)
                {
                    code = +Polacz();
                    int admin = 0;
                    if (is_admim)
                    {
                        admin = 1;
                    }
                    sqlDataAdapter.InsertCommand = new SqlCommand("insert into Uzytkownicy (nazwa,haslo,is_admin,pytanie,odpowiedz) values(@nazwa,@haslo,@admin,@pytanie,@odpowiedz)");
                    sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@haslo", SqlDbType.VarChar).Value = haslo;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@admin", SqlDbType.Bit).Value = admin;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@pytanie", SqlDbType.VarChar).Value = pytanie;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@odpowiedz", SqlDbType.VarChar).Value = odpowiedz;
                    sqlDataAdapter.InsertCommand.Connection = polaczenie;
                    sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                    code = +Rozlacz();
                }
            }
            catch (SqlException)
            {
                code = +Rozlacz();
                return code + 51;//84 - nazwa uzytkownika zajeta; 51 - blad dodania uzytkownika; //52 - blad polaczenia
            }
            code = +Rozlacz();
            return 33; //nazwa zajtea
        }

        public Tuple<int, String> PobierzPytanie(String login)
        {
            int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,pytanie from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == login)
                    {
                        return new Tuple<int, String>(code, reader.GetString(1));
                    }
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code += 100;
                return new Tuple<int, String>(code, "ERROR"); //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return new Tuple<int, String>(code, "null"); ;
        }

        public int SprawdzPytanie(String login, String odpowiedz)
        {
            int code = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,odpowiedz from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(1) == odpowiedz)
                    {
                        return 200; //odpowiedz prawidolowa
                    }
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code += 100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

        public int newPasssword(String login, String nowehaslo)
        {
            int code = 0;
            try
            {
                code = Polacz();
                String query = "update Uzytkownicy set haslo=@newPass where nazwa=@login";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@newPass", nowehaslo);
                sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.ExecuteNonQuery();
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code += 100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }


        //public LoginData getUser(string login)
        //{
        //    LoginData loginData = null;
        //    try
        //    {
        //        Polacz();
        //        String query = "select id_user,nazwa,haslo,is_admin from Uzytkownicy where nazwa=@login";
        //        SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
        //        sqlCommand.Parameters.AddWithValue("@login", login);
        //        SqlDataReader reader = sqlCommand.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            loginData = new LoginData(reader.GetInt64(0), reader.GetString(1), reader.getString(2), reader.GetBoolean(3));
        //        }
        //        Rozlacz();
        //    }
        //    catch (SqlException)
        //    {
        //        return null; 
        //    }
        //    return loginData;
        //}

        public int CheckUser(string nazwa, string haslo)
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
                        if (reader.GetBoolean(2) == true)
                        {
                            Rozlacz();
                            return 32; //uzytkownik podal poprawne dane, uzytkownik jest administratorem
                        }
                        else
                        {
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

        private int CheckUsernameOccupation(String nazwa)
        {
            int code = 0;
            try
            {
                code = Polacz();
                string query = "SELECT nazwa from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                        Rozlacz();
                    return 33; //nazwa uzytkownika zajeta
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                code += 100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

        public int DodajMape(int author_id, string nazwa, string map_contents)
        {
            int code = 0;
            try
            {
                code = CheckMapNameOccupation(nazwa);
                if (code == 0)
                {
                    code = +Polacz();
                    sqlDataAdapter.InsertCommand = new SqlCommand("insert into Mapy (id_autora,serial,nazwa) values(@autor,@serial,@nazwa)");
                    sqlDataAdapter.InsertCommand.Parameters.Add("@autor", SqlDbType.Int).Value = author_id;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@serial", SqlDbType.VarChar).Value = map_contents;
                    sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                    sqlDataAdapter.InsertCommand.Connection = polaczenie;
                    sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                    code = +Rozlacz();
                }
            }
            catch (SqlException)
            {
                return code + 10; //43 nazwa mapy zajeta //(11-122) - blad polaczenia
            }
            return 0;
        }

        private int CheckMapNameOccupation(String nazwa)
        {
            int code = 0;
            try
            {
                code = Polacz();
                string query = "SELECT nazwa from Mapy";
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
                code += 100;
                return code; //blad 100 = blad pobrania nazwy uzytkownika; 101 = blad polaczenia
            }
            return 0;
        }

        public string[] getMapNames()
        {
            string[] vs = null;
            int index = 0;
            try
            {
                Polacz();
                string query = "select nazwa from Mapy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    vs[index] = reader.GetString(0);
                    index++;
                }
                Rozlacz();
            }
            catch (SqlException)
            {

            }
            return vs;
        }

        public bool GetMap(ref string jsnon, string nazwa)
        {

            try
            {
                Polacz();
                string query = "select serial from Mapy where nazwa = @nazwa ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@nazwa", nazwa);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    jsnon = reader.GetString(0);
                }
                Rozlacz();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public bool addGame(int user_id, string game_contents, int map_id)
        {
            try
            {
                Polacz();
                sqlDataAdapter.InsertCommand = new SqlCommand("insert into Gry (id_gracz,serial,map_id) values(@gracz,@serial,@mapa)");
                sqlDataAdapter.InsertCommand.Parameters.Add("@gracz", SqlDbType.Int).Value = user_id;
                sqlDataAdapter.InsertCommand.Parameters.Add("@serial", SqlDbType.VarChar).Value = game_contents;
                sqlDataAdapter.InsertCommand.Parameters.Add("@mapa", SqlDbType.Int).Value = map_id;
                sqlDataAdapter.InsertCommand.Connection = polaczenie;
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                Rozlacz();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public bool GetGame(string nazwa, int gracz, ref string jsongame)
        {
            try
            {
                Polacz();
                string query = "select serial from Mapy where nazwa = @nazwa ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@nazwa", nazwa);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    jsongame = reader.GetString(0);
                }
                Rozlacz();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }

        }
    }
}