using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIGEBI.Domain.Entities;
using UI2.Adapters;
using UI2.AppConfig;
using UI2.Models.Libros;
using UI2.Services;
using UI2.ViewModels.Libros;

namespace UI2.Views.Libros
{
    public partial class LibroListadoForm : Form
    {
        private readonly LibroAdapter _libroAdapter;
        private readonly NotificationService _notificationService;
        private readonly ValidationService _validationService;
        private readonly LibroListadoViewModel _viewModel = new();

        public LibroListadoForm()
        {
            InitializeComponent();
            _libroAdapter = ServiceLocator.LibroAdapter;
            _notificationService = ServiceLocator.NotificationService;
            _validationService = ServiceLocator.ValidationService;
            gridLibros.AutoGenerateColumns = false;
            gridLibros.DataSource = _viewModel.Libros;
        }

        private async void LibroListadoForm_Load(object sender, EventArgs e)
        {
            await CargarLibrosAsync();
        }

        private async Task CargarLibrosAsync()
        {
            try
            {
                var resultado = await _libroAdapter.ListarLibrosAsync();
                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarLibros(resultado.Data);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al cargar libros: {ex.Message}");
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            _viewModel.Filtro.Criterio = txtBuscar.Text.Trim();
            try
            {
                var resultado = await _libroAdapter.BuscarLibrosAsync(_viewModel.Filtro.Criterio);
                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _viewModel.CargarLibros(resultado.Data);
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al buscar libros: {ex.Message}");
            }
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            await CargarLibrosAsync();
        }

        private Libro ObtenerLibroDesdeFormulario()
        {
            return new Libro
            {
                Id = int.TryParse(txtIdLibro.Text, out var id) ? id : 0,
                Titulo = txtTitulo.Text.Trim(),
                Autor = txtAutor.Text.Trim(),
                ISBN = txtIsbn.Text.Trim(),
                Editorial = txtEditorial.Text.Trim(),
                AnioPublicacion = int.TryParse(txtAnio.Text, out var anio) ? anio : 0,
                Categoria = txtCategoria.Text.Trim(),
                Disponible = chkDisponible.Checked,
                Activo = chkActivoLibro.Checked
            };
        }

        private bool ValidarCamposLibro(Libro libro, bool validarDisponibilidad = false)
        {
            if (!_validationService.ValidateRequired(libro.Titulo, "título", out var mensajeTitulo))
            {
                _notificationService.ShowError(mensajeTitulo);
                txtTitulo.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(libro.Autor, "autor", out var mensajeAutor))
            {
                _notificationService.ShowError(mensajeAutor);
                txtAutor.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(libro.ISBN, "ISBN", out var mensajeIsbn))
            {
                _notificationService.ShowError(mensajeIsbn);
                txtIsbn.Focus();
                return false;
            }

            if (libro.AnioPublicacion <= 0)
            {
                _notificationService.ShowError("Ingrese un año de publicación válido.");
                txtAnio.Focus();
                return false;
            }

            if (validarDisponibilidad && gridLibros.CurrentRow?.DataBoundItem is LibroListItemModel seleccionado)
            {
                libro.Disponible = seleccionado.Disponible;
            }

            return true;
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            var libro = ObtenerLibroDesdeFormulario();
            if (!ValidarCamposLibro(libro))
            {
                return;
            }

            try
            {
                var resultado = await _libroAdapter.CrearLibroAsync(libro);
                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                _viewModel.Libros.Add(resultado.Data);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al registrar libro: {ex.Message}");
            }
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            var libro = ObtenerLibroDesdeFormulario();
            if (libro.Id <= 0)
            {
                _notificationService.ShowError("Seleccione un libro del listado para actualizar.");
                return;
            }

            if (!ValidarCamposLibro(libro, validarDisponibilidad: true))
            {
                return;
            }

            try
            {
                var resultado = await _libroAdapter.ActualizarLibroAsync(libro.Id, libro);
                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo(resultado.Message);
                var seleccionado = _viewModel.Libros.FirstOrDefault(l => l.Id == libro.Id);
                if (seleccionado != null)
                {
                    seleccionado.Titulo = resultado.Data.Titulo;
                    seleccionado.Autor = resultado.Data.Autor;
                    seleccionado.ISBN = resultado.Data.ISBN;
                    seleccionado.Editorial = resultado.Data.Editorial;
                    seleccionado.AnioPublicacion = resultado.Data.AnioPublicacion;
                    seleccionado.Categoria = resultado.Data.Categoria;
                    seleccionado.Disponible = resultado.Data.Disponible;
                    seleccionado.Activo = resultado.Data.Activo;
                    gridLibros.Refresh();
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al actualizar libro: {ex.Message}");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void gridLibros_SelectionChanged(object sender, EventArgs e)
        {
            if (gridLibros.CurrentRow?.DataBoundItem is LibroListItemModel libro)
            {
                txtIdLibro.Text = libro.Id.ToString();
                txtTitulo.Text = libro.Titulo;
                txtAutor.Text = libro.Autor;
                txtIsbn.Text = libro.ISBN;
                txtEditorial.Text = libro.Editorial;
                txtAnio.Text = libro.AnioPublicacion.ToString();
                txtCategoria.Text = libro.Categoria;
                chkDisponible.Checked = libro.Disponible;
                chkActivoLibro.Checked = libro.Activo;
            }
        }

        private void LimpiarFormulario()
        {
            txtIdLibro.Clear();
            txtTitulo.Clear();
            txtAutor.Clear();
            txtIsbn.Clear();
            txtEditorial.Clear();
            txtAnio.Clear();
            txtCategoria.Clear();
            chkDisponible.Checked = true;
            chkActivoLibro.Checked = true;
        }
    }
}