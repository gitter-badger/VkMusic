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

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MusicItem.xaml
    /// </summary>
    public partial class MusicItemControl : UserControl
    {
        public MusicItemControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return textBlock.Text;
            }
            set
            {
                textBlock.Text = value;
            }
        }
        public new event MouseButtonEventHandler MouseDown
        {
            add
            {
                playButton.MouseDown += value;
            }
            remove
            {
                playButton.MouseDown -= value;
            }
        }

    }
}
