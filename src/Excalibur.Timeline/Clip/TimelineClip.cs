using Excalibur.Timeline.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Excalibur.Timeline
{
    /// <summary>
    /// TimlineClip
    /// </summary>
    public abstract class TimelineClip : ContentControl,IDisposable
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        /// <summary>
        /// 持续时间属性
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(TimelineClip), new FrameworkPropertyMetadata(BoxValue.False));

        /// <summary>
        /// 预览框选
        /// </summary>
        public bool? IsPreviewingSelection
        {
            get => (bool?)GetValue(IsPreviewingSelectionProperty);
            set => SetValue(IsPreviewingSelectionProperty, value);
        }
        /// <summary>
        /// 预览框选属性
        /// </summary>
        public static readonly DependencyProperty IsPreviewingSelectionProperty =
            DependencyProperty.Register(nameof(IsPreviewingSelection), typeof(bool?), typeof(TimelineClip), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 父容器
        /// </summary>
        protected TimelineTrackItemContainer container;

        /// <summary>
        /// Override OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = this.TryFindParent<TimelineTrackItemContainer>();
            if(container != null)
            {
                container.Selected += ContainerSelected;
                container.Unselected += ContainerUnselected;
                container.IsPreviewingSelectionChanged += ContainerIsPreviewingSelectionChanged;
            }
        }

        private void ContainerIsPreviewingSelectionChanged(object sender, IsPreviewingSelectionChangedEventArgs e)
        {
            IsPreviewingSelection = e.Value; 
        }

        private void ContainerSelected(object sender, RoutedEventArgs e)
        {
            IsSelected = true;
        }
        
        private void ContainerUnselected(object sender, RoutedEventArgs e)
        {
            IsSelected = false;
        }

        /// <summary>
        /// Override Dispose
        /// </summary>
        public void Dispose()
        {
            if (container != null)
            {
                container.Selected -= ContainerSelected;
                container.Unselected -= ContainerUnselected;
                container.IsPreviewingSelectionChanged -= ContainerIsPreviewingSelectionChanged;
            }
        }
    }
}
