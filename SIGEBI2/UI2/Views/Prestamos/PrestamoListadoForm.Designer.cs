using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Prestamos
{
    partial class PrestamoListadoForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtUsuarioId;
        private Button btnBuscarPorUsuario;
        private Button btnVerVencidos;
        private Button btnRefrescar;

        private Label lblRegistrarTitulo;
        private TextBox txtUsuarioRegistrar;
        private TextBox txtLibroRegistrar;
        private Button btnRegistrarPrestamo;

        private DataGridView gridPrestamos;

        private Label lblResumen;
        private ListView lstDetalles;

        private Button btnDevolverPrestamo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();


            // FORM PRINCIPAL

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1250, 720);
            Text = "Gestión de Préstamos";

            // BUSCAR POR USUARIO

            txtUsuarioId = new TextBox();
            txtUsuarioId.Location = new Point(230, 30);
            txtUsuarioId.Size = new Size(150, 28);
            txtUsuarioId.PlaceholderText = "Id usuario (activos)";

            btnBuscarPorUsuario = new Button();
            btnBuscarPorUsuario.Location = new Point(390, 30);
            btnBuscarPorUsuario.Size = new Size(120, 30);
            btnBuscarPorUsuario.Text = "Ver activos";
            btnBuscarPorUsuario.Click += btnBuscarPorUsuario_Click;

            btnVerVencidos = new Button();
            btnVerVencidos.Location = new Point(520, 30);
            btnVerVencidos.Size = new Size(140, 30);
            btnVerVencidos.Text = "Préstamos vencidos";
            btnVerVencidos.Click += btnVerVencidos_Click;

            btnRefrescar = new Button();
            btnRefrescar.Location = new Point(670, 30);
            btnRefrescar.Size = new Size(100, 30);
            btnRefrescar.Text = "Refrescar";
            btnRefrescar.Click += btnRefrescar_Click;


            // GRID PRINCIPAL

            gridPrestamos = new DataGridView();
            gridPrestamos.Location = new Point(230, 80);
            gridPrestamos.Size = new Size(1000, 300);
            gridPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridPrestamos.ReadOnly = true;
            gridPrestamos.AllowUserToAddRows = false;
            gridPrestamos.AllowUserToDeleteRows = false;
            gridPrestamos.SelectionChanged += gridPrestamos_SelectionChanged;

            // BOTÓN DEVOLVER PRÉSTAMO 

            btnDevolverPrestamo = new Button();
            btnDevolverPrestamo.Text = "Devolver préstamo";
            btnDevolverPrestamo.Size = new Size(180, 32);
            btnDevolverPrestamo.Location = new Point(230, 390);
            btnDevolverPrestamo.Click += btnDevolverPrestamo_Click;


            // RESUMEN

            lblResumen = new Label();
            lblResumen.Location = new Point(230, 430);
            lblResumen.Size = new Size(800, 30);
            lblResumen.Font = new Font("Segoe UI", 10);
            lblResumen.Text = "Seleccione un préstamo para ver sus detalles.";


            // LISTA DE DETALLES

            lstDetalles = new ListView();
            lstDetalles.Location = new Point(230, 470);
            lstDetalles.Size = new Size(1000, 180);
            lstDetalles.View = View.Details;
            lstDetalles.FullRowSelect = true;
            lstDetalles.GridLines = true;


            // SECCIÓN REGISTRAR PRÉSTAMO

            lblRegistrarTitulo = new Label();
            lblRegistrarTitulo.Text = "Registrar préstamo";
            lblRegistrarTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblRegistrarTitulo.Location = new Point(30, 40);
            lblRegistrarTitulo.Size = new Size(200, 25);

            txtUsuarioRegistrar = new TextBox();
            txtUsuarioRegistrar.Location = new Point(30, 80);
            txtUsuarioRegistrar.Size = new Size(160, 28);
            txtUsuarioRegistrar.PlaceholderText = "Id Usuario";

            txtLibroRegistrar = new TextBox();
            txtLibroRegistrar.Location = new Point(30, 120);
            txtLibroRegistrar.Size = new Size(160, 28);
            txtLibroRegistrar.PlaceholderText = "Id Libro";

            btnRegistrarPrestamo = new Button();
            btnRegistrarPrestamo.Location = new Point(30, 160);
            btnRegistrarPrestamo.Size = new Size(160, 35);
            btnRegistrarPrestamo.Text = "Registrar";
            btnRegistrarPrestamo.Click += btnRegistrarPrestamo_Click;


            // AGREGAR TODO AL FORM

            Controls.Add(txtUsuarioId);
            Controls.Add(btnBuscarPorUsuario);
            Controls.Add(btnVerVencidos);
            Controls.Add(btnRefrescar);

            Controls.Add(gridPrestamos);
            Controls.Add(btnDevolverPrestamo);

            Controls.Add(lblResumen);
            Controls.Add(lstDetalles);

            Controls.Add(lblRegistrarTitulo);
            Controls.Add(txtUsuarioRegistrar);
            Controls.Add(txtLibroRegistrar);
            Controls.Add(btnRegistrarPrestamo);

            Load += PrestamoListadoForm_Load;
        }
    }
}
