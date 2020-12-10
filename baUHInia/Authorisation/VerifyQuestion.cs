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
    public partial class VerifyQuestion : Form
    {

        public VerifyQuestion()
        {
            InitializeComponent();
            verifyQuestionBttn.DialogResult = DialogResult.OK;
        }
        private void newQuestionTextClicked(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 3000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show("To pole nie może być puste.", TB, 138, 0, VisibleTime);

        }
        private void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                verifyQuestionBttn.PerformClick();
        }

        private void newQuestionTextClicked(object sender, MouseEventArgs e)
        {

        }
    }
}
