using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace EasterEgg.Controls
{

    public enum RunningMotionState
    {
        StandBy,
        LeftLegAhead,
        RightLegAhead
    }

    public enum HumanPartDirection
    {
        Left,
        Right
    }

    public interface IHumanPart
    {

        void RenderHumanPartOfSize(Size size);

    }

    public interface IRunningMotionHummanPart : IHumanPart
    {

        HumanPartDirection Direction { get; set; }

        void RenderMotionOfSize(RunningMotionState motionState, Size size);

    }

    [TemplatePart(Name = HeadName, Type = typeof(IHumanPart))]
    [TemplatePart(Name = BodyName, Type = typeof(IHumanPart))]
    [TemplatePart(Name = LeftArmName, Type = typeof(IRunningMotionHummanPart))]
    [TemplatePart(Name = RightArmName, Type = typeof(IRunningMotionHummanPart))]
    [TemplatePart(Name = LeftLegName, Type = typeof(IRunningMotionHummanPart))]
    [TemplatePart(Name = RightLegName, Type = typeof(IRunningMotionHummanPart))]
    public partial class Human : Control
    {

        private const string HeadName = "Head";
        private const string BodyName = "Body";
        private const string LeftArmName = "LeftArm";
        private const string RightArmName = "RightArm";
        private const string LeftLegName = "LeftLeg";
        private const string RightLegName = "RightLeg";

        protected IHumanPart Head { get; private set; }
        protected IHumanPart Body { get; private set; }
        protected IRunningMotionHummanPart LeftArm { get; private set; }
        protected IRunningMotionHummanPart RightArm { get; private set; }
        protected IRunningMotionHummanPart LeftLeg { get; private set; }
        protected IRunningMotionHummanPart RightLeg { get; private set; }

        public Human()
        {
            DefaultStyleKey = typeof (Human);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Head = GetTemplateChild(HeadName) as IHumanPart;
            Body = GetTemplateChild(BodyName) as IHumanPart;
            LeftArm = GetTemplateChild(LeftArmName) as IRunningMotionHummanPart;
            RightArm = GetTemplateChild(RightArmName) as IRunningMotionHummanPart;
            LeftLeg = GetTemplateChild(LeftLegName) as IRunningMotionHummanPart;
            RightLeg = GetTemplateChild(RightLegName) as IRunningMotionHummanPart;
            HandleOnHumanSizeChanged();
            HandleOnRunningMotionStateChanged();
        }

    }

    partial class Human
    {

        IEnumerable<IHumanPart> PartsToRenderOnSizeChanged()
        {
            if (Head != null)
            {
                yield return Head;
            }
            if (Body != null)
            {
                yield return Body;
            }
            if (LeftArm != null)
            {
                yield return LeftArm;
            }
            if (RightArm != null)
            {
                yield return RightArm;
            }
            if (LeftLeg != null)
            {
                yield return LeftLeg;
            }
            if (RightLeg != null)
            {
                yield return RightLeg;
            }
        }

        IEnumerable<IRunningMotionHummanPart> PartsToRenderOnMotionStateChanged()
        {
            if (LeftArm != null)
            {
                yield return LeftArm;
            }
            if (RightArm != null)
            {
                yield return RightArm;
            }
            if (LeftLeg != null)
            {
                yield return LeftLeg;
            }
            if (RightLeg != null)
            {
                yield return RightLeg;
            }
        }

    }

    partial class Human
    {

        public static readonly DependencyProperty HumanSizeProperty = DependencyProperty.Register(
            "HumanSize", typeof(Size), typeof(Human), new PropertyMetadata(default(Size), OnHumanSizeChanged));

        public Size HumanSize
        {
            get { return (Size) GetValue(HumanSizeProperty); }
            set { SetValue(HumanSizeProperty, value); }
        }

        static void OnHumanSizeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((Human)obj).HandleOnHumanSizeChanged();
        }

        void HandleOnHumanSizeChanged()
        {
            foreach (var parts in PartsToRenderOnSizeChanged())
            {
                parts.RenderHumanPartOfSize(HumanSize);
            }
        }

        public static readonly DependencyProperty RunningMotionStateProperty = DependencyProperty.Register(
            "RunningMotionState", typeof(RunningMotionState), typeof(Human), new PropertyMetadata(default(RunningMotionState), OnRunningMotionStateChanged));

        public RunningMotionState RunningMotionState
        {
            get { return (RunningMotionState) GetValue(RunningMotionStateProperty); }
            set { SetValue(RunningMotionStateProperty, value); }
        }

        static void OnRunningMotionStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((Human)obj).HandleOnRunningMotionStateChanged();
        }

        void HandleOnRunningMotionStateChanged()
        {
            foreach (var parts in PartsToRenderOnMotionStateChanged())
            {
                parts.RenderMotionOfSize(RunningMotionState, HumanSize);
            }
        }

    }

    public abstract class HumanPartBase : Canvas
    {

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground", typeof (Brush), typeof (HumanPartBase), new PropertyMetadata(default(Brush)));

        public Brush Foreground
        {
            get { return (Brush) GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

    }

    public class HumanHead : HumanPartBase, IHumanPart
    {
        public void RenderHumanPartOfSize(Size size)
        {
            Children.Clear();
            Children.Add(new Ellipse
                         {
                             Width = size.Width,
                             Height = size.Width,
                             Fill = Foreground
                         });
        }
    }

    public class HumanBody : HumanPartBase, IHumanPart
    {
        public void RenderHumanPartOfSize(Size size)
        {
            Children.Clear();
            var w = size.Width;
            var h = (size.Height - size.Width)/2;
            var entity = new Polyline
                         {
                             Stroke = Foreground,
                             StrokeThickness = w,
                             StrokeStartLineCap = PenLineCap.Round,
                             StrokeEndLineCap = PenLineCap.Round,
                             StrokeLineJoin = PenLineJoin.Round,
                             StrokeMiterLimit = 4,
                             Points = new PointCollection
                                      {
                                          new Point(0, 0),
                                          new Point(0, h - w)
                                      }
                         };
            Children.Add(entity);
            SetLeft(this, w/2);
            SetTop(this, size.Width + w / 2);
        }
    }

    public class HumanArm : HumanPartBase, IRunningMotionHummanPart
    {

        public void RenderHumanPartOfSize(Size size)
        {
            RenderMotionOfSize(RunningMotionState.StandBy, size);
        }

        public void RenderMotionOfSize(RunningMotionState motionState, Size size)
        {
            Children.Clear();
            var w = size.Width / 2;
            var h = (size.Height - size.Width) / 2;
            var entity = new Polyline
            {
                Stroke = Foreground,
                StrokeThickness = w,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeMiterLimit = 4,
                Points = new PointCollection
                                      {
                                          new Point(0, 0),
                                          new Point(0, h - w),
                                          new Point(3, h - w),
                                          new Point(-1, h - w)
                                      }
            };
            Children.Add(entity);
            SetLeft(this, w);
            SetTop(this, size.Width + w / 2);
            bool? isAhead = null;
            switch (motionState)
            {
                case RunningMotionState.StandBy:
                    break;
                case RunningMotionState.LeftLegAhead:
                    isAhead = Direction == HumanPartDirection.Right;
                    break;
                case RunningMotionState.RightLegAhead:
                    isAhead = Direction == HumanPartDirection.Left;
                    break;
            }
            RenderTransform = isAhead.HasValue ? new RotateTransform { CenterX = 0.5, CenterY = 0, Angle = isAhead.Value ? -40 : 40 } : null;
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction", typeof(HumanPartDirection), typeof(IRunningMotionHummanPart), new PropertyMetadata(default(HumanPartDirection)));

        public HumanPartDirection Direction
        {
            get { return (HumanPartDirection) GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

    }

    public class HumanLeg : HumanPartBase, IRunningMotionHummanPart
    {

        public void RenderHumanPartOfSize(Size size)
        {
            RenderMotionOfSize(RunningMotionState.StandBy, size);
        }

        public void RenderMotionOfSize(RunningMotionState motionState, Size size)
        {
            Children.Clear();
            var w = size.Width/2;
            var h = (size.Height - size.Width) / 2;
            var entity = new Polyline
                         {
                             Stroke = Foreground,
                             StrokeThickness = w,
                             StrokeStartLineCap = PenLineCap.Round,
                             StrokeEndLineCap = PenLineCap.Round,
                             StrokeLineJoin = PenLineJoin.Round,
                             StrokeMiterLimit = 4,
                             Points = new PointCollection
                                      {
                                          new Point(0, 0),
                                          new Point(0, h - w),
                                          new Point(10, h - w)
                                      }
                         };
            Children.Add(entity);
            SetLeft(this, w);
            SetTop(this, size.Height - h);
            bool ? isAhead = null;
            switch (motionState)
            {
                case RunningMotionState.StandBy:
                    break;
                case RunningMotionState.LeftLegAhead:
                    isAhead = Direction == HumanPartDirection.Left;
                    break;
                case RunningMotionState.RightLegAhead:
                    isAhead = Direction == HumanPartDirection.Right;
                    break;
            }
            RenderTransform = isAhead.HasValue ? new RotateTransform {CenterX = 0.5, CenterY = 0, Angle = isAhead.Value ? -30 : 40} : null;
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction", typeof(HumanPartDirection), typeof(IRunningMotionHummanPart), new PropertyMetadata(default(HumanPartDirection)));

        public HumanPartDirection Direction
        {
            get { return (HumanPartDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

    }

}
