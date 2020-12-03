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
        BazaDanych bazaDanych = new BazaDanych();
        public Boolean isMapVisible = false;
        public String pytanie;
        public String odpowiedz;
        public String nowehaslo;
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
            int resultcheck = bazaDanych.CheckUser(login, passwd);
            if (resultcheck == 32)
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
            if (resultcheck == 31)
            {
                var result = MessageBox.Show("Pomyślnie zalogowano.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LoginData ld = LoginData.GetInstance();
                    ld.UserID = 0;
                    ld.isAdmin = false;
                    ld.name = login;
                    ld.hash = passwd;
                    isMapVisible = true;
                    Hide();
                    AdminGameWindow app = new AdminGameWindow(null);
                    app.Show();
                }
            }
            else if (resultcheck == 102)
            {
                MessageBox.Show("Błąd pobierania danych użytkowników", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginLoginBox.Text = "";
                loginPasswordBox.Text = "";
                return;
            }
            else if (resultcheck == 103)
            {
                MessageBox.Show("Błąd połączenia", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginLoginBox.Text = "";
                loginPasswordBox.Text = "";
                return;
            }
            else 
            {
                MessageBox.Show("Nieznany błąd", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ShowAddQuestionDialog();
            int resultadding = bazaDanych.DodajUzytkownika(login, passwd, Admin, pytanie, odpowiedz);
            if (resultadding == 0)
            {
                var result = MessageBox.Show("Pomyślnie utworzono konto.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Question);
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
            else if (resultadding == 84)
            {
                MessageBox.Show("Użytkownik z wybranym loginem już istnieje w systemie.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                registerLoginBox.Text = "";
                registerPasswordBox.Text = "";
                registerSecondPasswordBox.Text = "";
                return;
            }
            else if (resultadding == 51)
            {
                MessageBox.Show("Błąd dodania użytkownika", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var answer = bazaDanych.PobierzPytanie(this.loginLoginBox.Text);
            if (answer.Item1 == 100)
            {
                MessageBox.Show("Wybrany login nie istnieje w bazie danych.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (answer.Item1 == 101)
            {
                MessageBox.Show("Błąd połączenia z bazą danych.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.pytanie = answer.Item2;
            ShowVerifyQuestionDialog();
            var resultpasswd = bazaDanych.newPasssword(this.loginLoginBox.Text, this.odpowiedz);
            if (resultpasswd == 200)
            {
                ShowNewPasswdDialog();
                var newpasswdresult = bazaDanych.newPasssword(this.loginLoginBox.Text, this.nowehaslo);
                if (newpasswdresult == 0)
                {
                    MessageBox.Show("Hasło zmieniono pomyślnie.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Błąd połączenia z bazą danych.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Błąd połączenia z bazą danych.", "Autoryzacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }
        public void ShowAddQuestionDialog()
        {
            AddQuestion aq = new AddQuestion();

            if (aq.ShowDialog(this) == DialogResult.OK)
            {

                this.pytanie = aq.questionBox.Text;
                this.odpowiedz = aq.answerBox.Text;
            }
            Console.WriteLine(this.pytanie);
            Console.WriteLine(this.odpowiedz);

            aq.Dispose();
        }
        public void ShowVerifyQuestionDialog()
        {
            VerifyQuestion vq = new VerifyQuestion();

            vq.verifyQuestionBox.Text = this.pytanie;

            if (vq.ShowDialog(this) == DialogResult.OK)
            {
                this.odpowiedz = vq.verifyAnswerBox.Text;
            }
            
            Console.WriteLine(this.odpowiedz);
            
            vq.Dispose();
        }

        public void ShowNewPasswdDialog()
        {
            NewPassword np = new NewPassword();

            if (np.ShowDialog(this) == DialogResult.OK)
            {
                this.nowehaslo = np.newPasswordBox.Text;
            }

            np.Dispose();
            
        }
    }
}
