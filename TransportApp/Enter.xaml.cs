using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace TransportApp
{
    public partial class Enter : Window
    {
        public Enter()
        {
            InitializeComponent();
        }

        // Обработчик события для попытки входа пользователя в систему.
        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsAllTextBoxesFilled())
            {
                string login = Convert.ToString(Login.Text);
                string password = Convert.ToString(Password.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Transport;Integrated Security=True"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT Пароль FROM Users WHERE Логин = @UserLogin", connection))
                    {
                        command.Parameters.AddWithValue("@UserLogin", login);

                        string storedPassword = command.ExecuteScalar() as string;

                        if (storedPassword != null)
                        {
                            if (password == storedPassword)
                            {
                                MessageBox.Show("Вы успешно вошли", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такого аккаунта не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните все текстовые поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
