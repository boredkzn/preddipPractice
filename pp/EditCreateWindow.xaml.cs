using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace pp
{
    /// <summary>
    /// Логика взаимодействия для EditCreateWindow.xaml
    /// </summary>
    public partial class EditCreateWindow : Window
    {
        private Product _product;

        private bool _isEdit;

        private ppEntities _ppEntities;
        public EditCreateWindow(Product product, bool isEdit)
        {
            InitializeComponent();
            _ppEntities = new ppEntities();
            _product = product;
            editCreate.DataContext = product;
            categoryCom.ItemsSource = _ppEntities.ProductCategory.Select(f => f.ProductCategoryName).ToList();
            post.ItemsSource = _ppEntities.ProductManufacturer.Select(f => f.ProductManufacturerName).ToList();
            _isEdit = isEdit;
            unit.ItemsSource = _ppEntities.UnitType.Select(f=>f.UnitTypeName).ToList();
            if (isEdit == false)
                art.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

                try
                {
                    var Picture = _product.ProductPhoto;
                    OpenFileDialog opFD = new OpenFileDialog();
                    opFD.ShowDialog();
                    var imag = opFD.FileName;
                    string dest = "D:\\vss\\pp\\pp\\Photos\\" + System.IO.Path.GetFileName(imag);
                    File.Copy(imag, dest);
                    Image image = new Image();
                    var bi = new BitmapImage(new Uri(dest));
                    ima.Source = bi;
                    var pr = _ppEntities.Product.ToList().Find(f => f.ProductID == _product.ProductID);
                    if (_isEdit == false)
                        pr = _product;
                    pr.ProductPhoto = opFD.SafeFileName;
                    _ppEntities.SaveChanges();
                    editCreate.DataContext = pr;
           
                }
                catch
                {
                    MessageBox.Show("Попробуйте снова, возможно файл с таким именем уже существует!");
                }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ListUserWindow().Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var product = _ppEntities.Product.ToList().Find(f => f.ProductID == _product.ProductID);
            if (_product != null)
            {
                _ppEntities.Product.Remove(product);
                _ppEntities.SaveChanges();
                MessageBox.Show("Успешно удалено");
                this.Close();
            }
            else
            {
                MessageBox.Show("Некорректно");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isEdit)
                {
                    var updatedProduct = _ppEntities.Product.Find(_product.ProductID);
                    updatedProduct.ProductName = _product.ProductName;
                    updatedProduct.ProductCategoryID = _ppEntities.ProductCategory.ToList().Find(c => c.ProductCategoryName == categoryCom.SelectedValue.ToString()).ProductCategoryID;
                    updatedProduct.ProductManufacturerID = _ppEntities.ProductManufacturer.ToList().Find(m => m.ProductManufacturerName == post.SelectedValue.ToString()).ProductManufacturerID;
                    updatedProduct.ProductMaxDiscountAmount = _product.ProductMaxDiscountAmount;
                    updatedProduct.ProductDiscountAmount = _product.ProductDiscountAmount;
                    updatedProduct.ProductCost = _product.ProductCost;
                    updatedProduct.ProductDescription = _product.ProductDescription;
                }
                else
                {
                    _product.ProductManufacturerID = _ppEntities.ProductManufacturer.ToList().Find(m => m.ProductManufacturerName == post.SelectedValue.ToString()).ProductManufacturerID;
                    _product.ProductCategoryID = _ppEntities.ProductCategory.ToList().Find(c => c.ProductCategoryName == categoryCom.SelectedValue.ToString()).ProductCategoryID;
                    _product.UnitTypeID = _ppEntities.UnitType.ToList().Find(u => u.UnitTypeName == unit.SelectedValue.ToString()).UnitTypeID;
                    _ppEntities.Product.Add(_product);
                }

                _ppEntities.SaveChanges();
                MessageBox.Show("Успешно сохранено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
