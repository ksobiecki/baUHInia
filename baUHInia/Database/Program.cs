using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using baUHInia.Statistics;



namespace baUHInia.Database
{
    class BazaDanych
    {
        private SqlConnection polaczenie = new SqlConnection("Server=tcp:bauhiniaserver.database.windows.net,1433;Initial Catalog=baUHInia;Persist Security Info=False;User ID=qutlet;Password=hdmi007X;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        private SqlCommand komendaSQL = new SqlCommand();
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
            return code; //0 = git,33 =nazwa zjeta
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
                        Rozlacz();
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

        public Tuple<int, int> CheckUser(string nazwa, string haslo)
        {
            int code = 0;
            int uid = 0;
            try
            {
                code = Polacz();
                String query = "SELECT nazwa,haslo,is_admin,id_user from Uzytkownicy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    if (reader.GetString(0) == nazwa && reader.GetString(1) == haslo)
                    {
                        uid = reader.GetInt32(3);
                        if (reader.GetBoolean(2) == true)
                        {
                            Rozlacz();
                            Console.WriteLine("admin");
                            return new Tuple<int, int>(32, uid); //uzytkownik podal poprawne dane, uzytkownik jest administratorem

                        }
                        else
                        {
                            Rozlacz();
                            Console.WriteLine("nie-admin");
                            return new Tuple<int, int>(31, uid); ; //uzytkownik podal poprawne dane, uzytkownik nie jest administratorem
                        }
                    }
                    uid++;
                }
                code = Rozlacz();
            }
            catch (SqlException)
            {
                return new Tuple<int, int>(code + 102, 0);//blad 102 = blad pobrania danych uzytkownikow; 103 = blad polaczenia
            }
            return new Tuple<int, int>(50, 0); //dane niepoprwane
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
                    {
                        Rozlacz();
                        return 33; //nazwa uzytkownika zajeta
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

        public int DodajMape(int author_id, string nazwa, string map_contents, bool isPublic)
        {
            int code = 0;

            try
            { 
                code = +Polacz();
                sqlDataAdapter.InsertCommand = new SqlCommand("insert into Mapy (id_autora,serial,nazwa,isPublic) values(@autor,@serial,@nazwa,@isPublic)");
                sqlDataAdapter.InsertCommand.Parameters.Add("@autor", SqlDbType.Int).Value = author_id;
                sqlDataAdapter.InsertCommand.Parameters.Add("@serial", SqlDbType.VarChar).Value = map_contents;
                sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                sqlDataAdapter.InsertCommand.Parameters.Add("@isPublic", SqlDbType.Bit).Value = isPublic;
                sqlDataAdapter.InsertCommand.Connection = polaczenie;
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                code = +Rozlacz();
            }
            catch (SqlException)
            {
                return code + 10; //43 nazwa mapy zajeta //(11-122) - blad polaczenia
            }
            return code;
        }

        public int CheckMapNameOccupation(String nazwa, int author_id)
        {
            int code = 0;
            try
            {
                code = Polacz();
                string query = "SELECT nazwa from Mapy where id_autora = @id_autora and isPublic = 0";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@id_autora", author_id);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                    {
                        Rozlacz();
                        return 33; //nazwa mapy zajeta
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

        public int CheckPublishedMapNameOccupation(String nazwa, int author_id)
        {
            int code = 0;
            try
            {
                code = Polacz();
                string query = "SELECT nazwa from Mapy where id_autora = @id_autora and isPublic = 1";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@id_autora", author_id);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                    {
                        Rozlacz();
                        return 33; //nazwa mapy zajeta
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

        public Tuple<int, int, string, bool>[] getMapList()
        {
            List<Tuple<int, int, string,bool>> vs = new List<Tuple<int, int,string, bool>>();

            try
            {
                Polacz();
                string query = "select id_mapy, id_autora, nazwa, isPublic from Mapy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    vs.Add(new Tuple<int, int, string, bool>(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetBoolean(3)));
                }
                Rozlacz();
            }
            catch (SqlException)
            {
                return null;
            }
            return vs.ToArray();
        }

         public Tuple<int, int, string, bool>[] getGameList()
        {
            List<Tuple<int, int, string,bool>> vs = new List<Tuple<int, int, string, bool>>();

            try
            {
                Polacz();
                string query = "select id_gry, id_gracza, nazwa, isPublic from Gry";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    vs.Add(new Tuple<int, int, string, bool>(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetBoolean(3)));

                }
                Rozlacz();
            }
            catch (SqlException)
            {
                return null;
            }
            return vs.ToArray();
        }

        public int GetMapID(string nazwa) //Deprecated
        {
            int id = 0;
            try
            {
                Polacz();
                string query = "select id_mapy from Mapy where nazwa = @nazwa ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@nazwa", nazwa);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                Rozlacz();
            }

            catch (SqlException)
            {
                return 0;
            }
            return id; //jak zero to nie dobrze
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

        public int addGame(int user_id, string nazwa, string game_contents, int map_id, bool isPublic)
        {
            try
            {
                Polacz();
                sqlDataAdapter.InsertCommand = new SqlCommand("insert into Gry (id_gracza,serial,map_id,nazwa,isPublic) values(@gracz,@serial,@mapa,@nazwa,@isPublic)");
                sqlDataAdapter.InsertCommand.Parameters.Add("@gracz", SqlDbType.Int).Value = user_id;
                sqlDataAdapter.InsertCommand.Parameters.Add("@serial", SqlDbType.VarChar).Value = game_contents;
                sqlDataAdapter.InsertCommand.Parameters.Add("@nazwa", SqlDbType.VarChar).Value = nazwa;
                sqlDataAdapter.InsertCommand.Parameters.Add("@mapa", SqlDbType.Int).Value = map_id;
                sqlDataAdapter.InsertCommand.Parameters.Add("@isPublic", SqlDbType.Bit).Value = isPublic;
                sqlDataAdapter.InsertCommand.Connection = polaczenie;
                sqlDataAdapter.InsertCommand.ExecuteNonQuery();
                Rozlacz();
            }
            catch (SqlException)
            {
                return 1;
            }
            return 0;
        }

        public int CheckGameNameOccupation(String nazwa, int gracz)
        {
            int code = 0;
            try
            {
                code = Polacz();
                string query = "SELECT nazwa from Gry where id_gracza = @gracz";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@gracz", gracz);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) == nazwa)
                    {
                        Rozlacz();
                        return 33; //nazwa gry zajeta
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

        public bool GetGame(int ID, ref string jsongame, ref int mapID)
        {
            try
            {
                Polacz();
                string query = "select serial, map_id from Gry where id_gry = @ID ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@ID", ID);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    jsongame = reader.GetString(0);
                    mapID = reader.GetInt32(1);
                }
                Rozlacz();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }

        }

        public int GetScore(string nazwa)
        {
            int punkty = 0;
            try
            {
                Polacz();
                string query = "select wynik from Gry where nazwa = @nazwa ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@nazwa", nazwa);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    punkty = reader.GetInt32(0);
                }
                Rozlacz();
                return punkty;
            }
            catch (SqlException)
            {
                return 0;
            }
        }

        public List<Statistics.Statistics.UserScore> GetUserScores()
        {
            List<Statistics.Statistics.UserScore> userScores = new List<Statistics.Statistics.UserScore>();
            try
            {
                Polacz();
                string query = "select g.wynik,m.nazwa,u.nazwa from Mapy as m,Gry as g,Uzytkownicy as u where g.id_gracza = u.id_user and g.map_id = m.id_mapy";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if(reader.GetInt32(0) != null){
                        int wynik = reader.GetInt32(0);  //wynik
                        string mapa = reader.GetString(1); //nazwa mapy 
                        string user = reader.GetString(2); //nazwa uzytkownika
                        userScores.Add(new Statistics.Statistics.UserScore(mapa, user, wynik.ToString()));
                    }
                }
                Rozlacz();
            }
            catch (SqlException)
            {
                Rozlacz();
                return null;
            }
            return userScores;
        }

        public bool updateMap(int autorID, string json, string mapName)
        {
            try
            {
                Polacz();
                string query = "update Mapy set serial = @json where nazwa = @mapName and id_autora = @autorID and isPublic = 0";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@json", json);
                sqlCommand.Parameters.AddWithValue("@mapName", mapName);
                sqlCommand.Parameters.AddWithValue("@autorID", autorID);
                sqlCommand.ExecuteNonQuery();
                Rozlacz();
                return true;
            }
            catch (SqlException)
            {
                Rozlacz();
                return false;
            }
        }

        public bool updateGame(int palyerID, string json, string gameName, int mapID, bool isPublic)
        {
            try
            {
                Polacz();
                string query = "update Gry set serial = @json, map_id = @map_id, isPublic = @isPublic where nazwa = @gameName and id_gracza = @palyerID";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@json", json);
                sqlCommand.Parameters.AddWithValue("@map_id", mapID);
                sqlCommand.Parameters.AddWithValue("@gameName", gameName);
                sqlCommand.Parameters.AddWithValue("@palyerID", palyerID);
                sqlCommand.Parameters.AddWithValue("@isPublic", isPublic);
                sqlCommand.ExecuteNonQuery();
                Rozlacz();
                return true;
            }
            catch (SqlException)
            {
                Rozlacz();
                return false;
            }
        }

        public int CheckIfTheLoggedInUserIsTheOwnerOfTheMapHeOrSheWantToOverwrite(int loggedInUserID, int mapToOverwriteID)
        {
            try
            {
                Polacz();
                string query = "select id_autora from Mapy where id_mapy = @mapID";
                SqlCommand sqlCommand = new SqlCommand(query,polaczenie);
                sqlCommand.Parameters.AddWithValue("@mapID",mapToOverwriteID);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0).Equals(loggedInUserID))
                    {
                        Rozlacz();
                        return 1; // tak, jest wlascicielem tej mapy
                    } else if(!reader.GetInt32(0).Equals(loggedInUserID))
                    {
                        Rozlacz();
                        return 0; // oj nie byczq to nie twoja mapa
                    }
                }
                Rozlacz();
                return 420; //takiej mapy nie znamy, nie ma jej w bazie
            }
            catch (SqlException)
            {
                Rozlacz();
                return 2137; //zwrocenie 2137 oznacza klopoty
            }
        }

        public bool SetScoreInFinniszedGame(int gameID, int score, int palyerID)
        {
            try
            {
                Polacz();
                string query = "update Gry set wynik = @score where id_gry = @gameName and id_gracza = @palyerID";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@score", score);
                sqlCommand.Parameters.AddWithValue("@gameName", gameID);
                sqlCommand.Parameters.AddWithValue("@palyerID", palyerID);
                sqlCommand.ExecuteNonQuery();
                Rozlacz();
                return true;
            }
            catch (SqlException)
            {
                Rozlacz();
                return false;
            }
        }

        public bool GetMapByID(ref string jsnon, int ID)
        {

            try
            {
                Polacz();
                string query = "select serial from Mapy where id_mapy = @nazwa ";
                SqlCommand sqlCommand = new SqlCommand(query, polaczenie);
                sqlCommand.Parameters.AddWithValue("@nazwa", ID);
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

    }
}
