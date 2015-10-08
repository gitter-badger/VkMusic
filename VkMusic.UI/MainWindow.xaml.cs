﻿using System.Windows;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
