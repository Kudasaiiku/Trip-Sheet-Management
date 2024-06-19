using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Windows;

namespace TransportApp
{
    public partial class Fuel : Window
    {
        public Fuel()
        {
            InitializeComponent();

            DataBaseConnection();
        }

        // Класс, представляющий данные о расходе топлива из таблицы Fuel.
        public class FuelData
        {
            public int ID { get; set; }
            public string Марка { get; set; }
            public int Норма { get; set; }
            public int Факт { get; set; }
        }

        // Метод для подключения к базе данных и заполнения DataGrid данными из таблицы Fuel.
        public void DataBaseConnection()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Transport;Integrated Security=True"))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM Fuel", connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                List<FuelData> fuelList = new List<FuelData>();

                // Итерация по строкам DataTable для заполнения списка объектов FuelData.
                foreach (DataRow row in dataTable.Rows)
                {
                    FuelData fuelData = new FuelData
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Марка = row["Марка"].ToString(),
                        Норма = Convert.ToInt32(row["Норма"]),
                        Факт = Convert.ToInt32(row["Факт"]),
                    };

                    fuelList.Add(fuelData);
                }

                FuelGrid.ItemsSource = fuelList;
            }
        }

        // Обработчик события для возвращения на главное окно из окна учета расхода топлива.
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}