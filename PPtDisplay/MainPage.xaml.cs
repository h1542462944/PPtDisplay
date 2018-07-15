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
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            App.Window.SetTitleBar(GridTitle);
            NavigateTo(typeof(OpenPage));
            App.Start_Fresh += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    IconButtonFresh.Visibility = Visibility.Visible;
                });
            };
            App.End_Fresh += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    IconButtonFresh.Visibility = Visibility.Collapsed;
                });
            };
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            FrameSettings.Content = new SettingsPage();
        }

        void NavigateTo(Type pageType)
        {
            Page p = (Page)Activator.CreateInstance(pageType);
            FrameContent.Content = p;
        }

        private void BorderTool_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridExtra.Visibility = Visibility.Collapsed;
        }
        private void IconButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            GridExtra.Visibility = Visibility.Visible;
        }

    }
}
