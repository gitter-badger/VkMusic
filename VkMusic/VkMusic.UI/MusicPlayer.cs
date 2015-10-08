using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using VkMusicSync;
using DAL.Model;
using System.Windows;

namespace WpfUI
{
    public class MusicPlayer
    {
        MediaPlayer mediaPlayer = new MediaPlayer();

        ITrackList musicWorker;

        Track currentItem;

        public event Action<Track> MusicChanged;
        public event Action<object, EventArgs> MediaOpened;
        public event Action MusicPaused;
        public event Action MusicPlayed;
        public event Action MusicStopped;
        public event Action<List<Track>> MusicListChanged;
        public event Action<Track, List<Track>> TrackAdded;
        public event Action<long> TrackDeleted;
        public event Action Show;

        public bool CurrentlyPlaying { get; private set; }

        public Track CurrentMusic { get { return currentItem; } }


        public double Volume
        {
            get
            {
                return mediaPlayer.Volume;
            }
            set
            {
                mediaPlayer.Volume = value;
            }
        }

        public Duration NaturalDuration
        {
            get { return mediaPlayer.NaturalDuration; }
        }

        public TimeSpan Position
        {
            get { return mediaPlayer.Position; }
            set { mediaPlayer.Position = value; }
        }

        public MusicPlayer(ITrackList musicWorker)
        {
            this.musicWorker = musicWorker;

            musicWorker.AddedOneTrack += MusicWorker_AddedOneTrack;
            musicWorker.DeletedOneTrack += MusicWorker_DeletedOneTrack;

            mediaPlayer.MediaEnded += mediaPlayer_MediaEnded;
            mediaPlayer.MediaOpened += mediaPlayer_MediaOpened;

            Next();

        }

        private void MusicWorker_DeletedOneTrack(long obj)
        {
            OnTrackDeleted(obj);
        }

        void mediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (MediaOpened != null)
                MediaOpened(sender, e);
        }

        private void MusicWorker_AddedOneTrack(Track obj, List<Track> list)
        {
            OnTrackAdded(obj, list);
        }

        public void InvokeShow()
        {
            if (Show != null)
                Show();
        }

        void mediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            Next();
            Play();
        }

        public void Play()
        {
            mediaPlayer.Play();
            CurrentlyPlaying = true;
            if (MusicPlayed != null)
                MusicPlayed();

        }

        public void Stop()
        {
            mediaPlayer.Stop();
            CurrentlyPlaying = false;
            if (MusicStopped != null)
                MusicStopped();
        }

        public void Pause()
        {
            mediaPlayer.Pause();
            CurrentlyPlaying = false;
            if (MusicPaused != null)
                MusicPaused();
        }

        public void Next()
        {
            Track nextItem;
            if (currentItem != null)
                nextItem = musicWorker.GetNext(currentItem);
            else
            {
                var tracks = musicWorker.GetList();
                if (tracks.Count == 0)
                    return;
                nextItem = tracks.First();
            }

            ChangeMusic(nextItem);
        }

        public void Previous()
        {
            Track prevItem;
            prevItem = musicWorker.GetPrevious(currentItem);
            if (prevItem == null)
                return;

            ChangeMusic(prevItem);
        }

        public void ChangeMusic(Track musicItem, bool play = false)
        {
            if (currentItem!= null && currentItem.Equals(musicItem))
                return;
            
            currentItem = musicItem;

            var fileInfo = new FileInfo(musicItem.GetFilePath());

            mediaPlayer.Open(new Uri(fileInfo.FullName));

            if (MusicChanged != null)
                MusicChanged(currentItem);

            if (CurrentlyPlaying || play)
                Play();
            else
                Pause();
        }

        public IList<Track> GetList()
            => musicWorker.GetList();
        

        public bool IsRandomPlay
        {
            get { return musicWorker.IsRandomPlay; }
            set
            {
                if (musicWorker.IsRandomPlay != value)
                {
                    musicWorker.IsRandomPlay = value;
                    if (MusicListChanged != null)
                        MusicListChanged(GetList().ToList());
                }
            }
        }

        protected virtual void OnTrackAdded(Track arg1, List<Track> arg2)
        {
            if (TrackAdded != null)
                TrackAdded(arg1, arg2);
        }

        protected virtual void OnTrackDeleted(long obj)
        {
            if (TrackDeleted != null)
                TrackDeleted(obj);
        }
    }
}
