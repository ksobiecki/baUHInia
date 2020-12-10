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
    public partial class NewPassword : Form
    {
        public NewPassword()
        {
            InitializeComponent();
            changePasswdBttn.DialogResult = DialogResult.OK;
        }
        private void newPasswordTextClicked(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 8000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("Wprowadzone hasło:\n1. Musi zawierać co najmniej jedną cyfrę\n2. Musi zawierać co najmniej jedną wielką literę\n3. Musi zawierać co najmniej jedną małą literę\n4. Musi zawierać co najmniej 8 znaków\n5. Nie może zawierać spacji.", TB, 138, 0, VisibleTime);

        }
        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                changePasswdBttn.PerformClick();
        }

    }

}
