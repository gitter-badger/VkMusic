using DAL.Model;
using System;
using System.IO;
using VkNet.Model.Attachments;

namespace VkMusicSync
{
    public static class TrackExtension
    {
        public static string GetFilePath(this Track track)
            => Path.Combine(AppPaths.AppPath, track.Id + ".mp3");
        

        public static string GetNameWithSinger(this Track track)
            => track.Artist + " - " + track.Title;
        

        public static string GetTimeString(this Track track)
            => TimeSpan.FromSeconds(track.Duration).ToString(@"mm\:ss");
        

        public static Track Update(this Track track, Audio audio)
        {
            track.Id = audio.Id;
            track.OwnerId = audio.OwnerId.HasValue ? audio.OwnerId.Value : -1;
            track.Artist = audio.Artist;
            track.Title = audio.Title;
            track.Duration = audio.Duration;
            track.CreatedDateTime = DateTime.UtcNow;

            return track;
        }

    }
}
