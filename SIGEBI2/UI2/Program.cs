using UI2.AppConfig;
using UI2.Views.Login;

namespace UI2
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);


            ServiceLocator.Configure();

            var loginForm = new LoginForm(
                ServiceLocator.AuthApiService,
                ServiceLocator.SessionService,
                ServiceLocator.NotificationService,
                ServiceLocator.ValidationService);

            System.Windows.Forms.Application.Run(loginForm);
        }
    }
}