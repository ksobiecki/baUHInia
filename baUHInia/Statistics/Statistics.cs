using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace baUHInia.Statistics
{
    public partial class Statistics : Form
    {
        ArrayList users = new ArrayList();
        ArrayList maps = new ArrayList();
        List<UserScore> scores = new List<UserScore>();

        string chosenMap;
        string chosenUsername;

        DataGridViewColumn oldColumn;
        SortOrder sortOrder;

        public Statistics()
        {
            InitializeComponent();

            chosenUsername = "Wszyscy Użytkownicy";
            chosenMap = "Wszystkie Mapy";


            // setCustomData();
            getDataFromDatabase();

            foreach (string item in users)
            {
                userSelectBox.Items.Add(item);
                userSelectBox.AutoCompleteCustomSource.Add(item);
            }

            foreach (string item in maps)
            {
                mapSelectBox.Items.Add(item);
                mapSelectBox.AutoCompleteCustomSource.Add(item);
            }

            oldColumn = null;
            mapSelectBox.SelectedIndex = mapSelectBox.Items.IndexOf("Wszystkie Mapy");
            userSelectBox.SelectedIndex = userSelectBox.Items.IndexOf("Wszyscy Użytkownicy");

            statsView.RowHeadersVisible = false;
            statsView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(statsView_ColumnHeaderMouseClick);
            sortOrder = SortOrder.Ascending;

            statsView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statsView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statsView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statsView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statsView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            statsView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            groupBox1.Click += new EventHandler(groupBox1_OnClick);
        }

        public class UserScore
        {
            string username;
            string map;
            string score;

            public UserScore(string map, string username, string score)
            {
                this.username = username;
                this.map = map;
                this.score = score;
            }
            public UserScore()
            {
                this.username = "";
                this.map = "";
                this.score = "";
            }

            public string getUsername()
            {
                return this.username;
            }
            public string getMap()
            {
                return this.map;
            }
            public string getScore()
            {
                return this.score;
            }
        }

        private void Statistics_Load(object sender, EventArgs e)
        {

        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            statsView.ClearSelection();
            groupBox1.Focus();
        }

        private void mapSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            chosenMap = cmb.SelectedItem.ToString();
            updateStatsView();
            statsView.ClearSelection();
        }

        private void userSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            chosenUsername = cmb.SelectedItem.ToString();
            updateStatsView();
            statsView.ClearSelection();
        }

        public void updateStatsView()
        {
            statsView.Rows.Clear();
            foreach (UserScore score in scores)
            {
                if ((score.getMap().Equals(chosenMap) || chosenMap.Equals("Wszystkie Mapy")) && 
                    (score.getUsername().Equals(chosenUsername) || chosenUsername.Equals("Wszyscy Użytkownicy")))
                {
                    statsView.Rows.Add(score.getMap(), score.getUsername(), score.getScore());
                }
            }
            oldColumn = null;
            statsView_ColumnHeaderMouseClick(null, null);
        }

        private class RowComparer : System.Collections.IComparer
        {
            private static int sortOrderModifier = 1;
            private static int columnNumber;

            public RowComparer(SortOrder sortOrder, int columnNr)
            {
                columnNumber = columnNr;
                if (sortOrder == SortOrder.Descending)
                {
                    sortOrderModifier = -1;
                }
                else if (sortOrder == SortOrder.Ascending)
                {
                    sortOrderModifier = 1;
                }
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

                int CompareResult = System.String.Compare(
                    DataGridViewRow1.Cells[columnNumber].Value.ToString(),
                    DataGridViewRow2.Cells[columnNumber].Value.ToString());

                // Jesli wybrano sortowanie po mapie lub uzytkowniku, w drugiej kolejnosci sortuje po wyniku
                if (CompareResult == 0 && columnNumber != 2)
                {
                    CompareResult = System.String.Compare(
                        DataGridViewRow1.Cells[2].Value.ToString(),
                        DataGridViewRow2.Cells[2].Value.ToString());
                    return CompareResult * -1;
                }
                return CompareResult * sortOrderModifier;
            }
        }

        private void statsView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnIndex;
            // Uzytkownik kliknal na ktorys naglowek, zeby zmienic sortowanie
            if(e != null)
            {
                columnIndex = e.ColumnIndex;
            }
            else
            // Uzytkownik zmienil filtrowanie lub okno statystyk zostalo otwarte
            // Potrzebne przy sortowaniu wywolywanym w funkcji updateStatsView
            {
                columnIndex = 2;
            }
            DataGridViewColumn newColumn = statsView.Columns[columnIndex];

            // Uzytkownik zmienil kierunek sortowania lub sortowana kolumne
            if (oldColumn != null)
            {
                // Zmieniam kierunek sortowania na malejacy
                if (oldColumn == newColumn && sortOrder == SortOrder.Ascending)
                {
                    sortOrder = SortOrder.Descending;
                }
                // Kierunek byl ustawiony na malejacy, albo kolumna zostala zmieniona
                else
                {
                    sortOrder = SortOrder.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            // Okno wlasnie zostalo otwarte lub uzytkownik zmienil filtrowanie
            // (updateStatsView ustawia oldColumn na null)
            else
            {
                // Tutaj kierunek sortowania zawsze dotyczy kolumny z wynikami (bo columnIndex = 2)
                sortOrder = SortOrder.Descending;

                // Czyszczenie strzalek, jesli byly ustawione na innej kolumnie niz ostatnia
                statsView.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.None;
                statsView.Columns[1].HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            oldColumn = newColumn;

            // Sortuje i ustawiam strzalke w odpowiednia strone
            statsView.Sort(new RowComparer(sortOrder, columnIndex));
            newColumn.HeaderCell.SortGlyphDirection =
                sortOrder == SortOrder.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
        }

        private void getDataFromDatabase()
        {
            Database.BazaDanych bazaDanych = new Database.BazaDanych();
            scores = bazaDanych.GetUserScores();
            foreach(UserScore score in scores)
            {
                if(!maps.Contains(score.getMap()))
                {
                    maps.Add(score.getMap());
                }
            }
            maps.Sort();
            maps.Insert(0, "Wszystkie Mapy");

            foreach(UserScore score in scores)
            {
                if(!users.Contains(score.getUsername()))
                {
                    users.Add(score.getUsername());
                }
            }
            users.Sort();
            users.Insert(0, "Wszyscy Użytkownicy");
        }

        private void setCustomData()
        {
            users.Add("Wszyscy Użytkownicy");
            users.Add("user1");
            users.Add("user2");
            users.Add("user3");
            users.Add("user4");
            users.Add("user5");

            maps.Add("Wszystkie Mapy");
            maps.Add("map1");
            maps.Add("map2");
            maps.Add("map3");
            maps.Add("map4");
            maps.Add("map5");

            scores.Add(new UserScore("map1", "user1", "300"));
            scores.Add(new UserScore("map2", "user1", "420"));
            scores.Add(new UserScore("map3", "user1", "200"));
            scores.Add(new UserScore("map4", "user1", "350"));
            scores.Add(new UserScore("map5", "user1", "250"));

            scores.Add(new UserScore("map2", "user2", "360"));
            scores.Add(new UserScore("map4", "user2", "420"));
            scores.Add(new UserScore("map5", "user2", "300"));

            scores.Add(new UserScore("map2", "user3", "340"));
            scores.Add(new UserScore("map4", "user3", "405"));
            scores.Add(new UserScore("map5", "user3", "335"));

            scores.Add(new UserScore("map3", "user4", "280"));
            scores.Add(new UserScore("map4", "user4", "310"));

            scores.Add(new UserScore("map1", "user5", "460"));
            scores.Add(new UserScore("map2", "user5", "440"));
            scores.Add(new UserScore("map3", "user5", "420"));
            scores.Add(new UserScore("map4", "user5", "360"));
            scores.Add(new UserScore("map5", "user5", "360"));
        }

        // Do testow funkcji getDataFromDatabase()
        private void setCustomScores()
        {
            scores.Add(new UserScore("map1", "user1", "300"));
            scores.Add(new UserScore("map2", "user1", "420"));
            scores.Add(new UserScore("map3", "user1", "200"));
            scores.Add(new UserScore("map4", "user1", "350"));
            scores.Add(new UserScore("map5", "user1", "250"));

            scores.Add(new UserScore("map2", "user2", "360"));
            scores.Add(new UserScore("map4", "user2", "420"));
            scores.Add(new UserScore("map5", "user2", "300"));

            scores.Add(new UserScore("map2", "user3", "340"));
            scores.Add(new UserScore("map4", "user3", "405"));
            scores.Add(new UserScore("map5", "user3", "335"));

            scores.Add(new UserScore("map3", "user4", "280"));
            scores.Add(new UserScore("map4", "user4", "310"));

            scores.Add(new UserScore("map1", "user5", "460"));
            scores.Add(new UserScore("map2", "user5", "440"));
            scores.Add(new UserScore("map3", "user5", "420"));
            scores.Add(new UserScore("map4", "user5", "360"));
            scores.Add(new UserScore("map5", "user5", "360"));
        }

        private void groupBox1_OnClick(Object sender, EventArgs e)
        {
            statsView.ClearSelection();
            groupBox1.Focus();
        }

    }
}
