using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 触发Clip
    /// </summary>
    public class TimelineTriggerClip : TimelineClip
    {
        static TimelineTriggerClip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTriggerClip), new FrameworkPropertyMetadata(typeof(TimelineTriggerClip)));
            FocusableProperty.OverrideMetadata(typeof(TimelineTriggerClip), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Override DrawDraggingPrompt
        /// </summary>
        protected override void DrawDraggingPrompt(TimelinePointers pointers, DrawingContext dc, Rect area)
        {
            //base.DrawDraggingPrompt(pointers, dc, area);

            //if (container == null) return;

            //if (container.IsDragging)
            //{
            //    var curPos = container.PreviewPosition;
            //    dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(curPos, area.X), new Point(curPos, area.Y));

            //    pointers.DrawTimeText(dc, container.Scale.TimeToText(container.PreviewCurrentTime), curPos, true);
            //}
        }
    }
}
