using System;
using System.Windows.Forms;
using UI2.AppConfig;
using UI2.Services;
using UI2.Views.Libros;
using UI2.Views.Prestamos;
using UI2.Views.Usuarios;

namespace UI2.Views.Main
{
    public partial class MainForm : Form
    {
        private readonly SessionService _sessionService;
        private readonly NotificationService _notificationService;

        public MainForm()
        {
            InitializeComponent();
            _sessionService = ServiceLocator.SessionService;
            _notificationService = ServiceLocator.NotificationService;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_sessionService.EstaAutenticado)
            {
                lblBienvenida.Text =
                    $"Bienvenido, {_sessionService.NombreUsuario} ({_sessionService.Rol})";
            }
            else
            {
                lblBienvenida.Text = "Sesión no iniciada";
            }
        }


        private void AbrirFormulario(Form formulario)
        {
            panelContenido.Controls.Clear();

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            panelContenido.Controls.Add(formulario);
            formulario.Show();
        }
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            if (!_sessionService.EstaAutenticado)
            {
                _notificationService.ShowError("Debe iniciar sesión.");
                return;
            }

            // Validación robusta del rol
            if (!string.Equals(_sessionService.Rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                _notificationService.ShowError("Solo un administrador puede gestionar usuarios.");
                return;
            }

            AbrirFormulario(new UsuarioListadoForm());
        }


        private void btnLibros_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new LibroListadoForm());
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new PrestamoListadoForm());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (_notificationService.Confirm("¿Desea cerrar la sesión?") == DialogResult.Yes)
            {
                _sessionService.CerrarSesion();
                Close();
            }
        }
    }
}
