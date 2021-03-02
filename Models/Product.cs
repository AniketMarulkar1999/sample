using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Product_Inventory.Models
{
    [Serializable()]
    public class Product : INotifyDataErrorInfo, INotifyPropertyChanged 
    {
        private Dictionary<string, List<string>> propErrors = new Dictionary<string, List<string>>();

        private string _productId;
        private string _productName;
        private string _productQty;
        private string _productPrice;
        public static bool ProductIdFlag=true, ProductNameFlag=true, ProductQtyFlag=true, ProductPriceFlag=true;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;



        [XmlElement("Product_Id")]
        public string ProductId { get { return _productId; } set { _productId = value;
                NotifyPropertyChanged("ProductId");
            } }

        [XmlElement("Product_Name")]
        public string ProductName 
        {
            get 
            { 
                return _productName; 
            } 
            set { 
                _productName = value;
                NotifyPropertyChanged("ProductName");
                
            } }

        [XmlElement("Product_Qty")]
        public string ProductQty { get { return _productQty; } set { _productQty = value;
                NotifyPropertyChanged("ProductQty");
            } }

        [XmlElement("Product_Price")]
        public string ProductPrice { get { return _productPrice; } set { _productPrice = value;
                NotifyPropertyChanged("ProductPrice");
            } }


        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));        
                Validate(propertyName);
            }
        }

        #region INotifyDataErrorInfo
        private void Validate(string columnName)
        {
            Task.Run(() => DataValidation(columnName));
            //DataValidation(columnName);
        }

        private void DataValidation(string columnName)
        {

            if ("ProductId" == columnName)
            {
                //Validate Product ID property
                List<string> listErrors;
                if (propErrors.TryGetValue(ProductId, out listErrors) == false)
                    listErrors = new List<string>();
                else
                    listErrors.Clear();

                if (String.IsNullOrEmpty(ProductId))
                {
                    ProductIdFlag = false;
                    listErrors.Add("Product id can not be empty");
                }
                else
                {
                    ProductIdFlag = true;
                    listErrors.Clear();
                }

                propErrors["ProductId"] = listErrors;
                if (listErrors.Count > 0)
                {
                    OnPropertyErrorsChanged("ProductId");
                }
            }

            if (columnName == "ProductName")
            {
                //Validate Product Name property
                List<string> listErrors;
                if (propErrors.TryGetValue(ProductName, out listErrors) == false)
                    listErrors = new List<string>();
                else
                    listErrors.Clear();

                if (string.IsNullOrEmpty(ProductName))
                {
                    ProductNameFlag = false;
                    listErrors.Add("Product name not be empty");
                }
                else if (Int32.TryParse(ProductName, out int num) == true)
                {
                    ProductNameFlag = false;
                    listErrors.Add("Product name can not contains digit");
                }
                else
                {
                    ProductNameFlag = true;
                    listErrors.Clear();
                }

                propErrors["ProductName"] = listErrors;
                if (listErrors.Count > 0)
                {
                    OnPropertyErrorsChanged("ProductName");
                }
            }

            if("ProductQty" == columnName)
            {
                //Validate Product Qty property
                List<string> listErrors;
                if (propErrors.TryGetValue(ProductQty, out listErrors) == false)
                    listErrors = new List<string>();
                else
                    listErrors.Clear();

                if (string.IsNullOrEmpty(ProductQty))
                {
                    ProductQtyFlag = false;
                    listErrors.Add("Product quantity can not be blank");
                }
                else if (Int32.Parse(ProductQty) < 0)
                {
                    ProductQtyFlag = false;
                    listErrors.Add("Product quantity can not be negative");
                }
                else
                {
                    ProductQtyFlag = true;
                    listErrors.Clear();
                }

                propErrors["ProductQty"] = listErrors;
                if (listErrors.Count > 0)
                {
                    OnPropertyErrorsChanged("ProductQty");
                }
            }

            if("ProductPrice" == columnName)
            {
                //Validate Product price property
                List<string> listErrors;
                if (propErrors.TryGetValue(ProductPrice, out listErrors) == false)
                    listErrors = new List<string>();
                else
                    listErrors.Clear();

                if (string.IsNullOrEmpty(ProductPrice))
                {
                    ProductPriceFlag = false;
                    listErrors.Add("Product price can not be empty");
                }
                else if (int.Parse(ProductPrice) <= 0)
                {
                    ProductPriceFlag = false;
                    listErrors.Add("Price can not be 0 or less than 0");
                }
                else
                {
                    ProductPriceFlag = true;
                    listErrors.Clear();
                }

                propErrors["ProductPrice"] = listErrors;
                if (listErrors.Count > 0)
                {
                    OnPropertyErrorsChanged("ProductPrice");
                }
            }
            
            /*if (flag1 == true && flag2 == true && flag3 == true && flag4 == true)
            {
                //Do nothing
                propErrors.Clear();

            }*/
        }

        

        private void OnPropertyErrorsChanged(string p)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(p));
            else if(ErrorsChanged == null)
            {
                //Do nothing
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errors = new List<string>();
            if (propertyName != null)
            {
                propErrors.TryGetValue(propertyName, out errors);
                return errors;
            }

            else
                return null;
        }

        public bool HasErrors
        {
            get
            {
                try
                {
                    var propErrorsCount = propErrors.Values.FirstOrDefault(r => r.Count > 0);
                    if (propErrorsCount != null)
                        return true;
                    else
                    {
                        return false;
                    }
                }
                catch { }
               return true;
            }
        }
        #endregion

       


    }
}
