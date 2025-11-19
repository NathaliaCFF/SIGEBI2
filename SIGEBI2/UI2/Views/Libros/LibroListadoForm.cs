using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            // COLUMNAS DEL GRID
            gridLibros.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 50
            });

            gridLibros.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Titulo",
                HeaderText = "Título",
                Width = 150
            });

            gridLibros.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Autor",
                HeaderText = "Autor",
                Width = 150
            });

            gridLibros.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ISBN",
                HeaderText = "ISBN",
                Width = 120
            });

            gridLibros.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Disponible",
                HeaderText = "Disponible",
                Width = 70
            });

            gridLibros.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });

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
                _notificationService.ShowError($"Error al buscar: {ex.Message}");
            }
        }

        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            await CargarLibrosAsync();
        }

        // ================================
        //   📌 CREAR MODELO DESDE FORMULARIO
        // ================================
        private LibroCreateModel ObtenerModeloCrear()
        {
            return new LibroCreateModel
            {
                Titulo = txtTitulo.Text.Trim(),
                Autor = txtAutor.Text.Trim(),
                ISBN = txtIsbn.Text.Trim(),
                Editorial = txtEditorial.Text.Trim(),
                AnioPublicacion = int.TryParse(txtAnio.Text, out var anio) ? anio : 0,
                Categoria = txtCategoria.Text.Trim()
            };
        }

        // ================================
        //   📌 CREAR MODELO PARA ACTUALIZAR
        // ================================
        private LibroUpdateModel ObtenerModeloActualizar()
        {
            return new LibroUpdateModel
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

        // ================================
        //     📌 VALIDACIONES
        // ================================
        private bool ValidarCamposLibro()
        {
            if (!_validationService.ValidateRequired(txtTitulo.Text, "título", out var msg))
            {
                _notificationService.ShowError(msg);
                txtTitulo.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(txtAutor.Text, "autor", out msg))
            {
                _notificationService.ShowError(msg);
                txtAutor.Focus();
                return false;
            }

            if (!_validationService.ValidateRequired(txtIsbn.Text, "ISBN", out msg))
            {
                _notificationService.ShowError(msg);
                txtIsbn.Focus();
                return false;
            }

            if (!int.TryParse(txtAnio.Text, out var anio) || anio <= 0)
            {
                _notificationService.ShowError("Ingrese un año de publicación válido.");
                txtAnio.Focus();
                return false;
            }

            return true;
        }

        // ================================
        //   📌 REGISTRAR
        // ================================
        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposLibro())
                return;

            var libro = ObtenerModeloCrear();

            try
            {
                var resultado = await _libroAdapter.CrearLibroAsync(libro);

                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo("Libro registrado correctamente.");
                _viewModel.Libros.Add(resultado.Data);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Error al registrar: {ex.Message}");
            }
        }

        // ================================
        //   📌 ACTUALIZAR
        // ================================
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIdLibro.Text, out var id) || id <= 0)
            {
                _notificationService.ShowError("Seleccione un libro del listado.");
                return;
            }

            if (!ValidarCamposLibro())
                return;

            var libro = ObtenerModeloActualizar();

            try
            {
                var resultado = await _libroAdapter.ActualizarLibroAsync(libro);


                if (!resultado.Success || resultado.Data == null)
                {
                    _notificationService.ShowError(resultado.Message);
                    return;
                }

                _notificationService.ShowInfo("Libro actualizado correctamente.");

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
                _notificationService.ShowError($"Error al actualizar: {ex.Message}");
            }
        }

        // ================================
        //   📌 GRID SELECTION
        // ================================
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
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
