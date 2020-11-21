using baUHInia.Playground.View;
using System;
using System.Windows.Forms;

namespace baUHInia.Authorisation
{
    public partial class Authorisation : Form
    {
        public Boolean isClosing = false;
        public Authorisation()
        {
            InitializeComponent();
        }

        private void Authorisation_Load(object sender, EventArgs e)
        {

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                System.Environment.Exit(1);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            isClosing = true;
            Hide();
            UserGameWindow app = new UserGameWindow();
            app.Show();
        }


        private void loginButton_Click_1(object sender, EventArgs e)
        {
            LoginData LD = LoginData.GetInstance();
            LD.UserID = 0;
            LD.name = "defuser";
            LD.isAdmin = true;
            isClosing = true;
            Hide();
            LoggedIn loggedIn = new LoggedIn();
            loggedIn.Show();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {

        }
    }
}
