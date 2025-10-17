using System;
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
                MessageBox.Show("No Existe coneccíon con el Servidor");
                throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //this.buscar_usuario(txt_cedula.Text);
                DapperBoardGameRepository();
                buscar_usuario_hora();
                insert_maraccion();
            }
            catch(Exception ex) 
            {
            
                    MessageBox.Show("Consulte con el Proveedor");

             }
           
        }


        private async Task insert_maraccion()
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
                            MessageBox.Show("Marcación Registrada..", "Marcaciones:",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se registro marcación..", "Marcaciones:",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    connection.Close();

                }
                else
                {
                    MessageBox.Show("Ingrese Número de Cédula");
                }

                txt_cedula.Text = "";
                txt_observacion.Text = "";

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                connection.Close();
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
                NpgsqlCommand cmd = new NpgsqlCommand("select LOCALTIME", conn);

                // Execute a query
                NpgsqlDataReader dr = cmd.ExecuteReader();

                // Read all rows and output the first column in each row
                while (dr.Read())
                {
                    // Console.Write("{0}\n", dr[0]);
                    // this.cedula_db = cedula;
                    this.hora_db = dr[0].ToString();
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
    }
}
