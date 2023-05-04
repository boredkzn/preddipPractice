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
                "0-9,99%", "10-14,99%", "15% и более", "Все"
            };
            order.ItemsSource = new List<string>
            {
                "По возрастанию", "По убыванию"
            };
            count.Content = $"Кол-во записей {ListProducts.Items.Count} из {db.Product.ToList().Count}";

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

        private void GetByFilter()
        {
            var list = new List<Product>();
            var combo = ComboBoxFilterProductDiscountAmount.SelectedIndex;
            var orderB = order.SelectedIndex;
            var textChanged = nameFIlter.Text;
            if (combo != -1 && !string.IsNullOrEmpty(textChanged))
            {
                switch (combo)
                {

                    case 0:
                        {
                            if (!string.IsNullOrEmpty(textChanged))
                                list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged) && x.ProductMaxDiscountAmount < 10));
                            else
                                list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount < 10));
                            break;
                        }
                    case 1:
                        {
                            if (!string.IsNullOrEmpty(textChanged))
                                list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged) && x.ProductMaxDiscountAmount > 10 && x.ProductMaxDiscountAmount < 15));
                            else
                                list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount > 10 && p.ProductMaxDiscountAmount < 15));
                            break;
                        }
                    case 2:
                        {
                            if (!string.IsNullOrEmpty(textChanged))
                                list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged) && x.ProductMaxDiscountAmount > 15));
                            else
                                list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount > 15));
                            break;
                        }
                    case 3:
                        {
                            list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged)));

                            break;
                        }
                }

            }
            else if (!string.IsNullOrEmpty(textChanged))
            {
                list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged)));
            }
            else if (combo != -1)
            {
                switch (combo)
                {

                    case 0:
                        {

                            list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount < 10));
                            break;
                        }
                    case 1:
                        {
                            list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount > 10 && p.ProductMaxDiscountAmount < 15));
                            break;
                        }
                    case 2:
                        {
                            list.AddRange(db.Product.Where(p => p.ProductMaxDiscountAmount > 15));
                            break;
                        }
                    case 3:
                        {
                            list.AddRange(db.Product.Where(x => x.ProductName.Contains(textChanged)));
                            break;
                        }
                }
            }
            else
            {
                list = db.Product.ToList();
            }

            switch (orderB)
            {
                case 0:
                    {
                        list = list.OrderBy(f => f.ProductMaxDiscountAmount).ToList();
                        break;
                    }
                case 1:
                    {
                        list = list.OrderByDescending(f => f.ProductMaxDiscountAmount).ToList();
                        break;
                    }
            }

            ListProducts.ItemsSource = list;

            count.Content = $"Кол-во записей {ListProducts.Items.Count} из {db.Product.ToList().Count}";
        }

        private void ComboBoxFilterProductDiscountAmount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetByFilter();
        }

        private void nameFIlter_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetByFilter();
        }

        private void order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetByFilter();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Product product = (sender as Button)?.DataContext as Product;
            new EditCreateWindow(product, true).Show();
            this.Close();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            new EditCreateWindow(product, false).Show();
            this.Close();
        }
    }
}