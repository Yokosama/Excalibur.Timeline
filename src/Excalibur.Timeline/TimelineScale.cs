using Excalibur.Timeline.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 刻度，Track，Group，Clip的内容区域
    /// </summary>
    [TemplatePart(Name = ElementHorizontalScrollBar, Type = typeof(ScrollBar))]
    [TemplatePart(Name = ElementVerticalScrollBar, Type = typeof(ScrollBar))]
    [TemplatePart(Name = ElementTimelineScalePanel, Type = typeof(TimelineScalePanel))]
    [TemplatePart(Name = ElementTimelinePointers, Type = typeof(TimelinePointers))]
    public class TimelineScale : ItemsControl
    {
        private const string ElementHorizontalScrollBar = "PART_HorizontalScrollBar";
        private const string ElementVerticalScrollBar = "PART_VerticalScrollBar";
        private const string ElementTimelineScalePanel = "PART_TimelineScalePanel";
        private const string ElementTimelinePointers = "PART_TimelinePointers";

        /// <summary>
        /// 时间改变时的事件
        /// </summary>
        public event TimeScaleChangedEventHandler TimeScaleChanged
        {
            add => AddHandler(TimeScaleChangedEvent, value);
            remove => RemoveHandler(TimeScaleChangedEvent, value);
        }
        /// <summary>
        /// TimeScaleChanged事件
        /// </summary>
        public static readonly RoutedEvent TimeScaleChangedEvent = EventManager.RegisterRoutedEvent(nameof(TimeScaleChanged), RoutingStrategy.Bubble, typeof(TimeScaleChangedEventHandler), typeof(TimelineScale));

        /// <summary>
        /// 刻度线区域的高度
        /// </summary>
        public ZoomMode ZoomMode
        {
            get { return (ZoomMode)GetValue(ZoomModeProperty); }
            set { SetValue(ZoomModeProperty, value); }
        }
        /// <summary>
        /// ZoomMode属性
        /// </summary>
        public static readonly DependencyProperty ZoomModeProperty =
            DependencyProperty.Register(nameof(ZoomMode), typeof(ZoomMode), typeof(TimelineScale), new FrameworkPropertyMetadata(ZoomMode.Fixed));

        /// <summary>
        /// 刻度线区域的高度
        /// </summary>
        public double ScaleLineAreaHeight
        {
            get { return (double)GetValue(ScaleLineAreaHeightProperty); }
            set { SetValue(ScaleLineAreaHeightProperty, value); }
        }
        /// <summary>
        /// ScaleLineAreaHeight属性
        /// </summary>
        public static readonly DependencyProperty ScaleLineAreaHeightProperty =
            DependencyProperty.Register(nameof(ScaleLineAreaHeight), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double20, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度线的文字高度
        /// </summary>
        public double ScaleTextHeight
        {
            get { return (double)GetValue(ScaleTextHeightProperty); }
            set { SetValue(ScaleTextHeightProperty, value); }
        }
        /// <summary>
        /// ScaleTextHeight属性
        /// </summary>
        public static readonly DependencyProperty ScaleTextHeightProperty =
            DependencyProperty.Register(nameof(ScaleTextHeight), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double10, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度线的普通刻度线的高度
        /// </summary>
        public double ScaleNormalLineHeight
        {
            get { return (double)GetValue(ScaleNormalLineHeightProperty); }
            set { SetValue(ScaleNormalLineHeightProperty, value); }
        }
        /// <summary>
        /// ScaleNormalLineHeight属性
        /// </summary>
        public static readonly DependencyProperty ScaleNormalLineHeightProperty =
            DependencyProperty.Register(nameof(ScaleNormalLineHeight), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double5, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度线的文字刻度线的高度
        /// </summary>
        public double ScaleMiddleLineHeight
        {
            get { return (double)GetValue(ScaleMiddleLineHeightProperty); }
            set { SetValue(ScaleMiddleLineHeightProperty, value); }
        }
        /// <summary>
        /// ScaleMiddleLineHeight属性
        /// </summary>
        public static readonly DependencyProperty ScaleMiddleLineHeightProperty =
            DependencyProperty.Register(nameof(ScaleMiddleLineHeight), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double10, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度线的笔刷
        /// </summary>
        public Brush ScaleLineBrush
        {
            get { return (Brush)GetValue(ScaleLineBrushProperty); }
            set { SetValue(ScaleLineBrushProperty, value); }
        }
        /// <summary>
        /// ScaleLineBrush属性
        /// </summary>
        public static readonly DependencyProperty ScaleLineBrushProperty =
            DependencyProperty.Register(nameof(ScaleLineBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnScaleLineBrushChanged));

        /// <summary>
        /// 非文字刻度的刻度线的笔刷
        /// </summary>
        public Brush ScaleLineSecondaryBrush
        {
            get { return (Brush)GetValue(ScaleLineSecondaryBrushProperty); }
            set { SetValue(ScaleLineSecondaryBrushProperty, value); }
        }
        /// <summary>
        /// ScaleLineSecondaryBrush属性
        /// </summary>
        public static readonly DependencyProperty ScaleLineSecondaryBrushProperty =
            DependencyProperty.Register(nameof(ScaleLineSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnScaleLineSecondaryBrushChanged));

        /// <summary>
        /// Items区域内的刻度线笔刷
        /// </summary>
        public Brush ItemsLineBrush
        {
            get { return (Brush)GetValue(ItemsLineBrushProperty); }
            set { SetValue(ItemsLineBrushProperty, value); }
        }
        /// <summary>
        /// ItemsLineBrush属性
        /// </summary>
        public static readonly DependencyProperty ItemsLineBrushProperty =
            DependencyProperty.Register(nameof(ItemsLineBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnItemsLineBrushChanged));

        /// <summary>
        /// Items区域内的刻度线次级笔刷
        /// </summary>
        public Brush ItemsLineSecondaryBrush
        {
            get { return (Brush)GetValue(ItemsLineSecondaryBrushProperty); }
            set { SetValue(ItemsLineSecondaryBrushProperty, value); }
        }
        /// <summary>
        /// ItemsLineSecondaryBrush属性
        /// </summary>
        public static readonly DependencyProperty ItemsLineSecondaryBrushProperty =
            DependencyProperty.Register(nameof(ItemsLineSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(80, 0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnItemsLineSecondaryBrushChanged));

        /// <summary>
        /// 刻度文字笔刷
        /// </summary>
        public Brush ScaleFontBrush
        {
            get { return (Brush)GetValue(ScaleFontBrushProperty); }
            set { SetValue(ScaleFontBrushProperty, value); }
        }
        /// <summary>
        /// ScaleFontBrush属性
        /// </summary>
        public static readonly DependencyProperty ScaleFontBrushProperty =
            DependencyProperty.Register(nameof(ScaleFontBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度文字次级笔刷
        /// </summary>
        public Brush ScaleFontSecondaryBrush
        {
            get { return (Brush)GetValue(ScaleFontSecondaryBrushProperty); }
            set { SetValue(ScaleFontSecondaryBrushProperty, value); }
        }
        /// <summary>
        /// ScaleFontSecondaryBrush属性
        /// </summary>
        public static readonly DependencyProperty ScaleFontSecondaryBrushProperty =
            DependencyProperty.Register(nameof(ScaleFontSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 刻度文字字体大小
        /// </summary>
        public double ScaleFontSize
        {
            get { return (double)GetValue(ScaleFontSizeProperty); }
            set { SetValue(ScaleFontSizeProperty, value); }
        }
        /// <summary>
        /// ScaleFontSize属性
        /// </summary>
        public static readonly DependencyProperty ScaleFontSizeProperty =
            DependencyProperty.Register(nameof(ScaleFontSize), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double10, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 当前Timeline的有效时间长度
        /// </summary>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set
            {
                if (value <= MinDuration) value = MinDuration;

                SetValue(DurationProperty, value);
            }
        }
        /// <summary>
        /// Duration属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double20, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnDurationChanged));

        /// <summary>
        /// 用于计算时间之间的间隔
        /// </summary>
        public double[] Modulos
        {
            get { return (double[])GetValue(ModulosProperty); }
            set { SetValue(ModulosProperty, value); }
        }
        /// <summary>
        /// Modulos属性
        /// </summary>
        public static readonly DependencyProperty ModulosProperty =
            DependencyProperty.Register(nameof(Modulos), typeof(double[]), typeof(TimelineScale), new FrameworkPropertyMetadata(new double[]
            {
                0.1d, 0.5d, 1, 5, 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 250000, 500000
            }));

        /// <summary>
        /// 时间模式
        /// </summary>
        public TimeStepMode TimeStep
        {
            get { return (TimeStepMode)GetValue(TimeStepProperty); }
            set { SetValue(TimeStepProperty, value); }
        }
        /// <summary>
        /// TimeStep属性
        /// </summary>
        public static readonly DependencyProperty TimeStepProperty =
            DependencyProperty.Register(nameof(TimeStep), typeof(TimeStepMode), typeof(TimelineScale), new FrameworkPropertyMetadata(TimeStepMode.Seconds));

        /// <summary>
        /// 帧速率
        /// </summary>
        public int FrameRate
        {
            get { return (int)GetValue(FrameRateProperty); }
            set { SetValue(FrameRateProperty, value); }
        }
        /// <summary>
        /// FrameRate属性
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty =
            DependencyProperty.Register(nameof(FrameRate), typeof(int), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Int100, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFrameRateChanged));

        /// <summary>
        /// 当前时间
        /// </summary>
        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set
            {
                if (value < MinEffectiveTime) value = MinEffectiveTime;
                else if (value > Duration) value = Duration;

                SetValue(CurrentTimeProperty, value); 
            }
        }
        /// <summary>
        /// CurrentTime属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, OnCurrentTimeChanged));

        /// <summary>
        /// 0点时间的位置偏移
        /// </summary>
        public double TimePosOffset
        {
            get { return (double)GetValue(TimePosOffsetProperty); }
            set { SetValue(TimePosOffsetProperty, value); }
        }
        /// <summary>
        /// TimePosOffset属性
        /// </summary>
        public static readonly DependencyProperty TimePosOffsetProperty =
            DependencyProperty.Register(nameof(TimePosOffset), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double10, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Duration时间的位置偏移
        /// </summary>
        public double LastTimePosOffset
        {
            get { return (double)GetValue(LastTimePosOffsetProperty); }
            set { SetValue(LastTimePosOffsetProperty, value); }
        }
        /// <summary>
        /// LastTimePosOffset属性
        /// </summary>
        public static readonly DependencyProperty LastTimePosOffsetProperty =
            DependencyProperty.Register(nameof(LastTimePosOffset), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double40, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 拖拽时间指针或Clip的时候，自动移动
        /// </summary>
        public bool DisableAutoPanning
        {
            get { return (bool)GetValue(DisableAutoPanningProperty); }
            set { SetValue(DisableAutoPanningProperty, value); }
        }
        /// <summary>
        /// DisableAutoPanning属性
        /// </summary>
        public static readonly DependencyProperty DisableAutoPanningProperty =
            DependencyProperty.Register(nameof(DisableAutoPanning), typeof(bool), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.False, OnDisableAutoPanningChanged));

        /// <summary>
        /// 边界自动移动的距离
        /// </summary>
        public double AutoPanEdgeDistance
        {
            get { return (double)GetValue(AutoPanEdgeDistanceProperty); }
            set { SetValue(AutoPanEdgeDistanceProperty, value); }
        }
        /// <summary>
        /// AutoPanEdgeDistance属性
        /// </summary>
        public static readonly DependencyProperty AutoPanEdgeDistanceProperty =
            DependencyProperty.Register(nameof(AutoPanEdgeDistance), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double10));        
        /// <summary>
        /// 边界自动移动的速度
        /// </summary>
        public double AutoPanSpeed
        {
            get { return (double)GetValue(AutoPanSpeedProperty); }
            set { SetValue(AutoPanSpeedProperty, value); }
        }
        /// <summary>
        /// AutoPanSpeed属性
        /// </summary>
        public static readonly DependencyProperty AutoPanSpeedProperty =
            DependencyProperty.Register(nameof(AutoPanSpeed), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double5));

        /// <summary>
        /// 当前时间的最小有效时间
        /// </summary>
        public double MinEffectiveTime { get; set; } = 0d;
        /// <summary>
        /// 最小的有效持续时间
        /// </summary>
        public double MinDuration { get; set; } = 0.1d;
        /// <summary>
        /// ZoomMode.Fixed模式下，界面的最小有效时间
        /// </summary>
        public double MinEffectiveViewTime { get; set; } = 0d;
        /// <summary>
        /// 界面内的最小时间
        /// </summary>
        public double ViewTimeMin { get; set; } = 0d;
        /// <summary>
        /// 界面内的最大时间
        /// </summary>
        public double ViewTimeMax { get; set; } = 25d;
        /// <summary>
        /// 界面时间区间大小
        /// </summary>
        private double ViewTime => ViewTimeMax - ViewTimeMin;

        private double ViewWidth => ActualWidth;

        /// <summary>
        /// 当前时间的显示文本
        /// </summary>
        public string CurrentTimeText => TimeStep == TimeStepMode.Frames ? (CurrentTime * FrameRate).ToString("0") : CurrentTime.ToString("0.00");
        /// <summary>
        /// 当前有效时间的显示文本
        /// </summary>
        public string DurationText => TimeStep == TimeStepMode.Frames ? (Duration * FrameRate).ToString("0") : Duration.ToString("0.00");

        private double _snapInterval = 0.01f;
        /// <summary>
        /// 根据时间模式，四舍五入时间的间隔
        /// </summary>
        private double SnapInterval
        {
            get { return Math.Max(_snapInterval, 0.001f); }
            set
            {
                if (_snapInterval != value)
                {
                    _snapInterval = Math.Max(_snapInterval, value);
                }
            }
        }

        /// <summary>
        /// 是否正在批量更新
        /// </summary>
        public bool IsBulkUpdatingItems { get; private set; }

        #region ScrollBar
        /// <summary>
        /// 水平滚动条
        /// </summary>
        private ScrollBar _horizontalBar;
        /// <summary>
        /// 竖直滚动条
        /// </summary>
        private ScrollBar _verticalBar;
        /// <summary>
        /// 前一次水平滚动条的值
        /// </summary>
        private double _preHSBValue;
        /// <summary>
        /// 水平滚动条拖拽开始
        /// </summary>
        private bool _hsbDragStart = false;
        #endregion

        private TimelineScalePanel _itemsPanel;
        private TimelinePointers _pointers;

        #region Render
        private double _timeInfoStart;
        private double _timeInfoEnd;
        private Pen _scaleLinePen;
        private Pen _scaleLinePen2;
        private Pen _itemsLinePen;
        private Pen _itemsLinePen2;
        private HashSet<double> _middleLinePositions = new HashSet<double>();
        #endregion

        #region Pointer
        private bool _dragCurrentTimePointer = false;
        #endregion

        #region Drag
        private bool _dragStart = false;
        private Point _dragStartPosition;
        private static Cursor _panCursor;

        private DispatcherTimer _autoPanningTimer;
        /// <summary>
        /// 自动移动的间隔时间
        /// </summary>
        public double AutoPanningTickRate { get; set; } = 5;
        /// <summary>
        /// 是否正在自动移动
        /// </summary>
        public bool IsInAutoPanning { get; set; } = false;
        #endregion

        static TimelineScale()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineScale), new FrameworkPropertyMetadata(typeof(TimelineScale)));
            FocusableProperty.OverrideMetadata(typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.True));

            _panCursor = CursorHelper.GetCursor("Resources/Images/PanCursorMouseDown.cur");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TimelineScale()
        {
            Loaded += TimelineScaleLoaded;
            Unloaded += TimelineScaleUnloaded;
            LayoutUpdated += TimelineScaleLayoutUpdated;

            _scaleLinePen = new Pen(ScaleLineBrush, 1);
            _scaleLinePen2 = new Pen(ScaleLineSecondaryBrush, 1);
            _itemsLinePen = new Pen(ItemsLineBrush, 1);
            _itemsLinePen2 = new Pen(ItemsLineSecondaryBrush, 1);
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _horizontalBar = Template.FindName(ElementHorizontalScrollBar, this) as ScrollBar;
            _verticalBar = Template.FindName(ElementVerticalScrollBar, this) as ScrollBar;
            _itemsPanel = Template.FindName(ElementTimelineScalePanel, this) as TimelineScalePanel;
            _pointers = Template.FindName(ElementTimelinePointers, this) as TimelinePointers;

            if (_horizontalBar != null) _horizontalBar.Scroll += HorizontalBarScroll;

            OnDisableAutoPanningChanged(DisableAutoPanning);
        }

        /// <summary>
        /// Override OnRenderSizeChanged
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            RaiseTimeScaleChangedEvent();
        }

        /// <summary>
        /// Override OnRender
        /// </summary>
        /// <param name="dc"></param>
        protected override void OnRender(DrawingContext dc)
        {
            dc.PushGuidelineSet(new GuidelineSet(new[] { 0.5d }, new[] { 0.5d }));

            if (!_hsbDragStart)
            {
                UpdateHorizontalScrollBarRange();
            }

            var timeInfoInterval = 1000000d;
            var timeInfoHighMod = timeInfoInterval;

            var lowMod = 0.01d;
            if (Modulos == null || Modulos.Length == 0)
            {
                Modulos = new double[] { 0.1d, 0.5d, 1, 5, 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 250000, 500000 };
            }
            for (var i = 0; i < Modulos.Length; i++)
            {
                var count = ViewTime / Modulos[i];
                if (ViewWidth / count > 50)
                {
                    timeInfoInterval = Modulos[i];
                    lowMod = i > 0 ? Modulos[i - 1] : lowMod;
                    timeInfoHighMod = i < Modulos.Length - 1 ? Modulos[i + 1] : timeInfoHighMod;
                    break;
                }
            }

            var doFrames = TimeStep == TimeStepMode.Frames;
            var timeStep = doFrames ? (1f / FrameRate) : lowMod;
            var decimalPlaces = timeStep.GetDecimalPlaces();
            decimalPlaces = decimalPlaces == 0 ? 10 : (int)Math.Pow(10, decimalPlaces);
            _timeInfoStart = (double)Math.Floor(ViewTimeMin / timeInfoInterval) * timeInfoInterval;
            _timeInfoEnd = (double)Math.Ceiling(ViewTimeMax / timeInfoInterval) * timeInfoInterval;
            _timeInfoStart = Math.Round(_timeInfoStart * 10) / 10;
            _timeInfoEnd = Math.Round(_timeInfoEnd * 10) / 10;

            dc.DrawLine(_scaleLinePen, new Point(0, ScaleLineAreaHeight), new Point(ActualWidth, ScaleLineAreaHeight));

            _middleLinePositions.Clear();
            for (var i = _timeInfoStart; i <= _timeInfoEnd; i += timeInfoInterval)
            {
                var posX = TimeToPos(i);
                var rounded = Math.Round(i * decimalPlaces) / decimalPlaces;
                _middleLinePositions.Add(rounded);

                var start = new Point(posX, ScaleLineAreaHeight);
                var end = new Point(posX, ScaleLineAreaHeight - ScaleMiddleLineHeight);
                dc.DrawLine(_scaleLinePen, start, end);

                var text = doFrames ? (rounded * FrameRate).ToString("0") : rounded.ToString("0.00");
                var fontColor = rounded % timeInfoHighMod == 0 ? ScaleFontBrush : ScaleFontSecondaryBrush;
                dc.DrawText(new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Sergio UI"), ScaleFontSize, fontColor, VisualTreeHelper.GetDpi(this).PixelsPerDip), new Point(posX + 5, ScaleLineAreaHeight - ScaleTextHeight));
            }

            if (ViewWidth / (ViewTime / timeStep) > 6)
            {
                for (double i = _timeInfoStart, j = _timeInfoStart; i <= _timeInfoEnd; i += timeStep, j += timeInfoInterval)
                {
                    var posX = TimeToPos(i);
                    var rounded = Math.Round(i * decimalPlaces) / decimalPlaces;
                    if (_middleLinePositions.Contains(rounded))
                    {
                        continue;
                    }

                    var start = new Point(posX, ScaleLineAreaHeight);
                    var end = new Point(posX, ScaleLineAreaHeight - ScaleNormalLineHeight);

                    dc.DrawLine(_scaleLinePen2, start, end);
                }
            }

            // tracks area line
            for (var i = _timeInfoStart; i <= _timeInfoEnd; i += timeInfoInterval)
            {
                var posX = TimeToPos(i);
                var rounded = Math.Round(i * 10) / 10;

                var start = new Point(posX, ScaleLineAreaHeight);
                var end = new Point(posX, ActualHeight);
                if (rounded % timeInfoHighMod == 0)
                {
                    dc.DrawLine(_itemsLinePen, start, end);
                }
                else
                {
                    dc.DrawLine(_itemsLinePen2, start, end);
                }
            }

            base.OnRender(dc);
        }

        private void UpdateHorizontalScrollBarRange()
        {
            _horizontalBar.Minimum = MinEffectiveTime;
            _horizontalBar.ViewportSize = ViewTime;

            double maximun;
            var t = LastTimePosOffset / ViewWidth * ViewTime;

            if (ViewTimeMin < 0)
            {
                if (ViewTimeMax > Duration + t)
                {
                    maximun = Math.Abs(ViewTimeMax - ViewTimeMin) - ViewTime;
                }
                else
                {
                    maximun = Math.Abs(Duration - ViewTimeMin) - ViewTime;
                }
            }
            else
            {
                if (ViewTimeMax > Duration + t)
                {
                    maximun = Math.Abs(ViewTimeMax) - ViewTime;
                }
                else
                {
                    maximun = Math.Abs(Duration + t) - ViewTime;
                }
            }

            _horizontalBar.Maximum = maximun;
            _horizontalBar.Value = ViewTimeMin;
        }

        private void TimelineScaleLoaded(object sender, RoutedEventArgs e)
        {
            UpdateVerticalScrollBarValue();
            _horizontalBar.Track.Thumb.DragStarted += HorizontalBarThumbDragStarted;
            _horizontalBar.Track.Thumb.DragCompleted += HorizontalBarThumbDragCompleted;

            if (_pointers != null) _pointers.UpdatePointersPosition();
        }

        private void TimelineScaleUnloaded(object sender, RoutedEventArgs e)
        {
            _horizontalBar.Track.Thumb.DragStarted -= HorizontalBarThumbDragStarted;
            _horizontalBar.Track.Thumb.DragCompleted -= HorizontalBarThumbDragCompleted;
        }

        private void TimelineScaleLayoutUpdated(object sender, EventArgs e)
        {
            UpdateVerticalScrollBarValue();
        }

        private void HorizontalBarScroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollEventType == ScrollEventType.EndScroll)
            {
                _hsbDragStart = false;
                _preHSBValue = 0;
                _hsbDragStart = false;
                InvalidateVisual();
                RaiseTimeScaleChangedEvent();
            }
            else
            {
                if (!_hsbDragStart)
                {
                    _hsbDragStart = true;
                    _preHSBValue = _horizontalBar.Value;
                }
                if (_hsbDragStart)
                {
                    if (ViewTimeMin >= MinEffectiveTime)
                    {
                        var delta = _horizontalBar.Value - _preHSBValue;
                        if (delta == 0)
                        {
                            return;
                        }
                        ViewTimeMin = _horizontalBar.Value;
                        ViewTimeMax += delta;
                    }
                    else
                    {
                        var vt = ViewTime; // 先缓存当前的viewtime
                        ViewTimeMin = MinEffectiveTime;
                        ViewTimeMax = Math.Abs(vt);
                        _hsbDragStart = false;
                    }

                    _preHSBValue = _horizontalBar.Value;
                    InvalidateVisual();
                    RaiseTimeScaleChangedEvent();
                }
            }
        }

        private void HorizontalBarThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            _preHSBValue = _horizontalBar.Value;
            _hsbDragStart = true;
        }

        private void HorizontalBarThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _preHSBValue = 0;
            _hsbDragStart = false;
            InvalidateVisual();
            RaiseTimeScaleChangedEvent();
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                scale._pointers.UpdateDurationPointerPosition(scale.TimeToPos(scale.Duration),
                       scale.DurationText);

                if(scale.CurrentTime >= scale.Duration)
                {
                    scale.CurrentTime = scale.Duration;
                }
            }
        }

        private static void OnFrameRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                scale.SnapInterval = 1d / scale.FrameRate;
            }
        }

        private static void OnCurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale && !scale.IsInAutoPanning)
            {
                scale._pointers.UpdateCurrentTimePointerPosition(scale.TimeToPos(scale.CurrentTime),
                      scale.CurrentTimeText);
            }
        }

        private static void OnDisableAutoPanningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                scale.OnDisableAutoPanningChanged(scale.DisableAutoPanning);
            }
        }

        private static void OnScaleLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                if (scale._scaleLinePen == null) scale._scaleLinePen = new Pen(scale.ScaleLineBrush, 1);
                scale._scaleLinePen.Brush = scale.ScaleLineBrush;
                scale.InvalidateVisual();
            }
        }

        private static void OnScaleLineSecondaryBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                if (scale._scaleLinePen2 == null) scale._scaleLinePen2 = new Pen(scale.ScaleLineSecondaryBrush, 1);
                scale._scaleLinePen2.Brush = scale.ScaleLineSecondaryBrush;
                scale.InvalidateVisual();
            }
        }

        private static void OnItemsLineBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                if (scale._itemsLinePen == null) scale._itemsLinePen = new Pen(scale.ItemsLineBrush, 1);
                scale._itemsLinePen.Brush = scale.ItemsLineBrush;
                scale.InvalidateVisual();
            }
        }

        private static void OnItemsLineSecondaryBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                if (scale._itemsLinePen2 == null) scale._itemsLinePen2 = new Pen(scale.ItemsLineSecondaryBrush, 1);
                scale._itemsLinePen2.Brush = scale.ItemsLineSecondaryBrush;
                scale.InvalidateVisual();
            }
        }

        private void UpdateVerticalScrollBarValue()
        {
            if (_itemsPanel == null || _verticalBar == null) return;

            _verticalBar.ViewportSize = _itemsPanel.ViewportHeight;
            var max = _itemsPanel.ExtentHeight - _itemsPanel.ViewportHeight;
            max = Math.Max(max, 0);
            _verticalBar.Maximum = max;
        }

        private void RaiseTimeScaleChangedEvent()
        {
            var args = new RoutedEventArgs()
            {
                RoutedEvent = TimeScaleChangedEvent,
                Source = this
            };

            RaiseEvent(args);
        }

        private void OnDisableAutoPanningChanged(bool shouldDisable)
        {
            if (shouldDisable)
            {
                _autoPanningTimer?.Stop();
            }
            else if (_autoPanningTimer == null)
            {
                _autoPanningTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(AutoPanningTickRate),
                    DispatcherPriority.Background, HandleAutoPanning, Dispatcher);
            }
            else
            {
                _autoPanningTimer.Interval = TimeSpan.FromMilliseconds(AutoPanningTickRate);
                _autoPanningTimer.Start();
            }
        }

        private void HandleAutoPanning(object sender, EventArgs e)
        {
            if (IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed && Mouse.Captured != null &&
                _pointers != null &&
                (_dragCurrentTimePointer || _pointers.IsCurrentTimePointerDragging || _pointers.IsDurationPointerDragging))
            {
                //Point mousePosition = Mouse.GetPosition(this);
                double edgeDistance = AutoPanEdgeDistance;
                double autoPanSpeed = Math.Min(AutoPanSpeed, AutoPanSpeed * AutoPanningTickRate);
                double x = 0;

                if (_dragCurrentTimePointer || _pointers.IsCurrentTimePointerDragging)
                {
                    var pos = TimeToPos(CurrentTime);

                    if (pos <= edgeDistance)
                    {
                        x -= autoPanSpeed;
                    }
                    else if (pos >= ActualWidth - edgeDistance)
                    {
                        x += autoPanSpeed;
                    }
                    if (x != 0)
                    {
                        IsInAutoPanning = true;

                        HorizontalDrag(-x);
                        var time = PosToTime(Mouse.GetPosition(this).X);
                        CurrentTime = SnapTime(time);

                        RaiseTimeScaleChangedEvent();
                        InvalidateVisual();
                        return;
                    }
                }
                else if (_pointers.IsDurationPointerDragging)
                {
                    var pos = TimeToPos(Duration);

                    if (pos <= edgeDistance)
                    {
                        x -= autoPanSpeed;
                    }
                    else if (pos >= ActualWidth - edgeDistance)
                    {
                        x += autoPanSpeed;
                    }
                    if (x != 0)
                    {
                        IsInAutoPanning = true;

                        HorizontalDrag(-x);
                        var time = PosToTime(Mouse.GetPosition(this).X);
                        Duration = SnapTime(time);

                        RaiseTimeScaleChangedEvent();
                        InvalidateVisual();
                        return;
                    }
                }
            }
      
            if(IsInAutoPanning)
            {
                IsInAutoPanning = false;
            }
        }

        /// <summary>
        /// Override OnMouseWheel 处理鼠标滚轮滚动时，缩放视图
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var mousePosition = Mouse.GetPosition(this);
            var pointerTimeA = PosToTime(mousePosition.X);
            var delta = Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) ? 0 : -e.Delta * 0.01f;
            var t = (Math.Abs(delta * 25) / RenderSize.Width) * ViewTime;
            ViewTimeMin += delta > 0 ? -t : t;
            ViewTimeMax += delta > 0 ? t : -t;
            var pointerTimeB = PosToTime(mousePosition.X + 0);
            var diff = pointerTimeA - pointerTimeB;
            ViewTimeMin += diff;
            ViewTimeMax += diff;

            if (ZoomMode == ZoomMode.Fixed && ViewTimeMin <= MinEffectiveViewTime)
            {
                ViewTimeMin = MinEffectiveViewTime;
            }

            InvalidateVisual();
            RaiseTimeScaleChangedEvent();
        }

        /// <summary>
        /// Override OnMouseLeftButtonDown 设置当前时间
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            var pos = e.GetPosition(this);
            if (pos.Y < ScaleLineAreaHeight && pos.Y >= 0 && !_dragCurrentTimePointer) // 鼠标左键点击刻度线区域，设置当前时间
            {
                _dragCurrentTimePointer = true;
                PosToCurrentTime(pos.X);
                Mouse.Capture(this);
            }
            e.Handled = true;
        }

        /// <summary>
        /// Override OnPreviewMouseDown 拖拽起始设置
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // Ctrl+鼠标左键按住 或 按住鼠标滚轮 开始进行拖拽
            if ((e.ChangedButton == MouseButton.Left && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.LeftButton == MouseButtonState.Pressed) ||
                (e.ChangedButton == MouseButton.Middle && e.MiddleButton == MouseButtonState.Pressed))
            {
                Cursor = _panCursor;
                _dragStart = true;
                _dragStartPosition = e.GetPosition(this);
                Mouse.Capture(this);
            }
        }

        /// <summary>
        /// Override OnPreviewMouseUp 取消拖拽TimelinePointer或滚轮
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if (_dragCurrentTimePointer || _pointers.IsCurrentTimePointerDragging)
            {
                _dragCurrentTimePointer = false;
                _pointers.EndPointersDragging();
            }

            if (_dragStart)
            {
                _dragStart = false;
                Cursor = Cursors.Arrow;

                _dragStartPosition.X = 0;
                _dragStartPosition.Y = 0;
            }
            if (e.MouseDevice.Captured == this)
                ReleaseMouseCapture();
        }

        /// <summary>
        /// Override OnMouseMove 拖拽时移动界面
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point pos = e.GetPosition(this);
            if (_dragCurrentTimePointer)
            {
                if (!_pointers.IsCurrentTimePointerDragging) _pointers.IsCurrentTimePointerDragging = true;
                PosToCurrentTime(pos.X);
            }
            if (_dragStart)
            {
                if (e.LeftButton == MouseButtonState.Pressed ||
                   e.MiddleButton == MouseButtonState.Pressed)
                {
                    var delta = pos - _dragStartPosition;

                    HorizontalDrag(delta.X);

                    VerticalDrag(delta.Y);

                    _dragStartPosition = pos;

                    InvalidateVisual();
                    RaiseTimeScaleChangedEvent();
                }
                else
                {
                    if (e.MouseDevice.Captured == this)
                        ReleaseMouseCapture();
                    _dragStart = false;
                    _dragStartPosition.X = 0;
                    _dragStartPosition.Y = 0;
                }
            }
        }

        private void VerticalDrag(double delta)
        {
            var yoffset = _verticalBar.Value - delta;
            if (yoffset <= 0)
                yoffset = 0;
            else if (yoffset > _verticalBar.Maximum)
                yoffset = _verticalBar.Maximum;
            _verticalBar.Value = yoffset;
        }

        private void HorizontalDrag(double delta)
        {
            var t = (Math.Abs(delta) / ViewWidth) * ViewTime;
            var viewTime = ViewTime;
            var viewTimeMin = ViewTimeMin + (delta > 0 ? -t : t);
            if (ZoomMode == ZoomMode.Fixed && viewTimeMin <= MinEffectiveViewTime)
            {
                ViewTimeMin = MinEffectiveViewTime;
                ViewTimeMax = ViewTimeMin + viewTime;
                return;
            }
            ViewTimeMin = viewTimeMin;
            ViewTimeMax += delta > 0 ? -t : t;
        }

        /// <summary>
        /// 时间转换为界面上的位置
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>界面上的位置</returns>
        public double TimeToPos(double time)
        {
            return (time - ViewTimeMin) / ViewTime * ViewWidth + TimePosOffset;
        }

        /// <summary>
        /// 界面上的位置转换为时间
        /// </summary>
        /// <param name="pos">界面上的位置</param>
        /// <returns>时间</returns>
        public double PosToTime(double pos)
        {
            return (pos - TimePosOffset) / ViewWidth * ViewTime + ViewTimeMin;
        }

        /// <summary>
        /// 界面上的位置转换为当前时间
        /// </summary>
        /// <param name="pos">界面上的位置</param>
        public void PosToCurrentTime(double pos)
        {
            if (IsInAutoPanning) return;
            var time = PosToTime(pos);
            time = SnapTime(time);

            CurrentTime = time;
        }

        /// <summary>
        /// 界面上的位置转换为当前有效区间时间
        /// </summary>
        /// <param name="pos">界面上的位置</param>
        public void PosToDuration(double pos)
        {
            if (IsInAutoPanning) return;
            var time = PosToTime(pos);
            Duration = SnapTime(time);
        }

        /// <summary>
        /// 最小时间转换为界面位置
        /// </summary>
        /// <returns></returns>
        public double MinEffectiveTimeToPos()
        {
            return TimeToPos(MinEffectiveTime);
        }

        /// <summary>
        /// 有效时间转换为界面位置
        /// </summary>
        /// <returns></returns>
        public double DurationToPos()
        {
            return TimeToPos(Duration);
        }

        /// <summary>
        /// 四舍五入时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double SnapTime(double time)
        {
            return (Math.Round(time / SnapInterval) * SnapInterval);
        }

        #region Selection
        /// <summary>
        /// 取消所有选中项
        /// </summary>
        public void UnselectAll()
        {

        }
        #endregion
    }

    /// <summary>
    /// Scale区域缩放模式
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        /// 缩小时,0点固定，放大根据当前鼠标所处时间位置进行放大
        /// </summary>
        Fixed,
        /// <summary>
        /// 缩放时，根据鼠标所处位置的时间进行左右缩放
        /// </summary>
        Free,
    }

    /// <summary>
    /// 时间模式
    /// </summary>
    public enum TimeStepMode
    {
        /// <summary>
        /// 秒
        /// </summary>
        Seconds,
        /// <summary>
        /// 帧
        /// </summary>
        Frames
    }

    /// <summary>
    /// 时间刻度区域时间发生改变时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TimeScaleChangedEventHandler(object sender, RoutedEventArgs e);
}
