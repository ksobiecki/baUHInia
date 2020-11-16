using baUHInia.Playground.View;
using System;
using System.Windows.Forms;

namespace baUHInia.Authorisation
{
    public partial class Authorisation : Form
    {
        public Boolean isMapVisible = false;
        public Authorisation()
        {
            InitializeComponent();
        }

        private void Authorisation_Load(object sender, EventArgs e)
        {

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
            UserGameWindow app = new UserGameWindow();
            app.Show();
        }
    }
}
