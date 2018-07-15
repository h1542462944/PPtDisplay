using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PPtDisplay.Data;

namespace PPtDisplay
{
    /// <summary>
    /// 表示PPt的状态信息.
    /// </summary>
    public class PPtWatcher
    {
        public PPtWatcher()
        {
            Fresh();
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Fresh();
        }
        public Application Application { get; private set; }
        public Presentation Presentation
        {
            get
            {
                try
                {
                    return Application?.ActivePresentation;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public Slides Slides => Presentation?.Slides;
        public Slide Slide
        {
            get
            {
                try
                {
                    return Slides[Application.ActiveWindow.Selection.SlideRange.SlideNumber];
                }
                catch (Exception)
                {
                    try
                    {
                        return Application.SlideShowWindows[1].View.Slide;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }
        bool lastState = false;
        public bool IsDisplay
        {
            get
            {
                try
                {
                    MsoTriState state = Application.SlideShowWindows[1].Active;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public int SlideIndex => Slide.SlideIndex;
        //使用null传播表达式.
        public string Path => Presentation?.FullName;
        DispatcherTimer timer;

        /// <summary>
        /// 更新PPtState的信息.
        /// </summary>
        public void Fresh()
        {
            Application application;
            try
            {
                application = (Application)Marshal.GetActiveObject("PowerPoint.Application");
            }
            catch (Exception)
            {
                application = null;
            }
            if (Application != application)
            {
                Application = application;
                try
                {
                    Application.WindowSelectionChange += (selection) =>
                    {
                        try
                        {
                            PPtSlideChanged?.Invoke(this, new PPtSlideChangedEventArgs(Slides.Count, Slide));

                        }
                        catch (Exception)
                        {
                        }
                        if (lastState == true)
                        {
                            PPtPresentationChanged?.Invoke(this, new PPtPresentationChangedEventArgs(false));
                            lastState = false;
                        }
                    };

                    Application.SlideShowNextSlide += (wn) =>
                    {
                        try
                        {
                            PPtSlideChanged?.Invoke(this, new PPtSlideChangedEventArgs(Slides.Count, Slide));
                        }
                        catch (Exception)
                        {
                        }
                        if (lastState == false)
                        {
                            PPtPresentationChanged?.Invoke(this, new PPtPresentationChangedEventArgs(true));
                            lastState = true;
                        }
                    };

                }
                catch (Exception)
                {
                }
                PPtApplictionChanged?.Invoke(this, new EventArgs());
            }
        }
        public void NextSlide()
        {
            try
            {
                Application.SlideShowWindows[1].View.Next();
            }
            catch (Exception)
            {
                int s = SlideIndex + 1;
                if (s > Slides.Count)
                {
                    s = 1;
                }
                Slides[s].Select();
            }
        }
        public void PreviouSlide()
        {
            try
            {
                Application.SlideShowWindows[1].View.Previous();

            }
            catch (Exception)
            {
                int s = SlideIndex - 1;
                if (s < 1)
                {
                    s = Slides.Count;
                }
                Slides[s].Select();
            }
        }

        public void OpenFile(string fullName)
        {
            Application application = new Application();
            application.Presentations.Open(fullName, MsoTriState.msoTrue);
        }
        /// <summary>
        /// 开始放映ppt(从当前页开始).
        /// </summary>
        public void Display()
        {
            if (Presentation != null)
            {
                SlideShowSettings s = Presentation.SlideShowSettings;
                int cindex = SlideIndex;
                s.Run();
                Application.SlideShowWindows[1].View.GotoSlide(cindex);


            }
        }
        /// <summary>
        /// 退出放映PPt.
        /// </summary>
        public void ExitDisplay()
        {
            if (Presentation != null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Application.SlideShowWindows[1].View.Exit();
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        public event EventHandler PPtApplictionChanged;
        public event EventHandler<PPtPresentationChangedEventArgs> PPtPresentationChanged;
        public event EventHandler<PPtSlideChangedEventArgs> PPtSlideChanged;
    }

    public class PPtSlideChangedEventArgs : EventArgs
    {
        public PPtSlideChangedEventArgs(int slideCount, Slide slide)
        {
            SlideCount = slideCount;
            Slide = slide;
        }

        public int SlideCount { get; private set; }
        public Slide Slide { get; private set; }
        public int SlideIndex => Slide.SlideIndex;
    }
    public class PPtPresentationChangedEventArgs : EventArgs
    {
        public PPtPresentationChangedEventArgs(bool isActivate)
        {
            IsActivate = isActivate;
        }

        public bool IsActivate { get; private set; }
    }
}
