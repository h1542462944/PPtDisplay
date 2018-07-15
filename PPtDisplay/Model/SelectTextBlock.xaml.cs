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
    /// SelectTextBlock.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTextBlock : UserControl
    {
        public SelectTextBlock()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 记录控件是否处于点击状态.
        /// </summary>
        public bool IsInClick { get; set; } = false;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
       

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SelectTextBlock), new PropertyMetadata(""));
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(SelectTextBlock), new PropertyMetadata(UserBrushes.BlueGreen));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SelectTextBlock));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(SelectTextBlock), new PropertyMetadata(false,(d,e)=> 
            {
                SelectTextBlock b = (SelectTextBlock)d;
                if (b.Foreground!=Brushes.White)
                {
                    if (b.IsChecked)
                    {
                        b.TextBlock1.Foreground = b.SelectedBrush;
                    }
                    else
                    {
                        b.TextBlock1.Foreground = Brushes.LightGray;
                    }
                }
                if (b.IsChecked )
                {
                    b.TextBlock1.TextDecorations = TextDecorations.Underline;
                }
                else
                {
                    b.TextBlock1.TextDecorations = null;
                }
            }));

        private void SelectTextBlockthis_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock1.Foreground = Brushes.White;
        }
        private void SelectTextBlockthis_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsChecked)
            {
                TextBlock1.Foreground = Brushes.LightGray;
            }
            else
            {
                TextBlock1.Foreground = SelectedBrush;
            }
        }
        private void SelectTextBlockthis_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsInClick = true;

            }
            e.Handled = false;
        }
        private void SelectTextBlockthis_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsInClick)
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent));
            }
        }
    }
}
