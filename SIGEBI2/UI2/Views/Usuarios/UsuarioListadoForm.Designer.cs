using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Usuarios
{
    partial class UsuarioListadoForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView gridUsuarios;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.Button btnDesactivar;

        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.CheckBox chkActivo;

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblRol;

        private System.Windows.Forms.GroupBox grpDatos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.gridUsuarios = new System.Windows.Forms.DataGridView();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnActivar = new System.Windows.Forms.Button();
            this.btnDesactivar = new System.Windows.Forms.Button();

            this.txtId = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();

            this.lblId = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();

            this.grpDatos = new System.Windows.Forms.GroupBox();

            ((System.ComponentModel.ISupportInitialize)(this.gridUsuarios)).BeginInit();
            this.grpDatos.SuspendLayout();
            this.SuspendLayout();

            // GRID
            this.gridUsuarios.AllowUserToAddRows = false;
            this.gridUsuarios.AllowUserToDeleteRows = false;
            this.gridUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.gridUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUsuarios.Location = new Point(24, 24);
            this.gridUsuarios.MultiSelect = false;
            this.gridUsuarios.Name = "gridUsuarios";
            this.gridUsuarios.ReadOnly = true;
            this.gridUsuarios.RowTemplate.Height = 25;
            this.gridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridUsuarios.Size = new Size(560, 380);
            this.gridUsuarios.SelectionChanged += gridUsuarios_SelectionChanged;

            // BOTÓN REFRESCAR
            this.btnRefrescar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnRefrescar.Location = new Point(24, 420);
            this.btnRefrescar.Size = new Size(120, 32);
            this.btnRefrescar.Text = "Cargar listado";
            this.btnRefrescar.Click += btnRefrescar_Click;

            // GROUP BOX
            this.grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.grpDatos.Controls.Add(this.lblId);
            this.grpDatos.Controls.Add(this.txtId);
            this.grpDatos.Controls.Add(this.lblNombre);
            this.grpDatos.Controls.Add(this.txtNombre);
            this.grpDatos.Controls.Add(this.lblEmail);
            this.grpDatos.Controls.Add(this.txtEmail);
            this.grpDatos.Controls.Add(this.lblPassword);
            this.grpDatos.Controls.Add(this.txtPassword);
            this.grpDatos.Controls.Add(this.lblRol);
            this.grpDatos.Controls.Add(this.cmbRol);
            this.grpDatos.Controls.Add(this.chkActivo);
            this.grpDatos.Controls.Add(this.btnCrear);
            this.grpDatos.Controls.Add(this.btnActualizar);
            this.grpDatos.Controls.Add(this.btnActivar);
            this.grpDatos.Controls.Add(this.btnDesactivar);
            this.grpDatos.Location = new Point(604, 24);
            this.grpDatos.Size = new Size(260, 428);
            this.grpDatos.Text = "Gestión de usuarios";

            // LABELS + TEXTBOXES (ID)
            this.lblId.Location = new Point(16, 32);
            this.lblId.Text = "ID";

            this.txtId.Location = new Point(16, 50);
            this.txtId.ReadOnly = true;

            // Nombre
            this.lblNombre.Location = new Point(16, 84);
            this.lblNombre.Text = "Nombre";

            this.txtNombre.Location = new Point(16, 102);

            // Email
            this.lblEmail.Location = new Point(16, 138);
            this.lblEmail.Text = "Correo";

            this.txtEmail.Location = new Point(16, 156);

            // Password
            this.lblPassword.Location = new Point(16, 192);
            this.lblPassword.Text = "Contraseña";

            this.txtPassword.Location = new Point(16, 210);
            this.txtPassword.PasswordChar = '*';

            // ROL
            this.lblRol.Location = new Point(16, 246);
            this.lblRol.Text = "Rol";

            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "Administrador", "Bibliotecario", "Usuario" });
            this.cmbRol.Location = new Point(16, 264);

            // Activo
            this.chkActivo.Location = new Point(16, 302);
            this.chkActivo.Text = "Activo";
            this.chkActivo.Checked = true;

            // Botones CRUD
            this.btnCrear.Location = new Point(16, 336);
            this.btnCrear.Size = new Size(100, 32);
            this.btnCrear.Text = "Registrar";
            this.btnCrear.Click += btnCrear_Click;

            this.btnActualizar.Location = new Point(136, 336);
            this.btnActualizar.Size = new Size(100, 32);
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.Click += btnActualizar_Click;

            this.btnActivar.Location = new Point(16, 380);
            this.btnActivar.Size = new Size(100, 32);
            this.btnActivar.Text = "Activar";
            this.btnActivar.Click += btnActivar_Click;

            this.btnDesactivar.Location = new Point(136, 380);
            this.btnDesactivar.Size = new Size(100, 32);
            this.btnDesactivar.Text = "Desactivar";
            this.btnDesactivar.Click += btnDesactivar_Click;

            // FORM CONFIG
            this.ClientSize = new Size(888, 480);
            this.Controls.Add(this.gridUsuarios);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.grpDatos);
            this.Name = "UsuarioListadoForm";
            this.Load += UsuarioListadoForm_Load;

            ((System.ComponentModel.ISupportInitialize)(this.gridUsuarios)).EndInit();
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
