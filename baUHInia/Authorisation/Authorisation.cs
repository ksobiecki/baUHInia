using baUHInia.MapLogic.View; // Exists for debug purposes only, remove later.
using baUHInia.Playground.View;
using baUHInia.Database;

using System;
using System.Windows.Forms;

namespace baUHInia.Authorisation
{
    public partial class Authorisation : Form
    {
        BazaDanych bazaDanych = BazaDanych.GetBazaDanych();

        public Boolean isMapVisible = false;
        public Authorisation()
        {
            InitializeComponent();
        }

        private void Authorisation_Load(object sender, EventArgs e)
        {
            Form DEBUG = new DEBUG.DEBUG();
            DEBUG.Show();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!isMapVisible)
            {
                Application.Exit();
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            isMapVisible = true;
            Hide();
            //TODO: zmienisz potem :3
            AdminGameWindow app = new AdminGameWindow(null);
            app.Show();
        }

        private void button2_Click_1(object sender, EventArgs e) // Exists for debug purposes only, remove later.
        {
            Hide();
            isMapVisible = true;
            ChoiceWindow app = new ChoiceWindow();
            app.Show();
        }
    }
}
