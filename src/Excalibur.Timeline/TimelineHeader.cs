using Excalibur.Timeline.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excalibur.Timeline
{
    /// <summary>
    /// Group,Track的Header集合
    /// </summary>
    [TemplatePart(Name = ElementScrollViewer, Type = typeof(ScrollViewer))]
    public class TimelineHeader : ItemsControl
    {
        private const string ElementScrollViewer = "PART_ScrollViewer";

        /// <summary>
        /// 选中的GroupHeader、TrackHeader集合
        /// </summary>
        public System.Collections.IList SelectedHeaderItems
        {
            get { return (System.Collections.IList)GetValue(SelectedHeaderItemsProperty); }
            set { SetValue(SelectedHeaderItemsProperty, value); }
        }
        /// <summary>
        /// 选中的GroupHeader、TrackHeader集合
        /// </summary>
        public static readonly DependencyProperty SelectedHeaderItemsProperty =
            DependencyProperty.Register(nameof(SelectedHeaderItems), typeof(System.Collections.IList), typeof(TimelineScale), new FrameworkPropertyMetadata(new ObservableCollection<object>(), OnSelectedHeaderItemsSourceChanged));

        /// <summary>
        /// 选中的Group/Track改变事件
        /// </summary>
        public event SelectionChangedEventHandler SelectionHeaderItemsChanged
        {
            add { AddHandler(SelectionHeaderItemsChangedEvent, value); }
            remove { RemoveHandler(SelectionHeaderItemsChangedEvent, value); }
        }
        /// <summary>
        /// 选中的Clip改变事件
        /// </summary>
        public static readonly RoutedEvent SelectionHeaderItemsChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectionHeaderItemsChanged), RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(TimelineHeader));

        /// <summary>
        /// 选中的HeaderItems集合修改事件
        /// </summary>
        internal event NotifyCollectionChangedEventHandler SelectedHeaderItemsChanged;

        private Dictionary<object, object> _selectedHeaderItems = new Dictionary<object, object>();

        /// <summary>
        /// ScrollViewer
        /// </summary>
        public ScrollViewer ScrollViewer { get; private set; }

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
            ScrollViewer = Template.FindName(ElementScrollViewer, this) as ScrollViewer;

            AddHandler(TimelineGroupHeader.SelectedChangedEvent, new SelectedChangedEventHandler(OnGroupHeaderSelectedChanged));
            AddHandler(TimelineTrackHeader.SelectedChangedEvent, new SelectedChangedEventHandler(OnTrackHeaderSelectedChanged));
        }

        private void OnGroupHeaderSelectedChanged(object sender, SelectedChangedEventArgs e)
        {
            if(e.OriginalSource is TimelineGroupHeader groupHeader)
            {
                var item = GetTimelineGroupHeaderItemOrContainer(groupHeader.ParentHeader, groupHeader);
                if (item != null)
                {
                    if (e.Value && !SelectedHeaderItems.Contains(item))
                    {
                        SelectedHeaderItems.Add(item);
                        _selectedHeaderItems[item] = groupHeader;
                        SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                    }
                    else if (SelectedHeaderItems.Contains(item))
                    {
                        SelectedHeaderItems.Remove(item);
                        _selectedHeaderItems.Remove(item);
                        SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                    }
                }
            }
        }

        private object GetTimelineGroupHeaderItemOrContainer(ItemsControl parent, TimelineGroupHeader groupHeader)
        {
            object item = ItemContainerGenerator.ItemFromContainer(groupHeader.ContentPresenter);

            if (item == DependencyProperty.UnsetValue
                && ItemsControlFromItemContainer(groupHeader.ContentPresenter) == parent)
            {
                item = groupHeader;
            }

            return item;
        }

        private void OnTrackHeaderSelectedChanged(object sender, SelectedChangedEventArgs e)
        {
            if (e.OriginalSource is TimelineTrackHeader trackHeader)
            {
                var item = GetTimelineTrackHeaderItemOrContainer(trackHeader.ParentHeader, trackHeader);
                if (item != null)
                {
                    if (e.Value && !SelectedHeaderItems.Contains(item))
                    {
                        SelectedHeaderItems.Add(item);
                        _selectedHeaderItems[item] = trackHeader;
                        SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                        RaiseSelectionTrackItemsChanged(new List<object> { item }, new List<object>());
                    }
                    else if(SelectedHeaderItems.Contains(item))
                    {
                        SelectedHeaderItems.Remove(item);
                        _selectedHeaderItems.Remove(item);
                        SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
                        RaiseSelectionTrackItemsChanged(new List<object>(), new List<object> { item });
                    }
                }
            }
        }

        private object GetTimelineTrackHeaderItemOrContainer(ItemsControl parent, TimelineTrackHeader trackHeader)
        {
            object item = parent.ItemContainerGenerator.ItemFromContainer(trackHeader.ContentPresenter);

            if (item == DependencyProperty.UnsetValue
                && ItemsControlFromItemContainer(trackHeader.ContentPresenter) == parent)
            {
                item = trackHeader;
            }

            return item;
        }

        private static void OnSelectedHeaderItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((TimelineHeader)d).OnSelectedHeaderItemsSourceChanged((System.Collections.IList)e.OldValue, (System.Collections.IList)e.NewValue);

        private void OnSelectedHeaderItemsSourceChanged(IList oldValue, IList newValue)
        {
            if (oldValue is INotifyCollectionChanged oc)
            {
                oc.CollectionChanged -= OnSelectedHeaderItemsChanged;
            }

            if (newValue is INotifyCollectionChanged nc)
            {
                nc.CollectionChanged += OnSelectedHeaderItemsChanged;
            }

            IList selectedItems = SelectedHeaderItems;
            _selectedHeaderItems.Clear();
            selectedItems.Clear();
            if (newValue != null)
            {
                for (var i = 0; i < newValue.Count; i++)
                {
                    selectedItems.Add(newValue[i]);
                }
            }
        }

        private void OnSelectedHeaderItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in _selectedHeaderItems)
                    {
                        UnselectHeaderItem(item.Value);
                    }
                    _selectedHeaderItems.Clear();
                    SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    IList oldItems = e.OldItems;
                    if (oldItems != null)
                    {
                        List<object> items = new List<object>();
                        for (var i = 0; i < oldItems.Count; i++)
                        {
                            if (_selectedHeaderItems.TryGetValue(oldItems[i], out object item))
                            {
                                items.Add(item);
                                UnselectHeaderItem(item);
                                _selectedHeaderItems.Remove(oldItems[i]);
                            }
                        }
                        SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
                    }
                    break;
            }
        }

        private void UnselectHeaderItem(object item)
        {
            if (item is TimelineTrackHeader trackHeader) trackHeader.IsSelected = false;
            else if (item is TimelineGroupHeader groupHeader) groupHeader.IsSelected = false;
        }

        /// <summary>
        /// 清空所有选中的Header
        /// </summary>
        public void UnselectAllHeaderItems()
        {
            if (SelectedHeaderItems.Count <= 0) return;

            SelectedHeaderItems.Clear();
            List<object> unselected = new List<object>();
            foreach (var item in _selectedHeaderItems)
            {
                unselected.Add(item.Key);
                UnselectHeaderItem(item.Value);
            }
            _selectedHeaderItems.Clear();
            SelectedHeaderItemsChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            if(unselected.Count > 0)
            {
                RaiseSelectionTrackItemsChanged(new List<object>(), unselected);
            }
        }

        private void RaiseSelectionTrackItemsChanged(List<object> selected, List<object> unselected)
        {
            SelectionChangedEventArgs selectionChanged = new SelectionChangedEventArgs(SelectionHeaderItemsChangedEvent, unselected, selected)
            {
                Source = this,
            };
            RaiseEvent(selectionChanged);
        }

        /// <summary>
        /// Override OnPreviewMouseLeftButtonDown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            // 为点击在GroupHeader或TrackHeader上，取消选中的HeaderItem
            if(SelectedHeaderItems.Count > 0 && e.OriginalSource is DependencyObject source)
            {
                if(source.TryFindParent<TimelineTrackHeader>() != null ||
                   source.TryFindParent<TimelineGroupHeader>() != null)
                {
                    return;
                }
                UnselectAllHeaderItems();
            }
        }
    }
}
