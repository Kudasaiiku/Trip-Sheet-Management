using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TransportApp
{
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
        }

        // Обработчик события для добавления нового путевого листа в базу данных и рассчета нормы и расхода топлива.
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            // Проверка заполненности данными всех текстовых полей.
            if (IsAllTextBoxesFilled())
            {
                string date = IssueDate.Text;
                string marka = Marka.Text;
                string gosnumber = GosNumber.Text;
                string driver1 = Driver1.Text;
                string driver2 = Driver2.Text;
                string way = AddressFrom.Text + " - " + AddressTo.Text;
                string valume = Volume.Text;
                string fuelType = FuelType.Text;
                string rest = Rest.Text;
                string countOfFuel = CountOfFuel.Text;
                string speedometerFrom = SpeedometerFrom.Text;
                string speedometerTo = SpeedometerTo.Text;

                // Блок кода для добавления нового путевого листа в таблицу WayList базы данных.
                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Transport;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("INSERT INTO WayList (Дата_выдачи, Марка, Госномер, Водитель_1, Водитель_2, Маршрут, Объем_бака, Тип_топлива, Остаток_в_баке, Колво_заправл_топлива, Выдача_ПЛ, Сдача_ПЛ) VALUES" +
                    "('" + date + "', '" + marka + "', '" + gosnumber + "', '" + driver1 + "', '" + driver2 + "', '" + way + "', '" + valume + "', '" + fuelType + "', '" + rest + "', '" + countOfFuel + "', '" + speedometerFrom + "', '" + speedometerTo + "')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                double norma = 0;

                // Расчет фактического расхода топлива на основе введенных данных.
                int fact = (Convert.ToInt32(valume) + Convert.ToInt32(countOfFuel) + Convert.ToInt32(rest)) / 2;

                // Определение нормы расхода топлива в зависимости от марки транспорта.
                if (marka == "BMW")
                {
                    double BMWNorma = ((Convert.ToDouble(speedometerTo) - Convert.ToDouble(speedometerFrom)) * (10.0 / 100.0));
                    norma = BMWNorma;
                }
                else if (marka == "Mercedes")
                {
                    double MercedesNorma = ((Convert.ToDouble(speedometerTo) - Convert.ToDouble(speedometerFrom)) * (9.0 / 100.0));
                    norma = MercedesNorma;
                }
                else if (marka == "Audi")
                {
                    double AudiNorma = ((Convert.ToDouble(speedometerTo) - Convert.ToDouble(speedometerFrom)) * (8.0 / 100.0));
                    norma = AudiNorma;
                }

                // Блок кода для рассчета нормы и добавления данных о расходе топлива в таблицу Fuel базы данных.
                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Transport;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("INSERT INTO Fuel (Марка, Норма, Факт) VALUES" +
                    "('" + marka + "', '" + norma + "', '" + fact + "')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Путевой лист успешно добавлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                MessageBox.Show("Расход топлива обновлен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заполните все текстовые поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события для возвращения на главное окно из окна добавления путевого листа.
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        // Блок кода, ограничивающий ввод только цифр для TextBox.
        Regex regex = new Regex("[^0-9]+");
        private void Volume_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Rest_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CountOfFuel_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SpeedometerFrom_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SpeedometerTo_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        // Проверка на заполненность данными всех TextBox.
        private bool IsAllTextBoxesFilled()
        {
            foreach (var element in grid.Children)
            {
                if (element is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
