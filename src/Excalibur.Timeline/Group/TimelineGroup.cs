using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excalibur.Timeline
{
    /// <summary>
    /// Timeline轨道组
    /// </summary>
    public class TimelineGroup : ItemsControl
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        /// <summary>
        /// 是否选中属性
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.False));

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disabled
        {
            get { return (bool)GetValue(DisabledProperty); }
            set { SetValue(DisabledProperty, value); }
        }
        /// <summary>
        /// 是否禁用属性
        /// </summary>
        public static readonly DependencyProperty DisabledProperty =
            DependencyProperty.Register(nameof(Disabled), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.False));

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool Locked
        {
            get { return (bool)GetValue(LockedProperty); }
            set { SetValue(LockedProperty, value); }
        }
        /// <summary>
        /// 是否锁定属性
        /// </summary>
        public static readonly DependencyProperty LockedProperty =
            DependencyProperty.Register(nameof(Locked), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.False));

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        /// <summary>
        /// 是否展开属性
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.True));

        /// <summary>
        /// 子物体模板
        /// </summary>
        public DataTemplate TrackTemplate
        {
            get { return (DataTemplate)GetValue(TrackTemplateProperty); }
            set { SetValue(TrackTemplateProperty, value); }
        }
        /// <summary>
        /// TrackTemplate属性
        /// </summary>
        public static readonly DependencyProperty TrackTemplateProperty =
            DependencyProperty.Register(nameof(TrackTemplate), typeof(DataTemplate), typeof(TimelineGroup));

        /// <summary>
        /// IsExpanded=false，折叠后的子物体模板
        /// </summary>
        public DataTemplate FoldTrackTemplate
        {
            get { return (DataTemplate)GetValue(FoldTrackTemplateProperty); }
            set { SetValue(FoldTrackTemplateProperty, value); }
        }
        /// <summary>
        /// FoldTrackTemplate属性
        /// </summary>
        public static readonly DependencyProperty FoldTrackTemplateProperty =
            DependencyProperty.Register(nameof(FoldTrackTemplate), typeof(DataTemplate), typeof(TimelineGroup));

        static TimelineGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineGroup), new FrameworkPropertyMetadata(typeof(TimelineGroup)));
            FocusableProperty.OverrideMetadata(typeof(TimelineGroup), new FrameworkPropertyMetadata(true));
        }
    }
}
