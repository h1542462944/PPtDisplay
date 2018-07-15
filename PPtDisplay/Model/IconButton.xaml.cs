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
    /// IconButton.xaml 的交互逻辑
    /// </summary>
    public partial class IconButton : UserControl
    {
        public IconButton()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 记录控件是否处于点击状态.
        /// </summary>
        public bool IsInClick { get; set; } = false;

        /// <summary>
        /// 显示的文字
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public WindowColorStyle ColorStyle
        {
            get { return (WindowColorStyle)GetValue(ColorStyleProperty); }
            set { SetValue(ColorStyleProperty, value); }
        }

        public bool IsSpecialColor
        {
            get { return (bool)GetValue(IsSpecialColorProperty); }
            set { SetValue(IsSpecialColorProperty, value); }
        }
        public static readonly DependencyProperty IsSpecialColorProperty =
            DependencyProperty.Register("IsSpecialColor", typeof(bool), typeof(IconButton), new PropertyMetadata(false));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        private void IconButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                IsInClick = true;
                if (!IsSpecialColor)
                {
                    if (ColorStyle == WindowColorStyle.Dark)
                    {
                        Background = UserBrushes.MediumBlack;
                    }
                    else
                    {
                        Background = UserBrushes.MediumWhite;
                    }
                }
                else
                {
                    Background = UserBrushes.LightRed;
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton==MouseButtonState.Released)
            {
                if (!IsSpecialColor)
                {
                    if (ColorStyle == WindowColorStyle.Dark)
                    {
                        Background = UserBrushes.StrongBlack;
                    }
                    else
                    {
                        Background = UserBrushes.MereWhite;
                    }
                }
                else
                {
                    Background = UserBrushes.MereDarkRed;
                }
            }
            e.Handled = false;
        }
        private void IconButton_MouseLeave(object sender, MouseEventArgs e)
        {
            IsInClick = false;
            if (ColorStyle == WindowColorStyle.Transparent)
            {
                Background = Brushes.Transparent;
            }
            else
            {
                Background = UserBrushes.DeepBlack;
            }
            e.Handled = false;
        }
        private void IconButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                //处于单击状态.
                if (IsInClick)
                {
                    RaiseEvent(new RoutedEventArgs(ClickEvent));
                }
                if (!IsSpecialColor)
                {
                    if (ColorStyle == WindowColorStyle.Dark)
                    {
                        Background = UserBrushes.StrongBlack;
                    }
                    else
                    {
                        Background = UserBrushes.MereWhite;
                    }
                }
                else
                {
                    Background = UserBrushes.MereDarkRed;
                }
            }
            IsInClick = false;
            e.Handled = false;
        }
        //以下为静态成员
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButton), new PropertyMetadata(""));
        public static readonly DependencyProperty ColorStyleProperty =
            DependencyProperty.Register("ColorStyle", typeof(WindowColorStyle), typeof(IconButton), new PropertyMetadata(WindowColorStyle.Transparent,new PropertyChangedCallback(ColorStyle_Changed)));

        private static void ColorStyle_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconButton m = (IconButton)d;
            if ((WindowColorStyle)e.NewValue == WindowColorStyle.Dark)
            {
                m.Background = UserBrushes.DeepBlack;
            }
            else
            {
                m.Background = Brushes.Transparent;
            }
        }
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IconButton));
    }
}
