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

            // ============================================================
            // BÚSQUEDA
            // ============================================================
            lblBusqueda.AutoSize = true;
            lblBusqueda.Location = new Point(24, 24);
            lblBusqueda.Name = "lblBusqueda";
            lblBusqueda.Size = new Size(49, 15);
            lblBusqueda.TabIndex = 0;
            lblBusqueda.Text = "Buscar";

            txtBusqueda.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBusqueda.Location = new Point(90, 21);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.PlaceholderText = "Nombre, correo, rol o estado (activo/inactivo)";
            txtBusqueda.Size = new Size(494, 23);
            txtBusqueda.TabIndex = 1;
            txtBusqueda.TextChanged += txtBusqueda_TextChanged;

            // ============================================================
            // DATA GRID VIEW
            // ============================================================
            gridUsuarios.AllowUserToAddRows = false;
            gridUsuarios.AllowUserToDeleteRows = false;
            gridUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridUsuarios.Location = new Point(24, 60);
            gridUsuarios.MultiSelect = false;
            gridUsuarios.Name = "gridUsuarios";
            gridUsuarios.ReadOnly = true;
            gridUsuarios.RowTemplate.Height = 25;
            gridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridUsuarios.Size = new Size(560, 344);
            gridUsuarios.TabIndex = 2;

            // Columnas agregadas por código en el constructor
            // (NO agregarlas aquí)

            gridUsuarios.SelectionChanged += gridUsuarios_SelectionChanged;

            // ============================================================
            // BOTÓN REFRESCAR
            // ============================================================
            btnRefrescar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRefrescar.Location = new Point(24, 420);
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(120, 32);
            btnRefrescar.TabIndex = 3;
            btnRefrescar.Text = "Cargar listado";
            btnRefrescar.UseVisualStyleBackColor = true;
            btnRefrescar.Click += btnRefrescar_Click;

            // ============================================================
            // GROUP BOX DE DATOS
            // ============================================================
            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
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
            grpDatos.Controls.Add(btnCrear);
            grpDatos.Controls.Add(btnActualizar);
            grpDatos.Controls.Add(btnActivar);
            grpDatos.Controls.Add(btnDesactivar);
            grpDatos.Location = new Point(604, 24);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(260, 428);
            grpDatos.TabIndex = 2;
            grpDatos.TabStop = false;
            grpDatos.Text = "Gestión de usuarios";

            // ============================================================
            // LABEL ID
            // ============================================================
            lblId.AutoSize = true;
            lblId.Location = new Point(16, 32);
            lblId.Name = "lblId";
            lblId.Size = new Size(21, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID";

            // TXT ID
            txtId.Location = new Point(16, 50);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(220, 23);
            txtId.TabIndex = 1;

            // ============================================================
            // NOMBRE
            // ============================================================
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 84);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(51, 15);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre";

            txtNombre.Location = new Point(16, 102);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(220, 23);
            txtNombre.TabIndex = 3;

            // ============================================================
            // EMAIL
            // ============================================================
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(16, 138);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(101, 15);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Correo electrónico";

            txtEmail.Location = new Point(16, 156);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(220, 23);
            txtEmail.TabIndex = 5;

            // ============================================================
            // CONTRASEÑA
            // ============================================================
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(16, 192);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(67, 15);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Contraseña";

            txtPassword.Location = new Point(16, 210);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(220, 23);
            txtPassword.TabIndex = 7;

            // ============================================================
            // ROL
            // ============================================================
            lblRol.AutoSize = true;
            lblRol.Location = new Point(16, 246);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(24, 15);
            lblRol.TabIndex = 8;
            lblRol.Text = "Rol";

            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FormattingEnabled = true;
            cmbRol.Items.AddRange(new object[] { "Administrador", "Bibliotecario", "Usuario" });
            cmbRol.Location = new Point(16, 264);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(220, 23);
            cmbRol.TabIndex = 9;

            // ============================================================
            // ACTIVO
            // ============================================================
            chkActivo.AutoSize = true;
            chkActivo.Checked = true;
            chkActivo.CheckState = CheckState.Checked;
            chkActivo.Location = new Point(16, 302);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(63, 19);
            chkActivo.TabIndex = 10;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;

            // ============================================================
            // BOTONES CRUD
            // ============================================================
            btnCrear.Location = new Point(16, 336);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(100, 32);
            btnCrear.TabIndex = 11;
            btnCrear.Text = "Registrar";
            btnCrear.Click += btnCrear_Click;

            btnActualizar.Location = new Point(136, 336);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(100, 32);
            btnActualizar.TabIndex = 12;
            btnActualizar.Text = "Actualizar";
            btnActualizar.Click += btnActualizar_Click;

            btnActivar.Location = new Point(16, 380);
            btnActivar.Name = "btnActivar";
            btnActivar.Size = new Size(100, 32);
            btnActivar.TabIndex = 13;
            btnActivar.Text = "Activar";
            btnActivar.Click += btnActivar_Click;

            btnDesactivar.Location = new Point(136, 380);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(100, 32);
            btnDesactivar.TabIndex = 14;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.Click += btnDesactivar_Click;

            // ============================================================
            // FORMULARIO GENERAL
            // ============================================================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grpDatos);
            Controls.Add(btnRefrescar);
            Controls.Add(gridUsuarios);
            Controls.Add(txtBusqueda);
            Controls.Add(lblBusqueda);
            Name = "UsuarioListadoForm";
            Size = new Size(888, 480);
            Load += UsuarioListadoForm_Load;

            ((System.ComponentModel.ISupportInitialize)gridUsuarios).EndInit();
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }
    }
}
