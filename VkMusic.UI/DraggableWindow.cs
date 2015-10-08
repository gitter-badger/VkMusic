using System;
using System.Windows;
using System.Windows.Input;

namespace WpfUI
{
    public class DraggableWindow : System.IDisposable
    {

        Window window;

        public event EventHandler DragFinished;

        public DraggableWindow(Window window)
        {
            this.window = window;

            window.MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                window.DragMove();
                if (DragFinished != null)
                    DragFinished(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            window.MouseDown -= Window_MouseDown;
        }
    }
}
