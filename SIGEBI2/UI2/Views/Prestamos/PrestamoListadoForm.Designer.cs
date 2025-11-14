using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Prestamos
{
    partial class PrestamoListadoForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblUsuarioId = null!;
        private TextBox txtUsuarioId = null!;
        private Button btnBuscarPorUsuario = null!;
        private Button btnVerVencidos = null!;
        private DataGridView gridPrestamos = null!;
        private ListView lstDetalles = null!;
        private ColumnHeader colTitulo = null!;
        private ColumnHeader colLibroId = null!;
        private ColumnHeader colEstado = null!;
        private Label lblDetalles = null!;
        private Label lblResumen = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblUsuarioId = new Label();
            txtUsuarioId = new TextBox();
            btnBuscarPorUsuario = new Button();
            btnVerVencidos = new Button();
            gridPrestamos = new DataGridView();
            lstDetalles = new ListView();
            colTitulo = new ColumnHeader();
            colLibroId = new ColumnHeader();
            colEstado = new ColumnHeader();
            lblDetalles = new Label();
            lblResumen = new Label();
            ((System.ComponentModel.ISupportInitialize)gridPrestamos).BeginInit();
            SuspendLayout();

            // lblUsuarioId
            lblUsuarioId.AutoSize = true;
            lblUsuarioId.Location = new Point(24, 24);
            lblUsuarioId.Name = "lblUsuarioId";
            lblUsuarioId.Size = new Size(119, 15);
            lblUsuarioId.TabIndex = 0;
            lblUsuarioId.Text = "Id usuario (activos)";

            // txtUsuarioId
            txtUsuarioId.Location = new Point(149, 21);
            txtUsuarioId.Name = "txtUsuarioId";
            txtUsuarioId.Size = new Size(80, 23);
            txtUsuarioId.TabIndex = 1;

            // btnBuscarPorUsuario
            btnBuscarPorUsuario.Location = new Point(243, 21);
            btnBuscarPorUsuario.Name = "btnBuscarPorUsuario";
            btnBuscarPorUsuario.Size = new Size(120, 24);
            btnBuscarPorUsuario.TabIndex = 2;
            btnBuscarPorUsuario.Text = "Ver activos";
            btnBuscarPorUsuario.UseVisualStyleBackColor = true;
            btnBuscarPorUsuario.Click += btnBuscarPorUsuario_Click;

            // btnVerVencidos
            btnVerVencidos.Location = new Point(380, 21);
            btnVerVencidos.Name = "btnVerVencidos";
            btnVerVencidos.Size = new Size(120, 24);
            btnVerVencidos.TabIndex = 3;
            btnVerVencidos.Text = "Préstamos vencidos";
            btnVerVencidos.UseVisualStyleBackColor = true;
            btnVerVencidos.Click += btnVerVencidos_Click;

            // gridPrestamos
            gridPrestamos.AllowUserToAddRows = false;
            gridPrestamos.AllowUserToDeleteRows = false;
            gridPrestamos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridPrestamos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridPrestamos.Location = new Point(24, 64);
            gridPrestamos.MultiSelect = false;
            gridPrestamos.Name = "gridPrestamos";
            gridPrestamos.ReadOnly = true;
            gridPrestamos.RowTemplate.Height = 25;
            gridPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridPrestamos.Size = new Size(840, 220);
            gridPrestamos.TabIndex = 4;
            gridPrestamos.SelectionChanged += gridPrestamos_SelectionChanged;
            // ❗ IMPORTANTE: columnas se agregan por código en el constructor, no aquí.

            // lblDetalles
            lblDetalles.AutoSize = true;
            lblDetalles.Location = new Point(24, 300);
            lblDetalles.Name = "lblDetalles";
            lblDetalles.Size = new Size(49, 15);
            lblDetalles.TabIndex = 5;
            lblDetalles.Text = "Detalles";

            // lblResumen
            lblResumen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResumen.Location = new Point(24, 520);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(840, 24);
            lblResumen.TabIndex = 6;
            lblResumen.Text = "Seleccione un préstamo para ver los detalles";

            // lstDetalles
            lstDetalles.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstDetalles.Columns.AddRange(new ColumnHeader[] { colTitulo, colLibroId, colEstado });
            lstDetalles.FullRowSelect = true;
            lstDetalles.GridLines = true;
            lstDetalles.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lstDetalles.Location = new Point(24, 320);
            lstDetalles.Name = "lstDetalles";
            lstDetalles.Size = new Size(840, 190);
            lstDetalles.TabIndex = 7;
            lstDetalles.UseCompatibleStateImageBehavior = false;
            lstDetalles.View = View.Details;

            colTitulo.Text = "Título del libro";
            colTitulo.Width = 400;

            colLibroId.Text = "Id libro";
            colLibroId.Width = 100;

            colEstado.Text = "Estado";
            colEstado.Width = 240;

            // PrestamoListadoForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblResumen);
            Controls.Add(lstDetalles);
            Controls.Add(lblDetalles);
            Controls.Add(gridPrestamos);
            Controls.Add(btnVerVencidos);
            Controls.Add(btnBuscarPorUsuario);
            Controls.Add(txtUsuarioId);
            Controls.Add(lblUsuarioId);
            Name = "PrestamoListadoForm";
            Size = new Size(888, 560);
            Load += PrestamoListadoForm_Load;
            ((System.ComponentModel.ISupportInitialize)gridPrestamos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
