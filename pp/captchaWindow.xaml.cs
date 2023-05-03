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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pp
{
    /// <summary>
    /// Логика взаимодействия для captchaWindow.xaml
    /// </summary>
    public partial class captchaWindow : Window
    {
        static Random r = new Random();
        static string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private MainWindow PreviousWindow { get; set; }

        public captchaWindow(MainWindow previousWindow)
        {
            InitializeComponent();
            cap.Text = GetCap();
            PreviousWindow = previousWindow;
            
        }

        public string GetCap()
        {
           string cap = "";
           for(int i=0; i<4; i++)
            {
                var index = r.Next(symbols.Length);
                cap += symbols[index];
            }
            return cap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var isCorrect = capEnter.Text == cap.Text;
            if(isCorrect)
            {
                this.PreviousWindow.IsEnabled = true;
                this.Hide();
            }
            else
            {
                MainWindow.IsReadyToJoin = false;
                MessageBox.Show("Неправильный ввод, вас блокнули на 10 сек, ожидайте");

                Block();
                DispatcherTimer timer = new DispatcherTimer();

                timer.Tick += new EventHandler(Unblock);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();

                PreviousWindow.lblTime.Content = timer.ToString();
                this.PreviousWindow.IsEnabled = true;
                Hide();
               
            }
        }

        private void Block()
        {
            PreviousWindow.login.IsEnabled = false;
            PreviousWindow.password.IsEnabled = false;
        }

        private void Unblock(object sender, EventArgs e)
        {
            PreviousWindow.login.IsEnabled = true;
            PreviousWindow.password.IsEnabled = true;
        }
    }
}
