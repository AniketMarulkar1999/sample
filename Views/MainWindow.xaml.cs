using Microsoft.Win32;
using Product_Inventory.ViewModels;
using Product_Inventory.Models;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace Product_Inventory.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         
        //public ObservableCollection<Product> products;
        public MainWindowViewModel vm = new MainWindowViewModel();
        int TotalProductCount;
        string Path;
        string ProjectTitle = Properties.Resources.ProjectName;
        string SelectedFileName = "";
        bool IsFileOpened = false;
        public static bool IsSaved = true;
        //DataSet dataset = new DataSet();
        OpenFileDialog openFileDlg = new OpenFileDialog();
        XmlDocument doc = new XmlDocument();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadFile(string Path)
        {
            this.DataContext = null;
            vm.LoadProducts(Path);
            this.DataContext = vm;
            //DataContext = this;
            IsFileOpened = true;

            this.Title = ProjectTitle + " - " + SelectedFileName;
            IsSaved = true;
            ShowFileStatus();
        }
        private void OpenFile()
        {
            try
            {

                this.Title = ProjectTitle;
                // Launch OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selected file name and display in a TextBox.
                // Load content of file in a TextBlock
                openFileDlg.DefaultExt = ".xml";
                if (result == true)
                {
                    if (openFileDlg.FileName.Contains(".xml"))
                    {
                        //txt1.Text = openFileDlg.FileName;
                        string[] s = openFileDlg.FileName.Split('\\');
                        SelectedFileName = s[s.Length - 1];
                        //MessageBox.Show(this.ProjectTitle);
                        Path = openFileDlg.FileName;
                        //dataset.Clear();
                        //dataset.ReadXml(Path);
                        LoadFile(Path);


                    }
                    else
                    {
                        string text = Properties.Resources.SelectXml;
                        MessageBox.Show(text,ProjectTitle,MessageBoxButton.OK,MessageBoxImage.Exclamation);
                    }

                }
                else
                {
                    //Do thing
                }
            }
            catch(Exception e)
            {
                string text = Properties.Resources.IncorrectXml;
                MessageBox.Show(text, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveFile()
        {
            if (IsFileOpened == true)
            {
                var data = datagrid.ItemsSource;

                bool isFileSaved = vm.SaveFile(Path, data);
                if (isFileSaved == false)
                {
                    IsSaved = false;
                    string text = Properties.Resources.FillValid;
                    MessageBox.Show(text, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (isFileSaved == true)
                {
                    IsSaved = true;
                    string text = Properties.Resources.DataSaved;
                    MessageBox.Show(text, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ShowFileStatus();
                
            }
            else
            {
                SaveNewFile();
            }
            return;

        }

        private void SaveNewFile()
        {
            Console.Beep();
            if (datagrid.ItemsSource == null)
                MessageBox.Show(Properties.Resources.FillValid);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if(result == false)
            {
                return;
            }
            

            Path = saveFileDialog.FileName;
            Path += ".xml";
            string[] s = Path.Split('\\');
            SelectedFileName = s[s.Length - 1];
            //MessageBox.Show(SelectedFileName);
            this.Title = ProjectTitle + " - " + SelectedFileName;
            var data = datagrid.ItemsSource;          
            bool IsFileSaved = vm.SaveNewFile(Path, data);
            if(IsFileSaved == false)
            {
                IsSaved = false;
                string text = Properties.Resources.FillValid;
                MessageBox.Show(text, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }else if(IsFileSaved == true)
            {
                IsSaved = true;
                IsFileOpened = true;
                string text = Properties.Resources.DataSaved;
                MessageBox.Show(text, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            ShowFileStatus();
        }
        public void ShowFileStatus()
        {
            
                try
                { 

                    if(IsFileOpened == false && IsSaved == true)
                    {
                    txtcount.Content = "";
                    savestatus.Content = "";
                    return;
                    }

                    TotalProductCount = 0;
                   
                    ObservableCollection<Product> productList = datagrid.ItemsSource as ObservableCollection<Product>;
                    for (int listIndex = 0; listIndex < productList.Count; listIndex++)
                    {
                        //MessageBox.Show(list[i].Product_Qty);
                        if (productList[listIndex].ProductQty == "0")
                        {
                            TotalProductCount++;
                        }

                    }
                if (IsSaved == false)
                {
                    txtcount.Content = "Total Items : " + ((datagrid.Items.Count) - 1).ToString() + "     " + "Total Items with Quantity 0 : " + TotalProductCount;
                    savestatus.Content = "(Unsaved)";
                }
                else if (IsSaved == true)
                {
                    txtcount.Content = "Total Items : " + ((datagrid.Items.Count) - 1).ToString() + "     " + "Total Items with Quantity 0 : " + TotalProductCount;
                    savestatus.Content = "(Saved)";
                }
                //txtcount.Content = "Total Items: " + elemList.Count.ToString() + "     " + "Total Items with Quatity 0: " + count;

                //Showing Redundant Data Information
                txterror.Content = "";
                if (productList.Count > 1)
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();

                    for (int listIndex = 0; listIndex < productList.Count; listIndex++)
                    {
                        try
                        {
                            dict.Add(productList[listIndex].ProductId, 0);
                        }
                        catch (Exception e)
                        {
                            txterror.Content = "The product of Id: " + productList[listIndex].ProductId + " is already present.";
                        }
                    }
                    dict.Clear();
                }



            }
                catch (Exception e)
                {
                    return;
                }
        
            return;
        }
        private void CommandBindingOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(IsFileOpened == true && IsSaved == false)
            {
                string text = Properties.Resources.SaveFile + openFileDlg.FileName+" ?";
                MessageBoxResult messageBoxResult = MessageBox.Show(text, ProjectTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(messageBoxResult == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if(messageBoxResult == MessageBoxResult.No)
                {
                    //no code;
                }
            }
            OpenFile();
        }

        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsSaved == false)
            {
                ObservableCollection<Product> productList = datagrid.ItemsSource as ObservableCollection<Product>;
                //MessageBox.Show(list[(list.Count) - 1].Product_Name);
                string pname = productList[(productList.Count) - 1].ProductName;
                string pid = productList[(productList.Count) - 1].ProductId;
                /*doc.Load(Path);
                XmlElement root = doc.DocumentElement;
                XmlNodeList elemList1 = root.GetElementsByTagName("Product_Id");
                XmlNodeList elemList2 = root.GetElementsByTagName("Product_Name");
                for (int i = 0; i < elemList2.Count; i++)
                {
                    if (elemList1[i].InnerText == pid && String.Equals(elemList2[i].InnerText, pname, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Could not add data, The product " + pname + " is already present of Id: " + pid + "", ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }*/

                Dictionary<string, int> dict = new Dictionary<string, int>();          
                //int[] arr = new Int32[list.Count];
                for(int listIndex = 0; listIndex<productList.Count; listIndex++)
                {
                    try
                    {
                        dict.Add(productList[listIndex].ProductId, 0);
                    }
                    catch(Exception e1)
                    {
                        string text = Properties.Resources.RedundentString;
                        MessageBox.Show(text+" " + productList[listIndex].ProductId + " is already present.", ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                }
                dict.Clear();

                
            }
            else if(IsSaved == true)
            {
                //Do nothing
                //Do code of removing redundant with array latter.
            }
            SaveFile();
            return;
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = datagrid.SelectedItem;
                if (selectedItem != null)
                {
                    vm.RemoveSelectedItem((Product)selectedItem);
                    IsSaved = false;
                    ShowFileStatus();

                }
            }
            catch(Exception e5)
            {
                MessageBox.Show(Properties.Resources.WrongString, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void datagrid_BeginningEdit(object sender, System.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            IsSaved = false;
            //this.ProjectTitle += "...Not saved";
            //statusbar.Background = Brushes.Red;
            ShowFileStatus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Console.Beep();
            if (IsFileOpened == true && IsSaved == false)
            {
                string text = Properties.Resources.SaveFile + openFileDlg.FileName + " ?";
                MessageBoxResult messageBoxResult = MessageBox.Show(text, ProjectTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (messageBoxResult == MessageBoxResult.No)
                {
                    //no code;
                }
            }
            base.OnClosing(e);
        }

        private void CommandBindingNew_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            vm.ProductsCollection.Clear();
            Path = null;
            IsSaved = true;
            IsFileOpened = false;
            this.Title = ProjectTitle;
            ShowFileStatus();

        }

        private void datagrid_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            datagrid.CurrentCellChanged += Datagrid_CurrentCellChanged;
        }

        private void Datagrid_CurrentCellChanged(object sender, EventArgs e)
        {
            ShowFileStatus();         
        }

        //351 to 367 new update ....has exception on 366
        private void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+1).ToString();
            //datagrid.MouseDoubleClick += Datagrid_MouseDoubleClick;
        }

        private void Datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            datagrid.SelectionChanged += Datagrid_SelectionChanged;
            //MessageBox.Show(datagrid.SelectedItem.ToString());
        }

        private void Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView rowSelected = gd.SelectedItem as DataRowView;
            //MessageBox.Show(rowSelected["Product Name"].ToString());
        }

        private void CommandBindingClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show(Properties.Resources.Close);
            Console.Beep();
            if (IsFileOpened == true && IsSaved == false)
            {
                string text = Properties.Resources.SaveFile + openFileDlg.FileName + " ?";
                MessageBoxResult messageBoxResult = MessageBox.Show(text, ProjectTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (messageBoxResult == MessageBoxResult.No)
                {
                    //no code;
                }
            }
            vm.ProductsCollection.Clear();
            Path = null;
            IsSaved = true;
            IsFileOpened = false;
            this.Title = ProjectTitle;
            ShowFileStatus();
            //datagrid.ItemsSource = new ObservableCollection<Product>();
        }

        public static void CallShowFileStatus()
        {
            
        }

        /*private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = datagrid.SelectedItem;
                var dialog = new MyDialog((Product)data);
                if (dialog.ShowDialog() == true)
                {

                    //MessageBox.Show(dialog.name);
                    //int row = datagrid.Items.IndexOf(data);

                    doc.Load(Path);
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList element = root.GetElementsByTagName("Product_Id");
                    XmlNodeList element2 = root.GetElementsByTagName("Product_Name");
                    XmlNodeList element3 = root.GetElementsByTagName("Product_Qty");
                    XmlNodeList element4 = root.GetElementsByTagName("Product_Price");
                    for (int i = 0; i < element.Count; i++)
                    {
                        if (element[i].InnerText == dialog.id)
                        {
                            element2[i].InnerText = dialog.name;
                            element3[i].InnerText = dialog.qty;
                            element4[i].InnerText = dialog.price;
                        }
                    }
                    doc.Save(Path);
                    LoadFile(Path);
                    dialog.Close();
                }
                else
                {
                    //Do nothing
                }
            }catch(Exception e2)
            {
                MessageBox.Show(Properties.Resources.WrongString, ProjectTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }

        }*/
    }


   
}
