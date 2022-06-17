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
    }
}
