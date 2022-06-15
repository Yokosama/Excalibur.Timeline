using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 触发Clip
    /// </summary>
    public class TimelineTriggerClip : Control
    {
        static TimelineTriggerClip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTriggerClip), new FrameworkPropertyMetadata(typeof(TimelineTriggerClip)));
            FocusableProperty.OverrideMetadata(typeof(TimelineTriggerClip), new FrameworkPropertyMetadata(true));
        }
    }
}
