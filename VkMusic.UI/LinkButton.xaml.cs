using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for LinkButton.xaml
    /// </summary>
    public partial class LinkButton : UserControl
    {
        public LinkButton()
        {
            InitializeComponent();
            button.Click += (s, e) => System.Diagnostics.Process.Start(NavigateUrl);
        }

        public string Text
        {
            get { return button.Content as string; }
            set { button.Content = value; }
        }

        public string NavigateUrl { get; set; }

        public new double FontSize
        {
            get { return button.FontSize; }
            set { button.FontSize = value; }
        }
    }
}
