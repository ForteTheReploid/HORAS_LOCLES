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
        private void label3_Click(object sender, EventArgs e) { }

        // POST genérico al Apps Script: espera literalmente "OK"
        private static async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync(url, content);
            var text = (await resp.Content.ReadAsStringAsync())?.Trim();

            if (!resp.IsSuccessStatusCode || !string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Sheets webhook returned: " + text);
        }

        // Enviar marcación: tipo = entrada | salida | salida_partido | entrada_partido
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
                tipo    = (tipo ?? "entrada").ToLower(),
                token   = token
            };

            await PostToGoogleAppsScriptAsync(url, payload);
        }

        private void ToggleUI(bool enabled)
        {
            btnEntrada.Enabled = enabled;
            btnSalida.Enabled = enabled;
            btnSalidaPartido.Enabled = enabled;
            btnEntradaPartido.Enabled = enabled;
            txt_cedula.Enabled = enabled;
            txt_observacion.Enabled = enabled;
        }

        private void Mensaje(bool ok, string titulo)
        {
            if (ok)
                MessageBox.Show($"{titulo} registrada.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudo registrar en Sheets. Verifique su conexión e intente nuevamente.",
                    "Marcaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Flujo común (no usar returns dentro de finalmente)
        private async Task<bool> RegistrarMarcacionAsync(string tipo, string titulo)
        {
            var cedula = (txt_cedula.Text ?? "").Trim();
            var obs    = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese Número de Cédula");
                return false;
            }

            ToggleUI(false);
            try
            {
                await SendToSheetsAsync(cedula, obs, tipo);
                txt_cedula.Text = "";
                txt_observacion.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sheets error: " + ex.Message);
                return false;
            }
            finally
            {
                ToggleUI(true);
            }
        }

        // Handlers definitivos (asegúrate de que el Designer los tenga conectados)
        private async void btnEntrada_Click(object sender, EventArgs e)
        {
            var ok = await RegistrarMarcacionAsync("entrada", "Entrada");
            Mensaje(ok, "Entrada");
        }

        private async void btnSalida_Click(object sender, EventArgs e)
        {
            var ok = await RegistrarMarcacionAsync("salida", "Salida");
            Mensaje(ok, "Salida");
        }

        private async void btnSalidaPartido_Click(object sender, EventArgs e)
        {
            var ok = await RegistrarMarcacionAsync("salida_partido", "Salida Turno partido");
            Mensaje(ok, "Salida Turno partido");
        }

        private async void btnEntradaPartido_Click(object sender, EventArgs e)
        {
            var ok = await RegistrarMarcacionAsync("entrada_partido", "Entrada Turno partido");
            Mensaje(ok, "Entrada Turno partido");
        }
    }
}

