using System;
using System.Windows.Forms;
using UI2.AppConfig;
using UI2.Views.Login;
using static System.Windows.Forms.DataFormats;

namespace UI2
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceLocator.Configure();

            var loginForm = new LoginForm(ServiceLocator.AuthAdapter,
                                          ServiceLocator.SessionService,
                                          ServiceLocator.NotificationService,
                                          ServiceLocator.ValidationService);

            Application.Run(loginForm);
        }
    }
}