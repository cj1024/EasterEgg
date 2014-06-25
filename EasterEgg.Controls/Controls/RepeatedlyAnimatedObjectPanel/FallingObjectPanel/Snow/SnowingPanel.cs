using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace EasterEgg.Controls
{

    public sealed class SnowingPanel : FallingObjectPanel<Snow>
    {

        public SnowingPanel()
        {
            DefaultStyleKey = typeof(SnowingPanel);
        }

        protected override Timeline GenerateFallingTransition(Snow target, TimeSpan duration, TimeSpan delay)
        {
            var result = new Storyboard();
            result.Children.Add(base.GenerateFallingTransition(target, duration, delay));
            var translateX = new DoubleAnimation
                             {
                                 BeginTime = delay,
                                 Duration = duration,
                                 From = Random.Next(20, 40)*(Random.Next(1) > 0 ? 1 : -1),
                                 To = 0,
                                 EasingFunction = new ElasticEase {EasingMode = EasingMode.EaseInOut, Oscillations = Random.Next(3, 5), Springiness = 2 + Random.NextDouble()*2}
                             };
            Storyboard.SetTarget(translateX, target);
            Storyboard.SetTargetProperty(translateX, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
            result.Children.Add(translateX);
            return result;
        }

    }

}
