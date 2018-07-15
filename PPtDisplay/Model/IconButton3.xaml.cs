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
    /// IconButton3.xaml 的交互逻辑
    /// </summary>
    public partial class IconButton3 : UserControl
    {
        public IconButton3()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 记录控件是否处于点击状态.
        /// </summary>
        bool IsInClick { get; set; } = false;
        bool IsDeleteInClick { get; set; } = false;

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
        public bool IsDeleteShown
        {
            get { return (bool)GetValue(IsDeleteShownProperty); }
            set { SetValue(IsDeleteShownProperty, value); }
        }
        public bool IsError
        {
            get { return (bool)GetValue(IsErrorProperty); }
            set { SetValue(IsErrorProperty, value); }
        }
        public object ErrorText
        {
            get { return (object)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        public event RoutedEventHandler DeleteClick
        {
            add => AddHandler(DeleteClickEvent, value);
            remove => RemoveHandler(DeleteClickEvent, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconButton3), new PropertyMetadata("/xe943"));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButton3), new PropertyMetadata(""));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton3));
        public static readonly RoutedEvent DeleteClickEvent = EventManager.RegisterRoutedEvent("DeleteClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton3));
        public static readonly DependencyProperty IsDeleteShownProperty =
            DependencyProperty.Register("IsDeleteShown", typeof(bool), typeof(IconButton3), new PropertyMetadata(false));
        public static readonly DependencyProperty IsErrorProperty =
            DependencyProperty.Register("IsError", typeof(bool), typeof(IconButton3), new PropertyMetadata(false,(d,e)=> 
            {
                IconButton3 b = (IconButton3)d;
                if ((bool)e.NewValue)
                {
                    b.TextBlockError.Visibility = Visibility.Visible;
                }
                else
                {
                    b.TextBlockError.Visibility = Visibility.Collapsed;
                }
            })); 
        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(object), typeof(IconButton3), new PropertyMetadata(null));

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsInClick = true;
                Background = UserBrushes.MediumWhite;
            }
            e.Handled = false;
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            IsInClick = false;
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
        private void IconButton3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
            {
                Background = UserBrushes.MereWhite;
            }
            e.Handled = false;
            if (IsDeleteShown)
            {
                GridDelete.Visibility = Visibility.Visible;
            }
        }
        private void IconButton3_MouseLeave(object sender, MouseEventArgs e)
        {

            Background = Brushes.Transparent;
            e.Handled = false;
            if (IsDeleteShown)
            {
                GridDelete.Visibility = Visibility.Collapsed;
            }
        }
        private void IconButton3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                Background = UserBrushes.MereWhite;
            }
        }
        private void GridDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsDeleteInClick = true;
                GridDelete.Background = UserBrushes.MediumWhite;
            }
            e.Handled = false;
        }
        private void GridDelete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (IsDeleteInClick)
                {
                    RaiseEvent(new RoutedEventArgs(DeleteClickEvent));
                }
                GridDelete.Background = Brushes.Transparent;
            }
            IsDeleteInClick = false;
        }
        private void GridDelete_MouseLeave(object sender, MouseEventArgs e)
        {
            IsDeleteInClick = false;
            GridDelete.Background = Brushes.Transparent;
            e.Handled = false;
        }
    }
}
