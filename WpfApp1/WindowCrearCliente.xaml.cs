using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para WindowCrearCliente.xaml
    /// </summary>
    public partial class WindowCrearCliente : Window
    {
        public WindowCrearCliente()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.PruebaLeandroConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

        }

        SqlConnection miConexionSql;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string consulta = "INSERT INTO cliente (nombre, direccion, poblacion, telefono) " +
                    "VALUES (@nombreCreado, @direccionCreada, @poblacionCreada, @telefonoCreado)";

                SqlCommand miComando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptador = new SqlDataAdapter(miComando);

                miComando.Parameters.AddWithValue("@nombreCreado", crearNombreCliente.Text);

                miComando.Parameters.AddWithValue("@direccionCreada", crearDireccion.Text);

                miComando.Parameters.AddWithValue("@poblacionCreada", crearPoblacion.Text);

                miComando.Parameters.AddWithValue("@telefonoCreado", crearTelefono.Text);

                miConexionSql.Open();

                miComando.ExecuteNonQuery();

                miConexionSql.Close();

                MessageBox.Show("El cliente ha sido creado correctamente.");

                Close();

            }


            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }

        }
    }
}
