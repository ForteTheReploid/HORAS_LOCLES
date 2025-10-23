using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq.Expressions;
using Npgsql; //Npgsql .NET Data Provider for PostgreSQL

namespace HORAS_LOCLES
{
    public partial class Form1 : Form
    {
        String cedula_db = "";
        String usuario_db = "";
        String local_db = "";
        String fecha_db = "";
        String hora_db = "";

        NpgsqlConnection connection;

        //private const string CONNECTION_STRING = "Host=101.101.101.224:5432;" +
        //    "Username=postgres;" +
        //    "Password=valeria2005;" +
        //    "Database=appbyrondb";

        private const string CONNECTION_STRING = "Host=164.90.148.158:5432;" +
            "Username=byron;" +
            "Password=valeria2005;" +
            "Database=registros";

        //"Server=101.101.101.224;User Id=postgres;" + "Password=valeria2005;Database=appbyrondb;"


        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void DapperBoardGameRepository()
        {
            try {

                connection = new NpgsqlConnection(CONNECTION_STRING);
                connection.Open();
            } 
            catch(Exception e)
            {
                MessageBox.Show("No Existe coneccon con el Servidor");
                throw;
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Copias antes de que insert_maraccion() limpie los TextBox
            var cedulaCopia = (txt_cedula.Text ?? "").Trim();
            var observacionCopia = (txt_observacion.Text ?? "").Trim();

            bool dbOk = false;
            string horaParaSheets = null;

            try
            {
                DapperBoardGameRepository();

                // Intentar obtener hora del servidor (si falla, usaremos hora local)
                try
                {
                    buscar_usuario_hora();             // llena hora_db (server)
                    horaParaSheets = hora_db;
                }
                catch
                {
                    // ignorar, se pone hora local abajo
                }

                // Insertar en BD
                dbOk = await insert_maraccion();
            }
            catch
            {
                // Error de conexión (o similar): dejamos que el MessageBox existente informe
            }

            // Si no tenemos hora del server, usamos hora local formateada
            if (string.IsNullOrWhiteSpace(horaParaSheets))
                horaParaSheets = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Enviar SIEMPRE a Sheets (tanto si BD ok como si falló), sin bloquear el flujo
            try
            {
                var usuarioWindows = Environment.UserName;

                var url = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
                var token = ConfigurationManager.AppSettings["SheetsToken"];
                if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(token))
                {
                    await SendToSheetsAsync(usuarioWindows, cedulaCopia, observacionCopia, horaParaSheets);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Sheets webhook not configured (missing URL or token).");
                }
            }
            catch (Exception exSheets)
            {
                System.Diagnostics.Debug.WriteLine("Sheets error: " + exSheets.Message);
            }
        }
        catch (Exception exSheets)
        {
            System.Diagnostics.Debug.WriteLine("Sheets error: " + exSheets.Message);
        }
    }
    catch (Exception)
    {
        MessageBox.Show("Consulte con el Proveedor");
    }
}

        private async Task<bool> insert_maraccion()
        {
            try
            {
                int num_hora = Convert.ToInt16(hora_db.Substring(0, 2));
                if (txt_cedula.Text != "")
                {
                    string commandText = "";

                    if (num_hora >= 0 && num_hora <= 4)
                    {
                        commandText = $"INSERT INTO public.registros_pruebas (usuario, cedula, local, fecha_ing_madrugada, hora_ing_madrugada, observacion, estatus, numero_marcacion, fechahora) SELECT usuario, cedula, local, current_date ,LOCALTIME, @observacion, 'A', 2, now() FROM public.usuarios_pruebas WHERE  cedula = @cedula;";

                    }
                    else
                    {
                        commandText = $"INSERT INTO public.registros_pruebas (usuario, cedula, local, fecha_ing, hora_ing, observacion, estatus, numero_marcacion ,fechahora) SELECT usuario, cedula, local, current_date ,LOCALTIME, @observacion, 'A', 1, now() FROM public.usuarios_pruebas WHERE  cedula = @cedula;";

                    }
                    await using (var cmd = new NpgsqlCommand(commandText, connection))
                    {
                        cmd.Parameters.AddWithValue("cedula", txt_cedula.Text);
                        cmd.Parameters.AddWithValue("observacion", txt_observacion.Text);
                        int rows = await cmd.ExecuteNonQueryAsync();
                        if (rows > 0)
                        {
                            MessageBox.Show("Marcación registrada.", "Marcaciones:",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se registró marcación.", "Marcaciones:",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        connection.Close();

                        // Limpieza UI
                        txt_cedula.Text = "";
                        txt_observacion.Text = "";

                        return rows > 0;
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese Nmero de Cdula");
                }

                txt_cedula.Text = "";
                txt_observacion.Text = "";
                connection?.Close();
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                connection.Close();
                txt_cedula.Text = "";
                txt_observacion.Text = "";
                return false;
            }
        }


        private void buscar_usuario_hora()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(
                 "Server=164.90.148.158;User Id=byron;" +
                 "Password=valeria2005;Database=registros;");

                //NpgsqlConnection conn = new NpgsqlConnection(
                // "Server=101.101.101.224;User Id=postgres;" +
                // "Password=valeria2005;Database=appbyrondb;");
                conn.Open();
                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("select now()", conn);

                // Execute a query
                NpgsqlDataReader dr = cmd.ExecuteReader();

                // Read all rows and output the first column in each row
                while (dr.Read())
                {
                    // Console.Write("{0}\n", dr[0]);
                    // this.cedula_db = cedula;
                    this.hora_db = Convert.ToDateTime(dr[0]).ToString("yyyy-MM-dd HH:mm:ss");
                    //his.local_db = dr[1].ToString();
                }
                // Close connection
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async Task PostToGoogleAppsScriptAsync(string url, object payload)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(15);

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(url, content);
                var text = (await resp.Content.ReadAsStringAsync())?.Trim();

                if (!resp.IsSuccessStatusCode || !string.Equals(text, "OK", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Sheets webhook returned: " + text);
                }
            }
        }

        private async Task SendToSheetsAsync(string usuario, string cedula, string observacion, string horaDb)
        {
            var url   = ConfigurationManager.AppSettings["SheetsWebhookUrl"];
            var token = ConfigurationManager.AppSettings["SheetsToken"];

            var payload = new
            {
                usuario = usuario,
                cedula  = cedula,
                local   = "",
                locales = "",
                zonas   = "",
                fecha_ing = horaDb,
                numero_marcacion = "",
                mensaje = observacion,
                ingreso = "",
                salida  = "",
                horas_trabajadas   = "",
                hora_extra_normal  = "",
                hora_extra_madrugada = "",
                token = token
            };

            await PostToGoogleAppsScriptAsync(url, payload);
        }
    }
}
