using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// OpenPage.xaml 的交互逻辑
    /// </summary>
    public partial class OpenPage : Page
    {
        public OpenPage()
        {
            InitializeComponent();
            Loaded += OpenPage_Loaded;
        
            App.Inventories_AddItem += App_Inventories_AddItem;
            App.Inventories_RemoveAllItem += App_Inventories_RemoveAllItem;
        }

        private void App_Inventories_RemoveAllItem()
        {
            Dispatcher.Invoke(()=> 
            {
                StackPanel.Children.Clear();
            });             
        }
        private void App_Inventories_AddItem(string obj)
        {
            Dispatcher.Invoke(() => 
            {
                FileInfo f = new FileInfo(obj);
                Model.IconButton2 u = new Model.IconButton2
                {
                    Icon = "\xe8ae",
                    Text = f.Name,
                    Text2 = f.DirectoryName
                };
                u.Click += U_Click;
                StackPanel.Children.Add(u);
            });
        }

        private async void OpenPage_Loaded(object sender, RoutedEventArgs e)
        {
            await App.LoadUserSettingsAsync();
        }

        private void U_Click(object sender, RoutedEventArgs e)
        {
            Model.IconButton2 b = (Model.IconButton2)sender;
            string fullname = b.Text2 + @"\" + b.Text;
            App.PPtWatcher.OpenFile(fullname);
      
        }
        private void IconButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter ="演示文稿(*.ppt,*.pptx) | *.ppt; *.pptx"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                App.PPtWatcher.OpenFile(openFileDialog.FileName);
            }
        }
    }
}
