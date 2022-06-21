using Excalibur.Timeline.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
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
            DependencyProperty.Register(nameof(Locked), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLockedChanged));

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

        /// <summary>
        /// Group的状态显示内容
        /// </summary>
        public object StatusContent
        {
            get { return GetValue(StatusContentProperty); }
            set { SetValue(StatusContentProperty, value); }
        }
        /// <summary>
        /// StatusContent属性
        /// </summary>
        public static readonly DependencyProperty StatusContentProperty =
            DependencyProperty.Register(nameof(StatusContent), typeof(object), typeof(TimelineGroup), new FrameworkPropertyMetadata(default(object)));

        /// <summary>
        /// 是否显示Track的状态显示内容
        /// </summary>
        public bool ShowStatusContent
        {
            get { return (bool)GetValue(ShowStatusContentProperty); }
            set { SetValue(ShowStatusContentProperty, value); }
        }
        /// <summary>
        /// StatusContent属性
        /// </summary>
        public static readonly DependencyProperty ShowStatusContentProperty =
            DependencyProperty.Register(nameof(ShowStatusContent), typeof(bool), typeof(TimelineGroup), new FrameworkPropertyMetadata(BoxValue.True));

        private TimelineScale _scale;
        private Dictionary<FrameworkElement, object> _curPrepareItem = new Dictionary<FrameworkElement, object>();
        static TimelineGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineGroup), new FrameworkPropertyMetadata(typeof(TimelineGroup)));
            FocusableProperty.OverrideMetadata(typeof(TimelineGroup), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scale = this.TryFindParent<TimelineScale>();
        }

        /// <summary>
        /// Override PrepareContainerForItemOverride
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (item == null || _scale == null) return;

            if (element is FrameworkElement fe)
            {
                fe.Loaded += ContainerLoaded;
                _curPrepareItem[fe] = item;
            }
        }

        private void ContainerLoaded(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is FrameworkElement element) || _curPrepareItem == null) return;
            element.Loaded -= ContainerLoaded;

            if (_curPrepareItem.ContainsKey(element))
            {
                var group = element.TryFindChild<TimelineGroup>();
                if (group != null)
                {
                    _scale.AddGroupOrTrackItems(_curPrepareItem[element], group);
                    _curPrepareItem.Remove(element);
                }
                else
                {
                    var track = element.TryFindChild<TimelineTrack>();
                    if (track != null)
                    {
                        _scale.AddGroupOrTrackItems(_curPrepareItem[element], track);
                        _curPrepareItem.Remove(element);
                    }
                }
            }
        }

        /// <summary>
        /// Override ClearContainerForItemOverride
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            if (item == null || _scale == null) return;

            _scale.RemoveGroupOrTrackItems(item);
        }

        private static void OnLockedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimelineGroup)?.OnLockedChanged();
        }

        private void OnLockedChanged()
        {
            foreach (var item in Items)
            {
                var container = ItemContainerGenerator.ContainerFromItem(item);
                var track = container.TryFindChild<TimelineTrack>();
                if(track != null)
                {
                    track.PreviewLocked = !Locked;
                }
            }
        }
    }
}
