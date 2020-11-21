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
    public partial class LoggedIn : Form
    {
        public Boolean isClosing = false;
        public LoggedIn()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                System.Environment.Exit(1);
            }

        }

        private void LoggedIn_Load(object sender, EventArgs e)
        {
            LoginData LD = LoginData.GetInstance();
            loginTextBox.Text = LD.name;
            IDtextBox.Text = LD.UserID.ToString();
            if (LD.isAdmin)
            {
                adminRadio.Checked = true;
            }
            else
            {
                userRadio.Checked = true;
            }
        }

        private void logoffButton_Click(object sender, EventArgs e)
        {
            LoginData LD = LoginData.GetInstance();
            LD.flush();
            isClosing = true;
            Hide();
            Authorisation auth = new Authorisation();
            auth.Show();

        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Hide();
            UserGameWindow app = new UserGameWindow();
            app.Show();
        }

        private void statsButton_Click(object sender, EventArgs e)
        {
            //tutaj odpalanie statystyk będzie 
        }
    }
}
