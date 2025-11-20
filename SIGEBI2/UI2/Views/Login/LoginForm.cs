using Application.DTOs;
using System;
using System.Windows.Forms;
using UI2.Adapters;
using UI2.AppConfig;
using UI2.Models.Auth;
using UI2.Services;
using UI2.ViewModels.Auth;
using UI2.Views.Main;

namespace UI2.Views.Login
{
    public partial class LoginForm : Form
    {
        private readonly AuthAdapter _authAdapter;
        private readonly SessionService _sessionService;
        private readonly NotificationService _notificationService;
        private readonly ValidationService _validationService;
        private readonly LoginViewModel _viewModel = new();

        public LoginForm(AuthAdapter authAdapter,
                         SessionService sessionService,
                         NotificationService notificationService,
                         ValidationService validationService)
        {
            InitializeComponent();
            _authAdapter = authAdapter;
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

            var result = await ServiceLocator.AuthAdapter.LoginAsync(request);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Guarda la sesión
            _sessionService.AuthInfo = result.Data;

            MessageBox.Show("Login correcto");
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
