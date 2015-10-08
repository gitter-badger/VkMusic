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
using System.Windows.Shapes;
using VkMusicSync;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow(string title, IProgressable progressable)
        {
            InitializeComponent();
            new DraggableWindow(this);
            titleTextBlock.Text = title;
            progressable.ProgressChanged += Progressable_ProgressChanged;
            progressable.Completed += (s, e) => Dispatcher.Invoke(Close);
        }

        private void Progressable_ProgressChanged(object sender, ProgressStatus e)
        {
            Dispatcher.Invoke(() =>
            {
                syncProgress.Value = e.DoneCount;
                syncProgress.Maximum = e.TotalCount;
                syncProgressValue.Text = string.Format("{0} из {1}", e.DoneCount, e.TotalCount);
            });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
