using Product_Inventory.Models;
using Product_Inventory.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Product_Inventory.Views
{
    /// <summary>
    /// Interaction logic for MyDialog.xaml
    /// </summary>
    public partial class SelectedProductDialog : Window
    {
        Product product = new Product();
        SelectedProductDialogViewModel selectedProductDialog = new SelectedProductDialogViewModel();
        public string id, name, qty, price;
        public SelectedProductDialog(Product item)
        {
            InitializeComponent();


            product.ProductId = item.ProductId;
            product.ProductName = item.ProductName;
            product.ProductQty = item.ProductQty;
            product.ProductPrice = item.ProductPrice;

            //MessageBox.Show(p.SelectedProduct.ToString());
            this.DataContext = product;
            
        }

        private void updatebtn_Click(object sender, RoutedEventArgs e)
        {
            var isValidData = selectedProductDialog.ValidateData();
            if(isValidData==true)
            {
                id = productid.Text;
                name = productname.Text;
                qty = productqty.Text;
                price = productprice.Text;
                DialogResult = true;
                //this.Close();
            }
            else
            {
                //Do nothing
            }
        }
    }
}
