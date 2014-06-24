using System.Windows;
using System.Windows.Controls;

namespace EasterEgg.Controls
{

    public abstract class FallingObject : Control
    {

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(FallingObject), new PropertyMetadata(default(double)));

        public double Size
        {
            get { return (double) GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

    }

}
