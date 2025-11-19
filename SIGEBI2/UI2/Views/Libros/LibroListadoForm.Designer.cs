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
        private Button btnLimpiar = null!;
        private Label lblId = null!;
        private Label lblTitulo = null!;
        private Label lblAutor = null!;
        private Label lblIsbn = null!;
        private Label lblEditorial = null!;
        private Label lblAnio = null!;
        private Label lblCategoria = null!;

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;

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
            btnLimpiar = new Button();

            ((System.ComponentModel.ISupportInitialize)gridLibros).BeginInit();
            grpLibro.SuspendLayout();
            SuspendLayout();

            // gridLibros
            gridLibros.AllowUserToAddRows = false;
            gridLibros.AllowUserToDeleteRows = false;
            gridLibros.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridLibros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridLibros.Location = new Point(27, 85);
            gridLibros.Name = "gridLibros";
            gridLibros.ReadOnly = true;
            gridLibros.RowTemplate.Height = 25;
            gridLibros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridLibros.Size = new Size(640, 480);
            gridLibros.SelectionChanged += gridLibros_SelectionChanged;

            // txtBuscar
            txtBuscar.Location = new Point(27, 32);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar por título o autor";
            txtBuscar.Size = new Size(319, 27);

            // btnBuscar
            btnBuscar.Location = new Point(366, 32);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(103, 32);
            btnBuscar.Text = "Buscar";
            btnBuscar.Click += btnBuscar_Click;

            // btnRefrescar
            btnRefrescar.Location = new Point(480, 32);
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(103, 32);
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.Click += btnRefrescar_Click;

            // Groupbox
            grpLibro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
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
            grpLibro.Controls.Add(btnLimpiar);
            grpLibro.Location = new Point(690, 32);
            grpLibro.Size = new Size(297, 613);
            grpLibro.Text = "Gestión de libros";

            // Labels + Textboxes
            lblId.Location = new Point(18, 43);
            lblId.Text = "ID";

            txtIdLibro.Location = new Point(18, 67);
            txtIdLibro.ReadOnly = true;
            txtIdLibro.Size = new Size(251, 27);

            lblTitulo.Location = new Point(18, 107);
            lblTitulo.Text = "Título";

            txtTitulo.Location = new Point(18, 131);
            txtTitulo.Size = new Size(251, 27);

            lblAutor.Location = new Point(18, 176);
            lblAutor.Text = "Autor";

            txtAutor.Location = new Point(18, 200);
            txtAutor.Size = new Size(251, 27);

            lblIsbn.Location = new Point(18, 245);
            lblIsbn.Text = "ISBN";

            txtIsbn.Location = new Point(18, 269);
            txtIsbn.Size = new Size(251, 27);

            lblEditorial.Location = new Point(18, 315);
            lblEditorial.Text = "Editorial";

            txtEditorial.Location = new Point(18, 339);
            txtEditorial.Size = new Size(251, 27);

            lblAnio.Location = new Point(18, 384);
            lblAnio.Text = "Año";

            txtAnio.Location = new Point(18, 408);
            txtAnio.Size = new Size(251, 27);

            lblCategoria.Location = new Point(18, 453);
            lblCategoria.Text = "Categoría";

            txtCategoria.Location = new Point(18, 477);
            txtCategoria.Size = new Size(251, 27);

            chkDisponible.Location = new Point(18, 523);
            chkDisponible.Text = "Disponible";
            chkDisponible.Checked = true;

            chkActivoLibro.Location = new Point(137, 523);
            chkActivoLibro.Text = "Activo";
            chkActivoLibro.Checked = true;

            btnRegistrar.Location = new Point(18, 565);
            btnRegistrar.Size = new Size(82, 37);
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Click += btnRegistrar_Click;

            btnActualizar.Location = new Point(107, 565);
            btnActualizar.Size = new Size(91, 37);
            btnActualizar.Text = "Actualizar";
            btnActualizar.Click += btnActualizar_Click;

            btnLimpiar.Location = new Point(206, 565);
            btnLimpiar.Size = new Size(64, 37);
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.Click += btnLimpiar_Click;

            // Form config
            ClientSize = new Size(994, 577);
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
