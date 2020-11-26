﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using baUHInia.Admin;

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
                //  Form authForm = new Authorisation.Authorisation();
                //  authForm.ShowDialog();
                //todo tylko do testow, usun przed pull req
                Window adminWindow = new AdminRestrictionsWindow();
                adminWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
