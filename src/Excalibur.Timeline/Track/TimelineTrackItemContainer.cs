using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Excalibur.Timeline.Helper;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 轨道的子物体容器，包含Clip
    /// </summary>
    public class TimelineTrackItemContainer : ContentControl
    {
        /// <summary>
        /// 当前所在时间
        /// </summary>
        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        /// <summary>
        /// CurrentTime属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(double), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure, OnCurrentTimeChanged));

        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// Duration属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(double), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.Double0));

        /// <summary>
        /// 当前所在时间
        /// </summary>
        public double PreviewCurrentTime
        {
            get { return (double)GetValue(PreviewCurrentTimeProperty); }
            set { SetValue(PreviewCurrentTimeProperty, value); }
        }
        /// <summary>
        /// CurrentTime属性
        /// </summary>
        public static readonly DependencyProperty PreviewCurrentTimeProperty =
            DependencyProperty.Register(nameof(PreviewCurrentTime), typeof(double), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentTimeChanged));

        /// <summary>
        /// 当前在界面上的位置
        /// </summary>
        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        /// <summary>
        /// Position属性
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(double), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsMeasure, OnPositionChanged));

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
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsSelectedChanged));

        /// <summary>
        /// 是否可被选中
        /// </summary>
        public bool IsSelectable
        {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }
        /// <summary>
        /// IsSelectable属性
        /// </summary>
        public static readonly DependencyProperty IsSelectableProperty =
            DependencyProperty.Register(nameof(IsSelectable), typeof(bool), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.True));

        /// <summary>
        /// 是否可拖拽
        /// </summary>
        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }
        /// <summary>
        /// IsDraggable属性
        /// </summary>
        public static readonly DependencyProperty IsDraggableProperty =
            DependencyProperty.Register(nameof(IsDraggable), typeof(bool), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.True, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 拖拽开始事件
        /// </summary>
        public event DragStartedEventHandler DragStarted
        {
            add => AddHandler(DragStartedEvent, value);
            remove => RemoveHandler(DragStartedEvent, value);
        }
        /// <summary>
        /// 拖拽开始事件
        /// </summary>
        public static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent(nameof(DragStarted), RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(TimelineTrackItemContainer));

        /// <summary>
        /// 拖拽中事件
        /// </summary>
        public event DragDeltaEventHandler DragDelta
        {
            add => AddHandler(DragDeltaEvent, value);
            remove => RemoveHandler(DragDeltaEvent, value);
        }
        /// <summary>
        /// 拖拽中事件
        /// </summary>
        public static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent(nameof(DragDelta), RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(TimelineTrackItemContainer));

        /// <summary>
        /// 拖拽结束事件
        /// </summary>
        public event DragCompletedEventHandler DragCompleted
        {
            add => AddHandler(DragCompletedEvent, value);
            remove => RemoveHandler(DragCompletedEvent, value);
        }
        /// <summary>
        /// 拖拽结束事件
        /// </summary>
        public static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent(nameof(DragCompleted), RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(TimelineTrackItemContainer));

        /// <summary>
        /// 选中事件
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }
        /// <summary>
        /// 选中事件
        /// </summary>
        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(nameof(Selected), RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TimelineTrackItemContainer));

        /// <summary>
        /// 取消选择事件
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add => AddHandler(UnselectedEvent, value);
            remove => RemoveHandler(UnselectedEvent, value);
        }
        /// <summary>
        /// 取消选择事件
        /// </summary>
        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent(nameof(Unselected), RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(TimelineTrackItemContainer));

        /// <summary>
        /// 刻度，Track、Group的区域
        /// </summary>
        public TimelineScale Scale { get; private set; }

        /// <summary>
        /// ItemContainer所在的容器
        /// </summary>
        public TimelineTrackCanvas Owner { get; private set; }
        /// <summary>
        /// ItemContainer所在的Track
        /// </summary>
        public TimelineTrack Track { get; private set; }

        /// <summary>
        /// 预览时间的所在位置
        /// </summary>
        public double PreviewPosition { get; private set; }

        #region Drag
        private Point _previousDragPosition;
        private Point _initialDragPosition;
        /// <summary>
        /// 是否正在拖拽
        /// </summary>
        public bool IsDragging { get; private set; }
        #endregion

        static TimelineTrackItemContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(typeof(TimelineTrackItemContainer)));
            FocusableProperty.OverrideMetadata(typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.True));
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Scale = this.TryFindParent<TimelineScale>();
            if(Scale != null)
                Scale.TimeScaleChanged += TimeScaleChanged;
            Owner = this.TryFindParent<TimelineTrackCanvas>();
            Track = this.TryFindParent<TimelineTrack>();
            UpdatePosition();
        }

        private void TimeScaleChanged(object sender, RoutedEventArgs e)
        {
            UpdatePosition();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimelineTrackItemContainer container)) return;

            bool result = container.IsSelectable && (bool)e.NewValue;
            container.OnSelectedChanged(result);
            container.IsSelected = result;
        }

        private void OnSelectedChanged(bool newValue)
        {
            if (!(Scale?.IsSelecting ?? false))
            {
                RaiseEvent(new RoutedEventArgs(newValue ? SelectedEvent : UnselectedEvent, this));
            }
        }

        /// <summary>
        /// Override OnMouseLeftButtonDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (Owner == null) return;

            _initialDragPosition = e.GetPosition(Owner);
            _previousDragPosition = _initialDragPosition;

            Focus();
            CaptureMouse();
            e.Handled = true;
        }

        /// <summary>
        /// Override OnMouseMove
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured && IsDraggable)
            {
                Point position = e.GetPosition(Owner);

                if (_previousDragPosition != position)
                {
                    // Start dragging
                    if (!IsDragging)
                    {
                        RaiseEvent(new DragStartedEventArgs(_initialDragPosition.X, _initialDragPosition.Y)
                        {
                            RoutedEvent = DragStartedEvent
                        });

                        IsDragging = true;
                    }
                    else
                    {
                        Vector delta = position - _previousDragPosition;
                        _previousDragPosition = position;
                        
                        RaiseEvent(new DragDeltaEventArgs(delta.X, delta.Y)
                        {
                            RoutedEvent = DragDeltaEvent
                        });

                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Override OnPreviewMouseUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if (Owner == null || Scale == null) return;

            if (IsDragging)
            {
                IsDragging = false;
                Vector position = e.GetPosition(Owner) - _initialDragPosition;

                RaiseEvent(new DragCompletedEventArgs(position.X, position.Y, false)
                {
                    RoutedEvent = DragCompletedEvent
                });

                e.Handled = true;
            }
            else
            {
                switch (Keyboard.Modifiers)
                {
                    case ModifierKeys.Control:
                        IsSelected = !IsSelected;
                        break;
                    case ModifierKeys.Shift:
                        IsSelected = true;
                        break;
                    default:
                        Scale?.UnselectAllTrackItems();
                        IsSelected = true;
                        break;
                }
            }

            Focus();
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
            e.Handled = true;
        }

        private static void OnCurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineTrackItemContainer container)
            {
                container.UpdatePosition();
            }
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineTrackItemContainer container)
            {
                container.OnPositionChanged();
            }
        }

        private void OnPositionChanged()
        {
            if (!Scale.IsBulkUpdatingItems)
            {
                Owner.InvalidateVisual();
            }
        }

        private void UpdatePosition()
        {
            if (Scale == null) return;

            Position = Scale.TimeToPos(CurrentTime);
            PreviewPosition = Scale.TimeToPos(PreviewCurrentTime);
        }
    }

    /// <summary>
    /// TimelineTrackItemContainer选中事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TrackItemSelectedEventHandler(object sender, TrackItemSelectedEventArgs e);

    /// <summary>
    /// TimelineTrackItemContainer选中事件参数
    /// </summary>
    public class TrackItemSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// TimelineTrackItemContainer
        /// </summary>
        public TimelineTrackItemContainer ItemContainer { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemContainer"></param>
        public TrackItemSelectedEventArgs(TimelineTrackItemContainer itemContainer)
        {
            ItemContainer = itemContainer;
        }
    }

    /// <summary>
    /// TimelineTrackItemContainer取消选中事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TrackItemUnselectedEventHandler(object sender, TrackItemUnselectedEventArgs e);

    /// <summary>
    /// TimelineTrackItemContainer取消选中事件参数
    /// </summary>
    public class TrackItemUnselectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// TimelineTrackItemContainer
        /// </summary>
        public TimelineTrackItemContainer ItemContainer { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemContainer"></param>
        public TrackItemUnselectedEventArgs(TimelineTrackItemContainer itemContainer)
        {
            ItemContainer = itemContainer;
        }
    }
}
