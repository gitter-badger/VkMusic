using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

using System.Windows.Interactivity;

namespace WpfUI
{
    public class EnableSliderMouseWheel : Behavior<Slider>
    {
        double? smallChangeMultiplier;
        public double SmallChangeMultiplier
        {
            get { return smallChangeMultiplier ?? 5; }
            set { smallChangeMultiplier = value; }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += new MouseWheelEventHandler(SliderMouseScroll_MouseWheel);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= new MouseWheelEventHandler(SliderMouseScroll_MouseWheel);
        }

        void SliderMouseScroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                AssociatedObject.Value = AssociatedObject.Value + AssociatedObject.SmallChange * SmallChangeMultiplier;
            else
                AssociatedObject.Value = AssociatedObject.Value - AssociatedObject.SmallChange * SmallChangeMultiplier;
        }
    }
}
