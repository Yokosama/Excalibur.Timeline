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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsSelected = true;
            e.Handled = true;
        }
    }
}
