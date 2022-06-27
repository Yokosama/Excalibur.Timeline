using Excalibur.Timeline.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
        /// 持续时间区域显示高度
        /// </summary>
        public double DurationLineAreaHeight
        {
            get { return (double)GetValue(DurationLineAreaHeightProperty); }
            set { SetValue(DurationLineAreaHeightProperty, value); }
        }
        /// <summary>
        /// DurationLineAreaHeight属性
        /// </summary>
        public static readonly DependencyProperty DurationLineAreaHeightProperty =
            DependencyProperty.Register(nameof(DurationLineAreaHeight), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double5, FrameworkPropertyMetadataOptions.AffectsRender));

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
        /// 刻度背景的笔刷
        /// </summary>
        public Brush ScaleLineAreaBackground
        {
            get { return (Brush)GetValue(ScaleLineAreaBackgroundProperty); }
            set { SetValue(ScaleLineAreaBackgroundProperty, value); }
        }
        /// <summary>
        /// ScaleLineAreaBackground属性
        /// </summary>
        public static readonly DependencyProperty ScaleLineAreaBackgroundProperty =
            DependencyProperty.Register(nameof(ScaleLineAreaBackground), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

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
            DependencyProperty.Register(nameof(ScaleLineSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, OnScaleLineSecondaryBrushChanged));

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
            DependencyProperty.Register(nameof(ItemsLineBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, OnItemsLineBrushChanged));

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
            DependencyProperty.Register(nameof(ItemsLineSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(80, 0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender, OnItemsLineSecondaryBrushChanged));

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
            DependencyProperty.Register(nameof(ScaleFontBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

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
            DependencyProperty.Register(nameof(ScaleFontSecondaryBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 持续时间区域笔刷
        /// </summary>
        public Brush DurationLineBrush
        {
            get { return (Brush)GetValue(DurationLineBrushProperty); }
            set { SetValue(DurationLineBrushProperty, value); }
        }
        /// <summary>
        /// DurationLineBrush属性
        /// </summary>
        public static readonly DependencyProperty DurationLineBrushProperty =
            DependencyProperty.Register(nameof(DurationLineBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));   
        
        /// <summary>
        /// 刻度区域与Track区域的分割线笔刷
        /// </summary>
        public Brush ScaleAndItemsSeparatorBrush
        {
            get { return (Brush)GetValue(ScaleAndItemsSeparatorBrushProperty); }
            set { SetValue(ScaleAndItemsSeparatorBrushProperty, value); }
        }
        /// <summary>
        /// ScaleAndItemsSeparatorBrush属性
        /// </summary>
        public static readonly DependencyProperty ScaleAndItemsSeparatorBrushProperty =
            DependencyProperty.Register(nameof(ScaleAndItemsSeparatorBrush), typeof(Brush), typeof(TimelineScale), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, OnScaleAndItemsSeparatorBrushChanged));

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


        private double _viewTimeMin = 0d;
        /// <summary>
        /// 界面内的最小时间
        /// </summary>
        public double ViewTimeMin
        {
            get { return (double)GetValue(ViewTimeMinProperty); }
            set
            {
                if (ViewTimeMax > 0)
                {
                    _viewTimeMin = Math.Min(value, ViewTimeMax - 0.25d);
                    SetValue(ViewTimeMinProperty, _viewTimeMin);
                }
            }
        }
        /// <summary>
        /// 界面内的最小时间属性
        /// </summary>
        public static readonly DependencyProperty ViewTimeMinProperty =
            DependencyProperty.Register(nameof(ViewTimeMin), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private double _viewTimeMax = 0d;
        /// <summary>
        /// 界面内的最小时间
        /// </summary>
        public double ViewTimeMax
        {
            get { return (double)GetValue(ViewTimeMaxProperty); }
            set
            {
                var v = Math.Max(value, _viewTimeMin + 0.25d);
                _viewTimeMax = Math.Max(v, 0);
                SetValue(ViewTimeMaxProperty, _viewTimeMax);
            }
        }
        /// <summary>
        /// 界面内的最大时间属性
        /// </summary>
        public static readonly DependencyProperty ViewTimeMaxProperty =
            DependencyProperty.Register(nameof(ViewTimeMax), typeof(double), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Double25, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


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
        /// 选中的TimelineTrackItemContainer集合
        /// </summary>
        public System.Collections.IList SelectedTrackItems
        {
            get { return (System.Collections.IList)GetValue(SelectedTrackItemsProperty); }
            set { SetValue(SelectedTrackItemsProperty, value); }
        }
        /// <summary>
        /// 选中的TimelineTrackItemContainer集合
        /// </summary>
        public static readonly DependencyProperty SelectedTrackItemsProperty =
            DependencyProperty.Register(nameof(SelectedTrackItems), typeof(System.Collections.IList), typeof(TimelineScale), new FrameworkPropertyMetadata(new ObservableCollection<object>(), OnSelectedTrackItemsSourceChanged));

        /// <summary>
        /// 是否正在选择
        /// </summary>
        public bool IsSelecting
        {
            get { return (bool)GetValue(IsSelectingProperty); }
            set { SetValue(IsSelectingProperty, value); }
        }
        /// <summary>
        /// IsSelecting属性
        /// </summary>
        public static readonly DependencyProperty IsSelectingProperty =
            DependencyProperty.Register(nameof(IsSelecting), typeof(bool), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.False));

        /// <summary>
        /// 选中的Clip改变事件
        /// </summary>
        public event SelectionChangedEventHandler SelectionTrackItemsChanged
        {
            add { AddHandler(SelectionTrackItemsChangedEvent, value); }
            remove { RemoveHandler(SelectionTrackItemsChangedEvent, value); }
        }
        /// <summary>
        /// 选中的Clip改变事件
        /// </summary>
        public static readonly RoutedEvent SelectionTrackItemsChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectionTrackItemsChanged), RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(TimelineScale));

        /// <summary>
        /// Clip Items拖拽开始
        /// </summary>
        public ICommand ItemsDragStartedCommand
        {
            get => (ICommand)GetValue(ItemsDragStartedCommandProperty);
            set => SetValue(ItemsDragStartedCommandProperty, value);
        }
        /// <summary>
        /// Clip Items拖拽开始
        /// </summary>
        public static readonly DependencyProperty ItemsDragStartedCommandProperty = DependencyProperty.Register(nameof(ItemsDragStartedCommand), typeof(ICommand), typeof(TimelineScale));

        /// <summary>
        /// Clip Items拖拽结束
        /// </summary>
        public ICommand ItemsDragCompletedCommand
        {
            get => (ICommand)GetValue(ItemsDragCompletedCommandProperty);
            set => SetValue(ItemsDragCompletedCommandProperty, value);
        }
        /// <summary>
        /// Clip Items拖拽开始
        /// </summary>
        public static readonly DependencyProperty ItemsDragCompletedCommandProperty = DependencyProperty.Register(nameof(ItemsDragCompletedCommand), typeof(ICommand), typeof(TimelineScale));

        /// <summary>
        /// 多选框样式
        /// </summary>
        public Style SelectionRectangleStyle
        {
            get => (Style)GetValue(SelectionRectangleStyleProperty);
            set => SetValue(SelectionRectangleStyleProperty, value);
        }
        /// <summary>
        /// 多选框样式
        /// </summary>
        public static readonly DependencyProperty SelectionRectangleStyleProperty = DependencyProperty.Register(nameof(SelectionRectangleStyle), typeof(Style), typeof(TimelineScale));

        /// <summary>
        /// 滚动条样式
        /// </summary>
        public Style ScrollBarStyle
        {
            get => (Style)GetValue(ScrollBarStyleProperty);
            set => SetValue(ScrollBarStyleProperty, value);
        }
        /// <summary>
        /// 滚动条样式
        /// </summary>
        public static readonly DependencyProperty ScrollBarStyleProperty = DependencyProperty.Register(nameof(ScrollBarStyle), typeof(Style), typeof(TimelineScale));

        /// <summary>
        /// 选择框区域
        /// </summary>
        public Rect SelectedArea
        {
            get => (Rect)GetValue(SelectedAreaProperty);
            internal set => SetValue(SelectedAreaPropertyKey, value);
        }
        /// <summary>
        /// 选择框区域关键字
        /// </summary>
        protected static readonly DependencyPropertyKey SelectedAreaPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedArea), typeof(Rect), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.Rect));
        /// <summary>
        /// 选择框区域属性
        /// </summary>
        public static readonly DependencyProperty SelectedAreaProperty = SelectedAreaPropertyKey.DependencyProperty;

        /// <summary>
        /// 是否实时选择
        /// </summary>
        public bool EnableRealtimeSelection
        {
            get => (bool)GetValue(EnableRealtimeSelectionProperty);
            set => SetValue(EnableRealtimeSelectionProperty, value);
        }
        /// <summary>
        /// 是否实时选择属性
        /// </summary>
        public static readonly DependencyProperty EnableRealtimeSelectionProperty = DependencyProperty.Register(nameof(EnableRealtimeSelection), typeof(bool), typeof(TimelineScale), new FrameworkPropertyMetadata(BoxValue.False));

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
        /// 界面时间区间大小
        /// </summary>
        private double ViewTime => ViewTimeMax - ViewTimeMin;

        private double ViewWidth => ActualWidth;

        /// <summary>
        /// 当前时间的显示文本
        /// </summary>
        public string CurrentTimeText => TimeToText(CurrentTime);
        /// <summary>
        /// 当前有效时间的显示文本
        /// </summary>
        public string DurationText => TimeToText(Duration);

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
        public ScrollBar VerticalBar { get; private set; }
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
        /// <summary>
        /// Timeline指针
        /// </summary>
        public TimelinePointers Pointers { get; private set; }

        #region Render
        private double _timeInfoStart;
        private double _timeInfoEnd;
        private Pen _scaleLinePen;
        private Pen _scaleLinePen2;
        private Pen _itemsLinePen;
        private Pen _itemsLinePen2;
        private Pen _scaleItemsSeparatorPen;
        private HashSet<double> _middleLinePositions = new HashSet<double>();
        private Typeface _typeface;
        private GuidelineSet _guidelineSet;
        #endregion

        #region Pointer
        private bool _dragCurrentTimePointer = false;
        #endregion

        private bool _inSelection;
        private SelectionHelper _selection;
        private Dictionary<object, object> _groupOrTrackItems = new Dictionary<object, object>();
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

        /// <summary>
        /// TimelineScale下所有的Clip的Container
        /// </summary>
        public ObservableCollection<TimelineTrackItemContainer> TrackItems { get; private set; } = new ObservableCollection<TimelineTrackItemContainer>();

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
            _selection = new SelectionHelper(this);

            Loaded += TimelineScaleLoaded;
            Unloaded += TimelineScaleUnloaded;
            LayoutUpdated += TimelineScaleLayoutUpdated;

            _scaleLinePen = new Pen(ScaleLineBrush, 1);
            _scaleLinePen2 = new Pen(ScaleLineSecondaryBrush, 1);
            _itemsLinePen = new Pen(ItemsLineBrush, 1);
            _itemsLinePen2 = new Pen(ItemsLineSecondaryBrush, 1);
            _scaleItemsSeparatorPen = new Pen(ScaleAndItemsSeparatorBrush, 1);

            AddHandler(TimelineTrackItemContainer.DragStartedEvent, new DragStartedEventHandler(OnTrackItemsDragStarted));
            AddHandler(TimelineTrackItemContainer.DragCompletedEvent, new DragCompletedEventHandler(OnTrackItemsDragCompleted));
            AddHandler(TimelineTrackItemContainer.DragDeltaEvent, new DragDeltaEventHandler(OnTrackItemsDragDelta));
            AddHandler(TimelineTrackItemContainer.SelectedEvent, new RoutedEventHandler(OnTrackItemSelected));
            AddHandler(TimelineTrackItemContainer.UnselectedEvent, new RoutedEventHandler(OnTrackItemUnselected));
            
            TrackItems.CollectionChanged += TrackItemsCollectionChanged;
            if (SelectedTrackItems is INotifyCollectionChanged c) c.CollectionChanged += OnSelectedTrackItemsChanged;

            _typeface = new Typeface("Sergio UI");
            _guidelineSet = new GuidelineSet(new[] { 0.5d }, new[] { 0.5d });
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _horizontalBar = Template.FindName(ElementHorizontalScrollBar, this) as ScrollBar;
            VerticalBar = Template.FindName(ElementVerticalScrollBar, this) as ScrollBar;
            _itemsPanel = Template.FindName(ElementTimelineScalePanel, this) as TimelineScalePanel;
            Pointers = Template.FindName(ElementTimelinePointers, this) as TimelinePointers;

            if (_horizontalBar != null) _horizontalBar.Scroll += HorizontalBarScroll;
            if (VerticalBar != null) VerticalBar.ValueChanged += VerticalBarValueChanged;
               
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
            dc.PushGuidelineSet(_guidelineSet);

            if(ScaleLineAreaBackground != null)
            {
                dc.DrawRectangle(ScaleLineAreaBackground, null, new Rect(0, -1, ActualWidth, ScaleLineAreaHeight));
            }

            if (DurationLineBrush != null && DurationLineAreaHeight > 0)
            {
                var durationPos = TimeToPos(Duration);
                if (durationPos >= 0)
                {
                    var width = durationPos;
                    if(durationPos >= ActualWidth)
                    {
                        width = ActualWidth;
                    }
                    dc.DrawRectangle(DurationLineBrush, null, new Rect(0, ScaleLineAreaHeight - DurationLineAreaHeight, width, DurationLineAreaHeight));
                }
            }

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
            //decimalPlaces = 100;
            _timeInfoStart = (double)Math.Floor(ViewTimeMin / timeInfoInterval) * timeInfoInterval;
            _timeInfoEnd = (double)Math.Ceiling(ViewTimeMax / timeInfoInterval) * timeInfoInterval;
            _timeInfoStart = Math.Round(_timeInfoStart * 10) / 10;
            _timeInfoEnd = Math.Round(_timeInfoEnd * 10) / 10;

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
              
                dc.DrawText(new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, _typeface, ScaleFontSize, fontColor, VisualTreeHelper.GetDpi(this).PixelsPerDip), new Point(posX + 5, ScaleLineAreaHeight - ScaleTextHeight));
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

            dc.DrawLine(_scaleItemsSeparatorPen, new Point(0, ScaleLineAreaHeight), new Point(ActualWidth, ScaleLineAreaHeight));

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

            if (Pointers != null) Pointers.UpdatePointersPosition();

            Loaded -= TimelineScaleLoaded;
        }

        private void TimelineScaleUnloaded(object sender, RoutedEventArgs e)
        {
            _horizontalBar.Track.Thumb.DragStarted -= HorizontalBarThumbDragStarted;
            _horizontalBar.Track.Thumb.DragCompleted -= HorizontalBarThumbDragCompleted;

            Unloaded -= TimelineScaleUnloaded;
            LayoutUpdated -= TimelineScaleLayoutUpdated;
        }

        private void TimelineScaleLayoutUpdated(object sender, EventArgs e)
        {
            UpdateVerticalScrollBarValue();
        }

        private void VerticalBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_itemsPanel != null) _itemsPanel.VerticalOffset = -e.NewValue;
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
                if(scale.Pointers != null)
                    scale.Pointers.UpdateDurationPointerPosition(scale.TimeToPos(scale.Duration),
                       scale.DurationText, true);

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
                scale.Pointers.UpdateCurrentTimePointerPosition(scale.TimeToPos(scale.CurrentTime),
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
        
        private static void OnScaleAndItemsSeparatorBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimelineScale scale)
            {
                if (scale._scaleItemsSeparatorPen == null) scale._scaleItemsSeparatorPen = new Pen(scale.ScaleAndItemsSeparatorBrush, 1);
                scale._scaleItemsSeparatorPen.Brush = scale.ItemsLineSecondaryBrush;
                scale.InvalidateVisual();
            }
        }

        private void UpdateVerticalScrollBarValue()
        {
            if (_itemsPanel == null || VerticalBar == null) return;

            VerticalBar.ViewportSize = _itemsPanel.ViewportHeight;
            var max = _itemsPanel.ExtentHeight - _itemsPanel.ViewportHeight;
            max = Math.Max(max, 0);
            VerticalBar.Maximum = max;
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

        private double _dragItemAutoPanSpeed;
        private void HandleAutoPanning(object sender, EventArgs e)
        {
            if (IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed && Mouse.Captured != null &&
                Pointers != null &&
                (_dragCurrentTimePointer || Pointers.IsCurrentTimePointerDragging || Pointers.IsDurationPointerDragging || _dragItem != null))
            {
                //Point mousePosition = Mouse.GetPosition(this);
                double edgeDistance = AutoPanEdgeDistance;
                double autoPanSpeed = Math.Min(AutoPanSpeed, AutoPanSpeed * AutoPanningTickRate);
                double x = 0;

                if(_dragItem != null)
                {
                    var pos = Mouse.GetPosition(this).X;

                    if (pos <= edgeDistance)
                    {
                        x -= _dragItemAutoPanSpeed;
                    }
                    else if (pos >= ActualWidth - edgeDistance)
                    {
                        x += _dragItemAutoPanSpeed;
                    }
                    if (x != 0)
                    {
                        IsInAutoPanning = true;
                        HorizontalDrag(-x);
                        RaiseTimeScaleChangedEvent();
                        InvalidateVisual();
                        DragTrackItems(x);
                        return;
                    }
                }
                else if (_dragCurrentTimePointer || Pointers.IsCurrentTimePointerDragging)
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
                else if (Pointers.IsDurationPointerDragging)
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
            e.Handled = true;
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
                Pointers.StartPointersDragging();

                PosToCurrentTime(pos.X);
                CaptureMouse();
            }
            else if (Mouse.Captured == null)
            {
                Focus();
                CaptureMouse();

                _selection.Start(e.GetPosition(this));
                e.Handled = true;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Override OnLostMouseCapture
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            _selection.End();
            IsInAutoPanning = false;
        }

        /// <summary>
        /// Override OnPreviewMouseDown 拖拽起始设置
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // Ctrl+鼠标左键按住 或 按住鼠标滚轮 开始进行拖拽
            if ((e.ChangedButton == MouseButton.Left && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && e.LeftButton == MouseButtonState.Pressed) ||
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
            if (IsSelecting)
            {
                _selection.End();
            }
            if (_dragCurrentTimePointer || Pointers.IsCurrentTimePointerDragging)
            {
                _dragCurrentTimePointer = false;
                Pointers.EndPointersDragging();
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
                PosToCurrentTime(pos.X);
            }
            if (IsSelecting)
            {
                _selection.Update(pos);
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

        #region Selection
        private TimelineTrackItemContainer _dragItem;
        private double _preDragItemPos;
        private Dictionary<object, TimelineTrackItemContainer> _selectedTrackItems = new Dictionary<object, TimelineTrackItemContainer>();
        private double _dragAccumulator;
        private static void OnSelectedTrackItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
           => ((TimelineScale)d).OnSelectedTrackItemsSourceChanged((System.Collections.IList)e.OldValue, (System.Collections.IList)e.NewValue);

        private void OnSelectedTrackItemsSourceChanged(IList oldValue, IList newValue)
        {
            if (oldValue is INotifyCollectionChanged oc)
            {
                oc.CollectionChanged -= OnSelectedTrackItemsChanged;
            }

            if (newValue is INotifyCollectionChanged nc)
            {
                nc.CollectionChanged += OnSelectedTrackItemsChanged;
            }

            IList selectedItems = SelectedTrackItems;

            selectedItems.Clear();
            _selectedTrackItems.Clear();
            if (newValue != null)
            {
                for (var i = 0; i < newValue.Count; i++)
                {
                    selectedItems.Add(newValue[i]);
                }
            }
        }

        private void OnSelectedTrackItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _selectedTrackItems)
                    {
                        item.Value.IsSelected = false;
                    }
                    _selectedTrackItems.Clear();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    IList oldItems = e.OldItems;
                    if (oldItems != null)
                    {
                        for (var i = 0; i < oldItems.Count; i++)
                        {
                            if (_selectedTrackItems.TryGetValue(oldItems[i], out TimelineTrackItemContainer container))
                            {
                                container.IsSelected = false;
                                _selectedTrackItems.Remove(oldItems[i]);
                            }
                        }
                    }
                    break;
            }
        }

        private void TrackItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    IList oldItems = e.OldItems;
                    if (oldItems != null)
                    {
                        for (var i = 0; i < oldItems.Count; i++)
                        {
                            if (oldItems[i] is TimelineTrackItemContainer container)
                            {
                                container.IsSelected = false;
                                var item = container.Track.ItemContainerGenerator.ContainerFromItem(container);
                                if (item != null && _selectedTrackItems.ContainsKey(item))
                                {
                                    SelectedTrackItems.Remove(item);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void OnTrackItemSelected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TimelineTrackItemContainer itemContainer && itemContainer.Owner != null && SelectedTrackItems != null)
            {
                var item = GetItemOrContainerFromTrackItemContainer(itemContainer);

                if (item != null && !SelectedTrackItems.Contains(item))
                {
                    SelectedTrackItems.Add(item);
                    _selectedTrackItems[item] = itemContainer;
                    if (!_inSelection)
                    {
                        RaiseSelectionTrackItemsChanged(new List<object> { item }, null);
                    }
                }
            }
        }

        private void OnTrackItemUnselected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TimelineTrackItemContainer itemContainer && itemContainer.Owner != null && SelectedTrackItems != null)
            {
                var item = GetItemOrContainerFromTrackItemContainer(itemContainer);

                if (item != null && SelectedTrackItems.Contains(item))
                {
                    SelectedTrackItems.Remove(item);
                    _selectedTrackItems.Remove(item); 
                    if (!_inSelection)
                    {
                        RaiseSelectionTrackItemsChanged(null, new List<object> { item });
                    }
                }
            }
        }

        private object GetItemOrContainerFromTrackItemContainer(TimelineTrackItemContainer container)
        {
            object item = container.Track.ItemContainerGenerator.ItemFromContainer(container);

            if (item == DependencyProperty.UnsetValue
                && ItemsControlFromItemContainer(container) == container.Track)
            {
                item = container;
            }

            return item;
        }

        private void OnTrackItemsDragStarted(object sender, DragStartedEventArgs e)
        {
            _dragItem = e.OriginalSource as TimelineTrackItemContainer ?? (e.OriginalSource as UIElement)?.TryFindParent<TimelineTrackItemContainer>();
            System.Collections.IList selectedItems = SelectedTrackItems;
            _preDragItemPos = Mouse.GetPosition(this).X;
            if (_dragItem != null)
            {
                if (!(Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Shift || _dragItem.IsSelected))
                {
                    selectedItems.Clear();
                }

                _dragItem.IsSelected = true;
            }

            if (selectedItems.Count > 0)
            {
                if (ItemsDragStartedCommand?.CanExecute(null) ?? false)
                {
                    ItemsDragStartedCommand.Execute(null);
                }

                double minTime = 0, maxTime = 0;
                bool init = false;
                foreach (var item in _selectedTrackItems)
                {
                    TimelineTrackItemContainer container = item.Value;
                    var r = (TranslateTransform)container.RenderTransform;

                    var currentTime = container.CurrentTime;

                    var endTime = currentTime + container.Duration;

                    if (!init)
                    {
                        init = true;
                        minTime = currentTime;
                        maxTime = endTime;
                    }

                    if (currentTime < minTime)
                    {
                        minTime = currentTime;
                    }
                    if (endTime > maxTime)
                    {
                        maxTime = endTime;
                    }
                }
                Pointers.StartDraggingPrompt(minTime, maxTime);

                e.Handled = true;
            }
        }

        private void OnTrackItemsDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (_selectedTrackItems.Count > 0)
            {
                IsBulkUpdatingItems = true;

                foreach (var item in _selectedTrackItems)
                {
                    TimelineTrackItemContainer container = item.Value;
                    var r = (TranslateTransform)container.RenderTransform;

                    var pos = TimeToPos(container.CurrentTime);
                    var result = pos + r.X;

                    container.CurrentTime = SnapTime(PosToTime(result));

                    r.X = 0;
                    _dragAccumulator = 0;
                }

                IsBulkUpdatingItems = false;
                foreach (var item in _selectedTrackItems)
                {
                    TimelineTrackItemContainer container = item.Value;
                    container.Owner.InvalidateArrange();
                }

                _dragItem = null;

                if (ItemsDragCompletedCommand?.CanExecute(null) ?? false)
                {
                    ItemsDragCompletedCommand.Execute(null);
                }

                Pointers.EndDrawDraggingPrompt();
            }
        }

        private void OnTrackItemsDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_dragItem != null && _selectedTrackItems.Count > 0)
            {
                if (e.HorizontalChange != 0 && !IsInAutoPanning)
                {
                    _dragItemAutoPanSpeed = Math.Abs(e.HorizontalChange);
                    DragTrackItems(e.HorizontalChange);
                }
            }
        }

        private void DragTrackItems(double horizontalChange)
        {
            var dragAccumulator = _dragAccumulator + horizontalChange;

            double minTime = 0, maxTime = 0;
            bool init = false;
            IsBulkUpdatingItems = true;
            foreach (var item in _selectedTrackItems)
            {
                TimelineTrackItemContainer container = item.Value;
                var r = (TranslateTransform)container.RenderTransform;

                var pos = container.Position + dragAccumulator;
                var previewCurrentTime = SnapTime(PosToTime(pos));

                if (ZoomMode == ZoomMode.Fixed && previewCurrentTime < 0)
                {
                    previewCurrentTime = MinEffectiveTime;
                }
                else
                {
                    _dragAccumulator = dragAccumulator;
                }
                container.PreviewCurrentTime = previewCurrentTime;

                r.X = container.PreviewPosition - container.Position;

                var endTime = container.PreviewCurrentTime + container.Duration;

                if (!init)
                {
                    init = true;
                    minTime = container.PreviewCurrentTime;
                    maxTime = endTime;
                }

                if (container.PreviewCurrentTime < minTime)
                {
                    minTime = container.PreviewCurrentTime;
                }
                if (endTime > maxTime)
                {
                    maxTime = endTime;
                }
            }
            IsBulkUpdatingItems = false;
            Pointers.ShowDraggingPrompt(minTime, maxTime);
        }

        internal void RemoveSelectedTrackItem(TimelineTrackItemContainer itemContainer, object item)
        {
            if (item == null) return;
            if (SelectedTrackItems.Contains(item))
            {
                SelectedTrackItems.Remove(item);
            }
        }

        internal void ApplyPreviewingSelection()
        {
            _inSelection = true;
            List<object> selected = new List<object>();
            List<object> unselected = new List<object>();
            foreach (var item in TrackItems)
            {
                var obj = GetItemOrContainerFromTrackItemContainer(item);
                
                if (item.IsPreviewingSelection==true)
                {
                    if (obj != null && !item.IsSelected)
                    {
                        selected.Add(obj);
                    }
                    item.IsSelected = true;
                }
                else if(item.IsPreviewingSelection == false)
                {
                    if (obj != null && item.IsSelected)
                    {
                        unselected.Add(obj);
                    }
                    item.IsSelected = false;
                }
                item.IsPreviewingSelection = null;
            }
            _inSelection = false;
            if(selected.Count > 0 || unselected.Count > 0)
            {
                RaiseSelectionTrackItemsChanged(selected, unselected);
            }
        }

        private void RaiseSelectionTrackItemsChanged(List<object> selected, List<object> unselected)
        {
            SelectionChangedEventArgs selectionChanged = new SelectionChangedEventArgs(SelectionTrackItemsChangedEvent, unselected, selected)
            {
                Source = this,
            };
            RaiseEvent(selectionChanged);
        }

        #endregion

        #region Group or Track Items
        private Dictionary<FrameworkElement, object> _curPrepareItem = new Dictionary<FrameworkElement, object>();

        /// <summary>
        /// Override PrepareContainerForItemOverride
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (item == null) return;

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
                    AddGroupOrTrackItems(_curPrepareItem[element], group);
                    _curPrepareItem.Remove(element);
                }
                else
                {
                    var track = element.TryFindChild<TimelineTrack>();
                    if (track != null)
                    {
                        AddGroupOrTrackItems(_curPrepareItem[element], track);
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
            if (item != null)
            {
                RemoveGroupOrTrackItems(item);
            }
        }

        internal void AddGroupOrTrackItems(object item, object container)
        {
            if (item == null || container == null || _groupOrTrackItems.ContainsKey(item)) return;

            _groupOrTrackItems[item] = container;
        }

        internal void RemoveGroupOrTrackItems(object item)
        {
            if (item == null || !_groupOrTrackItems.ContainsKey(item)) return;
            _groupOrTrackItems.Remove(item);
        }

        internal void ClearGroupOrTrackItems()
        {
            _groupOrTrackItems.Clear();
        }

        internal void SetAllGroupOrTrackItemSelected(bool value)
        {
            foreach (var item in _groupOrTrackItems)
            {
                if (item.Value is TimelineGroup group) group.IsSelected = value;
                else if (item.Value is TimelineTrack track) track.IsSelected = value;
            }
        }

        internal void SetGroupOrTrackItemSelected(object item, bool value)
        {
            if (item == null || !_groupOrTrackItems.ContainsKey(item)) return;

            var container = _groupOrTrackItems[item];
            if (container is TimelineGroup group) group.IsSelected = value;
            else if (container is TimelineTrack track) track.IsSelected = value;
        }
        #endregion

        private void VerticalDrag(double delta)
        {
            var yoffset = VerticalBar.Value - delta;
            if (yoffset <= 0)
                yoffset = 0;
            else if (yoffset > VerticalBar.Maximum)
                yoffset = VerticalBar.Maximum;
            VerticalBar.Value = yoffset;
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
             
        /// <summary>
        /// 时间转为文本字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string TimeToText(double time)
        {
            return TimeStep == TimeStepMode.Frames ? (time * FrameRate).ToString("0") : time.ToString("0.00");
        }

        #region Selection
        /// <summary>
        /// 取消所有选中项
        /// </summary>
        public void UnselectAllTrackItems()
        {
            SelectedTrackItems.Clear();
            _selectedTrackItems.Clear();
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
