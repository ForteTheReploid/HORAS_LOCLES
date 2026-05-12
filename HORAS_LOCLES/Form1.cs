using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
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

        private static string ObtenerMachineGuid()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
                return (key?.GetValue("MachineGuid")?.ToString() ?? "").Trim();
            }
            catch
            {
                return "";
            }
        }

        private static string ObtenerNombreEquipo()
        {
            return Environment.MachineName;
        }

        private static string ObtenerUsuarioWindows()
        {
            return Environment.UserName;
        }

        private static string TraducirError(string error)
        {
            return error switch
            {
                "ERR_TOKEN" => "Token incorrecto. Revise la configuración del sistema.",
                "ERR_CEDULA_REQUERIDA" => "Debe ingresar la cédula.",
                "ERR_TOKEN_TOTP_REQUERIDO" => "Debe ingresar el código de Google Authenticator.",
                "ERR_TOTP_NO_CONFIGURADO" => "Este trabajador no tiene configurado Google Authenticator.",
                "ERR_TOKEN_TOTP_INVALIDO" => "El código de Google Authenticator no es correcto o ya expiró.",
                "ERR_USUARIO_NO_EXISTE" => "La cédula no existe en la hoja Usuarios.",
                "ERR_SIN_ENTRADA" => "Primero debe registrar la entrada del día.",
                "ERR_HOJA_NO_EXISTE" => "No existe la hoja Mark01 en el archivo de Google Sheets.",
                "ERR_TIPO_DESCONOCIDO" => "Tipo de marcación no reconocido.",
                "ERR_EQUIPO_NO_IDENTIFICADO" => "No se pudo identificar esta computadora.",
                "ERR_EQUIPO_NO_AUTORIZADO" => "Esta computadora no está autorizada para registrar marcaciones.",
                "ERR_EQUIPO_INACTIVO" => "Esta computadora está registrada, pero se encuentra inactiva.",
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

        private async Task SendToSheetsAsync(string cedula, string tokenTotp, string observacion, string tipo)
        {
            var url = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var tokenSistema = ConfigurationManager.AppSettings["SheetsToken"];

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(tokenSistema))
                throw new InvalidOperationException("Sheets webhook not configured. Falta URL o token.");

            var payload = new
            {
                cedula = (cedula ?? "").Trim(),
                token_totp = (tokenTotp ?? "").Trim(),
                mensaje = (observacion ?? "").Trim(),
                tipo = (tipo ?? "entrada").ToLower(),
                token = tokenSistema,
                equipo_id = ObtenerMachineGuid(),
                nombre_equipo = ObtenerNombreEquipo(),
                usuario_windows = ObtenerUsuarioWindows()
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
            txt_token.Enabled = enabled;
            txt_observacion.Enabled = enabled;
        }

        private async Task RegistrarMarcacionAsync(string tipo, string titulo)
        {
            var cedula = (txt_cedula.Text ?? "").Trim();
            var tokenTotp = (txt_token.Text ?? "").Trim();
            var obs = (txt_observacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Ingrese número de cédula.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tokenTotp))
            {
                MessageBox.Show("Ingrese el código de Google Authenticator.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tokenTotp.Length != 6)
            {
                MessageBox.Show("El código de Google Authenticator debe tener 6 dígitos.", "Marcaciones",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToggleUI(false);

            try
            {
                await SendToSheetsAsync(cedula, tokenTotp, obs, tipo);

                txt_cedula.Text = "";
                txt_token.Text = "";
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
