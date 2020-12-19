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
        ArrayList scores = new ArrayList();

        string chosenMap;
        string chosenUsername;

        DataGridViewColumn oldColumn;
        SortOrder sortOrder;

        public Statistics()
        {
            InitializeComponent();

            chosenUsername = "All Users";
            chosenMap = "All Maps";

            users.Add("All Users");
            users.Add("user1");
            users.Add("user2");
            users.Add("user3");
            users.Add("user4");
            users.Add("user5");
            foreach (string item in users)
            {
                userSelectBox.Items.Add(item);
                userSelectBox.AutoCompleteCustomSource.Add(item);
            }

            maps.Add("All Maps");
            maps.Add("map1");
            maps.Add("map2");
            maps.Add("map3");
            maps.Add("map4");
            maps.Add("map5");
            foreach (string item in maps)
            {
                mapSelectBox.Items.Add(item);
                mapSelectBox.AutoCompleteCustomSource.Add(item);
            }

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

            mapSelectBox.SelectedIndex = mapSelectBox.Items.IndexOf("All Maps");
            userSelectBox.SelectedIndex = userSelectBox.Items.IndexOf("All Users");

            statsView.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(statsView_ColumnHeaderMouseClick);
            oldColumn = null;
            sortOrder = SortOrder.Ascending;
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

        private void mapSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            chosenMap = cmb.SelectedItem.ToString();
            updateStatsView();
        }

        private void userSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            chosenUsername = cmb.SelectedItem.ToString();
            updateStatsView();
        }

        public void updateStatsView()
        {
            statsView.Rows.Clear();
            foreach (UserScore score in scores)
            {
                if ((score.getMap().Equals(chosenMap) || chosenMap.Equals("All Maps")) && 
                    (score.getUsername().Equals(chosenUsername) || chosenUsername.Equals("All Users")))
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
            if(e != null)
            {
                columnIndex = e.ColumnIndex;
            }
            else
            // Potrzebne przy sortowaniu wywolywanym w funkcji updateStatsView
            {
                columnIndex = 2;
            }
            DataGridViewColumn newColumn = statsView.Columns[columnIndex];
            ListSortDirection direction;

            // Sprawdzam czy wczesniej byla juz sortowana jakas kolumna
            // Uwaga: updateStatsView tez ustawia oldColumn na null, zeby domyslne sortowanie
            // po wyniku dzialalo poprawnie
            if (oldColumn != null)
            {
                // Zmieniam kierunek sortowania
                if (oldColumn == newColumn &&
                    sortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    // Sortuje i usuwam strzalke
                    direction = ListSortDirection.Ascending;
                    sortOrder = SortOrder.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                // Nazwy map i uzytkownikow domyslnie beda sortowane rosnaco, a wyniki malejaco
                if(columnIndex != 2)
                {
                    direction = ListSortDirection.Ascending;
                    sortOrder = SortOrder.Ascending;
                }
                else
                // Potrzebne przy sortowaniu wywolywanym w funkcji updateStatsView
                {
                    direction = ListSortDirection.Descending;
                    sortOrder = SortOrder.Descending;
                }
            }

            oldColumn = newColumn;

            // Ustawiam strzalke w odpowiednia strone
            statsView.Sort(new RowComparer(sortOrder, columnIndex));
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
        }
    }
}
