using Excalibur.Timeline.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 轨道
    /// </summary>
    public class TimelineTrack : ItemsControl
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
        /// IsSelected属性
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineTrack), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disabled
        {
            get { return (bool)GetValue(DisabledProperty); }
            set { SetValue(DisabledProperty, value); }
        }
        /// <summary>
        /// Disabled属性
        /// </summary>
        public static readonly DependencyProperty DisabledProperty =
            DependencyProperty.Register(nameof(Disabled), typeof(bool), typeof(TimelineTrack), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool Locked
        {
            get { return (bool)GetValue(LockedProperty); }
            set { SetValue(LockedProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LockedProperty =
            DependencyProperty.Register("Locked", typeof(bool), typeof(TimelineTrack), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static TimelineTrack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTrack), new FrameworkPropertyMetadata(typeof(TimelineTrack)));
            FocusableProperty.OverrideMetadata(typeof(TimelineTrack), new FrameworkPropertyMetadata(true));
        }

        private TimelineScale _scale;

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scale = this.TryFindParent<TimelineScale>();
        }

        /// <summary>
        /// Override GetContainerForItemOverride
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            base.GetContainerForItemOverride();
            var itemContainer = new TimelineTrackItemContainer
            {
                RenderTransform = new TranslateTransform()
            };
            if(_scale != null) _scale.TrackItems.Add(itemContainer);
            return itemContainer;
        }

        /// <summary>
        /// Override ClearContainerForItemOverride
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            if (_scale == null) return;
            if(element is TimelineTrackItemContainer itemContainer)
            {
                if (itemContainer.IsSelected)
                {
                    _scale.RemoveSelectedTrackItem(itemContainer, item);
                }
                _scale.TrackItems.Remove(itemContainer);
            }
        }
    }
}
