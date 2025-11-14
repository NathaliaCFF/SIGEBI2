namespace UI2.Views.Login
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Button btnIngresar = null!;
        private Label lblTitulo = null!;
        private Label lblEmail = null!;
        private Label lblPassword = null!;

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
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            lblTitulo = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitulo.Location = new Point(64, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(198, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "SIGEBI - Iniciar sesión";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(32, 70);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(115, 15);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Correo electrónico";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(32, 88);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "correo@instituto.edu";
            txtEmail.Size = new Size(260, 23);
            txtEmail.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(32, 124);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(67, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(32, 142);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(260, 23);
            txtPassword.TabIndex = 4;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(32, 188);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(260, 32);
            btnIngresar.TabIndex = 5;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 251);
            Controls.Add(btnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SIGEBI - Inicio de sesión";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}