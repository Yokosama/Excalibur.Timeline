using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Excalibur.Timeline
{
    /// <summary>
    /// 框选帮助类
    /// </summary>
    public sealed class SelectionHelper
    {
        private readonly TimelineScale _scale;
        private Point _startLocation;
        private SelectionType _selectionType;
        private bool _isRealtime;
        private IList<TimelineTrackItemContainer> _initialSelection = new List<TimelineTrackItemContainer>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scale"></param>
        public SelectionHelper(TimelineScale scale)
            => _scale = scale;

        /// <summary>
        /// 选择类型
        /// </summary>
        public enum SelectionType
        {
            /// <summary>
            /// 替换
            /// </summary>
            Replace,
            /// <summary>
            /// 移除
            /// </summary>
            Remove,
            /// <summary>
            /// 添加
            /// </summary>
            Append,
            /// <summary>
            /// 反转
            /// </summary>
            Invert
        }

        /// <summary>
        /// 开始框选
        /// </summary>
        /// <param name="location"></param>
        /// <param name="selectionType"></param>
        public void Start(Point location, SelectionType selectionType = default)
        {
            ModifierKeys modifiers = Keyboard.Modifiers;
            _selectionType = selectionType;
            switch (modifiers)
            {
                case ModifierKeys.Alt:
                    _selectionType = SelectionType.Remove;
                    break;
                case ModifierKeys.Control:
                    _selectionType = SelectionType.Invert;
                    break;
                case ModifierKeys.Shift:
                    _selectionType = SelectionType.Append;
                    break;
                case ModifierKeys.None:
                case ModifierKeys.Windows:
                default:
                    _selectionType = SelectionType.Replace;
                    break;
            }
           
            _initialSelection = GetSelectedContainers();

            _isRealtime = _scale.EnableRealtimeSelection;
            _startLocation = location;

            _scale.SelectedArea = new Rect();
            _scale.IsSelecting = true;
        }

        /// <summary>
        /// 更新框选大小和内容
        /// </summary>
        /// <param name="endLocation"></param>
        public void Update(Point endLocation)
        {
            double left = endLocation.X < _startLocation.X ? endLocation.X : _startLocation.X;
            double top = endLocation.Y < _startLocation.Y ? endLocation.Y : _startLocation.Y;
            double width = Math.Abs(endLocation.X - _startLocation.X);
            double height = Math.Abs(endLocation.Y - _startLocation.Y);

            _scale.SelectedArea = new Rect(left, top, width, height);

            if (_isRealtime)
            {
                PreviewSelection(_scale.SelectedArea);
            }
        }

        /// <summary>
        /// 结束框选
        /// </summary>
        public void End()
        {
            if (_scale.IsSelecting)
            {
                _scale.IsSelecting = false;

                PreviewSelection(_scale.SelectedArea);
                _scale.ApplyPreviewingSelection();
                _initialSelection.Clear();
            }
        }

        private void PreviewSelection(Rect area)
        {
            switch (_selectionType)
            {
                case SelectionType.Replace:
                    PreviewSelectArea(area);
                    break;

                case SelectionType.Remove:
                    PreviewSelectContainers(_initialSelection);

                    PreviewUnselectArea(area);
                    break;

                case SelectionType.Append:
                    PreviewUnselectAll();
                    PreviewSelectContainers(_initialSelection);

                    PreviewSelectArea(area, true);
                    break;

                case SelectionType.Invert:
                    PreviewUnselectAll();
                    PreviewSelectContainers(_initialSelection);

                    PreviewInvertSelection(area);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(SelectionType));
            }
        }

        private void PreviewUnselectAll()
        {
            var items = _scale.TrackItems;
            for (var i = 0; i < items.Count; i++)
            {
                items[i].IsPreviewingSelection = false;
            }
        }

        private void PreviewSelectArea(Rect area, bool append = false, bool fit = false)
        {
            if (!append)
            {
                PreviewUnselectAll();
            }

            for (var i = 0; i < _scale.TrackItems.Count; i++)
            {
                var container = _scale.TrackItems[i];
                if (container.IsSelectableInArea(area, fit))
                {
                    container.IsPreviewingSelection = true;
                }
            }
        }

        private void PreviewUnselectArea(Rect area, bool fit = false)
        {
            for (var i = 0; i < _scale.TrackItems.Count; i++)
            {
                var container = _scale.TrackItems[i];
                if (container.IsSelectableInArea(area, fit))
                {
                    container.IsPreviewingSelection = false;
                }
            }
        }

        private void PreviewSelectContainers(IList<TimelineTrackItemContainer> containers)
        {
            for (var i = 0; i < containers.Count; i++)
            {
                containers[i].IsPreviewingSelection = true;
            }
        }

        private void PreviewInvertSelection(Rect area, bool fit = false)
        {
            for (var i = 0; i < _scale.TrackItems.Count; i++)
            {
                var container = _scale.TrackItems[i];
                if (container.IsSelectableInArea(area, fit))
                {
                    container.IsPreviewingSelection = !container.IsPreviewingSelection;
                }
            }
        }

        private IList<TimelineTrackItemContainer> GetSelectedContainers()
        {
            var result = new List<TimelineTrackItemContainer>(32);
            for (var i = 0; i < _scale.TrackItems.Count; i++)
            {
                var container = _scale.TrackItems[i];
                result.Add(container);
            }
            return result;
        }
    }
}
