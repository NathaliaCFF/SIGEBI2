using UI2.AppConfig;
using UI2.Models.Auth;
using UI2.Services;
using UI2.Services.Interfaces;
using UI2.ViewModels.Auth;
using UI2.Views.Main;

namespace UI2.Views.Login
{
    public partial class LoginForm : Form
    {
        private readonly IAuthApiService _authService;
        private readonly SessionService _sessionService;
        private readonly NotificationService _notificationService;
        private readonly ValidationService _validationService;
        private readonly LoginViewModel _viewModel = new();

        public LoginForm(IAuthApiService authService,
                         SessionService sessionService,
                         NotificationService notificationService,
                         ValidationService validationService)
        {
            InitializeComponent();
            _authService = authService;
            _sessionService = sessionService;
            _notificationService = notificationService;
            _validationService = validationService;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var request = new LoginRequestModel
            {
                Email = txtEmail.Text,
                Password = txtPassword.Text
            };

            // Validaciones simples
            if (!_validationService.ValidateRequired(request.Email, "email", out var m1))
            {
                _notificationService.ShowError(m1);
                return;
            }

            if (!_validationService.ValidateRequired(request.Password, "contraseña", out var m2))
            {
                _notificationService.ShowError(m2);
                return;
            }

            // Llama al servicio SOLID (no adapter)
            var result = await ServiceLocator.AuthApiService.LoginAsync(request);

            if (!result.Success)
            {
                _notificationService.ShowError(result.Message);
                return;
            }

            // Guarda la sesión
            _sessionService.AuthInfo = result.Data;

            _notificationService.ShowInfo("Login correcto");
            AbrirPanelPrincipal();
        }

        private void AbrirPanelPrincipal()
        {
            Hide();

            using var main = new MainForm();
            main.ShowDialog();

            txtPassword.Clear();
            Show();
        }
    }
}
