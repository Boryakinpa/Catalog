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
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace CatalogNet
{
    /// <summary>
    /// Логика взаимодействия для AddProductsWindow.xaml
    /// </summary>
    public partial class AddProductsWindow : Window
    {
        private string selectedImages;
        public AddProductsWindow()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images (*.jpg)|*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImages = openFileDialog.FileName;
                SelectedImagesTextBox.Text = selectedImages;
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var product = new Product
            {
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text,
                Cost = Convert.ToInt32(CostTextBox.Text),
                QuantityInStock = Convert.ToInt32(QuantityInStockTextBox.Text),
                Manufacturer = ManufacturerTextBox.Text,
                Photo = SelectedImagesTextBox.Text,
            };

            //var product = new Product
            //    (
            //        SelectedImagesTextBox.Text,
            //        NameTextBox.Text,
            //        DescriptionTextBox.Text,
            //        Convert.ToInt32(CostTextBox.Text),
            //        Convert.ToInt32(QuantityInStockTextBox.Text),
            //        ManufacturerTextBox.Text
            //    );

            using (var context = new YourRestoredDatabaseNameContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
            ((MainWindow)Owner).Products.Add(product);
            ((MainWindow)Owner).UpdatePagedProducts();
            ((MainWindow)Owner).UpdateFilters(((MainWindow)Owner).Products, ((MainWindow)Owner).FilteredProducts);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
