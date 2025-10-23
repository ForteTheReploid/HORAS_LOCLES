using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HORAS_LOCLES
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        private async void button1_Click(object sender, EventArgs e)
        {
            // Validar entrada
            var cedula = (txt_cedula.Text ?? "").Trim();
            var observacion = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese Número de Cédula");
                return;
            }

            try
            {
                await SendToSheetsAsync(cedula, observacion);
                MessageBox.Show("Marcación registrada en Sheets.", "Marcaciones:", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar campos
                txt_cedula.Text = "";
                txt_observacion.Text = "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Sheets error: " + ex.Message);
                MessageBox.Show("No se pudo registrar en Sheets. Verifique su conexión e intente nuevamente.", "Marcaciones:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private static async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync(url, content);
            var text = (await resp.Content.ReadAsStringAsync())?.Trim();

            if (!resp.IsSuccessStatusCode || !string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Sheets webhook returned: " + text);
        }

        private async Task SendToSheetsAsync(string cedula, string observacion)
        {
            var url   = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var token = ConfigurationManager.AppSettings["SheetsToken"];

            var payload = new
            {
                cedula = (cedula ?? "").Trim(),
                mensaje = (observacion ?? "").Trim(),
                fecha_ing = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                token = token
            };

            await PostToGoogleAppsScriptAsync(url, payload);
        }
    }
}
