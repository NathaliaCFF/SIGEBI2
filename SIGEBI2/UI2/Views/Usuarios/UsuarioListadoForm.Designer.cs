using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Usuarios
{
    partial class UsuarioListadoForm
    {
        private System.ComponentModel.IContainer components = null!;

        private Label lblBusqueda = null!;
        private TextBox txtBusqueda = null!;
        private DataGridView gridUsuarios = null!;
        private Button btnRefrescar = null!;
        private Button btnNuevo = null!;
        private Button btnCrear = null!;
        private Button btnActualizar = null!;
        private Button btnActivar = null!;
        private Button btnDesactivar = null!;

        private TextBox txtId = null!;
        private TextBox txtNombre = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private ComboBox cmbRol = null!;
        private CheckBox chkActivo = null!;

        private Label lblId = null!;
        private Label lblNombre = null!;
        private Label lblEmail = null!;
        private Label lblPassword = null!;
        private Label lblRol = null!;

        private GroupBox grpDatos = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            lblBusqueda = new Label();
            txtBusqueda = new TextBox();
            gridUsuarios = new DataGridView();
            btnRefrescar = new Button();

            btnNuevo = new Button();
            btnCrear = new Button();
            btnActualizar = new Button();
            btnActivar = new Button();
            btnDesactivar = new Button();

            txtId = new TextBox();
            txtNombre = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            cmbRol = new ComboBox();
            chkActivo = new CheckBox();

            lblId = new Label();
            lblNombre = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            lblRol = new Label();

            grpDatos = new GroupBox();

            ((System.ComponentModel.ISupportInitialize)gridUsuarios).BeginInit();
            grpDatos.SuspendLayout();
            SuspendLayout();


            // BUSQUEDA

            lblBusqueda.AutoSize = true;
            lblBusqueda.Location = new Point(24, 24);
            lblBusqueda.Text = "Buscar";

            txtBusqueda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBusqueda.Location = new Point(90, 21);
            txtBusqueda.Size = new Size(494, 23);
            txtBusqueda.PlaceholderText = "Nombre, correo, rol o estado";
            txtBusqueda.TextChanged += txtBusqueda_TextChanged;

            // GRID

            gridUsuarios.AllowUserToAddRows = false;
            gridUsuarios.AllowUserToDeleteRows = false;
            gridUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridUsuarios.Location = new Point(24, 60);
            gridUsuarios.Size = new Size(560, 344);
            gridUsuarios.ReadOnly = true;
            gridUsuarios.MultiSelect = false;
            gridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridUsuarios.SelectionChanged += gridUsuarios_SelectionChanged;

            // BOTÓN REFRESCAR

            btnRefrescar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRefrescar.Location = new Point(24, 420);
            btnRefrescar.Size = new Size(560, 32);
            btnRefrescar.Text = "Cargar listado";
            btnRefrescar.Click += btnRefrescar_Click;

            // GROUPBOX

            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpDatos.Location = new Point(604, 24);

            grpDatos.Size = new Size(260, 460);

            grpDatos.Text = "Gestión de usuarios";

            // ID
            lblId.AutoSize = true;
            lblId.Location = new Point(16, 32);
            lblId.Text = "ID";
            txtId.Location = new Point(16, 50);
            txtId.Size = new Size(220, 23);
            txtId.ReadOnly = true;

            // NOMBRE
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 84);
            lblNombre.Text = "Nombre";
            txtNombre.Location = new Point(16, 102);
            txtNombre.Size = new Size(220, 23);

            // EMAIL
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(16, 138);
            lblEmail.Text = "Correo electrónico";
            txtEmail.Location = new Point(16, 156);
            txtEmail.Size = new Size(220, 23);

            // PASSWORD
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 192);
            lblPassword.Text = "Contraseña";
            txtPassword.Location = new Point(16, 210);
            txtPassword.Size = new Size(220, 23);
            txtPassword.PasswordChar = '*';

            // ROL
            lblRol.AutoSize = true;
            lblRol.Location = new Point(16, 246);
            lblRol.Text = "Rol";
            cmbRol.Location = new Point(16, 264);
            cmbRol.Size = new Size(220, 23);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.AddRange(new object[] { "Administrador", "Bibliotecario", "Usuario" });

            // ACTIVO
            chkActivo.AutoSize = true;
            chkActivo.Location = new Point(16, 302);
            chkActivo.Text = "Activo";
            chkActivo.Checked = true;

            // BOTÓN NUEVO
            btnNuevo.Location = new Point(16, 330);
            btnNuevo.Size = new Size(220, 28);
            btnNuevo.Text = "Nuevo";
            btnNuevo.Click += btnNuevo_Click;

            // BOTÓN CREAR
            btnCrear.Location = new Point(16, 364);
            btnCrear.Size = new Size(100, 32);
            btnCrear.Text = "Registrar";
            btnCrear.Click += btnCrear_Click;

            // BOTÓN ACTUALIZAR
            btnActualizar.Location = new Point(136, 364);
            btnActualizar.Size = new Size(100, 32);
            btnActualizar.Text = "Actualizar";
            btnActualizar.Click += btnActualizar_Click;

            // BOTÓN ACTIVAR
            btnActivar.Location = new Point(16, 402);
            btnActivar.Size = new Size(100, 32);
            btnActivar.Text = "Activar";
            btnActivar.Click += btnActivar_Click;

            // BOTÓN DESACTIVAR
            btnDesactivar.Location = new Point(136, 402);
            btnDesactivar.Size = new Size(100, 32);
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.Click += btnDesactivar_Click;


            // AGREGAR AL GRUPO

            grpDatos.Controls.Add(lblId);
            grpDatos.Controls.Add(txtId);
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombre);
            grpDatos.Controls.Add(lblEmail);
            grpDatos.Controls.Add(txtEmail);
            grpDatos.Controls.Add(lblPassword);
            grpDatos.Controls.Add(txtPassword);
            grpDatos.Controls.Add(lblRol);
            grpDatos.Controls.Add(cmbRol);
            grpDatos.Controls.Add(chkActivo);

            grpDatos.Controls.Add(btnNuevo);
            grpDatos.Controls.Add(btnCrear);
            grpDatos.Controls.Add(btnActualizar);
            grpDatos.Controls.Add(btnActivar);
            grpDatos.Controls.Add(btnDesactivar);


            // FORM

            Controls.Add(grpDatos);
            Controls.Add(btnRefrescar);
            Controls.Add(gridUsuarios);
            Controls.Add(txtBusqueda);
            Controls.Add(lblBusqueda);

            Name = "UsuarioListadoForm";
            Size = new Size(888, 480);


            this.MinimumSize = new Size(900, 520);

            Load += UsuarioListadoForm_Load;

            ((System.ComponentModel.ISupportInitialize)gridUsuarios).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
