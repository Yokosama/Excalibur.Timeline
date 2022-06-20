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
    /// Timeline轨道组
    /// </summary>
    [TemplatePart(Name = ElementExpander, Type = typeof(Expander))]
    public class TimelineGroupHeader : ItemsControl
    {
        private const string ElementExpander = "PART_Expander";

        /// <summary>
        /// Header
        /// </summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        /// <summary>
        /// Header属性
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(TimelineGroupHeader), new FrameworkPropertyMetadata(default(object)));

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
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineGroupHeader), new FrameworkPropertyMetadata(BoxValue.False, OnIsSelectedChanged));

        /// <summary>
        /// 是否展开组
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        /// <summary>
        /// 是否展开组
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(TimelineGroupHeader), new FrameworkPropertyMetadata(BoxValue.True, OnIsExpandedChanged));

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
        public static readonly RoutedEvent SelectedChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedChanged), RoutingStrategy.Bubble, typeof(SelectedChangedEventHandler), typeof(TimelineGroupHeader));

        /// <summary>
        /// 展开修改事件
        /// </summary>
        public event IsExpandedChangedEventHandler IsExpandedChanged
        {
            add => AddHandler(IsExpandedChangedEvent, value);
            remove => RemoveHandler(IsExpandedChangedEvent, value);
        }
        /// <summary>
        /// 展开修改事件
        /// </summary>
        public static readonly RoutedEvent IsExpandedChangedEvent = EventManager.RegisterRoutedEvent(nameof(IsExpandedChanged), RoutingStrategy.Bubble, typeof(IsExpandedChangedEventHandler), typeof(TimelineGroupHeader));

        /// <summary>
        /// TimelineGroupHeader or TimelineHeader
        /// </summary>
        public ItemsControl ParentHeader { get; private set; }
        /// <summary>
        /// 所处的容器
        /// </summary>
        public ContentPresenter ContentPresenter { get; private set; }

        private TimelineHeader _header;

        static TimelineGroupHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineGroupHeader), new FrameworkPropertyMetadata(typeof(TimelineGroupHeader)));
            FocusableProperty.OverrideMetadata(typeof(TimelineGroupHeader), new FrameworkPropertyMetadata(BoxValue.True));
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
            (d as TimelineGroupHeader)?.OnSelectedChanged();
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimelineGroupHeader)?.OnIsExpandedChanged();
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
        }

        private void OnSelectedChanged()
        {
            RaiseEvent(new SelectedChangedEventArgs(IsSelected)
            {
                RoutedEvent = SelectedChangedEvent,
                Source = this
            });
        }

        private void OnIsExpandedChanged()
        {
            RaiseEvent(new IsExpandedChangedEventArgs(IsSelected)
            {
                RoutedEvent = IsExpandedChangedEvent,
                Source = this
            });
        }
    }

    /// <summary>
    /// 选中变化事件委托
    /// </summary>
    public delegate void SelectedChangedEventHandler(object sender, SelectedChangedEventArgs e);

    /// <summary>
    /// 选中变化事件
    /// </summary>
    public class SelectedChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 当前值
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        public SelectedChangedEventArgs(bool value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// 展开修改事件委托
    /// </summary>
    public delegate void IsExpandedChangedEventHandler(object sender, SelectedChangedEventArgs e);

    /// <summary>
    /// 展开修改事件
    /// </summary>
    public class IsExpandedChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 当前值
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        public IsExpandedChangedEventArgs(bool value)
        {
            Value = value;
        }
    }
}
