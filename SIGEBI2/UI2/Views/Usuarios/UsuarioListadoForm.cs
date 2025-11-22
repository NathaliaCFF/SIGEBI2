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

        private bool _bloquearSeleccion = false;

        public UsuarioListadoForm()
        {
            InitializeComponent();

            _usuarioAdapter = ServiceLocator.UsuarioAdapter;
            _sessionService = ServiceLocator.SessionService;
            _notificationService = ServiceLocator.NotificationService;
            _validationService = ServiceLocator.ValidationService;

            gridUsuarios.AutoGenerateColumns = false;

            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Correo",
                Width = 180
            });
            gridUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Rol",
                HeaderText = "Rol",
                Width = 100
            });
            gridUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });

            gridUsuarios.DataSource = _viewModel.Usuarios;
        }

        private async void UsuarioListadoForm_Load(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
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
            if (!_validationService.ValidateRequired(model.Nombre, "nombre", out var m1))
            {
                _notificationService.ShowError(m1);
                txtNombre.Focus();
                return false;
            }

            if (!_validationService.ValidateEmail(model.Email, out var m2))
            {
                _notificationService.ShowError(m2);
                txtEmail.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(model.Password, "contraseña", out var m3))
            {
                _notificationService.ShowError(m3);
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        private async Task CargarUsuariosAsync()
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                var resultado = await _usuarioAdapter.ObtenerUsuariosAsync(usuarioActual);

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
                var usuarioActual = ObtenerUsuarioActual();
                var resultado = await _usuarioAdapter.CrearUsuarioAsync(model, usuarioActual);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.AgregarUsuario(resultado.Data);

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
                _notificationService.ShowError("Seleccione un usuario para actualizar.");
                return;
            }

            if (!_validationService.ValidateRequired(model.Nombre, "nombre", out var m1))
            {
                _notificationService.ShowError(m1);
                return;
            }

            if (!_validationService.ValidateEmail(model.Email, out var m2))
            {
                _notificationService.ShowError(m2);
                return;
            }

            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                var resultado = await _usuarioAdapter.ActualizarUsuarioAsync(model, usuarioActual);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.ActualizarUsuario(resultado.Data);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al actualizar usuario: {ex.Message}");
            }
        }

        private bool TryObtenerUsuarioSeleccionado(out UsuarioListItemModel usuario)
        {
            usuario = gridUsuarios.CurrentRow?.DataBoundItem as UsuarioListItemModel;

            if (usuario == null)
            {
                _notificationService.ShowError("Seleccione un usuario.");
                return false;
            }

            return true;
        }

        private SIGEBI.Domain.Entities.Usuario ObtenerUsuarioActual()
        {
            return new SIGEBI.Domain.Entities.Usuario
            {
                Nombre = _sessionService.NombreUsuario,
                Email = _sessionService.Email,
                Rol = _sessionService.Rol,
                Activo = true
            };
        }

        private async void btnActivar_Click(object sender, EventArgs e)
        {
            if (!TryObtenerUsuarioSeleccionado(out var usuario))
                return;

            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                var resultado = await _usuarioAdapter.ActivarUsuarioAsync(usuario.Id, usuarioActual);

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.ActualizarEstado(usuario.Id, true);
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

            if (_notificationService.Confirm("¿Desea desactivar este usuario?") != DialogResult.Yes)
                return;

            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                var resultado = await _usuarioAdapter.DesactivarUsuarioAsync(usuario.Id, usuarioActual);

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.ActualizarEstado(usuario.Id, false);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al desactivar usuario: {ex.Message}");
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            _viewModel.AplicarFiltro(txtBusqueda.Text);
        }

        private void gridUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (_bloquearSeleccion)
                return;

            if (gridUsuarios.CurrentRow?.DataBoundItem is UsuarioListItemModel usuario)
            {
                txtId.Text = usuario.Id.ToString();
                txtNombre.Text = usuario.Nombre;
                txtEmail.Text = usuario.Email;
                cmbRol.SelectedItem = usuario.Rol;
                chkActivo.Checked = usuario.Activo;
                txtPassword.Clear();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            _bloquearSeleccion = true;

            txtId.Clear();
            txtNombre.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = true;

            gridUsuarios.ClearSelection();
            txtNombre.Focus();

            _bloquearSeleccion = false;
        }

        private void LimpiarFormulario()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
            chkActivo.Checked = true;

            gridUsuarios.ClearSelection();
        }
    }
}
