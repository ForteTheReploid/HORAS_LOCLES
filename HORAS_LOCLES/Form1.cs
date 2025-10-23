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
        // HttpClient reutilizable
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public Form1()
        {
            InitializeComponent();
        }

        // El diseñador referencia este handler; lo dejamos vacío
        private void Form1_Load(object sender, EventArgs e)
        {
            // Intencionalmente vacío
        }

        // El diseñador referencia este handler; lo dejamos vacío
        private void label3_Click(object sender, EventArgs e)
        {
            // Intencionalmente vacío
        }

        // POST genérico al Apps Script; valida "OK"
        private static async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync(url, content);
            var text = (await resp.Content.ReadAsStringAsync())?.Trim();

            if (!resp.IsSuccessStatusCode || !string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Sheets webhook returned: " + text);
        }

        // Arma el payload mínimo (la hoja 'Usuarios' completa el resto)
        private async Task SendToSheetsAsync(string cedula, string observacion)
        {
            var url   = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var token = ConfigurationManager.AppSettings["SheetsToken"];
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Sheets webhook not configured (missing URL or token).");

            var payload = new
            {
                cedula    = (cedula ?? "").Trim(),
                mensaje   = (observacion ?? "").Trim(),
                // si prefieres usar SIEMPRE la hora del script, puedes quitar esta línea:
                fecha_ing = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                token     = token
            };

            await PostToGoogleAppsScriptAsync(url, payload);
        }

        // Botón "Marcar"
        private async void button1_Click(object sender, EventArgs e)
        {
            var cedula = (txt_cedula.Text ?? "").Trim();
            var observacion = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese Número de Cédula");
                return;
            }

            button1.Enabled = false;
            try
            {
                await SendToSheetsAsync(cedula, observacion);
                MessageBox.Show("Marcación registrada en Sheets.", "Marcaciones:",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                button1.Enabled = true;
            }
        }
    }
}
