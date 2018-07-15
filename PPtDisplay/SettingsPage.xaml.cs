using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PPtDisplay.Model;
using System.Windows.Forms;

namespace PPtDisplay
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadList();
        }
        async void LoadList()
        {
            StackPanel1.Children.Clear();
            foreach (var folder in App.Inventory.Inventories)
            {
                string d = System.IO.Path.GetDirectoryName(folder);
                string q = "";
                try
                {
                    q = d.Split('\\').Last();
                }
                catch (Exception)
                {
                    q = d;
                }
     
                IconButton3 b = new IconButton3() { Icon = "\xe8b7", Text = q, IsDeleteShown = true };
                b.ToolTip = folder;
                //用Lambda表达式.
                b.DeleteClick += async (sender, e) =>
                {
                    App.Inventory.Inventories.Remove(folder);
                    LoadList();
                    await App.LoadUserSettingsAsync();
                };

                StackPanel1.Children.Add(b);
            }
            //这一层{try}是为了防止异步导致的foreach发生集合已修改而产生的异常.
            try
            {
                foreach (IconButton3 b in StackPanel1.Children)
                {
                    string folder = b.ToolTip as string;
                    try
                    {
                        if (!Directory.Exists(folder))
                        {
                            b.IsError = true;
                            b.ErrorText = "文件夹不存在.";
                        }
                        else if (!await App.IsFolderHasPPt(folder))
                        {
                            b.IsError = true;
                            b.ErrorText = "文件夹中不含有ppt文件.";
                        }
                    }
                    catch (Exception)
                    {
                        b.IsError = true;
                        b.ErrorText = "其他未知的错误.";
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        
        private async void IconButton3AddFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog fd = new FolderBrowserDialog
                {
                    Description = "选择要加入列表的文件夹"
                };
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    
                    string path = fd.SelectedPath + @"\";
                    if (!path.EndsWith(@"\\"))
                    {
                        if (!App.Inventory.Inventories.Contains(path))
                        {
                            App.Inventory.Inventories.Add(path);
                            LoadList();
                            await App.LoadUserSettingsAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
