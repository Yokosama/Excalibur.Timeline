using System.Windows;
using System.Windows.Controls;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 轨道画布
    /// </summary>
    public class TimelineTrackCanvas : Panel
    {
        static TimelineTrackCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineTrackCanvas), new FrameworkPropertyMetadata(typeof(TimelineTrackCanvas)));
        }

        /// <summary>
        /// Overrid ArrangeOverride
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i] as TimelineTrackItemContainer;
                child.Arrange(new Rect(new Point(child.Position, 0), child.DesiredSize));
            }
            return finalSize;
        }

        /// <summary>
        /// Override MeasureOverride
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                InternalChildren[i].Measure(size);
            }

            return default;
        }
    }
}
