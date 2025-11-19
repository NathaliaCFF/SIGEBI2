using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            ConfigurarColumnas();
            gridPrestamos.DataSource = _viewModel.Prestamos;
        }

        private void ConfigurarColumnas()
        {
            gridPrestamos.Columns.Clear();
            gridPrestamos.AutoGenerateColumns = false;

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 50
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UsuarioId",
                HeaderText = "Usuario",
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
                HeaderText = "Fecha préstamo",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            gridPrestamos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaVencimiento",
                HeaderText = "Vencimiento",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            gridPrestamos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });
        }

        private void PrestamoListadoForm_Load(object sender, EventArgs e)
        {
            txtUsuarioId.Focus();
        }

        private async void btnBuscarPorUsuario_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUsuarioId.Text, out var usuarioId))
            {
                _notificationService.ShowError("Ingrese un identificador de usuario válido.");
                return;
            }

            await CargarPrestamosActivosAsync(usuarioId);
        }

        private async void btnVerVencidos_Click(object sender, EventArgs e)
        {
            await CargarPrestamosVencidosAsync();
        }

        private async Task CargarPrestamosActivosAsync(int usuarioId)
        {
            try
            {
                var resultado = await _prestamoAdapter.ObtenerPrestamosActivosAsync(usuarioId);

                if (!resultado.Success || resultado.Data == null)
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

                if (!resultado.Success || resultado.Data == null)
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
                    lblResumen.Text = "No hay detalles para este préstamo.";
                    return;
                }

                foreach (var detalle in prestamo.Detalles)
                {
                    string estado;

                    if (detalle.Devuelto)
                    {
                        var fechaDev = detalle.FechaDevolucion?.ToString("dd/MM/yyyy") ?? "Fecha no disponible";
                        estado = $"Devuelto el {fechaDev}";
                    }
                    else
                    {
                        estado = "Pendiente";
                    }

                    var item = new ListViewItem(detalle.TituloLibro);
                    item.SubItems.Add(detalle.LibroId.ToString());
                    item.SubItems.Add(estado);

                    lstDetalles.Items.Add(item);
                }

                lblResumen.Text =
                    $"Usuario: {prestamo.NombreUsuario} | " +
                    $"Vencimiento: {prestamo.FechaVencimiento:dd/MM/yyyy} | " +
                    $"Activo: {(prestamo.Activo ? "Sí" : "No")}";
            }
            else
            {
                lblResumen.Text = "Seleccione un préstamo para ver los detalles.";
            }
        }
    }
}
