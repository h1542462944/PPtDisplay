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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PPtDisplay
{
    /// <summary>
    /// StartPage.xaml 的交互逻辑
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            App.Window.SetTitleBar(GridTitle);
            App.Window.SetTitleBar(GridMain);
            Loaded += StartPage_Loaded;
        }

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => System.Threading.Thread.Sleep(1000));
            await App.Inventory.LoadAsync();
            if (App.PPtWatcher.Application != null)
            {
                if (App.PPtWatcher.IsDisplay)
                {
                    App.GotoDisplayWindow();
                    App.PPtWatcher.Display();
                }        
            }
            else
            {
            }
            App.Window.NavigateTo(typeof(MainPage));
        }
    }
}
