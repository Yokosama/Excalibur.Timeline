using System.Collections.ObjectModel;

namespace Excalibur.Timeline.Demo
{
    public class MainWindowVM : ObservableObject
    {
        public CutScene CutScene { get; set; } = new CutScene
        {
            Items = new ObservableCollection<IDirectable>
            {
                new CutSceneTrack
                {
                    Items = new ObservableCollection<IClip>
                    {
                        new TriggerClip(0),
                        new TriggerClip(5.5),
                        new TriggerClip(12.5)
                    }
                },
                new CutSceneTrack 
                {
                    Items = new ObservableCollection<IClip>
                    {
                        new TriggerClip(2),
                        new TriggerClip(7.5),
                        new TriggerClip(13.5)
                    }
                },
                new CutSceneTrack(),
                new CutSceneGroup
                {
                    Items = new ObservableCollection<IDirectable>()
                    {
                        new CutSceneTrack
                        {
                            Items = new ObservableCollection<IClip>
                            {
                                new TriggerClip(3),
                                new TriggerClip(8),
                                new TriggerClip(13)
                            }
                        },
                        new CutSceneTrack {
                            Items = new ObservableCollection<IClip>
                            {
                                new TriggerClip(2),
                                new TriggerClip(6),
                                new TriggerClip(10.5)
                            }
                        },
                    }
                },
                new CutSceneGroup
                {
                    Items = new ObservableCollection<IDirectable>()
                    {
                        new CutSceneTrack
                        {
                            Items = new ObservableCollection<IClip>
                            {
                                new TriggerClip(3),
                            }
                        },
                        new CutSceneTrack {
                            Items = new ObservableCollection<IClip>
                            {
                                new DurationClip(2, 3),
                                new TriggerClip(6),
                                new DurationClip(10.5, 5)
                            }
                        },
                    }
                }
            }
        };
    }
}
