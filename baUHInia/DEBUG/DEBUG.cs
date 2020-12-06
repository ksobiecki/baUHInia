using baUHInia.MapLogic.View;
using baUHInia.Playground.View;
using System;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using baUHInia.Database;

namespace baUHInia.DEBUG
{
    public partial class DEBUG : Form
    {
        public DEBUG()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            AdminGameWindow app = new AdminGameWindow(null);
            //Formsy psują wprowadzanie tekstu. Poniższa linijka jest niezbędna
            ElementHost.EnableModelessKeyboardInterop(app);
            app.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ChoiceWindow app = new ChoiceWindow();
            app.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BazaDanych bd = BazaDanych.GetBazaDanych();
            bd.DodajUzytkownika("test12345aaa", "qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq", true, "a", "a");
            bd.CheckUser("test12345aaa", "qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq");
        }
    }
}
