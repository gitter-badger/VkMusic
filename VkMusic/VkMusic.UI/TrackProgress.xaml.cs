using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfUI
{
	/// <summary>
	/// Interaction logic for TrackProgress.xaml
	/// </summary>
    public partial class TrackProgress : UserControl, INotifyPropertyChanged
	{
        TimeSpan position;
        DispatcherTimer timer = new DispatcherTimer();
        MusicPlayer musicPlayer;
        bool isDragging = false;
        public event PropertyChangedEventHandler PropertyChanged;

        private string lengthTimeString;

        public string LengthTimeString
        {
            get
            {
                return lengthTimeString;
            }
            private set
            {
                lengthTimeString = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LengthTimeString"));
            }
        }

        private string currentTimeString;

        public string CurrentTimeString
        {
            get
            {
                return currentTimeString;
            }
            private set
            {
                currentTimeString = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentTimeString"));
            }
        }		
        public TrackProgress()
		{
			this.InitializeComponent();
            progressBar.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(progressBar_MouseLeftButtonUp), true);
		}
        public void BindData(MusicPlayer musicPlayer)
        {
            this.musicPlayer = musicPlayer;
            musicPlayer.MediaOpened += musicPlayer_MediaOpened;
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            musicPlayer.MusicChanged += (t) => progressBar.Value = 0;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                progressBar.Value = musicPlayer.Position.TotalSeconds;
                CurrentTimeString = musicPlayer.Position.ToString(@"mm\:ss");
            }
        }

        private void musicPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (musicPlayer.NaturalDuration.HasTimeSpan)
            {
                position = musicPlayer.NaturalDuration.TimeSpan;
                progressBar.Minimum = 0;
                progressBar.Maximum = position.TotalSeconds;

                LengthTimeString = position.ToString(@"mm\:ss");
            }
            timer.Start();
        }

        private void progressBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void progressBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            MouseMoved();
        }

        private void progressBar_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            MouseMoved();
        }
		
        private void MouseMoved()
        {
            if (musicPlayer != null)
                musicPlayer.Position = TimeSpan.FromSeconds(progressBar.Value);
        }
	}
}