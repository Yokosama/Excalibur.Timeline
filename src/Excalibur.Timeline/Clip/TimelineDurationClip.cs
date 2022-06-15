using Excalibur.Timeline.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 持续时间的Clip
    /// </summary>
    public class TimelineDurationClip : Control
    {
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

        private double CurrentTime => _container == null ? 0 : _container.CurrentTime;
        private double EndTime => CurrentTime + Duration;

        private TimelineTrackItemContainer _container;

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

            _container = this.TryFindParent<TimelineTrackItemContainer>();
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimelineDurationClip)?.UpdateDuration();
        }

        private void UpdateDuration()
        {
            if (_container == null || _container.Scale == null) return;

            var width = _container.Scale.TimeToPos(EndTime) - _container.Scale.TimeToPos(CurrentTime);
            Width = Math.Max(MinWidth, width);
        }
    }
}
