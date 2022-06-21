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

        /// <summary>
        /// 持续时间调整开始事件
        /// </summary>
        public event DurationResizeStartedEventHandler DurationResizeStarted
        {
            add => AddHandler(DurationResizeStartedEvent, value);
            remove => RemoveHandler(DurationResizeStartedEvent, value);
        }
        /// <summary>
        /// 持续时间调整开始事件
        /// </summary>
        public static readonly RoutedEvent DurationResizeStartedEvent =
            EventManager.RegisterRoutedEvent(nameof(DurationResizeStarted), RoutingStrategy.Bubble, typeof(DurationResizeStartedEventHandler), typeof(TimelineDurationClip));

        /// <summary>
        /// 持续时间调整中事件
        /// </summary>
        public event DurationResizeDeltaEventHandler DurationResizeDelta
        {
            add => AddHandler(DurationResizeDeltaEvent, value);
            remove => RemoveHandler(DurationResizeDeltaEvent, value);
        }
        /// <summary>
        /// 持续时间调整中事件
        /// </summary>
        public static readonly RoutedEvent DurationResizeDeltaEvent =
            EventManager.RegisterRoutedEvent(nameof(DurationResizeDelta), RoutingStrategy.Bubble, typeof(DurationResizeDeltaEventHandler), typeof(TimelineDurationClip));

        /// <summary>
        /// 持续时间调整结束事件
        /// </summary>
        public event DurationResizeCompletedEventHandler DurationResizeCompleted
        {
            add => AddHandler(DurationResizeCompletedEvent, value);
            remove => RemoveHandler(DurationResizeCompletedEvent, value);
        }
        /// <summary>
        /// 持续时间调整结束事件
        /// </summary>
        public static readonly RoutedEvent DurationResizeCompletedEvent =
            EventManager.RegisterRoutedEvent(nameof(DurationResizeCompleted), RoutingStrategy.Bubble, typeof(DurationResizeCompletedEventHandler), typeof(TimelineDurationClip));

        private double CurrentTime => container == null ? 0 : container.CurrentTime;
        private double EndTime
        {
            get
            {
                if (container == null || container.Scale == null)
                    return CurrentTime + Duration;
                else return container.Scale.SnapTime(CurrentTime + Duration);
            }
        }

        private double _preEndTime;

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

            if (container == null) return;

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
            container.SelectedSelf();
            _preEndTime = EndTime;
            container.Scale.Pointers.StartDraggingPrompt(CurrentTime, CurrentTime);
            RaiseEvent(new DurationResizeStartedEventArgs(CurrentTime, Duration, DurationResizeMode.StartTime)
            {
                RoutedEvent = DurationResizeStartedEvent,
                Source = this
            });
            e.Handled = true;
        }

        private void LeftResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if(e.HorizontalChange != 0)
            {
                var pos = container.Position + e.HorizontalChange;
                var time = container.Scale.SnapTime(container.Scale.PosToTime(pos));

                var deltaTime = time - CurrentTime;
                var duration = _preEndTime - time;
                if (duration < 0)
                {
                    duration = 0;
                    Duration = duration;
                    container.CurrentTime = _preEndTime;
                }
                else
                {
                    Duration = duration;
                    container.CurrentTime = time;
                }
                container.Scale.Pointers.ShowDraggingPrompt(CurrentTime, CurrentTime);
                RaiseEvent(new DurationResizeDeltaEventArgs(deltaTime, DurationResizeMode.StartTime)
                {
                    RoutedEvent = DurationResizeDeltaEvent,
                    Source = this
                });
            }
          
            e.Handled = true;
        }

        private void LeftResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            container.Scale.Pointers.EndDrawDraggingPrompt();
            RaiseEvent(new DurationResizeCompletedEventArgs(CurrentTime, Duration, DurationResizeMode.EndTime)
            {
                RoutedEvent = DurationResizeCompletedEvent,
                Source = this
            });
            e.Handled = true;
        }

        private void RightResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            container.SelectedSelf();
            _preEndTime = EndTime;
            container.Scale.Pointers.StartDraggingPrompt(EndTime, EndTime);
            RaiseEvent(new DurationResizeStartedEventArgs(CurrentTime, Duration, DurationResizeMode.EndTime)
            {
                RoutedEvent = DurationResizeStartedEvent,
                Source = this
            });
            e.Handled = true;
        }

        private void RightResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                var endPos = container.Scale.TimeToPos(EndTime);
                var pos = endPos + e.HorizontalChange;
                var time = container.Scale.SnapTime(container.Scale.PosToTime(pos));

                var deltaTime = time - EndTime;
                var duration = time - CurrentTime;
                if (duration < 0)
                {
                    duration = 0;
                    Duration = duration;
                }
                else
                {
                    Duration = duration;
                }
                container.Scale.Pointers.ShowDraggingPrompt(EndTime, EndTime);
                RaiseEvent(new DurationResizeDeltaEventArgs(deltaTime, DurationResizeMode.EndTime)
                {
                    RoutedEvent = DurationResizeDeltaEvent,
                    Source = this
                });
            }
            e.Handled = true;
        }

        private void RightResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            container.Scale.Pointers.EndDrawDraggingPrompt();
            RaiseEvent(new DurationResizeCompletedEventArgs(CurrentTime, Duration, DurationResizeMode.EndTime)
            {
                RoutedEvent = DurationResizeCompletedEvent,
                Source = this
            });
            e.Handled = true;
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
    }

    /// <summary>
    /// 持续时间调整开始事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DurationResizeStartedEventHandler(object sender, DurationResizeStartedEventArgs e);

    /// <summary>
    /// 持续时间调整开始事件参数
    /// </summary>
    public class DurationResizeStartedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public double StartTime { get; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration { get; }
        /// <summary>
        /// 调整模式
        /// </summary>
        public DurationResizeMode Mode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="mode"></param>
        public DurationResizeStartedEventArgs(double startTime, double duration, DurationResizeMode mode)
        {
            StartTime = startTime;
            Duration = duration;
            Mode = mode;
        }
    }

    /// <summary>
    /// 持续时间调整中事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DurationResizeDeltaEventHandler(object sender, DurationResizeDeltaEventArgs e);

    /// <summary>
    /// 持续时间调整中事件参数
    /// </summary>
    public class DurationResizeDeltaEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 每次调整的事件
        /// </summary>
        public double DeltaTime { get; }

        /// <summary>
        /// 调整模式
        /// </summary>
        public DurationResizeMode Mode { get; }

        /// <summary>
        /// 构造参数
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="mode"></param>
        public DurationResizeDeltaEventArgs(double deltaTime, DurationResizeMode mode)
        {
            DeltaTime = deltaTime;
            Mode = mode;
        }
    }

    /// <summary>
    /// 持续时间调整结束事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DurationResizeCompletedEventHandler(object sender, DurationResizeCompletedEventArgs e);

    /// <summary>
    /// 持续时间调整结束参数
    /// </summary>
    public class DurationResizeCompletedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public double StartTime { get; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration { get; }
        /// <summary>
        /// 调整模式
        /// </summary>
        public DurationResizeMode Mode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="mode"></param>
        public DurationResizeCompletedEventArgs(double startTime, double duration, DurationResizeMode mode)
        {
            StartTime = startTime;
            Duration = duration;
            Mode = mode;
        }
    }

    /// <summary>
    /// 持续时间调整大小模式
    /// </summary>
    public enum DurationResizeMode
    {
        /// <summary>
        /// 调整开始时间，结束时间不变
        /// </summary>
        StartTime,
        /// <summary>
        /// 调整结束时间，开始时间不变
        /// </summary>
        EndTime
    }
}
