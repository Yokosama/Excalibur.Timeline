﻿using Excalibur.Timeline.Helper;
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
    public abstract class TimelineClip : ContentControl
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
            }
            container.Scale.Pointers.Rendering += PointersRendering;
        }

        private void PointersRendering(object sender, TimelinePointersRenderingEventArgs e)
        {
            if (e.Context == null) return;

            DrawDraggingPrompt(e.Pointers, e.Context, e.Area);
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
        /// 绘制拖拽提示
        /// </summary>
        /// <param name="pointers">TimelinePointers</param>
        /// <param name="dc">TimelinePointers的绘制上下文</param>
        /// <param name="area">绘制的区域</param>
        protected virtual void DrawDraggingPrompt(TimelinePointers pointers, DrawingContext dc, Rect area)
        {
        }
    }
}