using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;


namespace Product_Inventory.Models
{
    [Serializable()]
    [XmlRoot("ProductDetails")] 
    public class ProductDetails
    {
        [XmlElement("Product")]
        public List<Product> Product { get; set; }
        //public Product product { get; set; }
    }
}
