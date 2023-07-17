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
using System.Data;
using System.Data.SqlClient;

namespace Anubis
{
    public partial class MainWindow : Window
    {
        private string connectionString;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            string query = "SELECT CustomerID, FirstName, LastName FROM Customers";
            DataTable dataTable = ExecuteQuery(query);

            CustomersComboBox.ItemsSource = dataTable.DefaultView;
            CustomersComboBox.DisplayMemberPath = "FirstName";
            CustomersComboBox.SelectedValuePath = "CustomerID";
        }

        private void CustomersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CustomersComboBox.SelectedValue != null)
            {
                int customerId = (int)CustomersComboBox.SelectedValue;
                string query = $"SELECT OrderID, OrderDate, TotalAmount FROM Orders WHERE CustomerID = {customerId}";
                DataTable dataTable = ExecuteQuery(query);

                OrdersDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    return dataTable;
                }
            }
        }
    }
}

