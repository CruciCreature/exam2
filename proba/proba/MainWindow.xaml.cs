using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DbApp
{
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter Table;
        DataTable table;

        public MainWindow()
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = "server=DESKTOP-CKDNKCP;Trusted_Connection=Yes;DataBase=Givno;";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM [dbo].[table]";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                Table = new SqlDataAdapter(command);

                // установка команды на добавление для вызова хранимой процедуры
                Table.InsertCommand = new SqlCommand("Insert", connection);
                Table.InsertCommand.CommandType = CommandType.StoredProcedure;
                Table.InsertCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.Int, 50, "Phone"));
                Table.InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 10, "Name"));
               // SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                //parameter.Direction = ParameterDirection.Output;

                connection.Open();
                Table.Fill(table);
                Widne.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }


    }
}