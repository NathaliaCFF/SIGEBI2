using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Libros
{
    partial class LibroListadoForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView gridLibros = null!;
        private TextBox txtBuscar = null!;
        private Button btnBuscar = null!;
        private Button btnRefrescar = null!;
        private GroupBox grpLibro = null!;

        private TextBox txtIdLibro = null!;
        private TextBox txtTitulo = null!;
        private TextBox txtAutor = null!;
        private TextBox txtIsbn = null!;
        private TextBox txtEditorial = null!;
        private TextBox txtAnio = null!;
        private TextBox txtCategoria = null!;

        private CheckBox chkDisponible = null!;
        private CheckBox chkActivoLibro = null!;

        private Button btnRegistrar = null!;
        private Button btnActualizar = null!;
        private Button btnEliminar = null!;
        private Button btnActivar = null!;  
        private Button btnLimpiar = null!;

        private Label lblId = null!;
        private Label lblTitulo = null!;
        private Label lblAutor = null!;
        private Label lblIsbn = null!;
        private Label lblEditorial = null!;
        private Label lblAnio = null!;
        private Label lblCategoria = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            gridLibros = new DataGridView();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnRefrescar = new Button();
            grpLibro = new GroupBox();

            lblId = new Label();
            txtIdLibro = new TextBox();

            lblTitulo = new Label();
            txtTitulo = new TextBox();

            lblAutor = new Label();
            txtAutor = new TextBox();

            lblIsbn = new Label();
            txtIsbn = new TextBox();

            lblEditorial = new Label();
            txtEditorial = new TextBox();

            lblAnio = new Label();
            txtAnio = new TextBox();

            lblCategoria = new Label();
            txtCategoria = new TextBox();

            chkDisponible = new CheckBox();
            chkActivoLibro = new CheckBox();

            btnRegistrar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            btnActivar = new Button();   
            btnLimpiar = new Button();

            ((System.ComponentModel.ISupportInitialize)gridLibros).BeginInit();
            grpLibro.SuspendLayout();
            SuspendLayout();

            // ======================== GRID ==========================
            gridLibros.AllowUserToAddRows = false;
            gridLibros.AllowUserToDeleteRows = false;
            gridLibros.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            gridLibros.Location = new Point(27, 85);
            gridLibros.ReadOnly = true;
            gridLibros.Size = new Size(650, 600);
            gridLibros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridLibros.SelectionChanged += gridLibros_SelectionChanged;

            // sombreado según estado
            gridLibros.CellFormatting += gridLibros_CellFormatting;

            // ======================== BUSCAR ==========================
            txtBuscar.Location = new Point(27, 32);
            txtBuscar.PlaceholderText = "Buscar por título o autor";
            txtBuscar.Size = new Size(319, 27);

            btnBuscar.Location = new Point(366, 32);
            btnBuscar.Size = new Size(103, 32);
            btnBuscar.Text = "Buscar";
            btnBuscar.Click += btnBuscar_Click;

            btnRefrescar.Location = new Point(480, 32);
            btnRefrescar.Size = new Size(103, 32);
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.Click += btnRefrescar_Click;

            // ======================== GROUPBOX ==========================
            grpLibro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpLibro.Location = new Point(700, 20);
            grpLibro.Size = new Size(350, 720);
            grpLibro.Text = "Gestión de libros";

            // ======================== CAMPOS ==========================
            lblId.Location = new Point(18, 40);
            lblId.Text = "ID";
            txtIdLibro.Location = new Point(18, 60);
            txtIdLibro.ReadOnly = true;
            txtIdLibro.Size = new Size(300, 27);

            lblTitulo.Location = new Point(18, 100);
            lblTitulo.Text = "Título";
            txtTitulo.Location = new Point(18, 120);
            txtTitulo.Size = new Size(300, 27);

            lblAutor.Location = new Point(18, 160);
            lblAutor.Text = "Autor";
            txtAutor.Location = new Point(18, 180);
            txtAutor.Size = new Size(300, 27);

            lblIsbn.Location = new Point(18, 220);
            lblIsbn.Text = "ISBN";
            txtIsbn.Location = new Point(18, 240);
            txtIsbn.Size = new Size(300, 27);

            lblEditorial.Location = new Point(18, 280);
            lblEditorial.Text = "Editorial";
            txtEditorial.Location = new Point(18, 300);
            txtEditorial.Size = new Size(300, 27);

            lblAnio.Location = new Point(18, 340);
            lblAnio.Text = "Año";
            txtAnio.Location = new Point(18, 360);
            txtAnio.Size = new Size(300, 27);

            lblCategoria.Location = new Point(18, 400);
            lblCategoria.Text = "Categoría";
            txtCategoria.Location = new Point(18, 420);
            txtCategoria.Size = new Size(300, 27);

            chkDisponible.Location = new Point(18, 460);
            chkDisponible.Text = "Disponible";

            chkActivoLibro.Location = new Point(150, 460);
            chkActivoLibro.Text = "Activo";

            // ======================== BOTONES ==========================
            btnRegistrar.Location = new Point(18, 510);
            btnRegistrar.Size = new Size(100, 40);
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Click += btnRegistrar_Click;

            btnActualizar.Location = new Point(125, 510);
            btnActualizar.Size = new Size(100, 40);
            btnActualizar.Text = "Actualizar";
            btnActualizar.Click += btnActualizar_Click;

            btnEliminar.Location = new Point(232, 510);
            btnEliminar.Size = new Size(100, 40);
            btnEliminar.Text = "Eliminar";
            btnEliminar.BackColor = Color.Firebrick;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Click += btnEliminar_Click;

            // ⭐ NUEVO BOTÓN ACTIVAR
            btnActivar.Location = new Point(18, 560);
            btnActivar.Size = new Size(314, 40);
            btnActivar.Text = "Activar libro";
            btnActivar.BackColor = Color.LightGreen;
            btnActivar.Click += btnActivar_Click;

            btnLimpiar.Location = new Point(18, 610);
            btnLimpiar.Size = new Size(314, 40);
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Click += btnLimpiar_Click;

            // ======================== ADD TO GROUPBOX ==========================
            grpLibro.Controls.Add(lblId);
            grpLibro.Controls.Add(txtIdLibro);
            grpLibro.Controls.Add(lblTitulo);
            grpLibro.Controls.Add(txtTitulo);
            grpLibro.Controls.Add(lblAutor);
            grpLibro.Controls.Add(txtAutor);
            grpLibro.Controls.Add(lblIsbn);
            grpLibro.Controls.Add(txtIsbn);
            grpLibro.Controls.Add(lblEditorial);
            grpLibro.Controls.Add(txtEditorial);
            grpLibro.Controls.Add(lblAnio);
            grpLibro.Controls.Add(txtAnio);
            grpLibro.Controls.Add(lblCategoria);
            grpLibro.Controls.Add(txtCategoria);
            grpLibro.Controls.Add(chkDisponible);
            grpLibro.Controls.Add(chkActivoLibro);
            grpLibro.Controls.Add(btnRegistrar);
            grpLibro.Controls.Add(btnActualizar);
            grpLibro.Controls.Add(btnEliminar);
            grpLibro.Controls.Add(btnActivar); 
            grpLibro.Controls.Add(btnLimpiar);

            // ======================== FORM ==========================
            ClientSize = new Size(1100, 770);
            Controls.Add(grpLibro);
            Controls.Add(btnRefrescar);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(gridLibros);
            Name = "LibroListadoForm";
            Load += LibroListadoForm_Load;

            ((System.ComponentModel.ISupportInitialize)gridLibros).EndInit();
            grpLibro.ResumeLayout(false);
            grpLibro.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
