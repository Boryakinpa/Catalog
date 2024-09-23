using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.SqlServer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace CatalogNet
{


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int ItemsPerPage = 15;
        private int _currentPage = 1;
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> PagedProducts { get; set; } = new ObservableCollection<Product>();

        public string DisplayedItemsCount
        {
            get
            {
                int totalItems = FilteredProducts.Cast<Product>().Count();
                int startItem = (_currentPage - 1) * ItemsPerPage + 1;
                int endItem = _currentPage * ItemsPerPage;
                if (endItem > totalItems) endItem = totalItems;
                return $"{startItem}-{endItem} из {totalItems}";
            }
        }
        public string CurrentPageDisplay => $"Страница {_currentPage}";

        private ICollectionView _filteredProducts;
        public ICollectionView FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
                OnPropertyChanged(nameof(DisplayedItemsCount));
            }
        }

        public bool IsPreviousPageEnabled => _currentPage > 1;
        public bool IsNextPageEnabled => _currentPage < (FilteredProducts.Cast<Product>().Count() + ItemsPerPage - 1) / ItemsPerPage;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Создание экземпляра контекста базы данных
            using (var context = new YourRestoredDatabaseNameContext())
            {
                // Получение всех продуктов из базы данных
                Products = new ObservableCollection<Product>(context.Products.ToList());
            }
            UpdateFilters(Products, FilteredProducts);
            FilteredProducts = CollectionViewSource.GetDefaultView(Products);
            FilteredProducts.Filter = FilterProducts;
            UpdatePagedProducts();
            ManufacturerFilter.SelectedItem = ManufacturerFilter.Items[0];
        }

        private bool Contain(ComboBox comboBox, Product product)
        {
            foreach (var item in comboBox.Items) 
            {
                if(product.Manufacturer == "Ошибка" || ((ComboBoxItem)item).Content == null) return false;
                //MessageBox.Show(((ComboBoxItem)item).Content.ToString(), product.Manufacturer, MessageBoxButton.OK);
                if (((ComboBoxItem)item).Content.ToString() == product.Manufacturer) return true; 
            }
            return false;
        }

        public void UpdateFilters(ObservableCollection<Product> products, ICollectionView filteredProducts)
        {
            foreach (var product in products)
            {
                if (!Contain(ManufacturerFilter, product))
                {
                    ComboBoxItem newItem = new ComboBoxItem();
                    newItem.Content = product.Manufacturer;
                    ManufacturerFilter.Items.Add(newItem);
                }
            }
            //filteredProducts = CollectionViewSource.GetDefaultView(products);
            //filteredProducts.Filter = FilterProducts;
            //UpdatePagedProducts();
            //UpdatePagedProducts();
        }

        private bool FilterProducts(object obj)
        {
            if (obj is Product product)
            {
                string searchText = SearchBox?.Text ?? string.Empty;
                string selectedManufacturer = ManufacturerFilter?.SelectedItem is ComboBoxItem selectedItem ? (string)selectedItem.Content : "Все производители";
                //MessageBox.Show(selectedManufacturer, selectedManufacturer, MessageBoxButton.OK);
                return (string.IsNullOrEmpty(searchText) || product.Name.Contains(searchText) || product.Description.Contains(searchText)) &&
                       (selectedManufacturer == "Все производители" || product.Manufacturer == selectedManufacturer);
            }
            return false;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilteredProducts.Refresh();
            _currentPage = 1;
            UpdatePagedProducts();
        }

        private void ManufacturerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilteredProducts.Filter = FilterProducts;
            FilteredProducts.Refresh();
            _currentPage = 1;
            UpdatePagedProducts();
        }

        private void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            FilteredProducts.SortDescriptions.Clear();
            FilteredProducts.SortDescriptions.Add(new SortDescription(nameof(Product.Cost), ListSortDirection.Ascending));
            _currentPage = 1;
            UpdatePagedProducts();
        }

        private void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            FilteredProducts.SortDescriptions.Clear();
            FilteredProducts.SortDescriptions.Add(new SortDescription(nameof(Product.Cost), ListSortDirection.Descending));
            _currentPage = 1;
            UpdatePagedProducts();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdatePagedProducts();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < (FilteredProducts.Cast<Product>().Count() + ItemsPerPage - 1) / ItemsPerPage)
            {
                _currentPage++;
                UpdatePagedProducts();
            }
        }

        public void UpdatePagedProducts()
        {
            PagedProducts.Clear();
            var items = FilteredProducts.Cast<Product>().Skip((_currentPage - 1) * ItemsPerPage).Take(ItemsPerPage);
            foreach (var item in items)
            {
                PagedProducts.Add(item);
            }
            OnPropertyChanged(nameof(IsPreviousPageEnabled));
            OnPropertyChanged(nameof(IsNextPageEnabled));
            OnPropertyChanged(nameof(CurrentPageDisplay));
            OnPropertyChanged(nameof(DisplayedItemsCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProductsWindow();

            window.Owner = this;
            window.ShowDialog();
        }

        private void ChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            if(ProductList.SelectedItem != null)
            {
                var item = ProductList.SelectedItem as Product;
                AddProductsWindow window = new AddProductsWindow();
                MessageBox.Show(window, Name = "Change", Title = "Change", MessageBoxButton.OKCancel);
            }
            
        }
    }
}