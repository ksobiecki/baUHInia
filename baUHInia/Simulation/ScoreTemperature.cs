using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baUHInia.Simulation
{
    public partial class ScoreTemperature : Form
    {
        public ScoreTemperature()
        {
            InitializeComponent();
        }


        public String LabelScoreTemperatureMessage {
            set { this.ScoreTempMessage.Text = value; }
            get { return this.ScoreTempMessage.Text; }
        }


        public String returnScoreTemperature()
        {

            double a = new double();
            string komunikat = "brak";

            //a = (double)(temperatures + grassTempValue) / 2;



            if (a > 30.0)
            {
                komunikat = ("Temperatura za wysoka słabo ci poszło z rozmieszczeniem obiektów, " +
                    "zdobyłeś tylko: " + a.ToString());
            }
            else if (a > 20.0 && a <= 30.0)
            {
                komunikat = ("Temeperatura przyzwoita nie jest najgorzej ale mogłobyć lepiej :D, zdobyłeś: " + a.ToString());
            }
            else if (a > 5.0 && a <= 20.0)
            {
                komunikat = ("Temperatura idealna udało ci się uzyskać ciekawy rezulatat ! Gratulacje zdobyłeś: " + a.ToString());
            }

            //ile = 0;
            //temperatures = 0;
            return komunikat;

        }

    }
}
