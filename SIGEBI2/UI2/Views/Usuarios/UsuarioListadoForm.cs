using System;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using UI2.Adapters;
using UI2.AppConfig;
using UI2.Models.Usuarios;
using UI2.Services;
using UI2.ViewModels.Usuarios;

namespace UI2.Views.Usuarios
{
    public partial class UsuarioListadoForm : Form
    {
        private readonly UsuarioAdapter _usuarioAdapter;
        private readonly SessionService _sessionService;
        private readonly NotificationService _notificationService;
        private readonly ValidationService _validationService;
        private readonly UsuarioListadoViewModel _viewModel = new();

        public UsuarioListadoForm()
        {
            InitializeComponent();

            _usuarioAdapter = ServiceLocator.UsuarioAdapter;
            _sessionService = ServiceLocator.SessionService;
            _notificationService = ServiceLocator.NotificationService;
            _validationService = ServiceLocator.ValidationService;

            // CONFIGURAR GRID
            gridUsuarios.AutoGenerateColumns = false;
            gridUsuarios.Columns.Clear();

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 50
            });

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 140
            });

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Width = 180
            });

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Rol",
                DataPropertyName = "Rol",
                Width = 100
            });

            gridUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Activo",
                DataPropertyName = "Activo",
                Width = 60
            });

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Creado",
                DataPropertyName = "FechaCreacion",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            gridUsuarios.DataSource = _viewModel.Usuarios;
        }

        private async void UsuarioListadoForm_Load(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            try
            {
                var actual = _sessionService.UsuarioActual ?? throw new InvalidOperationException("No hay un usuario en sesión.");
                var resultado = await _usuarioAdapter.ObtenerUsuariosAsync(actual);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarUsuarios(resultado.Data);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al cargar usuarios: {ex.Message}");
            }
        }

        private UsuarioFormModel ObtenerDatosFormulario()
        {
            return new UsuarioFormModel
            {
                Id = int.TryParse(txtId.Text, out var id) ? id : 0,
                Nombre = txtNombre.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text,
                Rol = cmbRol.SelectedItem?.ToString() ?? "Usuario",
                Activo = chkActivo.Checked
            };
        }

        private bool ValidarNuevoUsuario(UsuarioFormModel model)
        {
            if (!_validationService.ValidateRequired(model.Nombre, "nombre", out var mensajeNombre))
            {
                _notificationService.ShowError(mensajeNombre);
                txtNombre.Focus();
                return false;
            }

            if (!_validationService.ValidateEmail(model.Email, out var mensajeEmail))
            {
                _notificationService.ShowError(mensajeEmail);
                txtEmail.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(model.Password, "contraseña", out var mensajePassword))
            {
                _notificationService.ShowError(mensajePassword);
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
        }

        private async void btnCrear_Click(object sender, EventArgs e)
        {
            var model = ObtenerDatosFormulario();

            if (!ValidarNuevoUsuario(model))
                return;

            try
            {
                var actual = _sessionService.UsuarioActual ?? throw new InvalidOperationException("No hay un usuario en sesión.");

                var resultado = await _usuarioAdapter.CrearUsuarioAsync(model, actual);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.Usuarios.Add(resultado.Data);

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al crear usuario: {ex.Message}");
            }
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            var model = ObtenerDatosFormulario();

            if (model.Id <= 0)
            {
                _notificationService.ShowError("Debe seleccionar un usuario para actualizar.");
                return;
            }

            if (!_validationService.ValidateRequired(model.Nombre, "nombre", out var mensajeNombre))
            {
                _notificationService.ShowError(mensajeNombre);
                txtNombre.Focus();
                return;
            }

            if (!_validationService.ValidateEmail(model.Email, out var mensajeEmail))
            {
                _notificationService.ShowError(mensajeEmail);
                txtEmail.Focus();
                return;
            }

            try
            {
                var actual = _sessionService.UsuarioActual ?? throw new InvalidOperationException("No hay un usuario en sesión.");
                var resultado = await _usuarioAdapter.ActualizarUsuarioAsync(model, actual);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                var seleccionado = _viewModel.Usuarios.FirstOrDefault(u => u.Id == model.Id);
                if (seleccionado != null)
                {
                    seleccionado.Nombre = resultado.Data.Nombre;
                    seleccionado.Email = resultado.Data.Email;
                    seleccionado.Rol = resultado.Data.Rol;
                    seleccionado.Activo = resultado.Data.Activo;
                }

                _notificationService.ShowInfo(resultado.Message);
                gridUsuarios.Refresh();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al actualizar usuario: {ex.Message}");
            }
        }

        private async void btnActivar_Click(object sender, EventArgs e)
        {
            if (!TryObtenerUsuarioSeleccionado(out var usuario))
                return;

            try
            {
                var actual = _sessionService.UsuarioActual ?? throw new InvalidOperationException("No hay un usuario en sesión.");
                var resultado = await _usuarioAdapter.ActivarUsuarioAsync(usuario.Id, actual);

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                usuario.Activo = true;
                _notificationService.ShowInfo(resultado.Message);

                gridUsuarios.Refresh();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al activar usuario: {ex.Message}");
            }
        }

        private async void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (!TryObtenerUsuarioSeleccionado(out var usuario))
                return;

            if (_notificationService.Confirm("¿Desea desactivar al usuario seleccionado?") != DialogResult.Yes)
                return;

            try
            {
                var actual = _sessionService.UsuarioActual ?? throw new InvalidOperationException("No hay un usuario en sesión.");
                var resultado = await _usuarioAdapter.DesactivarUsuarioAsync(usuario.Id, actual);

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                usuario.Activo = false;
                _notificationService.ShowInfo(resultado.Message);

                gridUsuarios.Refresh();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al desactivar usuario: {ex.Message}");
            }
        }

        private bool TryObtenerUsuarioSeleccionado(out UsuarioListItemModel usuario)
        {
            usuario = gridUsuarios.CurrentRow?.DataBoundItem as UsuarioListItemModel ?? null;

            if (usuario == null)
            {
                _notificationService.ShowError("Seleccione un usuario de la lista.");
                return false;
            }

            return true;
        }

        private void gridUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (gridUsuarios.CurrentRow?.DataBoundItem is UsuarioListItemModel usuario)
            {
                txtId.Text = usuario.Id.ToString();
                txtNombre.Text = usuario.Nombre;
                txtEmail.Text = usuario.Email;
                cmbRol.SelectedItem = usuario.Rol;
                chkActivo.Checked = usuario.Activo;
            }
        }

        private void LimpiarFormulario()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = true;
        }
    }
}
