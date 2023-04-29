using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace pp
{
    /// <summary> 
    /// Логика взаимодействия для ProductsWindow.xaml 
    /// </summary> 
    public partial class ListUserWindow : Window
    {
        private ppEntities db;

        public ListUserWindow()
        {
            InitializeComponent();
            db = new ppEntities();

            ListProducts.ItemsSource = db.Product.ToList();

            ComboBoxFilterProductDiscountAmount.ItemsSource = new List<string>
            {
                "0-10%", "10-15%", "15-∞%", "All ranges"
            };
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new MainWindow().Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void ComboBoxFilterProductDiscountAmount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBoxFilterProductDiscountAmount.SelectedIndex)
            {
                case 0:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductDiscountAmount <= 10).ToList();
                        break;
                    }
                case 1:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount <= 15).ToList();
                        break;
                    }
                case 2:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductDiscountAmount >= 15).ToList();
                        break;
                    }
                case 3:
                    {
                        ListProducts.ItemsSource = db.Product.ToList();
                        break;
                    }
            }
        }
    }
}