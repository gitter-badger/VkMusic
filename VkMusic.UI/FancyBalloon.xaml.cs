using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using DAL.Model;
using Hardcodet.Wpf.TaskbarNotification;
using WpfUI;
using System.ComponentModel;
using VkMusicSync;

namespace Samples
{
    /// <summary>
    /// Interaction logic for FancyBalloon.xaml
    /// </summary>
    public partial class FancyBalloon : UserControl, INotifyPropertyChanged
    {
        private bool isClosing = false;

        public event Action<DAL.Model.Track> PlayClicked;

        string count;

        public string Count
        {
            get { return count; }
            set
            {
                count = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Count"));
            }
        }

        public FancyBalloon(int count, params DAL.Model.Track[] musicItems)
        {
            InitializeComponent();
            TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);

            Count = " (" + count + ")";

            if (musicItems.Length > 2 || musicItems.Length < 1)
            {
                throw new Exception("Something went wrong");
            }

            if (musicItems.Length == 1)
            {
                musicItem2.Visibility = System.Windows.Visibility.Hidden;
            }

            if (musicItems.Length >= 1)
            {
                musicItem1.Text = musicItems[0].TrackTitle;
                musicItem1.MouseDown += (s, e) =>
                {
                    if (PlayClicked != null)
                        PlayClicked(musicItems[0]);
                    ForceClose();
                };
            }

            if (musicItems.Length >= 2)
            {
                musicItem2.Text = musicItems[1].TrackTitle;
                musicItem2.MouseDown += (s, e) =>
                {
                    if (PlayClicked != null)
                        PlayClicked(musicItems[1]);
                    ForceClose();
                };
            }

        }


        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the custom fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true; //suppresses the popup from being closed immediately
            isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ForceClose();
        }

        private void ForceClose()
        {
            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.CloseBalloon();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;

            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }


        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup)Parent;
            pp.IsOpen = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}