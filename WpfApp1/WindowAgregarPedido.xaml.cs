using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Lógica de interacción para WindowAgregarPedido.xaml
    /// </summary>
    public partial class WindowAgregarPedido : Window
    {
        public WindowAgregarPedido()
        {
            InitializeComponent();


            string miConexion = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.PruebaLeandroConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MostrarClientes();
            
        }

        SqlConnection miConexionSql;

        private void MostrarClientes()
        {
            try
            {
                string consulta = "SELECT * FROM cliente";

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSQL)
                {
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSQL.Fill(clientesTabla);

                    listaClientesAgregarPedido.DisplayMemberPath = "nombre";
                    listaClientesAgregarPedido.SelectedValuePath = "Id";
                    listaClientesAgregarPedido.ItemsSource = clientesTabla.DefaultView;

                }
            }

            catch (Exception ex) 
            {
                MessageBox.Show("Error: "+ex);
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string consulta = "INSERT INTO pedido (cCliente, fechaPedido, formaPago) " + 
                    "VALUES (@cClienteCreado, @fechaPedidoCreado, @formaPagoCreado)";

                SqlCommand miComando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptador = new SqlDataAdapter(miComando);

                miComando.Parameters.AddWithValue("@cClienteCreado", listaClientesAgregarPedido.SelectedValue);

                miComando.Parameters.AddWithValue("@fechaPedidoCreado", crearFechaPedido.Text);

                miComando.Parameters.AddWithValue("@formaPagoCreado", crearFormaPago.Text);

                miConexionSql.Open();

                miComando.ExecuteNonQuery();

                miConexionSql.Close();
                
                MessageBox.Show("El pedido ha sido agregado correctamente.");

                Close();

            }

            catch (Exception ex) 
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            

        }
    }
}
