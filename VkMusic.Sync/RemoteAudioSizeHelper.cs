using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace VkMusicSync
{
    public class RemoteAudioSizeHelper
    {
        string fileName;

        Dictionary<long, long> cacheDictionary = new Dictionary<long, long>();

        public RemoteAudioSizeHelper(string fileName)
        {
            this.fileName = fileName;
            Load();
        }

        
        // TODO change this method to more understandable
        public long GetSize(VkNet.Model.Attachments.Audio audio)
        {
            if (cacheDictionary.ContainsKey(audio.Id))
                return cacheDictionary[audio.Id];
            try
            {
                //from http://stackoverflow.com/questions/122853/get-http-file-size
                System.Net.WebRequest req = System.Net.HttpWebRequest.Create(audio.Url);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    long length;
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out length))
                    {
                        cacheDictionary[audio.Id] = length;
                        Save();
                        return length;
                    }
                    return 0;
                }
            }
            catch (Exception) { return 0; }
        }

        object saveLock = new object();

        public void Save()
        {
            if (Monitor.IsEntered(saveLock))
                return;
            Monitor.Enter(saveLock);

            using (var stream = File.Create(fileName))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, cacheDictionary);
            }

            Monitor.Exit(saveLock);
        }

        private void Load()
        {
            if (!File.Exists(fileName))
                return;

            try
            {
                using (var stream = File.OpenRead(fileName))
                {
                    var formatter = new BinaryFormatter();
                    cacheDictionary = formatter.Deserialize(stream) as Dictionary<long, long>;
                }
            }
            catch
            {

            }
        }

    }
}
