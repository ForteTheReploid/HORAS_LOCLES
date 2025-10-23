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
        // HttpClient reutilizable para toda la app
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public Form1()
        {
            InitializeComponent();
        }

        // Stubs requeridos por el diseñador
        private void Form1_Load(object sender, EventArgs e)
        {
            // Intencionalmente vacío
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Intencionalmente vacío
        }

        // POST genérico al Apps Script; valida que la respuesta sea "OK"
        private static async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync(url, content);
            var text = (await resp.Content.ReadAsStringAsync())?.Trim();

            if (!resp.IsSuccessStatusCode || !string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Sheets webhook returned: " + text);
        }

        // Arma el payload para el webhook (tipo = "entrada" | "salida")
        private async Task SendToSheetsAsync(string cedula, string observacion, string tipo)
        {
            var url   = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var token = ConfigurationManager.AppSettings["SheetsToken"];

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Sheets webhook not configured (missing URL or token).");

            var payload = new
            {
                cedula  = (cedula ?? "").Trim(),
                mensaje = (observacion ?? "").Trim(),
                tipo    = (tipo ?? "entrada").ToLower(), // "entrada" o "salida"
                token   = token
            };

            Debug.WriteLine($"POST -> {url}  tipo={payload.tipo}  token='{token}'");
            await PostToGoogleAppsScriptAsync(url, payload);
        }

        // Flujo común para ambos botones
        private async Task EnviarMarcacionAsync(string tipo)
        {
            var cedula = (txt_cedula.Text ?? "").Trim();
            var obs    = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese Número de Cédula");
                return;
            }

            btnEntrada.Enabled = btnSalida.Enabled = false;
            try
            {
                await SendToSheetsAsync(cedula, obs, tipo);
                MessageBox.Show($"Marcación de {tipo} registrada en Sheets.", "Marcaciones:",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpieza de campos
                txt_cedula.Text = "";
                txt_observacion.Text = "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sheets error: " + ex.Message);
                MessageBox.Show("No se pudo registrar en Sheets. Verifique su conexión e intente nuevamente.",
                    "Marcaciones:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnEntrada.Enabled = btnSalida.Enabled = true;
            }
        }

        // Handlers de los botones (ya conectados en el Designer)
        private async void btnEntrada_Click(object sender, EventArgs e) => await EnviarMarcacionAsync("entrada");
        private async void btnSalida_Click(object sender, EventArgs e)  => await EnviarMarcacionAsync("salida");
    }
}
