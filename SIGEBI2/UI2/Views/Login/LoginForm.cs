using Application.DTOs;
using System;
using System.Windows.Forms;
using UI2.Adapters;
using UI2.AppConfig;
using UI2.Models.Auth;
using UI2.Services;
using UI2.ViewModels.Auth;

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

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            var request = new AuthRequestDTO
            {
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text
            };

            if (!ValidarEntrada(request))
                return;

            btnIngresar.Enabled = false;

            try
            {
                var resultado = await _authAdapter.LoginAsync(request);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _sessionService.RegistrarSesion(request.Email, resultado.Data);
                AbrirPanelPrincipal();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al iniciar sesión: {ex.Message}");
            }
            finally
            {
                btnIngresar.Enabled = true;
            }
        }


        private bool ValidarEntrada(AuthRequestDTO request)
        {
            if (!_validationService.ValidateEmail(request.Email, out var mensajeEmail))
            {
                _notificationService.ShowError(mensajeEmail);
                txtEmail.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(request.Password, "contraseña", out var mensajePassword))
            {
                _notificationService.ShowError(mensajePassword);
                txtPassword.Focus();
                return false;
            }

            return true;
        }


        private void AbrirPanelPrincipal()
        {
            Hide();
            using var main = new Main.MainForm();
            main.ShowDialog();
            txtPassword.Clear();
            Show();
        }
    }
}
