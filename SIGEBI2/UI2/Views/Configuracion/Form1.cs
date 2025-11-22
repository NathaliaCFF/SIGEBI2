using SIGEBI.Application.Buses.Configuration;
using UI2.AppConfig;

namespace UI2.Views.Configuracion
{
    public partial class ConfiguracionForm : Form
    {
        private readonly GetConfiguracionHandler _getHandler;
        private readonly UpdateConfiguracionHandler _updateHandler;

        public ConfiguracionForm()
        {
            InitializeComponent();

            _getHandler = ServiceLocator.GetConfiguracionHandler;
            _updateHandler = ServiceLocator.UpdateConfiguracionHandler;

        }

        private async void ConfiguracionForm_Load(object sender, EventArgs e)
        {
            var result = await _getHandler.Handle();

            if (!result.Success || result.Data == null)
            {
                MessageBox.Show("No fue posible cargar la configuración.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtDias.Text = result.Data.DuracionPrestamoDias.ToString();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtDias.Text, out int dias) || dias <= 0)
            {
                MessageBox.Show("Ingrese un número válido mayor que 0.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cmd = new UpdateConfiguracionCommand { Dias = dias };
            var result = await _updateHandler.Handle(cmd);

            if (!result.Success)
            {
                MessageBox.Show(result.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Configuración actualizada correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
