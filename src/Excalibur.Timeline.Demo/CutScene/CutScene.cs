using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Excalibur.Timeline.Demo
{
    public interface IDirector
    {
        double Duration { get; set; }
        double CurrentTime { get; set; }
        ObservableCollection<IDirectable> Items { get; set; }
    }

    public interface IDirectable
    {
        string Name { get; set; }
        bool IsLocked { get; set; }
        bool IsDisabled { get; set; }
        string StatusContent { get; }
        IDirectable Parent { get; set; }
    }

    public interface IClip
    {
        string Name { get; set; }
        string Dispaly { get; }
        double StartTime { get; set; }
    }

    public class DirectableObject: ObservableObject, IDirectable
    {
        protected string _name;
        public virtual string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        protected IDirectable _parent;
        public virtual IDirectable Parent
        {
            get => _parent;
            set => SetProperty(ref _parent, value);
        }

        protected bool _isLocked;
        public virtual bool IsLocked
        {
            get => Parent != null ? Parent.IsLocked || _isLocked : _isLocked;
            set 
            {
                SetProperty(ref _isLocked, value); 
                NotifyOfPropertyChange(nameof(StatusContent));
            }
        }

        protected bool _isDisabled;
        public virtual bool IsDisabled
        {
            get => Parent != null ? Parent.IsDisabled || _isDisabled : _isDisabled;
            set 
            {
                SetProperty(ref _isDisabled, value); 
                NotifyOfPropertyChange(nameof(StatusContent)); 
            }
        }

        protected bool _previewLocked;
        public virtual bool PreviewLocked
        {
            get => _previewLocked;
            set => SetProperty(ref _previewLocked, value);
        }

        protected bool _previewDisabled;
        public virtual bool PreviewDisabled
        {
            get => _previewDisabled;
            set => SetProperty(ref _previewDisabled, value);
        }

        public string StatusContent
        {
            get
            {
                var locked = _isLocked;
                var disabled = _isDisabled;
                if (Parent != null)
                {
                    locked = _isLocked && !Parent.IsLocked;
                    disabled = _isDisabled && !Parent.IsDisabled;
                }
                PreviewLocked = locked;
                PreviewDisabled = disabled;

                if (locked && disabled)
                {
                    return "Locked / Disabled";
                }
                else if (disabled)
                {
                    return "Disabled";
                }
                else if (locked)
                {
                    return "Locked";
                }
                return null;
            }
        }

    }

    public class CutSceneGroup : DirectableObject
    {
 
        public ObservableCollection<IDirectable> Items { get; set; } = new ObservableCollection<IDirectable>();

        public override bool IsLocked
        {
            get => Parent != null ? Parent.IsLocked || _isLocked : _isLocked;
            set
            {
                SetProperty(ref _isLocked, value);
                NotifyItemsPropertyChanged(nameof(IsLocked), nameof(StatusContent));
                NotifyOfPropertyChange(nameof(StatusContent)); 
            }
        }

        public override bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                SetProperty(ref _isDisabled, value);
                NotifyItemsPropertyChanged(nameof(IsDisabled), nameof(StatusContent));
                NotifyOfPropertyChange(nameof(StatusContent)); 
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private void NotifyItemsPropertyChanged(params string[] properties)
        {
            foreach (var item in Items)
            {
                if (item is ObservableObject ob)
                {
                    foreach (var p in properties)
                    {
                        ob.NotifyOfPropertyChange(p);
                    }
                }
            }
        }

    }

    public class CutSceneTrack : DirectableObject
    {
        public ObservableCollection<IClip> Items { get; set; } = new ObservableCollection<IClip>();
    }

    public class Clip : ObservableObject, IClip
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private double _startTime;
        public double StartTime
        {
            get => _startTime;
            set { SetProperty(ref _startTime, value); /*NotifyOfPropertyChange(nameof(Dispaly)); */}
        }
        
        private double _previewStartTime;
        public double PreviewStartTime
        {
            get => _previewStartTime;
            set { SetProperty(ref _previewStartTime, value); NotifyOfPropertyChange(nameof(Dispaly)); }
        }

        public virtual string Dispaly => $"{Name}({CutScene.TimeToText(PreviewStartTime)})";

        public CutScene CutScene { get; set; }
    }

    public class TriggerClip : Clip
    {
        public TriggerClip(double startTime)
        {
            StartTime = startTime;
            PreviewStartTime = startTime;
        }
    }

    public class DurationClip : Clip
    {
        private double _duration;
        public double Duration
        {
            get => _duration;
            set { SetProperty(ref _duration, value); NotifyOfPropertyChange(nameof(Dispaly)); }
        }

        public override string Dispaly => $"{Name}({CutScene.TimeToText(PreviewStartTime)},{CutScene.TimeToText(Duration)})";

        public DurationClip(double startTime, double duration)
        {
            StartTime = startTime;
            PreviewStartTime = startTime;
            Duration = duration;
        }
    }

    public class CutScene : ObservableObject, IDirector
    {
        private ZoomMode _zoomMode = ZoomMode.Fixed;
        public ZoomMode ZoomMode
        {
            get => _zoomMode;
            set => SetProperty(ref _zoomMode, value);
        }

        private TimeStepMode _stepMode = TimeStepMode.Seconds;
        public TimeStepMode StepMode
        {
            get => _stepMode;
            set => SetProperty(ref _stepMode, value);
        }

        private int _frameRate = 100;
        public int FrameRate
        {
            get => _frameRate;
            set => SetProperty(ref _frameRate, value);
        }

        //private int _viewTimeMin = 0;
        //public int ViewTimeMin
        //{
        //    get => _viewTimeMin;
        //    set => SetProperty(ref _viewTimeMin, value);
        //}

        //private int _viewTimeMax = 25;
        //public int ViewTimeMax
        //{
        //    get => _viewTimeMax;
        //    set => SetProperty(ref _viewTimeMax, value);
        //}

        public ObservableCollection<IDirectable> Items { get; set; } = new ObservableCollection<IDirectable>();
     
        private double _duration;
        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private double _currentTime;
        public double CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        public string TimeToText(double time)
        {
            return StepMode == TimeStepMode.Frames ? (time * FrameRate).ToString("0") : time.ToString("0.00");
        }
    }
}
