using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 时间轴控件
    /// </summary>
    [TemplatePart(Name = ElementTimelineScale, Type = typeof(TimelineScale))]
    public class Timeline : Control
    {
        private const string ElementTimelineScale = "PART_TineimeScale";

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
            DependencyProperty.Register("HeaderMinWidth", typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double10));

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
            DependencyProperty.Register("ScaleMinWidth", typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double10));
        
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
            DependencyProperty.Register("ScaleLineAreaHeight", typeof(double), typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.Double20));

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
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(Timeline), new FrameworkPropertyMetadata(default(object)));

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
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(Timeline));

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            FocusableProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(BoxValue.True));
        }
    }
}
