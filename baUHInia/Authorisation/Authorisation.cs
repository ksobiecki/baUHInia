using baUHInia.MapLogic.View; // Exists for debug purposes only, remove later.
using baUHInia.Playground.View;
using baUHInia.Database;

using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace baUHInia.Authorisation
{
    public partial class Authorisation : Form
    {
        BazaDanych bazaDanych = BazaDanych.GetBazaDanych();
        public int secVal = 5;

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

        private void loginButton_Click(object sender, EventArgs e)
        {
            SHA256 mySHA256 = SHA256.Create();
            String login;
            String passwd;
            login = loginLoginBox.Text;
            if (loginLoginBox.Text.Length == 0)
            {
                MessageBox.Show("Proszę podać login.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (loginPasswordBox.Text.Length == 0)
            {
                MessageBox.Show("Wprowadzono puste hasło.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            passwd = ComputeSha256Hash(loginPasswordBox.Text);
            Console.WriteLine(login);
            Console.WriteLine(passwd);

            if (true)
            {
                var result = MessageBox.Show("Pomyślnie zalogowano.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LoginData ld = LoginData.GetInstance();
                    ld.UserID = 0;
                    ld.isAdmin = true;
                    ld.name = login;
                    ld.hash = passwd;
                    isMapVisible = true;
                    Hide();
                    AdminGameWindow app = new AdminGameWindow(null);
                    app.Show();
                }
            }
            else
            {
                MessageBox.Show("Wprowadzone dane nie są poprawne.\nIlość prób do zablokowania systemu: " + secVal, "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginLoginBox.Text = "";
                loginPasswordBox.Text = "";
                return;
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            SHA256 mySHA256 = SHA256.Create();
            String login;
            String passwd;
            String passwd2;
            login = registerLoginBox.Text;
            passwd = registerPasswordBox.Text;
            passwd2 = registerSecondPasswordBox.Text;
            //czy w ogóle coś wpisali:
            if (registerLoginBox.Text.Length == 0)
            {
                MessageBox.Show("Proszę podać login.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (registerLoginBox.Text.Length < 5)
            {
                MessageBox.Show("Login musi posiadać co najmniej 5 znaków.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (registerPasswordBox.Text.Length == 0)
            {
                MessageBox.Show("Wprowadzono puste hasło.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //czy takie samo:
            if (passwd != passwd2)
            {
                MessageBox.Show("Wprowadzone hasła nie są takie same.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                registerLoginBox.Text = "";
                registerPasswordBox.Text = "";
                registerSecondPasswordBox.Text = "";
                return;
            }
            //czy ma cyfre:
            if ((!passwd.Any(c => char.IsDigit(c))) || (!passwd.Any(c => char.IsLower(c))) || (!passwd.Any(c => char.IsUpper(c))) || (passwd.Any(c => char.IsWhiteSpace(c))) || (passwd.Length < 8))
            {
                MessageBox.Show("Wprowadzone hasło:\n1. Musi zawierać co najmniej jedną cyfrę\n2. Musi zawierać co najmniej jedną wielką literę\n3. Musi zawierać co najmniej jedną małą literę\n4. Musi zawierać co najmniej 8 znaków\n5. Nie może zawierać spacji.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                registerPasswordBox.Text = "";
                registerSecondPasswordBox.Text = "";
                return;
            }

            passwd = ComputeSha256Hash(registerPasswordBox.Text);
            Console.WriteLine(login);
            Console.WriteLine(passwd);
            Boolean Admin = false;
            if (isAdmin.Checked)
            {
                Admin = true;
            }
            if (true)
            {
                var result = MessageBox.Show("Pomyślnie zalogowano.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    LoginData ld = LoginData.GetInstance();
                    ld.UserID = 0;
                    ld.isAdmin = true;
                    ld.name = login;
                    ld.hash = passwd;
                    isMapVisible = true;
                    Hide();
                    AdminGameWindow app = new AdminGameWindow(null);
                    app.Show();
                }
            }
            else
            {
                MessageBox.Show("Użytkownik z wybranym loginem już istnieje w systemie.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                registerLoginBox.Text = "";
                registerPasswordBox.Text = "";
                registerSecondPasswordBox.Text = "";
                return;
            }
        }
        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void registerLoginBox_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 5000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("Login musi zawierać co najmniej 5 znaków", TB, 138, 0, VisibleTime);
        }

        private void registerPasswordBox_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 8000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("Wprowadzone hasło:\n1. Musi zawierać co najmniej jedną cyfrę\n2. Musi zawierać co najmniej jedną wielką literę\n3. Musi zawierać co najmniej jedną małą literę\n4. Musi zawierać co najmniej 8 znaków\n5. Nie może zawierać spacji.", TB, 138, 0, VisibleTime);
        }


        private void recoverPasswdBttn_Click(object sender, EventArgs e)
        {
            if (loginLoginBox.Text.Length == 0)
            {
                MessageBox.Show("Proszę podać login.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

    }
}
