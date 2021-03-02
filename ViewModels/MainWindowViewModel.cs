using Product_Inventory.Models;
using Prism.Mvvm;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Xml;
using System.Windows.Input;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using Prism.Services.Dialogs;
using Prism.Commands;
using Product_Inventory.Views;

namespace Product_Inventory.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        private string _ProjectTitle = Properties.Resources.ProjectName;
        private ObservableCollection<Product> _productsCollection = new ObservableCollection<Product>();
        public readonly IDialogService _dialogService;
        private string FilePath = "";
        private Product _selectedProduct;

        public DelegateCommand ShowDialogCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _saveStatus;

        public string SaveStatus2
        {
            get { return _saveStatus; }
            set
            {
                _saveStatus = value;
                NotifyPropertyChanged("SaveStatus2");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                //Validate(propertyName);
            }
        }
        public string ProjectTitle
        {
            get { return _ProjectTitle; }
            set { SetProperty(ref _ProjectTitle, value); }
        }

        public MainWindowViewModel()
        {
            _productsCollection = new ObservableCollection<Product>();
            ShowDialogCommand = new DelegateCommand(ShowDialog);

        }
        

        public ObservableCollection<Product> ProductsCollection
        {
            get
            {
                return _productsCollection;
            }
            set
            {
                _productsCollection = value;
            }
        }   

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                
            }
        }


        public void LoadProducts(string Path)
        {
            FilePath = Path;
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ProductDetails";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductDetails), xRoot);
            StreamReader reader = new StreamReader(Path);
            var productdetails = (ProductDetails)xmlSerializer.Deserialize(reader);
            ProductsCollection = new ObservableCollection<Product>(productdetails.Product);
            
            reader.Close();

            
        }
        public bool SaveFile(string Path, object data)
        {
            bool isFileSaved;
            if ((Product.ProductIdFlag == false) || (Product.ProductNameFlag == false) || (Product.ProductQtyFlag == false) || (Product.ProductPriceFlag == false))
            {
                isFileSaved = false;
                return isFileSaved;
            }
           

            //create and specify root element of xml document
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "ProductDetails";

            //all data of datagrid is casted to ObservableCollection Employees
            //ObservableCollection is responsible for typecasting datagrid to list without giving any error
            ProductsCollection = (ObservableCollection<Product>)data;
            //declaring XmlSerializer using ObservableCollection and Root Element
            var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Product>), xRoot);

            //using gives flexiblity of not closing the streamwriter object
            //openFileDlg.FileName = Path of xml file
            using (StreamWriter writer = new StreamWriter(Path))
            {
                xmlSerializer.Serialize(writer, ProductsCollection);
            }
            isFileSaved = true;
            return isFileSaved;

        }
        public bool SaveNewFile(string path,object data)
        {
            bool isValidData;
            ProductsCollection = data as ObservableCollection<Product>;
            try
            {
                if (string.IsNullOrWhiteSpace(ProductsCollection[0].ProductId))
                {
                    isValidData = false;
                    return isValidData;
                }
                for (int index = 0; index < ProductsCollection.Count;)
                {

                    if (index == 0)
                    {
                        XmlWriterSettings xmlwritersettings = new XmlWriterSettings();
                        xmlwritersettings.Indent = true;
                        xmlwritersettings.NewLineOnAttributes = true;

                        XmlWriter writer = XmlWriter.Create(path, xmlwritersettings);

                        writer.WriteStartElement("ProductDetails");
                        writer.WriteStartElement("Product");
                        writer.WriteElementString("Product_Id", ProductsCollection[i].ProductId);
                        writer.WriteElementString("Product_Name", ProductsCollection[i].ProductName);
                        writer.WriteElementString("Product_Qty", ProductsCollection[i].ProductQty);
                        writer.WriteElementString("Product_Price", ProductsCollection[i].ProductPrice);

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.Flush();

                        writer.Dispose();

                    }
                    else if (index != 0)
                    {

                        XDocument doc = XDocument.Load(path);
                        Console.Beep();
                        XElement root = doc.Element("ProductDetails");
                        IEnumerable<XElement> rows = root.Descendants("Product");
                        XElement firstRow = rows.First();
                        firstRow.AddBeforeSelf(
                           new XElement("Product",
                           new XElement("Product_Id", ProductsCollection[i].ProductId),
                           new XElement("Product_Name", ProductsCollection[i].ProductName),
                           new XElement("Product_Qty", ProductsCollection[i].ProductQty),
                           new XElement("Product_Price", ProductsCollection[i].ProductPrice)));

                        doc.Save(path);
                    }
                    index++;

                }
            }
            catch(Exception e)
            {
                isValidData = false;
                return isValidData;
            }
            isValidData = true;
            return isValidData;

        }
        public void RemoveSelectedItem(Product item)
        {
            if (item != null)
            {
                ProductsCollection.Remove(item);
            }
            else
                return;
        }


        //Dialog related code.  see constructor also.
        public void ShowDialog()
        {
 
            try
            {
                Console.Beep();
                if (SelectedProduct == null)
                    return;
                //Console.WriteLine(SelectedProduct.ProductId + " " + SelectedProduct.ProductName);

                var dialog = new SelectedProductDialog(SelectedProduct);
                if (dialog.ShowDialog() == true)
                {
                    //New code 
                    for(int ProductIndex=0; ProductIndex<ProductsCollection.Count;ProductIndex++)
                    {
                        if(ProductsCollection[ProductIndex].ProductId == dialog.id)
                        {
                            ProductsCollection[ProductIndex].ProductName = dialog.name;
                            ProductsCollection[ProductIndex].ProductQty = dialog.qty;
                            ProductsCollection[ProductIndex].ProductPrice = dialog.price;
                        }
                    }

                    MainWindow.IsSaved = false;
                    //SaveStatus2 = "(Unsaved)";

                    dialog.Close();
                }
                else
                {
                    //Do nothing
                }
            }
            catch (Exception e2)
            {
                //MessageBox.Show(Properties.Resources.WrongString, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }



        }

    }

}

