using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Main
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelMenu = null!;
        private Panel panelContenido = null!;
        private Button btnUsuarios = null!;
        private Button btnLibros = null!;
        private Button btnPrestamos = null!;
        private Button btnCerrarSesion = null!;
        private Label lblBienvenida = null!;
        private Button btnConfiguracion;


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
            panelMenu = new Panel();
            panelContenido = new Panel();
            btnUsuarios = new Button();
            btnLibros = new Button();
            btnPrestamos = new Button();
            btnCerrarSesion = new Button();
            lblBienvenida = new Label();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(37, 41, 52);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 600);
            panelMenu.TabIndex = 0;
            panelMenu.Controls.Add(btnCerrarSesion);
            panelMenu.Controls.Add(btnPrestamos);
            panelMenu.Controls.Add(btnLibros);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(lblBienvenida);
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = false;
            lblBienvenida.ForeColor = Color.White;
            lblBienvenida.Location = new Point(16, 20);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(168, 48);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido";
            // 
            // btnUsuarios
            // 
            btnUsuarios.Location = new Point(16, 100);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Size = new Size(168, 36);
            btnUsuarios.TabIndex = 1;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnLibros
            // 
            btnLibros.Location = new Point(16, 146);
            btnLibros.Name = "btnLibros";
            btnLibros.Size = new Size(168, 36);
            btnLibros.TabIndex = 2;
            btnLibros.Text = "Libros";
            btnLibros.UseVisualStyleBackColor = true;
            btnLibros.Click += btnLibros_Click;
            // 
            // btnPrestamos
            // 
            btnPrestamos.Location = new Point(16, 192);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(168, 36);
            btnPrestamos.TabIndex = 3;
            btnPrestamos.Text = "Préstamos";
            btnPrestamos.UseVisualStyleBackColor = true;
            btnPrestamos.Click += btnPrestamos_Click;


            // BOTÓN CONFIGURACIÓN
            this.btnConfiguracion = new Button();
            this.btnConfiguracion.Text = "Configuración";
            this.btnConfiguracion.Size = new Size(150, 35);
            this.btnConfiguracion.Location = new Point(15, 250);
            this.btnConfiguracion.Click += btnConfiguracion_Click;
            this.Controls.Add(this.btnConfiguracion);

            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(16, 540);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(168, 32);
            btnCerrarSesion.TabIndex = 4;
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // panelContenido
            // 
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(200, 0);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(800, 600);
            panelContenido.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(panelContenido);
            Controls.Add(panelMenu);
            MinimumSize = new Size(1016, 639);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SIGEBI - Panel principal";
            ResumeLayout(false);
        }
    }
}