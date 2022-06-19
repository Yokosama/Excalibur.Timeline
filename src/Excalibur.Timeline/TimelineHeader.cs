using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// Group,Track的Header集合
    /// </summary>
    [TemplatePart(Name = ElementScrollViewer, Type = typeof(ScrollViewer))]
    public class TimelineHeader : ItemsControl
    {
        private const string ElementScrollViewer = "PART_ScrollViewer";

        private ScrollViewer _scrollViewer;

        static TimelineHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineHeader), new FrameworkPropertyMetadata(typeof(TimelineHeader)));
            FocusableProperty.OverrideMetadata(typeof(TimelineHeader), new FrameworkPropertyMetadata(BoxValue.True));
        }

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = Template.FindName(ElementScrollViewer, this) as ScrollViewer;
        }
    }
}
