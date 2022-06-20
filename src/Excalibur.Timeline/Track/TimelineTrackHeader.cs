using Excalibur.Timeline.Helper;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 轨道标题
    /// </summary>
    public class TimelineTrackHeader : ContentControl
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
        /// 是否选中
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineTrackHeader), new FrameworkPropertyMetadata(BoxValue.False, OnIsSelectedChanged));

        /// <summary>
        /// 选中变化事件
        /// </summary>
        public event SelectedChangedEventHandler SelectedChanged
        {
            add => AddHandler(SelectedChangedEvent, value);
            remove => RemoveHandler(SelectedChangedEvent, value);
        }
        /// <summary>
        /// 选中变化事件
        /// </summary>
        public static readonly RoutedEvent SelectedChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedChanged), RoutingStrategy.Bubble, typeof(SelectedChangedEventHandler), typeof(TimelineTrackHeader));

        /// <summary>
        /// TimelineGroupHeader or TimelineHeader
        /// </summary>
        public ItemsControl ParentHeader { get; private set; }
        /// <summary>
        /// 所处的容器
        /// </summary>
        public ContentPresenter ContentPresenter { get; private set; }
        private TimelineHeader _header;

        static TimelineTrackHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTrackHeader), new FrameworkPropertyMetadata(typeof(TimelineTrackHeader)));
            FocusableProperty.OverrideMetadata(typeof(TimelineTrackHeader), new FrameworkPropertyMetadata(BoxValue.True));
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ParentHeader = this.TryFindParent<ItemsControl>(); 
            ContentPresenter = this.TryFindParent<ContentPresenter>();
            _header = this.TryFindParent<TimelineHeader>();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimelineTrackHeader)?.OnSelectedChanged();
        }

        private void OnSelectedChanged()
        {
            RaiseEvent(new SelectedChangedEventArgs(IsSelected)
            {
                RoutedEvent = SelectedChangedEvent,
                Source = this
            });
        }

        /// <summary>
        /// Override OnMouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                    IsSelected = !IsSelected;
                    break;
                case ModifierKeys.Shift:
                    IsSelected = true;
                    break;
                default:
                    if (!IsSelected)
                    {
                        _header?.UnselectAllHeaderItems();
                        IsSelected = true;
                    }
                    break;
            }
            e.Handled = true;
        }
    }
}
