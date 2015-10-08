using DAL.Model;
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
using VkMusicSync;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MusicSelectControl.xaml
    /// </summary>
    public partial class MusicSelectControl : UserControl
    {

        class ComboBoxItem
        {
            public string NameWithSinger { get; set; }
            public string TimeString { get; set; }
            public Track Track { get; set; }
        }

        public event Action<Track> MusicChanged;

        private List<ComboBoxItem> items;

        public MusicSelectControl()
        {
            InitializeComponent();

        }

        public void AddItems(Track item, List<Track> tracks)
        {

            var isNewItem = items.All(mi => !mi.Track.Equals(item));
            if (items == null)
            {
                items = new List<ComboBoxItem>();
                items.Add(new ComboBoxItem()
                {
                    NameWithSinger = item.GetNameWithSinger(),
                    TimeString = item.GetTimeString(),
                    Track = item
                });

                comboBox.ItemsSource = items;
            }
            else if (isNewItem)
            {
                Bind(tracks);
            }

        }

        public void DeleteItem(long trackId)
        {
            items.RemoveAll(i => i.Track.Id == trackId);
            Dispatcher.Invoke(() =>
            {
                Bind(items.Select(i=>i.Track).ToList());
            });
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MusicChanged != null)
            {
                var item = comboBox.SelectedItem as ComboBoxItem;
                if (item != null)
                    MusicChanged(item.Track);
            }
        }

        public void SetSelected(Track musicItem)
        {
            var list = comboBox.ItemsSource as List<ComboBoxItem>;

            var item = list.FirstOrDefault(i => i.Track.Equals(musicItem));

            comboBox.SelectedItem = item;
        }

        public void Bind(List<Track> musicItems)
        {
            var selectedItem = comboBox.SelectedValue as ComboBoxItem;
            items = musicItems.Select(t => new ComboBoxItem
            {
                NameWithSinger = t.GetNameWithSinger(),
                TimeString = t.GetTimeString(),
                Track = t
            }).ToList();


            comboBox.ItemsSource = items;

            if (selectedItem != null)
                comboBox.SelectedItem = items.FirstOrDefault(i => selectedItem.Track.Equals(i.Track));
        }
    }
}
