using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Excalibur.Timeline.Demo
{
    public class MainWindowVM : ObservableObject
    {
        public CutScene CutScene { get; set; } = new CutScene();
        public ICommand LockCommand { get; private set; }
        public ICommand DisableCommand { get; private set; }

        public MainWindowVM()
        {
            var track1 = new CutSceneTrack
            {
                Name = "Track_1",
                Items = new ObservableCollection<IClip>
                {
                    new TriggerClip(0) { Name = "Clip1", CutScene = CutScene },
                    new TriggerClip(5.5) { Name = "Clip2", CutScene = CutScene },
                    new TriggerClip(12.5) { Name = "Clip3", CutScene = CutScene }
                }
            };
            var track2 = new CutSceneTrack
            {
                Name = "Track_2",
                Items = new ObservableCollection<IClip>
                {
                    new TriggerClip(2) { CutScene = CutScene },
                    new TriggerClip(7.5) { Name = "Clip4", CutScene = CutScene },
                    new TriggerClip(13.5) { CutScene = CutScene },
                }
            };
            var track3 = new CutSceneTrack
            {
                Name = "Track_3",
            };

            CutScene.Items.Add(track1);
            CutScene.Items.Add(track2);
            CutScene.Items.Add(track3);

            var group1 = new CutSceneGroup
            {
                Name = "Group_1",
                IsExpanded = true,
            };
            var track4 = new CutSceneTrack
            {
                Name = "Track_4",
                Parent = group1,
                Items = new ObservableCollection<IClip>
                {
                    new TriggerClip(3) { CutScene = CutScene },
                    new TriggerClip(8) { CutScene = CutScene },
                    new TriggerClip(13) { CutScene = CutScene },
                }
            };
            var track5 = new CutSceneTrack
            {
                Name = "Track_5",
                Parent = group1,
                Items = new ObservableCollection<IClip>
                {
                    new TriggerClip(2) { CutScene = CutScene },
                    new TriggerClip(6) { CutScene = CutScene },
                    new DurationClip(2, 3) { CutScene = CutScene },
                    new TriggerClip(10.5) { CutScene = CutScene },
                }
            };
            group1.Items.Add(track4);
            group1.Items.Add(track5);

            var group2 = new CutSceneGroup
            {
                Name = "Group_2",
            };
            var track6 = new CutSceneTrack
            {
                Parent = group2,
                Name = "Track_6",
                Items = new ObservableCollection<IClip>
                {
                    new DurationClip(2, 3) { Name="Duration_1", CutScene = CutScene },
                    new TriggerClip(3) { CutScene = CutScene },
                }
            };
            var track7 = new CutSceneTrack
            {
                Parent = group2,
                Name = "Track_7",
                Items = new ObservableCollection<IClip>
                {
                    new DurationClip(2, 3) { Name="Duration_2", CutScene = CutScene },
                    new TriggerClip(6) { CutScene = CutScene },
                    new DurationClip(10.5, 5) { Name="Duration_3", CutScene = CutScene },
                }
            };
            group2.Items.Add(track6);
            group2.Items.Add(track7);

            CutScene.Items.Add(group1);
            CutScene.Items.Add(group2);

            LockCommand = new RelayCommand(OnLockCommandExecute);
            DisableCommand = new RelayCommand(OnDisableCommandExecute);
        }

        private void OnLockCommandExecute(object obj)
        {
            if (obj is CutSceneTrack track)
            {
                track.IsLocked = !track.IsLocked;
            }
            else if (obj is CutSceneGroup group)
            {
                group.IsLocked = !group.IsLocked;
                //  LockChildren(group.Items, group.IsLocked);
            }
        }

        private void LockChildren(ObservableCollection<IDirectable> items, bool isLocked)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                item.IsLocked = isLocked;
                if (item is CutSceneGroup group)
                {
                    LockChildren(group.Items, group.IsLocked);
                }
            }
        }

        private void OnDisableCommandExecute(object obj)
        {
            if (obj is CutSceneTrack track)
            {
                track.IsDisabled = !track.IsDisabled;
            }
            else if (obj is CutSceneGroup group)
            {
                group.IsDisabled = !group.IsDisabled;
                //  DisableChildren(group.Items, group.IsDisabled);
            }
        }

        private void DisableChildren(ObservableCollection<IDirectable> items, bool isDisabled)
        {
            if (items == null || items.Count == 0) return;
            foreach (var item in items)
            {
                item.IsDisabled = isDisabled;
                if (item is CutSceneGroup group)
                {
                    DisableChildren(group.Items, group.IsLocked);
                }
            }
        }
    }
}
