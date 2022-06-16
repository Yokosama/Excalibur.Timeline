using Excalibur.Timeline.Helper;
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
    public class TimelinePointers : Control
    {
        private const string ElementCurrentTimePointer = "PART_CurrentTimePointer";
        private const string ElementDurationPointer = "PART_DurationPointer";

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
            DependencyProperty.Register(nameof(CurrentTimeBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnCurrentTimeBrushChanged));

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
            DependencyProperty.Register(nameof(MinEffectiveTimeEdgeLineBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnMinEffectiveTimeEdgeLineBrushChanged));

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
            DependencyProperty.Register(nameof(DurationEdgeLineBrush), typeof(Brush), typeof(TimelinePointers), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnDurationEdgeLineBrushChanged));

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

        private Thumb _currentTimePointer;
        private Thumb _durationPointer;

        /// <summary>
        /// 当前时间指针是否正在拖拽
        /// </summary>
        public bool IsCurrentTimePointerDragging { get; set; }

        /// <summary>
        /// 有效时间指针是否正在拖拽
        /// </summary>
        public bool IsDurationPointerDragging { get; set; }

        /// <summary>
        /// 当前显示的时间
        /// </summary>
        private string _currentTimeText;
        /// <summary>
        /// 有效时间的显示文本
        /// </summary>
        private string _durationTimeText;

        private TimelineScale _scale;

        #region Draw
        private Pen _minTimeLinePen;
        private Pen _durationLinePen;
        private Pen _currentTimePen;

        private bool _isDrawDraggingPrompt = false;
        private double _draggingMinTime;
        private double _draggingMaxTime;
        #endregion

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
            _minTimeLinePen = new Pen(MinEffectiveTimeEdgeLineBrush, 2);
            _durationLinePen = new Pen(DurationEdgeLineBrush, 2);
            _currentTimePen = new Pen(CurrentTimeBrush, 2);

        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _currentTimePointer = Template.FindName(ElementCurrentTimePointer, this) as Thumb;
            _durationPointer = Template.FindName(ElementDurationPointer, this) as Thumb;

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

        private static void OnCurrentTimeBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelinePointers p)
            {
                if (p._currentTimePen == null)
                {
                    p._currentTimePen = new Pen(p.CurrentTimeBrush, 2);
                }
                p._currentTimePen.Brush = p.CurrentTimeBrush;
            }
        }

        private static void OnMinEffectiveTimeEdgeLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelinePointers p)
            {
                if (p._minTimeLinePen == null)
                {
                    p._minTimeLinePen = new Pen(p.MinEffectiveTimeEdgeLineBrush, 2);
                }
                p._minTimeLinePen.Brush = p.MinEffectiveTimeEdgeLineBrush;
            }
        }

        private static void OnDurationEdgeLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelinePointers p)
            {
                if (p._durationLinePen == null)
                {
                    p._durationLinePen = new Pen(p.DurationEdgeLineBrush, 2);
                }
                p._durationLinePen.Brush = p.DurationEdgeLineBrush;
            }
        }

        private void CurrentTimePointerDragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsCurrentTimePointerDragging = false;
            InvalidateVisual();
        }

        private void CurrentTimePointerDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!IsCurrentTimePointerDragging)
                IsCurrentTimePointerDragging = true;

            if (_scale != null)
                _scale.PosToCurrentTime(CurrentTimePointerPosition + e.HorizontalChange + CurrentTimePointerPositionOffset);
        }

        private void DurationPointerDragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsDurationPointerDragging = false;
            InvalidateVisual();
        }

        private void DurationPointerDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!IsDurationPointerDragging)
                IsDurationPointerDragging = true;

            if (_scale != null)
            {
                _scale.PosToDuration(DurationPointerPosition + e.HorizontalChange + DurationPointerPositionOffset);
            }
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
            var zeroPos = _scale.TimeToPos(_scale.MinEffectiveTime);

            UpdateCurrentTimePointerPosition(_scale.TimeToPos(_scale.CurrentTime), _scale.CurrentTimeText);
            UpdateDurationPointerPosition(durationPos, _scale.DurationText);

            UpdateEdgeWidth(zeroPos, durationPos);
        }

        /// <summary>
        /// 更新当前时间指针的位置
        /// </summary>
        /// <param name="timePos">当前时间位置</param>
        /// <param name="timeText">当前时间文本</param>
        public void UpdateCurrentTimePointerPosition(double timePos, string timeText)
        {
            _currentTimeText = timeText;
            CurrentTimePointerPosition = timePos - CurrentTimePointerPositionOffset;
        }

        /// <summary>
        /// 更新当前有效时间指针的位置
        /// </summary>
        /// <param name="timePos">当前有效时间位置</param>
        /// <param name="timeText">当前有效时间位置</param>
        public void UpdateDurationPointerPosition(double timePos, string timeText)
        {
            _durationTimeText = timeText;
            DurationPointerPosition = timePos - DurationPointerPositionOffset;
        }

        /// <summary>
        /// 结束时间指针拖拽
        /// </summary>
        public void EndPointersDragging()
        {
            if (IsCurrentTimePointerDragging || IsDurationPointerDragging)
            {
                IsCurrentTimePointerDragging = false;
                IsDurationPointerDragging = false;

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

        /// <summary>
        /// Override OnRender
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (_scale == null) return;
            dc.PushGuidelineSet(new GuidelineSet(new[] { 0.5 }, new[] { 0.5 }));

            if (IsCurrentTimePointerDragging)
            {
                DrawTimeText(dc, _currentTimeText, CurrentTimePointerPosition + TimeTextBox.X);
            }

            if (IsDurationPointerDragging)
            {
                DrawTimeText(dc, _durationTimeText, DurationPointerPosition + TimeTextBox.X);
            }

            var startPosY = _scale.ScaleLineAreaHeight;
            var height = ActualHeight;
            // 0边界线
            var zeroPos = _scale.MinEffectiveTimeToPos();
            var start = new Point(zeroPos, startPosY);
            var end = new Point(zeroPos, height);
            dc.DrawLine(_minTimeLinePen, start, end);

            // Duration边界线
            var durationPos = _scale.DurationToPos();
            start = new Point(durationPos, startPosY);
            end = new Point(durationPos, height);
            dc.DrawLine(_durationLinePen, start, end);

            UpdateEdgeWidth(zeroPos, durationPos);

            var posx = CurrentTimePointerPosition + CurrentTimePointerPositionOffset;
            start = new Point(posx, startPosY);
            end = new Point(posx, height);
            dc.DrawLine(_currentTimePen, start, end);

            if (_isDrawDraggingPrompt)
            {
                var curPos = _scale.TimeToPos(_draggingMinTime);
                dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(curPos, startPosY), new Point(curPos, height));
                DrawTimeText(dc, _scale.TimeToText(_draggingMinTime), curPos, true);

                if(_draggingMinTime != _draggingMaxTime)
                {
                    var endPos = _scale.TimeToPos(_draggingMaxTime);
                    dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(endPos, startPosY), new Point(endPos, height));
                    DrawTimeText(dc, _scale.TimeToText(_draggingMaxTime), endPos, true);
                }
            }
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

        /// <summary>
        /// 绘制事件文字
        /// </summary>
        /// <param name="dc">绘制上下文</param>
        /// <param name="timeText">时间文本</param>
        /// <param name="boxPosX">背景框位置</param>
        public void DrawTimeText(DrawingContext dc, string timeText, double boxPosX, bool alignMiddle = false)
        {
            var ft = new FormattedText(timeText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Sergio UI"), TimeTextFontSize, TimeTextFontBrush, VisualTreeHelper.GetDpi(this).PixelsPerDip);
            var boxPosY = TimeTextBox.Y - ft.Height;
            var boxWidth = TimeTextBox.Width + ft.Width;
            if (alignMiddle)
            {
                boxPosX -= boxWidth / 2;
            }
            // TODO: 自动根据边界，替换左右侧位置

            dc.DrawRoundedRectangle(TimeTextBoxBrush, null, new Rect(boxPosX, boxPosY, boxWidth, TimeTextBox.Height + ft.Height), 3, 3);

            dc.DrawText(ft, new Point(boxPosX + TimeTextPosition.X, boxPosY + TimeTextPosition.Y));
        }

        internal void DrawDraggingPrompt(double minTime, double maxTime)
        {
            if (!_isDrawDraggingPrompt) _isDrawDraggingPrompt = true;
            _draggingMinTime = minTime;
            _draggingMaxTime = maxTime;
            InvalidateVisual();
        }

        internal void EndDrawDraggingPrompt()
        {
            _isDrawDraggingPrompt = false;
            InvalidateVisual();
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
