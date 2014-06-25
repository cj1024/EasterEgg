using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using EasterEgg.Controls.Util;

namespace EasterEgg.Controls
{

    public abstract class FallingObjectPanel<T> : RepeatedlyAnimatedObjectPanel
        where T : FallingObject, new()
    {

        public static readonly DependencyProperty FallingObjectStyleProperty = DependencyProperty.Register(
            "FallingObjectStyle", typeof(Style), typeof(FallingObjectPanel<T>), new PropertyMetadata(default(Style)));

        public Style FallingObjectStyle
        {
            get { return (Style)GetValue(FallingObjectStyleProperty); }
            set { SetValue(FallingObjectStyleProperty, value); }
        }

        protected readonly Queue<T> ObjectsPool;

        protected FallingObjectPanel()
        {
            ObjectsPool = new Queue<T>();
        }

        protected sealed override void GenerateItemTimer_Tick(object sender, EventArgs e)
        {
            var target = ObjectsPool.SafeDequeue(true);
            target.Style = FallingObjectStyle;
            if (Holder != null && !Holder.Children.Contains(target))
            {
                Holder.Children.Add(target);
            }
            target.Size = Size + Random.NextDouble() * SizeRange;
            Canvas.SetLeft(target, Random.NextDouble() * ActualWidth);
            Canvas.SetTop(target, -Size);
            var sb = GenerateTransition(target);
            sb.Completed += (storyboard, eventargs) => HandleSnowTransitionComplete(target);
            sb.Begin();
        }

        protected virtual Storyboard GenerateTransition(T target)
        {
            target.RenderTransform = new CompositeTransform();
            target.Opacity = 1;
            var result = new Storyboard();
            var speed = Speed + Random.NextDouble() * SpeedRange;
            var fallDuration = TimeSpan.FromSeconds(ActualHeight / speed);
            var nonMeltDuration = TimeSpan.FromMilliseconds(NonMeltDuration.Milliseconds + Random.NextDouble() * NonMeltDurationRange.Milliseconds);
            var meltDuration = TimeSpan.FromMilliseconds(MeltDuration.Milliseconds + Random.NextDouble() * MeltDurationRange.Milliseconds);
            result.Children.Add(GenerateFallingTransition(target, fallDuration, TimeSpan.Zero)); 
            result.Children.Add(GenerateMeltTransition(target, nonMeltDuration, meltDuration, fallDuration));
            result.BeginTime = TimeSpan.FromMilliseconds(Random.NextDouble() * FrequenceRange.Milliseconds);
            return result;
        }

        protected virtual Timeline GenerateFallingTransition(T target, TimeSpan duration, TimeSpan delay)
        {
            var translationY = new DoubleAnimation
            {
                BeginTime = delay,
                From = 0,
                To = ActualHeight - target.Size,
                Duration = duration,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(translationY, target);
            Storyboard.SetTargetProperty(translationY, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));
            return translationY;
        }

        protected virtual Timeline GenerateMeltTransition(T target, TimeSpan nonMeltDuration, TimeSpan meltDuration, TimeSpan delay)
        {
            var melt = new DoubleAnimationUsingKeyFrames();
            melt.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(0),
                Value = 1
            });
            melt.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = delay,
                Value = 1
            });
            melt.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(delay.TotalMilliseconds + nonMeltDuration.TotalMilliseconds),
                Value = 1
            });
            melt.KeyFrames.Add(new EasingDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(delay.TotalMilliseconds + nonMeltDuration.TotalMilliseconds + meltDuration.TotalMilliseconds),
                Value = 0
            });
            Storyboard.SetTarget(melt, target);
            Storyboard.SetTargetProperty(melt, new PropertyPath("UIElement.Opacity"));
            return melt;
        }

        protected virtual void HandleSnowTransitionComplete(T target)
        {
            if (Holder != null && Holder.Children.Contains(target))
            {
                Holder.Children.Remove(target);
            }
            ObjectsPool.Enqueue(target);
        }

    }

}
