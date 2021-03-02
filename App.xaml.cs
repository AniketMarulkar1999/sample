using Prism.Ioc;
using Product_Inventory.Dialogs;
using Product_Inventory.Views;
using System.Windows;
using Prism.Mvvm;

namespace Product_Inventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        App()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("gr-GR");
            new System.Globalization.CultureInfo("en");
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<Dialogs.MessageDialog, MessageDialogViewModel>();
            
        }

        
    }
}
