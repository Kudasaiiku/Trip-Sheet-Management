using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Windows;

namespace TransportApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataBaseConnection();
        }

        // Класс с характеристиками из таблицы WayList.
        public class Transport
        {
            public int ID { get; set; }
            public string Дата_выдачи { get; set; }
            public string Марка { get; set; }
            public string Госномер { get; set; }
            public string Водитель_1 { get; set; }
            public string Водитель_2 { get; set; }
            public string Маршрут { get; set; }
            public int Объем_бака { get; set; }
            public string Тип_топлива { get; set; }
            public int Остаток_в_баке { get; set; }
            public int Колво_заправл_топлива { get; set; }
            public int Выдача_ПЛ { get; set; }
            public int Сдача_ПЛ { get; set; }
        }

        // Метод для установления соединения с базой данных и заполнения DataGrid данными из таблицы WayList.
        public void DataBaseConnection()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Transport;Integrated Security=True"))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM WayList", connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                List<Transport> transportList = new List<Transport>();

                // Итерация по строкам DataTable для заполнения списка объектов Transport.
                foreach (DataRow row in dataTable.Rows)
                {
                    Transport transport = new Transport
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Дата_выдачи = row["Дата_выдачи"].ToString(),
                        Марка = row["Марка"].ToString(),
                        Госномер = row["Госномер"].ToString(),
                        Водитель_1 = row["Водитель_1"].ToString(),
                        Водитель_2 = row["Водитель_2"].ToString(),
                        Маршрут = row["Маршрут"].ToString(),
                        Объем_бака = Convert.ToInt32(row["Объем_бака"]),
                        Тип_топлива = row["Тип_топлива"].ToString(),
                        Остаток_в_баке = Convert.ToInt32(row["Остаток_в_баке"]),
                        Колво_заправл_топлива = Convert.ToInt32(row["Колво_заправл_топлива"]),
                        Выдача_ПЛ = Convert.ToInt32(row["Выдача_ПЛ"]),
                        Сдача_ПЛ = Convert.ToInt32(row["Сдача_ПЛ"]),
                    };

                    transportList.Add(transport);
                }

                TransportGrid.ItemsSource = transportList;
            }
        }

        // Обработчик события для открытия окна добавления нового путевого листа.
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
            Close();
        }

        // Обработчик события для открытия окна учета расхода топлива.
        private void FuelBtn_Click(object sender, RoutedEventArgs e)
        {
            Fuel fuel = new Fuel();
            fuel.Show();
            Close();
        }

        // Выход из программы.
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Подтверждение завершения работы программы.
            MessageBoxResult result = MessageBox.Show("Завершение работы?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
