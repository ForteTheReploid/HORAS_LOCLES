using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HORAS_LOCLES
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private static string TraducirError(string error)
        {
            return error switch
            {
                "ERR_TOKEN" => "Token incorrecto. Revise la configuración del sistema.",
                "ERR_CEDULA_REQUERIDA" => "Debe ingresar la cédula.",
                "ERR_CLAVE_REQUERIDA" => "Debe ingresar la clave.",
                "ERR_USUARIO_NO_EXISTE" => "La cédula no existe en la hoja Usuarios.",
                "ERR_CLAVE_INVALIDA" => "La clave ingresada no es correcta.",
                "ERR_SIN_ENTRADA" => "Primero debe registrar la entrada del día.",
                "ERR_HOJA_NO_EXISTE" => "No existe la hoja Mark01 en el archivo de Google Sheets.",
                "ERR_TIPO_DESCONOCIDO" => "Tipo de marcación no reconocido.",
                "ERR_GENERAL" => "Ocurrió un error general en el Apps Script.",
                _ => "No se pudo registrar la marcación. Respuesta del servidor: " + error
            };
        }

        private static async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync(url, content);
            var text = (await resp.Content.ReadAsStringAsync())?.Trim() ?? "";

            if (!resp.IsSuccessStatusCode)
                throw new Exception("No hubo conexión correcta con Google Apps Script.");

            if (!string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                throw new Exception(TraducirError(text));
        }

        private async Task SendToSheetsAsync(string cedula, string clave, string observacion, string tipo)
        {
            var url = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var token = ConfigurationManager.AppSettings["SheetsToken"];

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Sheets webhook not configured. Falta URL o token.");

            var payload = new
            {
                cedula = (cedula ?? "").Trim(),
                clave = (clave ?? "").Trim(),
                mensaje = (observacion ?? "").Trim(),
                tipo = (tipo ?? "entrada").ToLower(),
                token = token
            };

            await PostToGoogleAppsScriptAsync(url, payload);
        }

        private void ToggleUI(bool enabled)
        {
            btnEntrada.Enabled = enabled;
            btnSalida.Enabled = enabled;
            btnSalidaPartido.Enabled = enabled;
            btnEntradaPartido.Enabled = enabled;
            btnAlmuerzoSalida.Enabled = enabled;
            btnAlmuerzoEntrada.Enabled = enabled;
            txt_cedula.Enabled = enabled;
            txt_clave.Enabled = enabled;
            txt_observacion.Enabled = enabled;
        }

        private async Task RegistrarMarcacionAsync(string tipo, string titulo)
        {
            var cedula = (txt_cedula.Text ?? "").Trim();
            var clave = (txt_clave.Text ?? "").Trim();
            var obs = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese número de cédula.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Ingrese la clave.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToggleUI(false);

            try
            {
                await SendToSheetsAsync(cedula, clave, obs, tipo);

                txt_cedula.Text = "";
                txt_clave.Text = "";
                txt_observacion.Text = "";

                MessageBox.Show($"{titulo} registrada.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sheets error: " + ex.Message);

                MessageBox.Show(ex.Message, "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                ToggleUI(true);
            }
        }

        private async void btnEntrada_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("entrada", "Entrada");
        }

        private async void btnSalida_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("salida", "Salida");
        }

        private async void btnSalidaPartido_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("salida_partido", "Salida Turno partido");
        }

        private async void btnEntradaPartido_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("entrada_partido", "Entrada Turno partido");
        }

        private async void btnAlmuerzoSalida_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("almuerzo_salida", "Almuerzo Salida");
        }

        private async void btnAlmuerzoEntrada_Click(object sender, EventArgs e)
        {
            await RegistrarMarcacionAsync("almuerzo_entrada", "Almuerzo Entrada");
        }
    }
}
