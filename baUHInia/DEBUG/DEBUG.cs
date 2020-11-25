using baUHInia.MapLogic.View;
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
            Hide();
            AdminGameWindow app = new AdminGameWindow(null);
            app.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            ChoiceWindow app = new ChoiceWindow();
            app.Show();
        }

    }
}
