using UI2.Adapters;
using UI2.AppConfig;
using UI2.Models.Prestamos;
using UI2.Services;
using UI2.ViewModels.Prestamos;

namespace UI2.Views.Prestamos
{
    public partial class PrestamoListadoForm : Form
    {
        private readonly PrestamoAdapter _prestamoAdapter;
        private readonly NotificationService _notificationService;
        private readonly PrestamoListadoViewModel _viewModel = new();

        public PrestamoListadoForm()
        {
            InitializeComponent();

            _prestamoAdapter = ServiceLocator.PrestamoAdapter;
            _notificationService = ServiceLocator.NotificationService;

            ConfigurarColumnasPrestamos();
            ConfigurarColumnasDetalles();

            gridPrestamos.DataSource = _viewModel.Prestamos;
        }

        // ======================================================
        // CONFIGURAR COLUMNAS GRID PRINCIPAL
        // ======================================================
        private void ConfigurarColumnasPrestamos()
        {
            gridPrestamos.Columns.Clear();
            gridPrestamos.AutoGenerateColumns = false;

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 60
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "Usuario Id",
                Width = 80
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreUsuario",
                HeaderText = "Nombre",
                Width = 180
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaPrestamo",
                HeaderText = "Prestado",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaVencimiento",
                HeaderText = "Vence",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            gridPrestamos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });
        }

        // ======================================================
        // CONFIGURAR COLUMNAS DETALLES
        // ======================================================
        private void ConfigurarColumnasDetalles()
        {
            lstDetalles.View = View.Details;
            lstDetalles.Columns.Clear();

            lstDetalles.FullRowSelect = true;
            lstDetalles.HideSelection = false;
            lstDetalles.MultiSelect = false;

            lstDetalles.Columns.Add("Título", 250);
            lstDetalles.Columns.Add("Id libro", 80);
            lstDetalles.Columns.Add("Estado", 150);
        }

        private void PrestamoListadoForm_Load(object sender, EventArgs e)
        {
            txtUsuarioId.Focus();
        }

        // ======================================================
        // BOTÓN CONSULTAR PRÉSTAMOS ACTIVOS
        // ======================================================
        private async void btnBuscarPorUsuario_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUsuarioId.Text, out var usuarioId))
            {
                _notificationService.ShowError("Ingrese un ID de usuario válido.");
                return;
            }

            await CargarPrestamosActivosAsync(usuarioId);
        }

        // ======================================================
        // BOTÓN CONSULTAR VENCIDOS
        // ======================================================
        private async void btnVerVencidos_Click(object sender, EventArgs e)
        {
            await CargarPrestamosVencidosAsync();
        }

        // ======================================================
        // BOTÓN REFRESCAR
        // ======================================================
        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtUsuarioId.Text, out int usuarioId))
            {
                var resultado = await _prestamoAdapter.ObtenerPrestamosActivosAsync(usuarioId);

                if (!resultado.Success)
                {
                    _viewModel.CargarPrestamos(new List<PrestamoListItemModel>());
                    lstDetalles.Items.Clear();
                    lblResumen.Text = "No hay préstamos activos.";
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarPrestamos(resultado.Data);
                MostrarDetallesSeleccionados();
                return;
            }

            var resultadoV = await _prestamoAdapter.ObtenerPrestamosVencidosAsync();

            if (!resultadoV.Success)
            {
                _viewModel.CargarPrestamos(new List<PrestamoListItemModel>());
                lstDetalles.Items.Clear();
                lblResumen.Text = "No hay vencidos.";
                _notificationService.ShowError(resultadoV.Message);
                return;
            }

            _viewModel.CargarPrestamos(resultadoV.Data);
            MostrarDetallesSeleccionados();
        }

        private async Task CargarPrestamosActivosAsync(int usuarioId)
        {
            try
            {
                var resultado = await _prestamoAdapter.ObtenerPrestamosActivosAsync(usuarioId);

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarPrestamos(resultado.Data);
                MostrarDetallesSeleccionados();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al cargar préstamos activos: {ex.Message}");
            }
        }

        private async Task CargarPrestamosVencidosAsync()
        {
            try
            {
                var resultado = await _prestamoAdapter.ObtenerPrestamosVencidosAsync();

                if (!resultado.Success)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarPrestamos(resultado.Data);
                MostrarDetallesSeleccionados();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al cargar préstamos vencidos: {ex.Message}");
            }
        }

        // ======================================================
        // MOSTRAR DETALLES
        // ======================================================
        private void gridPrestamos_SelectionChanged(object sender, EventArgs e)
        {
            MostrarDetallesSeleccionados();
        }

        private void MostrarDetallesSeleccionados()
        {
            lstDetalles.Items.Clear();

            if (gridPrestamos.CurrentRow?.DataBoundItem is PrestamoListItemModel prestamo)
            {
                if (prestamo.Detalles == null || prestamo.Detalles.Count == 0)
                {
                    lblResumen.Text = "Este préstamo no tiene detalles.";
                    return;
                }

                foreach (var d in prestamo.Detalles)
                {
                    string estado = d.Devuelto
                        ? $"Devuelto el {d.FechaDevolucion?.ToString("dd/MM/yyyy")}"
                        : "Pendiente";

                    var item = new ListViewItem(d.TituloLibro);
                    item.SubItems.Add(d.LibroId.ToString());
                    item.SubItems.Add(estado);

                    lstDetalles.Items.Add(item);
                }

                lblResumen.Text =
                    $"Usuario: {prestamo.NombreUsuario} | Vence: {prestamo.FechaVencimiento:dd/MM/yyyy} | Activo: {(prestamo.Activo ? "Sí" : "No")}";
            }
            else
            {
                lblResumen.Text = "Seleccione un préstamo para ver sus detalles.";
            }
        }

        // ======================================================
        // REGISTRAR PRÉSTAMO
        // ======================================================
        private async void btnRegistrarPrestamo_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUsuarioRegistrar.Text, out int usuarioId))
            {
                _notificationService.ShowError("Ingrese un ID de usuario válido.");
                return;
            }

            if (!int.TryParse(txtLibroRegistrar.Text, out int libroId))
            {
                _notificationService.ShowError("Ingrese un ID de libro válido.");
                return;
            }

            var model = new PrestamoCreateModel
            {
                UsuarioId = usuarioId,
                LibrosIds = new System.Collections.Generic.List<int> { libroId }
            };

            var resultado = await _prestamoAdapter.RegistrarPrestamoAsync(model);

            if (!resultado.Success)
            {
                _notificationService.ShowError(resultado.Message);
                return;
            }

            _notificationService.ShowInfo("Préstamo registrado correctamente.");

            txtLibroRegistrar.Clear();

            await CargarPrestamosActivosAsync(usuarioId);
        }

        // ======================================================
        // DEVOLVER PRÉSTAMO COMPLETO
        // ======================================================
        private async void btnDevolverPrestamo_Click(object sender, EventArgs e)
        {
            if (gridPrestamos.CurrentRow?.DataBoundItem is not PrestamoListItemModel prestamo)
            {
                _notificationService.ShowError("Seleccione un préstamo.");
                return;
            }

            if (!prestamo.Activo)
            {
                _notificationService.ShowError("Este préstamo ya está cerrado.");
                return;
            }

            var librosIds = prestamo.Detalles
                .Where(x => !x.Devuelto)
                .Select(x => x.LibroId)
                .ToList();

            if (librosIds.Count == 0)
            {
                _notificationService.ShowInfo("Todos los libros ya fueron devueltos.");
                return;
            }

            var result = await _prestamoAdapter.RegistrarDevolucionAsync(prestamo.Id, librosIds);

            if (!result.Success)
            {
                _notificationService.ShowError(result.Message);
                return;
            }

            _notificationService.ShowInfo("Préstamo devuelto correctamente.");

            await ActualizarVistaDespuesDeDevolucion(prestamo.UsuarioId);
        }

        private async Task ActualizarVistaDespuesDeDevolucion(int usuarioId)
        {
            if (usuarioId > 0)
                await CargarPrestamosActivosAsync(usuarioId);
            else
                await CargarPrestamosVencidosAsync();
        }
    }
}

