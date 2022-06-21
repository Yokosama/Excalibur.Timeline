using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 时间轴控件
    /// </summary>
    [TemplatePart(Name = ElementTimelineScale, Type = typeof(TimelineScale))]
    [TemplatePart(Name = ElementTimelineHeader, Type = typeof(TimelineHeader))]
    public class Timeline : Control
    {
        private const string ElementTimelineScale = "PART_TimelineScale";
        private const string ElementTimelineHeader = "PART_TimelineHeader";

        /// <summary>
        /// 左边区域，Track和Group的Header区域最小宽度
        /// </summary>
        public double HeaderMinWidth
        {
            get { return (double)GetValue(HeaderMinWidthProperty); }
            set { SetValue(HeaderMinWidthProperty, value); }
        }
        /// <summary>
        /// HeaderMinWidth属性
        /// </summary>
        public static readonly DependencyProperty HeaderMinWidthProperty =
            DependencyProperty.Register(nameof(HeaderMinWidth), typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double10));

        /// <summary>
        /// 右边区域，刻度，Track，Group，Clip的区域最小宽度
        /// </summary>
        public double ScaleMinWidth
        {
            get { return (double)GetValue(ScaleMinWidthProperty); }
            set { SetValue(ScaleMinWidthProperty, value); }
        }
        /// <summary>
        /// HeaderMinWidth属性
        /// </summary>
        public static readonly DependencyProperty ScaleMinWidthProperty =
            DependencyProperty.Register(nameof(ScaleMinWidth), typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double10));
        
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
            DependencyProperty.Register(nameof(ScaleLineAreaHeight), typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double20));

        /// <summary>
        /// 左上区域，可添加控件的区别内容
        /// </summary>
        public object AdditionalContent
        {
            get { return GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        /// <summary>
        /// AdditionalContent属性
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(nameof(AdditionalContent), typeof(object), typeof(Timeline), new FrameworkPropertyMetadata(default(object)));

        /// <summary>
        /// TimelineHeader离底部的偏移量，主要是为了与TimelineScale的高度对齐
        /// </summary>
        public double TimelineHeaderBottomOffset
        {
            get { return (double)GetValue(TimelineHeaderBottomOffsetProperty); }
            set { SetValue(TimelineHeaderBottomOffsetProperty, value); }
        }
        /// <summary>
        /// TimelineHeaderBottomOffset属性
        /// </summary>
        public static readonly DependencyProperty TimelineHeaderBottomOffsetProperty =
            DependencyProperty.Register(nameof(TimelineHeaderBottomOffset), typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double0));

        /// <summary>
        /// Timeline的Items，包括Group、Track
        /// </summary>
        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        /// <summary>
        /// Items属性
        /// </summary>
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(IEnumerable), typeof(Timeline));

        private TimelineScale _scale; 
        private TimelineHeader _header;

        private bool _scaleVBValueChanging;

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            FocusableProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.True));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Timeline()
        {
            Loaded += TimelineLoaded;
        }

        private void TimelineLoaded(object sender, RoutedEventArgs e)
        {
            if (_header != null)
            {
                _header.ScrollViewer.ScrollChanged += ScrollViewerScrollChanged;
            }
            if (_scale != null)
            {
                _scale.VerticalBar.ValueChanged += VerticalBarValueChanged;
            }

            Loaded -= TimelineLoaded;
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scale = Template.FindName(ElementTimelineScale, this) as TimelineScale;
            _header = Template.FindName(ElementTimelineHeader, this) as TimelineHeader;
            if(_header != null)
            {
                _header.SelectedHeaderItemsChanged += HeaderSelectedHeaderItemsChanged;
            }
        }

        private void VerticalBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_scaleVBValueChanging) return;
            _header.ScrollViewer.ScrollToVerticalOffset(e.NewValue);
        }

        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _scaleVBValueChanging = true;
            _scale.VerticalBar.Value = e.VerticalOffset;
            _scaleVBValueChanging = false;
        }

        private void HeaderSelectedHeaderItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_scale == null) return;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if(e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {
                            _scale.SetGroupOrTrackItemSelected(item, true);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            _scale.SetGroupOrTrackItemSelected(item, false);
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    _scale.SetAllGroupOrTrackItemSelected(false);
                    break;
                default:
                    break;
            }
        }
    }
}
