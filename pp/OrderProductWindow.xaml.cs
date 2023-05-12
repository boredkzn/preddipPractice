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
using Excel = Microsoft.Office.Interop.Excel;

namespace pp
{
    /// <summary>
    /// Логика взаимодействия для OrderProductWindow.xaml
    /// </summary>
    public partial class OrderProductWindow : Window
    {
        private List<OrderProduct> _products;
        private ppEntities db;
        private Order _order;

        public OrderProductWindow(List<OrderProduct> products)
        {
            InitializeComponent();
            db = new ppEntities();
            _products = products;
            ListProducts.ItemsSource = products.ToList();
            PriceText.Text = products.Select(f => f.Product).Select(f => f.ProductCostWithAmount).Sum().ToString();
            PickupPointComboBox.ItemsSource = db.PickupPoint.Select(x => x.Address).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderProduct product = (sender as Button)?.DataContext as OrderProduct;
            if (product.Count == 1)
                _products.Remove(product);
            else
                product.Count -= 1;

            MessageBox.Show("Успешно");
            ListProducts.ItemsSource = _products.ToList();
            PriceText.Text = _products.Select(f => f.Product).Select(f => f.ProductCostWithAmount).Sum().ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            m1:
                var code = random.Next(100, 999);
            var isHave = db.Order.Select(f => f.OrderGetCode).ToList().Contains(code);
            if (isHave)
                goto m1;
            
            var isThreeDDay = _products.Select(f => f.Count).All(f => f > 3);
            
           
                var order = new Order()
                {
                    OrderID = db.Order.Select(f => f.OrderID).Max() + 1,
                    OrderStatusID = 1,
                    PickupPointID = db.PickupPoint.ToList().Find(x => x.Address == PickupPointComboBox.SelectedValue.ToString()).PickupPointID,
                    OrderCreateDate = DateTime.Now,
                    OrderDeliveryDate = isThreeDDay ? DateTime.Now.AddDays(3) : DateTime.Now.AddDays(6),
                    UserID = null,
                    OrderGetCode = code
                };
                db.Order.Add(order);
                
                db.SaveChanges();
                _order = order ;
                foreach (var prod in _products)
                {
                    var pr = new OrderProduct()
                    {
                        OrderProductID = prod.OrderProductID,
                        
                        ProductID = prod.ProductID,
                        OrderID = order.OrderID,
                        
                        Count = prod.Count,
                    };
                    db.OrderProduct.Add(pr);
                    
                }
            db.SaveChanges();
            _order.OrderProduct = _products;
            MessageBox.Show("Успешно создан заказ!");
        }

       

   

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (_order != null)
            {
                try
                {
                    var orders = _order;
                    var app = new Excel.Application
                    {
                        SheetsInNewWorkbook = 1
                    };
                    var workbook = app.Workbooks.Add(Type.Missing);

                    Excel.Worksheet worksheet = app.Worksheets.Item[1];
                    worksheet.Name = "Card";

                    worksheet.Cells[1][1] = "Order number";
                    worksheet.Cells[1][2] = "Product list";
                    worksheet.Cells[1][3] = "Total cost";

                    worksheet.Cells[2][1] = orders.OrderID;

                    var fullProductList = string.Empty;
                    fullProductList = orders.OrderProduct.Aggregate(fullProductList,
                        (current, product) => current + $"{product.Product.ProductName}\n");
                    worksheet.Cells[2][2] = fullProductList;
                    worksheet.Cells[2][3] = orders.OrderProduct.Sum(p => p.Product.ProductCost);

                    worksheet.Columns.AutoFit();

                    app.Visible = true;

                    app.Application.ActiveWorkbook.SaveAs(@"C:\Users\User\Source\Repos\preddipPractice\pp\test.xlsx");

                    var excelDocument = app.Workbooks.Open(@"C:\Users\User\Source\Repos\preddipPractice\pp\test.xlsx");
                    excelDocument.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, @"C:\Users\User\Source\Repos\preddipPractice\pp\test.pdf");
                    excelDocument.Close(false, "", false);
                    app.Quit();
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }

            }
            else
            {
                MessageBox.Show("Сначала сформируйте заказ");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
