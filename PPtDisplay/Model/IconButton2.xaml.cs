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

namespace PPtDisplay.Model
{
    /// <summary>
    /// IconButton2.xaml 的交互逻辑
    /// </summary>
    public partial class IconButton2 : UserControl
    {
        public IconButton2()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 记录控件是否处于点击状态.
        /// </summary>
        public bool IsInClick { get; set; } = false;

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconButton2), new PropertyMetadata("/xe943"));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButton2), new PropertyMetadata(""));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton2));
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(IconButton2), new PropertyMetadata("",(d,e)=>
            {
                IconButton2 b = (IconButton2)d;
                if (b.Text2 == "")
                {
                    b.TextBlock2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    b.TextBlock2.Visibility = Visibility.Visible;
                    b.TextBlock2.Text = b.Text2;
                }
            }));

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsInClick = true;
                Background = UserBrushes.MediumWhite;
            }
            e.Handled = false;
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                Background = UserBrushes.MereWhite;
            }
            e.Handled = false;
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            IsInClick = false;
            Background = Brushes.Transparent;
            e.Handled = false;
        }
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (IsInClick)
                {
                    RaiseEvent(new RoutedEventArgs(ClickEvent));
                }
                Background = UserBrushes.MereWhite;
            }
            IsInClick = false;
        }

    }
}
