using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.PruebaLeandroConnectionString"].ConnectionString;
        
            miConexionSql = new SqlConnection(miConexion);

            MuestraClientes();

            MuestraPedidosTotales();
        }

        SqlConnection miConexionSql;

        private void MuestraClientes() 
        {
            try
            {
                string consulta = "SELECT * FROM cliente";

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSQL)
                {
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSQL.Fill(clientesTabla);

                    listaClientes.DisplayMemberPath = "nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clientesTabla.DefaultView;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void MuestraPedidos()
        {
            try
            {
                string consulta = "SELECT * FROM pedido p INNER JOIN cliente c ON c.id = p.cCliente WHERE c.Id = @ClienteId";

                SqlCommand comandoSql = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(comandoSql);

                using (miAdaptadorSQL)
                {
                    comandoSql.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSQL.Fill(pedidosTabla); 

                    listaPedidos.DisplayMemberPath = "fechaPedido";
                    listaPedidos.SelectedValuePath = "id";
                    listaPedidos.ItemsSource = pedidosTabla.DefaultView;
                }

            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
            
        }

        private void MuestraPedidosTotales()
        
        {
            try
            {
                string consulta = "SELECT *, CONCAT (cCliente,' ',fechaPedido,' ',formaPago) AS INFOCOMPLETA FROM pedido";
                //string consulta = "SELECT * FROM Pedido";

                SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSQL)
                {
                    DataTable pedidosTotalesTabla = new DataTable();

                    miAdaptadorSQL.Fill(pedidosTotalesTabla);

                    listaPedidosTotales.DisplayMemberPath = "INFOCOMPLETA";
                    listaPedidosTotales.SelectedValuePath = "Id";
                    listaPedidosTotales.ItemsSource = pedidosTotalesTabla.DefaultView;
                
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void BorrarPedido()
        {
            try
            {
                //int idSeleccionado = Convert.ToInt32(listaPedidosTotales.SelectedValue);

                string consulta = "DELETE FROM pedido WHERE Id = @pedidoId";

                SqlCommand miComandoSql = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miComandoSql);

                miComandoSql.Parameters.AddWithValue("@pedidoId", listaPedidosTotales.SelectedValue);

                //SqlCommand miAdaptadorSql = new SqlCommand(consulta, miConexionSql);

                miComandoSql.Connection.Open();

                miComandoSql.ExecuteNonQuery();

                miComandoSql.Connection.Close();

                MuestraPedidosTotales();

            }
                   
            catch (Exception ex)
            {
                MessageBox.Show("Error "+ex);
            }
                
            

        }

        //EVENTOS:
        private void listaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedidos();
            
        }

        private void borrarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea borrar el pedido seleccionado?",
                    "Borrar Pedido",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BorrarPedido();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            WindowAgregarPedido windowAgregarPedido = new WindowAgregarPedido();
            windowAgregarPedido.Show();

        }

        private void ActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            MuestraPedidosTotales();
        }
    }
}
