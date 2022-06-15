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
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
            DependencyProperty.Register(nameof(IsDraggable), typeof(bool), typeof(TimelineTrackItemContainer), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
        /// 刻度，Track、Group的区域
        /// </summary>
        public TimelineScale Scale { get; private set; }

        private TimelineTrackCanvas _owner;

        #region Drag
        private Point _previousDragPosition;
        private Point _initialDragPosition;
        private bool _isDragStart;
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
            _owner = this.TryFindParent<TimelineTrackCanvas>();
            UpdatePosition();
        }

        private void TimeScaleChanged(object sender, RoutedEventArgs e)
        {
            UpdatePosition();
        }

        /// <summary>
        /// Override OnMouseLeftButtonDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (_owner == null) return;

            _initialDragPosition = e.GetPosition(_owner);
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
                Point position = e.GetPosition(_owner);

                if (_previousDragPosition != position)
                {
                    // Start dragging
                    if (!_isDragStart)
                    {
                        RaiseEvent(new DragStartedEventArgs(_initialDragPosition.X, _initialDragPosition.Y)
                        {
                            RoutedEvent = DragStartedEvent
                        });

                        _isDragStart = true;
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

            if (_owner == null || Scale == null) return;

            if (_isDragStart)
            {
                _isDragStart = false;
                Vector position = e.GetPosition(_owner) - _initialDragPosition;

                RaiseEvent(new DragCompletedEventArgs(position.X, position.Y, false)
                {
                    RoutedEvent = DragCompletedEvent
                });

                e.Handled = true;
            }

            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                    IsSelected = !IsSelected;
                    break;
                case ModifierKeys.Shift:
                    IsSelected = true;
                    break;
                default:
                    Scale?.UnselectAll();
                    IsSelected = true;
                    break;
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
                _owner.InvalidateVisual();
            }
        }

        private void UpdatePosition()
        {
            if (Scale == null) return;

            Position = Scale.TimeToPos(CurrentTime);
        }
    }
}
