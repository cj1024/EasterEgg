using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EasterEgg.Controls
{

    [TemplatePart(Name = HolderName, Type = typeof(Panel))]
    public abstract partial class RepeatedlyAnimatedObjectPanel : Control
    {

        private const string HolderName = "Holder";

        protected Panel Holder { get; private set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Holder = GetTemplateChild(HolderName) as Panel;
            HandleOnFrequenceChanged();
        }

        public static readonly DependencyProperty FrequenceProperty = DependencyProperty.Register(
            "Frequence", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan), OnFrequenceChanged));

        /// <summary>
        /// 物体产生频率
        /// </summary>
        public TimeSpan Frequence
        {
            get { return (TimeSpan)GetValue(FrequenceProperty); }
            set { SetValue(FrequenceProperty, value); }
        }

        static void OnFrequenceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((RepeatedlyAnimatedObjectPanel)obj).HandleOnFrequenceChanged();
        }

        protected void HandleOnFrequenceChanged()
        {
            Timer.Stop();
            if (Frequence != TimeSpan.Zero && Frequence.Duration() != TimeSpan.MaxValue)
            {
                Timer.Interval = Frequence.Duration();
                Timer.Start();
            }
        }

        public static readonly DependencyProperty FrequenceRangeProperty = DependencyProperty.Register(
            "FrequenceRange", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 物体产生频率范围
        /// </summary>
        public TimeSpan FrequenceRange
        {
            get { return (TimeSpan)GetValue(FrequenceRangeProperty); }
            set { SetValue(FrequenceRangeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(double)));

        /// <summary>
        /// 物体大小
        /// </summary>
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeRangeProperty = DependencyProperty.Register(
            "SizeRange", typeof(double), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(double)));

        /// <summary>
        /// 物体大小范围
        /// </summary>
        public double SizeRange
        {
            get { return (double)GetValue(SizeRangeProperty); }
            set { SetValue(SizeRangeProperty, value); }
        }

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
            "Speed", typeof(double), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(double)));

        /// <summary>
        /// 物体下落速率
        /// </summary>
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        public static readonly DependencyProperty SpeedRangeProperty = DependencyProperty.Register(
            "SpeedRange", typeof(double), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(double)));

        /// <summary>
        /// 物体下落速率范围
        /// </summary>
        public double SpeedRange
        {
            get { return (double)GetValue(SpeedRangeProperty); }
            set { SetValue(SpeedRangeProperty, value); }
        }

        public static readonly DependencyProperty NonMeltDurationProperty = DependencyProperty.Register(
            "NonMeltDuration", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 融化前保持的时长
        /// </summary>
        public TimeSpan NonMeltDuration
        {
            get { return (TimeSpan)GetValue(NonMeltDurationProperty); }
            set { SetValue(NonMeltDurationProperty, value); }
        }

        public static readonly DependencyProperty NonMeltDurationRangeProperty = DependencyProperty.Register(
            "NonMeltDurationRange", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 融化前保持的时长范围
        /// </summary>
        public TimeSpan NonMeltDurationRange
        {
            get { return (TimeSpan)GetValue(NonMeltDurationRangeProperty); }
            set { SetValue(NonMeltDurationRangeProperty, value); }
        }

        public static readonly DependencyProperty MeltDurationProperty = DependencyProperty.Register(
            "MeltDuration", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 融化时长
        /// </summary>
        public TimeSpan MeltDuration
        {
            get { return (TimeSpan)GetValue(MeltDurationProperty); }
            set { SetValue(MeltDurationProperty, value); }
        }

        public static readonly DependencyProperty MeltDurationRangeProperty = DependencyProperty.Register(
            "MeltDurationRange", typeof(TimeSpan), typeof(RepeatedlyAnimatedObjectPanel), new PropertyMetadata(default(TimeSpan)));

        /// <summary>
        /// 融化时长范围
        /// </summary>
        public TimeSpan MeltDurationRange
        {
            get { return (TimeSpan)GetValue(MeltDurationRangeProperty); }
            set { SetValue(MeltDurationRangeProperty, value); }
        }

    }

    partial class RepeatedlyAnimatedObjectPanel
    {

        private readonly DispatcherTimer Timer;

        protected static readonly Random Random;

        static RepeatedlyAnimatedObjectPanel()
        {
            Random = new Random();
        }

        protected RepeatedlyAnimatedObjectPanel()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += GenerateItemTimer_Tick;
            Loaded += FallingObjectPanel_Loaded;
            Unloaded += FallingObjectPanel_Unloaded;
        }

        void FallingObjectPanel_Loaded(object sender, RoutedEventArgs e)
        {
            HandleOnFrequenceChanged();
        }

        void FallingObjectPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        protected abstract void GenerateItemTimer_Tick(object sender, EventArgs e);

    }

}
