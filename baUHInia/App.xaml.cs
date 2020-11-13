using System.Diagnostics;
using System.Windows.Forms;

namespace baUHInia
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            try
            {
                Form authForm = new Authorisation.Authorisation();
                authForm.ShowDialog();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
