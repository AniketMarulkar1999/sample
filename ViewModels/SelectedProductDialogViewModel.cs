using Prism.Commands;
using Product_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Inventory.ViewModels
{
    public class SelectedProductDialogViewModel
    {
        public static bool IsDataValidFlag;
        public SelectedProductDialogViewModel()
        { 
        }

        public bool ValidateData()
        {
            if (Product.ProductIdFlag == true && Product.ProductNameFlag == true && Product.ProductQtyFlag == true && Product.ProductPriceFlag == true)
            {
                IsDataValidFlag = true;
                return IsDataValidFlag;
            }
            else
            {
                IsDataValidFlag = false;
                return IsDataValidFlag;
            }
        }
    }
}
