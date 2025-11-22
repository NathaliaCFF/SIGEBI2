using System.Drawing;
using System.Windows.Forms;

namespace UI2.Views.Configuracion
{
    partial class ConfiguracionForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitulo;
        private Label lblDias;
        private TextBox txtDias;
        private Button btnGuardar;

        /// <summary>
        /// Limpieza de recursos
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Inicialización del formulario
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            lblTitulo = new Label();
            lblDias = new Label();
            txtDias = new TextBox();
            btnGuardar = new Button();

            // ==========================================================
            // lblTitulo
            // ==========================================================
            lblTitulo.Text = "Configuración del Sistema";
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Size = new Size(350, 30);

            // ==========================================================
            // lblDias
            // ==========================================================
            lblDias.Text = "Duración estándar de préstamo (días):";
            lblDias.Location = new Point(30, 80);
            lblDias.Size = new Size(260, 25);

            // ==========================================================
            // txtDias
            // ==========================================================
            txtDias.Location = new Point(300, 80);
            txtDias.Size = new Size(100, 28);

            // ==========================================================
            // btnGuardar
            // ==========================================================
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(30, 130);
            btnGuardar.Size = new Size(120, 35);
            btnGuardar.Click += btnGuardar_Click;

            // ==========================================================
            // ConfiguracionForm
            // ==========================================================
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(450, 200);
            Controls.Add(lblTitulo);
            Controls.Add(lblDias);
            Controls.Add(txtDias);
            Controls.Add(btnGuardar);

            Name = "ConfiguracionForm";
            Text = "Configuración";
            Load += ConfiguracionForm_Load;


        }
    }
}

