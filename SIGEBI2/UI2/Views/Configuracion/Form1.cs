using UI2.AppConfig;
using UI2.Services.Interfaces;

namespace UI2.Views.Configuracion
{
    public partial class ConfiguracionForm : Form
    {
        private readonly IConfigurationApiService _configService;

        public ConfiguracionForm()
        {
            InitializeComponent();
            _configService = ServiceLocator.ConfigurationApiService;
        }

        private async void ConfiguracionForm_Load(object sender, EventArgs e)
        {
            var result = await _configService.ObtenerConfiguracionAsync();

            if (!result.Success || result.Data == null)
            {
                MessageBox.Show(result.Message ?? "No fue posible cargar la configuración.");
                return;
            }

            txtDias.Text = result.Data.Valor;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtDias.Text, out int dias) || dias <= 0)
            {
                MessageBox.Show("Ingrese un valor válido.");
                return;
            }

            var result = await _configService.ActualizarDiasAsync(dias);

            MessageBox.Show(result.Message);
        }
    }
}
