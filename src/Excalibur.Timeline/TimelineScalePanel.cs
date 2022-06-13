using System;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// Track，Group的容器
    /// </summary>
    public class TimelineScalePanel : Panel
    {
        /// <summary>
        /// 排列方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// Orientation属性
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(TimelineScalePanel),
                        new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 竖直方向偏移
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }
        /// <summary>
        /// VerticalOffset属性
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(TimelineScalePanel), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 构造函数
        /// </summary>
        public TimelineScalePanel() : base()
        {
        }

        /// <summary>
        /// 当前的总宽度
        /// </summary>
        public double ExtentWidth { get; private set; } = 0d;
        /// <summary>
        /// 当前的总高度
        /// </summary>
        public double ExtentHeight { get; private set; } = 0d;
        /// <summary>
        /// 视口宽度
        /// </summary>
        public double ViewportWidth { get; private set; } = 0d;
        /// <summary>
        /// 视口高度
        /// </summary>
        public double ViewportHeight { get; private set; } = 0d;

        /// <summary>
        /// Override MeasureOverride
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var stackDesiredSize = new Size();
            var children = InternalChildren;
            var layoutSlotSize = constraint;

            if (Orientation == Orientation.Horizontal)
            {
                layoutSlotSize.Width = double.PositiveInfinity;

                for (int i = 0, count = children.Count; i < count; ++i)
                {
                    var child = children[i];
                    if (child == null) continue;

                    child.Measure(layoutSlotSize);
                    var childDesiredSize = child.DesiredSize;

                    stackDesiredSize.Width += childDesiredSize.Width;
                    stackDesiredSize.Height = Math.Max(stackDesiredSize.Height, childDesiredSize.Height);
                }
            }
            else
            {
                layoutSlotSize.Height = double.PositiveInfinity;

                for (int i = 0, count = children.Count; i < count; ++i)
                {
                    var child = children[i];
                    if (child == null) continue;

                    child.Measure(layoutSlotSize);
                    var childDesiredSize = child.DesiredSize;

                    stackDesiredSize.Width = Math.Max(stackDesiredSize.Width, childDesiredSize.Width);
                    stackDesiredSize.Height += childDesiredSize.Height;
                }
            }

            ExtentWidth = stackDesiredSize.Width;
            ExtentHeight = stackDesiredSize.Height;
            ViewportWidth = constraint.Width;
            ViewportHeight = constraint.Height;
            return stackDesiredSize;
        }

        /// <summary>
        /// Override ArrangeOverride
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var children = InternalChildren;
            var rcChild = new Rect(arrangeSize);
            var previousLocation = VerticalOffset;

            if (Orientation == Orientation.Horizontal)
            {
                for (int i = 0, count = children.Count; i < count; ++i)
                {
                    var child = children[i];
                    if (child == null) continue;

                    rcChild.X += previousLocation;
                    previousLocation = child.DesiredSize.Width;
                    rcChild.Width = previousLocation;
                    rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);

                    child.Arrange(rcChild);
                }
            }
            else
            {
                for (int i = 0, count = children.Count; i < count; ++i)
                {
                    var child = children[i];
                    if (child == null) continue;

                    rcChild.Y += previousLocation;
                    previousLocation = child.DesiredSize.Height;
                    rcChild.Height = previousLocation;
                    rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

                    child.Arrange(rcChild);
                }
            }

            return arrangeSize;
        }
    }
}
