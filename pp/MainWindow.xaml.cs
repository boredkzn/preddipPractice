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

namespace pp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool IsReadyToJoin { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void joinB_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new ppEntities())
            {
                var userConnect = db.User.ToList().Find(f=>f.UserLogin == login.Text && f.UserPassword == password.Text);
                if(userConnect == null)
                {
                    captchaWindow captchaWindow = new captchaWindow(this);
                    captchaWindow.Show();

                    IsEnabled = false;
                }
                else
                {
                    ListUserWindow listUserWindow = new ListUserWindow();
                    MessageBox.Show($"Успешно! Добро пожаловать, {userConnect.UserName}");
                
                    this.Close();
                    listUserWindow.Show();
                }
            }
        }

        private void viewB_Click(object sender, RoutedEventArgs e)
        {
            new ListUserWindow().Show();
            this.Close();
        }
    }
}
