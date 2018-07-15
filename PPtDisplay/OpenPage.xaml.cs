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
        }
        private void OpenPage_Loaded(object sender, RoutedEventArgs e)
        {
            int index = 0;        
            foreach (var item in App.PPts)
            {
                FileInfo f = new FileInfo(item);
                Model.IconButton2 u = new Model.IconButton2
                {
                    Name = "IconButton2_" + index,
                    Icon = "\xe8ae",
                    Text = f.Name,
                    Text2 = f.DirectoryName
                };
                u.Click += U_Click;
                StackPanel.Children.Add(u);
                index++;
            }
        }
        private void U_Click(object sender, RoutedEventArgs e)
        {
            Model.IconButton2 b = (Model.IconButton2)sender;
            string fullname = b.Text2 + @"\" + b.Text;
            App.PPtWatcher.OpenFile(fullname);
      
        }
    }
}
