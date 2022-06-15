using System.Collections.ObjectModel;

namespace Excalibur.Timeline.Demo
{
    public interface IDirector
    {
        public double Duration { get; set; }
        public double CurrentTime { get; set; }
        public ObservableCollection<IDirectable> Items { get; set; }
    }

    public interface IDirectable
    {
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDisabled { get; set; }
    }

    public interface IClip
    {
        public string Name { get; set; }
        public double StartTime { get; set; }
    }

    public class CutSceneGroup : IDirectable
    {
        public string Name { get; set; }
        public ObservableCollection<IDirectable> Items { get; set; } = new ObservableCollection<IDirectable>();
        public bool IsLocked { get; set; }
        public bool IsDisabled
        {
            get; set;
        }
    }

    public class CutSceneTrack : IDirectable
    {
        public string Name { get; set; }
        public ObservableCollection<IClip> Items { get; set; } = new ObservableCollection<IClip>();
        public bool IsLocked { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class TriggerClip : IClip
    {
        public string Name { get; set; }
        public double StartTime { get; set; }

        public TriggerClip(double startTime)
        {
            StartTime = startTime;
        }
    }

    public class DurationClip : IClip
    {
        public string Name { get; set; }
        public double StartTime { get; set; }
        public double Duration { get; set; }

        public DurationClip(double startTime, double duration)
        {
            StartTime = startTime;
            Duration = duration;
        }
    }

    public class CutScene : IDirector
    {
        public ObservableCollection<IDirectable> Items { get; set; } = new ObservableCollection<IDirectable>();
        public double Duration { get; set; }
        public double CurrentTime { get; set; }
    }
}
