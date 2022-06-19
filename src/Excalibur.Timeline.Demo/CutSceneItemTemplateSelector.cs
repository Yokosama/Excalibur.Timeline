using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline.Demo
{
    public class TimelineScaleItemSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null )
            {
                if(item is CutSceneTrack)
                {
                   return element.FindResource("CutSceneTrackTemplate") as DataTemplate;
                }
                else if (item is CutSceneGroup)
                {
                    return
                     element.FindResource("CutSceneGroupTemplate") as DataTemplate;
                }
            }
            return null;
        }
    }

    public class TimelineTrackItemSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item is TriggerClip)
                {
                    return element.FindResource("TriggerClipTemplate") as DataTemplate;
                }
                else if (item is DurationClip)
                {
                    return
                     element.FindResource("DurationClipTemplate") as DataTemplate;
                }
            }
            return null;
        }
    }   
    
    public class TimelineHeaderItemSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item is CutSceneTrack)
                {
                    return element.FindResource("CutSceneTrackHeaderTemplate") as DataTemplate;
                }
                else if (item is CutSceneGroup)
                {
                    return
                     element.FindResource("CutSceneGroupHeaderTemplate") as DataTemplate;
                }
            }
            return null;
        }
    }
}
