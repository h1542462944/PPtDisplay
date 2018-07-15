using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PPtDisplay
{
    /// <summary>
    /// DisplayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DisplayWindow : Window
    {
        public List<StrokeCollection> Strokes = new List<StrokeCollection>();
        public DisplayWindow()
        {
            InitializeComponent();
            Width = App.ScreenSize.Width; Height = App.ScreenSize.Height;
            Top = 0; Left = 0;
            App.PPtWatcher.PPtSlideChanged += PPtWatcher_PPtSlideChanged;
            for (int i = 0; i < App.PPtWatcher.Slides.Count; i++)
            {
                Strokes.Add(new StrokeCollection());
            }
            TextBlock1Left.Text = App.PPtWatcher.SlideIndex + "/" + App.PPtWatcher.Slides.Count;
            TextBlock1Right.Text = TextBlock1Left.Text;

            #region Extra
            Penwidth = 4;
            InkColorIndex = 14;
            GridInkColor_InitilizeComponent();
            #endregion
        }
        private void PPtWatcher_PPtSlideChanged(object sender, PPtSlideChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TextBlock1Left.Text = App.PPtWatcher.SlideIndex + "/" + App.PPtWatcher.Slides.Count;
                TextBlock1Right.Text = TextBlock1Left.Text;
                for (int i = Strokes.Count; i < App.PPtWatcher.Slides.Count; i++)
                {
                    Strokes.Add(new StrokeCollection());
                }
                InkCanvas1.Strokes = Strokes[App.PPtWatcher.SlideIndex - 1];
            });
        }
        bool isMenuVisiable = true;
        DisplayEditingMode displayEditingMode;
        public bool IsMenuVisiable
        {
            get => isMenuVisiable;
            set
            {
                SetMenuVisibility(value);
                isMenuVisiable = value;
            }
        }
        public DisplayEditingMode DisplayEditingMode
        {
            get => displayEditingMode; set
            {
                if (value == DisplayEditingMode.Mouse)
                {
                    IconButton0.Foreground = Model.UserBrushes.BlueGreen;
                    IconButton1.Foreground = Brushes.White;
                    IconButton2.Foreground = Brushes.White;
                    IconButton3.Foreground = Brushes.White;

                    InkCanvas1.Visibility = Visibility.Collapsed;
                }
                else if (value == DisplayEditingMode.Pen)
                {
                    IconButton0.Foreground = Brushes.White;
                    IconButton1.Foreground = Model.UserBrushes.BlueGreen;
                    IconButton2.Foreground = Brushes.White;
                    IconButton3.Foreground = Brushes.White;

                    InkCanvas1.Visibility = Visibility.Visible;
                    InkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
                }
                else if (value == DisplayEditingMode.Eraser)
                {
                    IconButton0.Foreground = Brushes.White;
                    IconButton1.Foreground = Brushes.White;
                    IconButton2.Foreground = Model.UserBrushes.BlueGreen;
                    IconButton3.Foreground = Brushes.White;

                    InkCanvas1.Visibility = Visibility.Visible;
                    InkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;
                }
                else
                {
                    IconButton0.Foreground = Brushes.White;
                    IconButton1.Foreground = Brushes.White;
                    IconButton2.Foreground = Brushes.White;
                    IconButton3.Foreground = Model.UserBrushes.BlueGreen;

                    InkCanvas1.Visibility = Visibility.Visible;
                    InkCanvas1.EditingMode = InkCanvasEditingMode.Select;
                }
                displayEditingMode = value;
            }
        }

        void SetMenuVisibility(bool isVisiable)
        {
            if (isVisiable)
            {
                GridMenu.Visibility = Visibility.Visible;
                IconButtonHide.Foreground = Model.UserBrushes.BlueGreen;
                IconButtonHide.Text = "\xe81d";
                IconButtonHide.FontSize = 24;
                InkCanvas1.Visibility = Visibility.Visible;
            }
            else
            {
                GridMenu.Visibility = Visibility.Collapsed;
                IconButtonHide.Foreground = Brushes.White;
                IconButtonHide.Text = "\xea3a";
                IconButtonHide.FontSize = 20;
                InkCanvas1.Visibility = Visibility.Collapsed;
            }
        }

        private void IconButtonClose_Click(object sender, RoutedEventArgs e)
        {
            App.GotoMainWindow();
            //{debug}:测试发现这一语句可能会发生错误.
            App.PPtWatcher.ExitDisplay();

        }
        private void IconButtonHide_Click(object sender, RoutedEventArgs e)
        {
            IsMenuVisiable = !IsMenuVisiable;
        }
        private void IconButtonPres_Click(object sender, RoutedEventArgs e)
        {
            App.PPtWatcher.PreviouSlide();
        }
        private void IconButtonNext_Click(object sender, RoutedEventArgs e)
        {
            App.PPtWatcher.NextSlide();
        }
        private void IconButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(IconButton0))
            {
                DisplayEditingMode = DisplayEditingMode.Mouse;
            }
            else if (sender.Equals(IconButton1))
            {
                if (DisplayEditingMode == DisplayEditingMode.Pen)
                {
                    if (State1 == false)
                    {
                        Popup1.IsOpen = true;
                        State1 = true;
                    }
                    else
                    {
                        State1 = false;
                    }
                }
                DisplayEditingMode = DisplayEditingMode.Pen;
            }
            else if (sender.Equals(IconButton2))
            {
                if (DisplayEditingMode == DisplayEditingMode.Eraser)
                {
                    if (State2 == false)
                    {
                        Popup2.IsOpen = true;
                        State2 = true;
                    }
                    else
                    {
                        State2 = false;
                    }
                }
                DisplayEditingMode = DisplayEditingMode.Eraser;
            }
            else
            {
                DisplayEditingMode = DisplayEditingMode.Select;
            }
        }

        #region Extra
        bool State1 = false;
        bool State2 = false;

        public static double Checkdouble(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }
        public static double GetSlideValue(Border border, double offset)
        {
            return Checkdouble((Mouse.GetPosition(border).X - offset) / (border.ActualWidth - 2 * offset), 0, 1);
        }
        private void GridInkColor_InitilizeComponent()
        {
            int column = 0;
            int row = 0;
            for (row = 0; row < 5; row++)
            {
                for (column = 0; column < 5; column++)
                {
                    int index = 5 * row + column;
                    Ellipse ellipse = new Ellipse()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(40 * column + 5, 30 * row, 0, 0),
                        Width = 30,
                        Height = 30
                    };
                    ElpInkColor[index] = ellipse;
                    ElpInkColor[index].MouseUp += BdrInkColor_MouseUp;
                    GridInkColorBox.Children.Add(ellipse);
                }
            }
            for (int i = 0; i < ElpInkColor.Length; i++)
            {
                ElpInkColor[i].Fill = new SolidColorBrush(InkColorDefault[i]);
            }
        }

        private readonly Color[] InkColorDefault = new Color[]
        {
            Colors.Black,Colors.DarkGray,Colors.Gray,Colors.LightGray,Colors.White,
            Colors.Red,Colors.OrangeRed,Colors.Orange,Colors.Gold,Colors.Yellow,
            Colors.LawnGreen,Colors.Green,Colors.SeaGreen,Colors.DeepSkyBlue,Colors.Blue,
            Colors.Tomato,Colors.Violet,Colors.LightYellow,Colors.LightGreen,Colors.LightBlue,
            Colors.Pink,Colors.RosyBrown,Colors.Chocolate,Colors.Brown,Colors.Purple,
        };
        internal Ellipse[] ElpInkColor = new Ellipse[25];

        const double penwidthmin = 2;
        const double penwidthmax = 12;

        private double penwidth = 4;
        public double Penwidth
        {
            get => penwidth;

            set
            {
                double bi = (value - penwidthmin) / (penwidthmax - penwidthmin);
                this.BdISM.Margin = new Thickness(6 + bi * 120, 2.5, 0, 2.5);
                this.ElpPenwidth.Width = (int)value;
                this.ElpPenwidth.Height = (int)value;
                this.LblPenwidth.Text = ((int)value).ToString();
                this.InkCanvas1.DefaultDrawingAttributes.Width = (int)value;
                this.InkCanvas1.DefaultDrawingAttributes.Height = (int)value;
                penwidth = value;
            }
        }
        private int inkColorIndex = 0;
        public int InkColorIndex
        {
            get => inkColorIndex;
            set
            {
                int arg = value;
                int row = arg / 5;
                int column = arg % 5;
                ElpInkColorSelect.Margin = new Thickness(40 * column + 5, 30 * row, 0, 0);
                Color color = InkColorDefault[arg];
                InkCanvas1.DefaultDrawingAttributes.Color = InkColorDefault[arg];
                inkColorIndex = value;
            }
        }

        bool isPenwidthSlideMouseDown;
        bool isEraseSlideMouseDown;

        private void BorderInkSize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isPenwidthSlideMouseDown = true;
            double v = GetSlideValue(this.BorderInkSize, 10);
            this.Penwidth = penwidthmin + v * (penwidthmax - penwidthmin);
        }
        private void BorderInkSize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPenwidthSlideMouseDown)
            {
                double v = GetSlideValue(this.BorderInkSize, 10);

                this.Penwidth = penwidthmin + v * (penwidthmax - penwidthmin);
            }
        }
        private void BdrInkColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < ElpInkColor.Length; i++)
            {
                if (sender.Equals(ElpInkColor[i]))
                {
                    InkColorIndex = i;
                    break;
                }
            }
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isPenwidthSlideMouseDown = false;
            if (isEraseSlideMouseDown)
            {
                if (e.GetPosition(BdrEraser).X >= 180)
                {
                    InkCanvas1.Strokes = new StrokeCollection();
                }
                BdEM.Margin = new Thickness(-15, 0, 0, 0);
                DisplayEditingMode = DisplayEditingMode.Pen;
                Popup2.IsOpen = false;
            }
            isEraseSlideMouseDown = false;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            BorderInkSize_MouseMove(sender, e);
            if (isEraseSlideMouseDown)
            {
                Point point = e.GetPosition(BdrEraser);
                if (point.X < 20)
                {
                    point.X = 20;
                }
                else if (point.X > 180)
                {
                    point.X = 180;
                }
                BdEM.Margin = new Thickness(point.X - 35, 0, 0, 0);
            }
            if (State1 == true || State2 == true)
            {
                IconButton1.Background = Model.UserBrushes.DeepBlack;
                IconButton2.Background = Model.UserBrushes.DeepBlack;
            }
        }
        private void BdrEraser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BdrEraser);
            if (point.X >= 5 && point.X <= 35)
            {
                if (point.X >= 20)
                {
                    BdEM.Margin = new Thickness(point.X - 35, 0, 0, 0);
                }
                isEraseSlideMouseDown = true;
            }
        }

        #endregion


    }

    public enum DisplayEditingMode
    {
        Mouse,
        Pen,
        Eraser,
        Select
    }
}
