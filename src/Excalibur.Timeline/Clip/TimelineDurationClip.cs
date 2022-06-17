using Excalibur.Timeline.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 持续时间的Clip
    /// </summary>
    [TemplatePart(Name = ElementLeftResizeThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = ElementRightResizeThumb, Type = typeof(Thumb))]
    public class TimelineDurationClip : TimelineClip
    {
        private const string ElementLeftResizeThumb = "PART_LeftResizeThumb";
        private const string ElementRightResizeThumb = "PART_RightResizeThumb";

        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 持续时间属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(double), typeof(TimelineDurationClip), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure, OnDurationChanged));

        private double CurrentTime => container == null ? 0 : container.CurrentTime;
        private double EndTime => CurrentTime + Duration;

        static TimelineDurationClip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineDurationClip), new FrameworkPropertyMetadata(typeof(TimelineDurationClip)));
            FocusableProperty.OverrideMetadata(typeof(TimelineDurationClip), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var leftResizeThumb = Template.FindName(ElementLeftResizeThumb, this) as Thumb;
            leftResizeThumb.DragStarted += LeftResizeThumbDragStarted;
            leftResizeThumb.DragDelta += LeftResizeThumbDragDelta;
            leftResizeThumb.DragCompleted += LeftResizeThumbDragCompleted;

            var rightResizeThumb = Template.FindName(ElementRightResizeThumb, this) as Thumb;
            rightResizeThumb.DragStarted += RightResizeThumbDragStarted;
            rightResizeThumb.DragDelta += RightResizeThumbDragDelta;
            rightResizeThumb.DragCompleted += RightResizeThumbDragCompleted;

            if (container != null && container.Scale != null)
            {
                container.Scale.TimeScaleChanged += TimeScaleChanged;
            }
            UpdateDuration();
        }

        private void LeftResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void LeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
        }

        private void LeftResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
        }


        private void RightResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void RightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
        }

        private void RightResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
        }

        private void TimeScaleChanged(object sender, RoutedEventArgs e)
        {
            UpdateDuration();
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimelineDurationClip)?.UpdateDuration();
        }

        private void UpdateDuration()
        {
            if (container == null || container.Scale == null) return;

            container.Duration = Duration;

            var width = container.Scale.TimeToPos(EndTime) - container.Scale.TimeToPos(CurrentTime);
            Width = Math.Max(MinWidth, width);
        }

        /// <summary>
        /// Override DrawDraggingPrompt
        /// </summary>
        protected override void DrawDraggingPrompt(TimelinePointers pointers, DrawingContext dc, Rect area)
        {
            //base.DrawDraggingPrompt(pointers, dc, area);

            //if (container == null) return;
        
            //if (container.IsDragging)
            //{
            //    var curPos = container.PreviewPosition;
            //    dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(curPos, area.X), new Point(curPos, area.Y));
            //    var endPos = curPos + ActualWidth;
            //    dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(endPos, area.X), new Point(endPos, area.Y));

            //    pointers.DrawTimeText(dc, container.Scale.TimeToText(container.PreviewCurrentTime), curPos, true);
            //    pointers.DrawTimeText(dc, container.Scale.TimeToText(container.PreviewCurrentTime + Duration), endPos, true);
            //}
        }
    }
}
