using Excalibur.Timeline.Helper;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Excalibur.Timeline
{
    /// <summary>
    /// Timeline的指针，指示当前时间，和有效时间的区域
    /// </summary>
    [TemplatePart(Name = ElementCurrentTimePointer, Type = typeof(Thumb))]
    [TemplatePart(Name = ElementDurationPointer, Type = typeof(Thumb))]
    [TemplatePart(Name = ElementMinDraggingTimeTextBox, Type = typeof(Border))]
    [TemplatePart(Name = ElementMaxDraggingTimeTextBox, Type = typeof(Border))]
    public class TimelinePointers : Control
    {
        private const string ElementCurrentTimePointer = "PART_CurrentTimePointer";
        private const string ElementDurationPointer = "PART_DurationPointer";
        private const string ElementMinDraggingTimeTextBox = "PART_MinDraggingTimeTextBox";
        private const string ElementMaxDraggingTimeTextBox = "PART_MaxDraggingTimeTextBox";

        /// <summary>
        /// 当前时间指针的位置
        /// </summary>
        public double CurrentTimePointerPosition
        {
            get { return (double)GetValue(CurrentTimePointerPositionProperty); }
            set { SetValue(CurrentTimePointerPositionProperty, value); }
        }
        /// <summary>
        /// CurrentTimePointerPosition属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimePointerPositionProperty =
            DependencyProperty.Register(nameof(CurrentTimePointerPosition), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 有效时间区域的位置
        /// </summary>
        public double DurationPointerPosition
        {
            get { return (double)GetValue(DurationPointerPositionProperty); }
            set { SetValue(DurationPointerPositionProperty, value); }
        }
        /// <summary>
        /// DurationPointerPosition属性
        /// </summary>
        public static readonly DependencyProperty DurationPointerPositionProperty =
            DependencyProperty.Register(nameof(DurationPointerPosition), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 当前时间指针的位置偏移
        /// </summary>
        public double CurrentTimePointerPositionOffset
        {
            get { return (double)GetValue(CurrentTimePointerPositionOffsetProperty); }
            set { SetValue(CurrentTimePointerPositionOffsetProperty, value); }
        }
        /// <summary>
        /// CurrentTimePointerPositionOffset属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimePointerPositionOffsetProperty =
            DependencyProperty.Register(nameof(CurrentTimePointerPositionOffset), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 有效时间指针的位置偏移
        /// </summary>
        public double DurationPointerPositionOffset
        {
            get { return (double)GetValue(DurationPointerPositionOffsetProperty); }
            set { SetValue(DurationPointerPositionOffsetProperty, value); }
        }
        /// <summary>
        /// DurationPointerPositionOffset属性
        /// </summary>
        public static readonly DependencyProperty DurationPointerPositionOffsetProperty =
            DependencyProperty.Register(nameof(DurationPointerPositionOffset), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// 有效时间指针的位置偏移
        /// </summary>
        public double MinEffectiveTimePosition
        {
            get { return (double)GetValue(MinEffectiveTimePositionProperty); }
            set { SetValue(MinEffectiveTimePositionProperty, value); }
        }
        /// <summary>
        /// MinEffectiveTimePosition属性
        /// </summary>
        public static readonly DependencyProperty MinEffectiveTimePositionProperty =
            DependencyProperty.Register(nameof(MinEffectiveTimePosition), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 当前时间的刻度线笔刷
        /// </summary>
        public Brush CurrentTimeBrush
        {
            get { return (Brush)GetValue(CurrentTimeBrushProperty); }
            set { SetValue(CurrentTimeBrushProperty, value); }
        }
        /// <summary>
        /// CurrentTimeBrush属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimeBrushProperty =
            DependencyProperty.Register(nameof(CurrentTimeBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black));

        /// <summary>
        /// 最小时间的边界刻度线笔刷
        /// </summary>
        public Brush MinEffectiveTimeEdgeLineBrush
        {
            get { return (Brush)GetValue(MinEffectiveTimeEdgeLineBrushProperty); }
            set { SetValue(MinEffectiveTimeEdgeLineBrushProperty, value); }
        }
        /// <summary>
        /// MinEffectiveTimeEdgeLineBrush属性
        /// </summary>
        public static readonly DependencyProperty MinEffectiveTimeEdgeLineBrushProperty =
            DependencyProperty.Register(nameof(MinEffectiveTimeEdgeLineBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black));

        /// <summary>
        /// 最小时间的边界宽度
        /// </summary>
        public double MinEffectiveTimeEdgeWidth
        {
            get { return (double)GetValue(MinEffectiveTimeEdgeWidthProperty); }
            set { SetValue(MinEffectiveTimeEdgeWidthProperty, value); }
        }
        /// <summary>
        /// MinEffectiveTimeEdgeWidth属性
        /// </summary>
        public static readonly DependencyProperty MinEffectiveTimeEdgeWidthProperty =
            DependencyProperty.Register(nameof(MinEffectiveTimeEdgeWidth), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 有效时间的边界宽度
        /// </summary>
        public double DurationEdgeWidth
        {
            get { return (double)GetValue(DurationEdgeWidthProperty); }
            set { SetValue(DurationEdgeWidthProperty, value); }
        }
        /// <summary>
        /// DurationEdgeWidth属性
        /// </summary>
        public static readonly DependencyProperty DurationEdgeWidthProperty =
            DependencyProperty.Register(nameof(DurationEdgeWidth), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 有效时间的边界刻度线笔刷
        /// </summary>
        public Brush DurationEdgeLineBrush
        {
            get { return (Brush)GetValue(DurationEdgeLineBrushProperty); }
            set { SetValue(DurationEdgeLineBrushProperty, value); }
        }
        /// <summary>
        /// DurationEdgeLineBrush属性
        /// </summary>
        public static readonly DependencyProperty DurationEdgeLineBrushProperty =
            DependencyProperty.Register(nameof(DurationEdgeLineBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black));

        /// <summary>
        /// MinEffectiveTime的边界背景笔刷
        /// </summary>
        public Brush MinEffectiveTimeEdgeBrush
        {
            get { return (Brush)GetValue(MinEffectiveTimeEdgeBrushProperty); }
            set { SetValue(MinEffectiveTimeEdgeBrushProperty, value); }
        }
        /// <summary>
        /// MinEffectiveTimeEdgeBrush属性
        /// </summary>
        public static readonly DependencyProperty MinEffectiveTimeEdgeBrushProperty =
            DependencyProperty.Register(nameof(MinEffectiveTimeEdgeBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(76, 0, 0, 0)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// MinEffectiveTime的边界背景笔刷
        /// </summary>
        public Brush DurationEdgeBrush
        {
            get { return (Brush)GetValue(DurationEdgeBrushProperty); }
            set { SetValue(DurationEdgeBrushProperty, value); }
        }
        /// <summary>
        /// DurationEdgeBrush属性
        /// </summary>
        public static readonly DependencyProperty DurationEdgeBrushProperty =
            DependencyProperty.Register(nameof(DurationEdgeBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(76, 0, 0, 0)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 时间文字显示的笔刷
        /// </summary>
        public Brush TimeTextFontBrush
        {
            get { return (Brush)GetValue(TimeTextFontBrushProperty); }
            set { SetValue(TimeTextFontBrushProperty, value); }
        }
        /// <summary>
        /// TimeTextFontBrush属性
        /// </summary>
        public static readonly DependencyProperty TimeTextFontBrushProperty =
            DependencyProperty.Register(nameof(TimeTextFontBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 时间文字背景框笔刷
        /// </summary>
        public Brush TimeTextBoxBrush
        {
            get { return (Brush)GetValue(TimeTextBoxBrushProperty); }
            set { SetValue(TimeTextBoxBrushProperty, value); }
        }
        /// <summary>
        /// TimeTextBoxBrush属性
        /// </summary>
        public static readonly DependencyProperty TimeTextBoxBrushProperty =
            DependencyProperty.Register(nameof(TimeTextBoxBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 时间文字显示的背景框，(xOffset, y, width, height)
        /// </summary>
        public Rect TimeTextBox
        {
            get { return (Rect)GetValue(TimeTextBoxProperty); }
            set { SetValue(TimeTextBoxProperty, value); }
        }
        /// <summary>
        /// TimeTextBox属性
        /// </summary>
        public static readonly DependencyProperty TimeTextBoxProperty =
            DependencyProperty.Register(nameof(TimeTextBox), typeof(Rect), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Rect0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 时间文字的字体大小
        /// </summary>
        public double TimeTextFontSize
        {
            get { return (double)GetValue(TimeTextFontSizeProperty); }
            set { SetValue(TimeTextFontSizeProperty, value); }
        }
        /// <summary>
        /// TimeTextFontSize属性
        /// </summary>
        public static readonly DependencyProperty TimeTextFontSizeProperty =
            DependencyProperty.Register(nameof(TimeTextFontSize), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double12, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 时间文字的文字(xOffset,y)
        /// </summary>
        public Point TimeTextPosition
        {
            get { return (Point)GetValue(TimeTextPositionProperty); }
            set { SetValue(TimeTextPositionProperty, value); }
        }
        /// <summary>
        /// TimeTextPosition属性
        /// </summary>
        public static readonly DependencyProperty TimeTextPositionProperty =
            DependencyProperty.Register(nameof(TimeTextPosition), typeof(Point), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Point0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 渲染过程中的事件
        /// </summary>
        public event TimelinePointersRenderingHandler Rendering
        {
            add => AddHandler(RenderingEvent, value);
            remove => RemoveHandler(RenderingEvent, value);
        }
        /// <summary>
        /// 渲染过程中的事件
        /// </summary>
        public static readonly RoutedEvent RenderingEvent = EventManager.RegisterRoutedEvent(nameof(Rendering), RoutingStrategy.Bubble, typeof(TimelinePointersRenderingHandler), typeof(TimelinePointers));

        /// <summary>
        /// 显示最小拖拽时间文本
        /// </summary>
        public bool IsShowMinDraggingTimeText
        {
            get { return (bool)GetValue(IsShowMinDraggingTimeTextProperty); }
            set { SetValue(IsShowMinDraggingTimeTextProperty, value); }
        }
        /// <summary>
        /// 显示最小拖拽时间文本属性
        /// </summary>
        public static readonly DependencyProperty IsShowMinDraggingTimeTextProperty =
            DependencyProperty.Register(nameof(IsShowMinDraggingTimeText), typeof(bool), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 显示最大拖拽时间文本
        /// </summary>
        public bool IsShowMaxDraggingTimeText
        {
            get { return (bool)GetValue(IsShowMaxDraggingTimeTextProperty); }
            set { SetValue(IsShowMaxDraggingTimeTextProperty, value); }
        }
        /// <summary>
        /// 显示最大拖拽时间文本属性
        /// </summary>
        public static readonly DependencyProperty IsShowMaxDraggingTimeTextProperty =
            DependencyProperty.Register(nameof(IsShowMaxDraggingTimeText), typeof(bool), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 最小拖拽时间文本的位置
        /// </summary>
        public double MinDraggingTimeTextPosition
        {
            get { return (double)GetValue(MinDraggingTimeTextPositionProperty); }
            set { SetValue(MinDraggingTimeTextPositionProperty, value); }
        }
        /// <summary>
        /// 最小拖拽时间文本的位置属性
        /// </summary>
        public static readonly DependencyProperty MinDraggingTimeTextPositionProperty =
            DependencyProperty.Register(nameof(MinDraggingTimeTextPosition), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 最大拖拽时间文本的位置
        /// </summary>
        public double MaxDraggingTimeTextPosition
        {
            get { return (double)GetValue(MaxDraggingTimeTextPositionProperty); }
            set { SetValue(MaxDraggingTimeTextPositionProperty, value); }
        }
        /// <summary>
        /// 最大拖拽时间文本的位置属性
        /// </summary>
        public static readonly DependencyProperty MaxDraggingTimeTextPositionProperty =
            DependencyProperty.Register(nameof(MaxDraggingTimeTextPosition), typeof(double), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 最大拖拽时间文本
        /// </summary>
        public string MinDraggingTimeText
        {
            get { return (string)GetValue(MinDraggingTimeTextProperty); }
            set { SetValue(MinDraggingTimeTextProperty, value); }
        }
        /// <summary>
        /// 最大拖拽时间文本
        /// </summary>
        public static readonly DependencyProperty MinDraggingTimeTextProperty =
            DependencyProperty.Register(nameof(MinDraggingTimeText), typeof(string), typeof(TimelinePointers), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 最大拖拽时间文本
        /// </summary>
        public string MaxDraggingTimeText
        {
            get { return (string)GetValue(MaxDraggingTimeTextProperty); }
            set { SetValue(MaxDraggingTimeTextProperty, value); }
        }
        /// <summary>
        /// 最大拖拽时间文本
        /// </summary>
        public static readonly DependencyProperty MaxDraggingTimeTextProperty =
            DependencyProperty.Register(nameof(MaxDraggingTimeText), typeof(string), typeof(TimelinePointers), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 当前时间指针是否正在拖拽
        /// </summary>
        public bool IsCurrentTimePointerDragging
        {
            get { return (bool)GetValue(IsCurrentTimePointerDraggingProperty); }
            set { SetValue(IsCurrentTimePointerDraggingProperty, value); }
        }
        /// <summary>
        /// 当前时间指针是否正在拖拽
        /// </summary>
        public static readonly DependencyProperty IsCurrentTimePointerDraggingProperty =
            DependencyProperty.Register(nameof(IsCurrentTimePointerDragging), typeof(bool), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 当前时间指针是否正在拖拽
        /// </summary>
        public bool IsDurationPointerDragging
        {
            get { return (bool)GetValue(IsDurationPointerDraggingProperty); }
            set { SetValue(IsDurationPointerDraggingProperty, value); }
        }
        /// <summary>
        /// 当前时间指针是否正在拖拽
        /// </summary>
        public static readonly DependencyProperty IsDurationPointerDraggingProperty =
            DependencyProperty.Register(nameof(IsDurationPointerDragging), typeof(bool), typeof(TimelinePointers), new FrameworkPropertyMetadata(BoxValue.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 拖拽提示线笔刷
        /// </summary>
        public Brush DraggingPromptLineBrush
        {
            get { return (Brush)GetValue(DraggingPromptLineBrushProperty); }
            set { SetValue(DraggingPromptLineBrushProperty, value); }
        }
        /// <summary>
        /// DraggingPromptLineBrush属性
        /// </summary>
        public static readonly DependencyProperty DraggingPromptLineBrushProperty =
            DependencyProperty.Register(nameof(DraggingPromptLineBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black));

        private Thumb _currentTimePointer;
        private Thumb _durationPointer;
        private Border _minTimeTextBox;
        private Border _maxTimeTextBox;

        private TimelineScale _scale;

        private bool _isShowDraggingPrompt = false;

        static TimelinePointers()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelinePointers), new FrameworkPropertyMetadata(typeof(TimelinePointers)));
            FocusableProperty.OverrideMetadata(typeof(TimelinePointers), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TimelinePointers()
        {
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _currentTimePointer = Template.FindName(ElementCurrentTimePointer, this) as Thumb;
            _durationPointer = Template.FindName(ElementDurationPointer, this) as Thumb;
            _minTimeTextBox = Template.FindName(ElementMinDraggingTimeTextBox, this) as Border;
            _maxTimeTextBox = Template.FindName(ElementMaxDraggingTimeTextBox, this) as Border;

            _scale = this.TryFindParent<TimelineScale>();
            if (_scale != null)
            {
                _scale.TimeScaleChanged += TimeScaleChanged;
            }

            if (_currentTimePointer != null)
            {
                _currentTimePointer.DragCompleted += CurrentTimePointerDragCompleted;
                _currentTimePointer.DragDelta += CurrentTimePointerDragDelta;
            }

            if (_durationPointer != null)
            {
                _durationPointer.DragCompleted += DurationPointerDragCompleted;
                _durationPointer.DragDelta += DurationPointerDragDelta;
            }
        }

        private void CurrentTimePointerDragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsShowMinDraggingTimeText = false;
            IsCurrentTimePointerDragging = false;
            InvalidateVisual();
        }

        private void CurrentTimePointerDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!IsShowMinDraggingTimeText)
                IsShowMinDraggingTimeText = true;

            if (!IsCurrentTimePointerDragging)
                IsCurrentTimePointerDragging = true;

            if (_scale != null)
                _scale.PosToCurrentTime(CurrentTimePointerPosition + e.HorizontalChange + CurrentTimePointerPositionOffset);
        }

        private void DurationPointerDragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsShowMinDraggingTimeText = false;
            IsDurationPointerDragging = false;
            InvalidateVisual();
        }

        private void DurationPointerDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!IsShowMinDraggingTimeText)
                IsShowMinDraggingTimeText = true;
            if (!IsDurationPointerDragging)
                IsDurationPointerDragging = true;

            if (_scale != null)
            {
                _scale.PosToDuration(DurationPointerPosition + e.HorizontalChange + DurationPointerPositionOffset);
            }

            var durationPos = _scale.TimeToPos(_scale.Duration);
            UpdateEdgeWidth(MinEffectiveTimePosition, durationPos);
        }

        private void TimeScaleChanged(object sender, RoutedEventArgs e)
        {
            UpdatePointersPosition();
        }

        /// <summary>
        /// 更新当前时间指针和有效时间指针位置
        /// </summary>
        public void UpdatePointersPosition()
        {
            if (_scale == null) return;
            var durationPos = _scale.TimeToPos(_scale.Duration);
            MinEffectiveTimePosition = _scale.TimeToPos(_scale.MinEffectiveTime);

            UpdateCurrentTimePointerPosition(_scale.TimeToPos(_scale.CurrentTime), _scale.CurrentTimeText);
            UpdateDurationPointerPosition(durationPos, _scale.DurationText);

            UpdateEdgeWidth(MinEffectiveTimePosition, durationPos);
        }

        /// <summary>
        /// 更新当前时间指针的位置
        /// </summary>
        /// <param name="timePos">当前时间位置</param>
        /// <param name="timeText">当前时间文本</param>
        public void UpdateCurrentTimePointerPosition(double timePos, string timeText)
        {
            MinDraggingTimeText = timeText;
            CurrentTimePointerPosition = timePos - CurrentTimePointerPositionOffset;

            if (IsCurrentTimePointerDragging && IsShowMinDraggingTimeText) MinDraggingTimeTextPosition = CurrentTimePointerPosition + TimeTextBox.X;
            Debug.WriteLine(_minTimeTextBox.Visibility);
        }

        /// <summary>
        /// 更新当前有效时间指针的位置
        /// </summary>
        /// <param name="timePos">当前有效时间位置</param>
        /// <param name="timeText">当前有效时间位置</param>
        public void UpdateDurationPointerPosition(double timePos, string timeText)
        {
            MinDraggingTimeText = timeText;
            DurationPointerPosition = timePos - DurationPointerPositionOffset;
            if (IsDurationPointerDragging && IsShowMinDraggingTimeText) MinDraggingTimeTextPosition = DurationPointerPosition + TimeTextBox.X; Debug.WriteLine(_minTimeTextBox.Visibility);
        }

        /// <summary>
        /// 时间指针开始拖拽
        /// </summary>
        internal void StartPointersDragging()
        {
            if (!IsCurrentTimePointerDragging)
            {
                IsCurrentTimePointerDragging = true;
            }
            if (!IsShowMinDraggingTimeText)
            {
                IsShowMinDraggingTimeText = true;
            }
        }

        /// <summary>
        /// 结束时间指针拖拽
        /// </summary>
        public void EndPointersDragging()
        {
            if (IsShowMinDraggingTimeText)
            {
                IsShowMinDraggingTimeText = false;
                IsCurrentTimePointerDragging = false;
                UpdatePointersPosition(); // 防止位置不准确，重新计算位置

                InvalidateVisual();
            }
        }

        /// <summary>
        /// Override OnPreviewMouseUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            EndPointersDragging();
        }

        private void UpdateEdgeWidth(double zeroPos, double durationPos)
        {
            if (zeroPos > 0)
                MinEffectiveTimeEdgeWidth = zeroPos - 1;
            else MinEffectiveTimeEdgeWidth = 0;

            if (ActualWidth > durationPos)
                DurationEdgeWidth = ActualWidth - durationPos - 1;
            else DurationEdgeWidth = 0;
        }

        internal void StartDraggingPrompt(double minTime, double maxTime)
        {
            MinDraggingTimeText = _scale.TimeToText(minTime);
            MinDraggingTimeTextPosition = _scale.TimeToPos(minTime) - _minTimeTextBox.ActualWidth / 2;

            MaxDraggingTimeText = _scale.TimeToText(maxTime);
            MaxDraggingTimeTextPosition = _scale.TimeToPos(maxTime) - _maxTimeTextBox.ActualWidth / 2;
        }

        internal void ShowDraggingPrompt(double minTime, double maxTime)
        {
            if (!_isShowDraggingPrompt) _isShowDraggingPrompt = true;
            IsShowMinDraggingTimeText = true;

            MinDraggingTimeText = _scale.TimeToText(minTime);

            MinDraggingTimeTextPosition = _scale.TimeToPos(minTime) - _minTimeTextBox.ActualWidth / 2;

            if (minTime != maxTime)
            {
                MaxDraggingTimeText = _scale.TimeToText(maxTime);
                MaxDraggingTimeTextPosition = _scale.TimeToPos(maxTime) - _maxTimeTextBox.ActualWidth / 2;
                IsShowMaxDraggingTimeText = true;
            }
        }

        internal void EndDrawDraggingPrompt()
        {
            if (_isShowDraggingPrompt)
            {
                IsShowMinDraggingTimeText = false;
                IsShowMaxDraggingTimeText = false;

                _isShowDraggingPrompt = false;
            }
        }
    }

    /// <summary>
    /// Timeline指针渲染过程中事件处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TimelinePointersRenderingHandler(object sender, TimelinePointersRenderingEventArgs e);

    /// <summary>
    /// Timeline指针渲染过程中事件参数
    /// </summary>
    public class TimelinePointersRenderingEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Timeline指针
        /// </summary>
        public TimelinePointers Pointers { get; set; }

        /// <summary>
        /// DrawingContext
        /// </summary>
        public DrawingContext Context { get; }

        /// <summary>
        /// 绘制区域, 起始Y值，结束Y值，实际宽，实际高
        /// </summary>
        public Rect Area { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public TimelinePointersRenderingEventArgs(TimelinePointers pointers, DrawingContext context, Rect area)
        {
            Pointers = pointers;
            Context = context;
            Area = area;
        }
    }
}
