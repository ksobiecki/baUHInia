using baUHInia.Playground.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.Hide();
            UserGameWindow app = new Playground.View.UserGameWindow();
            app.Show();
        }
    }
}
