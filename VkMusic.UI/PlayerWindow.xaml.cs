using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DAL.Model;
using Microsoft.Practices.Unity;
using VkMusicSync;
using WpfUI;

namespace VkMusic.UI
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window, INotifyPropertyChanged
    {

        [Dependency]
        public MusicPlayer musicPlayer { get; set; }

        private string playingMusicName;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsRandomPlay
        {
            get
            {
                return musicPlayer.IsRandomPlay;
            }
            private set
            {
                musicPlayer.IsRandomPlay = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRandomPlay"));
            }
        }

        public string PlayingMusicName
        {
            get
            {
                return playingMusicName;
            }
            private set
            {
                playingMusicName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PlayingMusicName"));
            }
        }

        public double Volume
        {
            get
            {
                return musicPlayer.Volume;
            }
            private set
            {
                musicPlayer.Volume = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Volume"));
            }
        }

        private string playingMusicSinger;

        public string PlayingMusicSinger
        {
            get
            {
                return playingMusicSinger;
            }
            private set
            {
                playingMusicSinger = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PlayingMusicSinger"));
            }
        }


        public PlayerWindow()
        {
            DependencyUtility.BuildUp(this);
            InitializeComponent();

            

            this.Left = Properties.Settings.Default.Left;
            this.Top = Properties.Settings.Default.Top;

            new DraggableWindow(this).DragFinished += (s, e) =>
            {
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Save();
            };

            musicSelectControl.Bind(musicPlayer.GetList().ToList());
            musicSelectControl.MusicChanged += musicItem => { musicPlayer.ChangeMusic(musicItem); musicPlayer.Play(); };


            pauseButton.Visibility = Visibility.Hidden;

            musicPlayer.MusicPlayed += musicPlayer_MusicPlayed;
            musicPlayer.MusicPaused += musicPlayer_MusicStopped;
            musicPlayer.MusicChanged += musicPlayer_MusicChanged;

            musicPlayer.Show += MusicPlayer_Show;

            musicPlayer.TrackAdded += MusicPlayer_TrackAdded;
            musicPlayer.TrackDeleted += MusicPlayer_TrackDeleted;

            trackProgress.BindData(musicPlayer);
            musicPlayer_MusicChanged(null);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;
            Hide();
        }

        private void MusicPlayer_TrackDeleted(long obj)
        {
            Dispatcher.Invoke(() =>
            {
                musicSelectControl.DeleteItem(obj);
            });
        }

        private void MusicPlayer_TrackAdded(Track item, List<Track> list)
        {
            Dispatcher.Invoke(() =>
            {
                musicSelectControl.AddItems(item, list);
            });
        }

        private void MusicPlayer_Show()
        {
            this.Show();
            this.Activate();
        }

        void musicPlayer_MusicChanged(Track track)
        {
            Dispatcher.Invoke(() =>
            {
                if (track != null)
                    musicSelectControl.SetSelected(track);
                if (musicPlayer.CurrentMusic != null)
                {
                    PlayingMusicName = musicPlayer.CurrentMusic.Title;
                    PlayingMusicSinger = musicPlayer.CurrentMusic.Artist;

                }
            });
        }


        void musicPlayer_MusicStopped()
        {
            playButton.Visibility = Visibility.Visible;
            pauseButton.Visibility = Visibility.Hidden;
        }

        void musicPlayer_MusicPlayed()
        {
            playButton.Visibility = Visibility.Hidden;
            pauseButton.Visibility = Visibility.Visible;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            musicPlayer.Play();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            musicPlayer.Pause();
        }

        private void soundButton_Click(object sender, RoutedEventArgs e)
        {
            if (Volume == 0)
                Volume = 0.5;
            else
                Volume = 0;
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            musicPlayer.Next();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            musicPlayer.Previous();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

    }
}
