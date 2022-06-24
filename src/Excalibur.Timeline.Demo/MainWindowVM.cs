using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Excalibur.Timeline.Demo
{
    public class MainWindowVM : ObservableObject
    {
        public CutScene CutScene { get; set; } = new CutScene();
        public ICommand ChangeThemeCommand { get; private set; }

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

            InitThemes();
            ChangeThemeCommand = new RelayCommand(OnChangeThemeCommandExecute);
        }

        #region Themes
        private string _activeTheme;
        private readonly List<string> _availableThemes = new List<string>();
        private readonly Dictionary<string, List<Uri>> _themes = new Dictionary<string, List<Uri>>();
        private readonly Dictionary<string, List<ResourceDictionary>> _themesResources = new Dictionary<string, List<ResourceDictionary>>();

        private void InitThemes()
        {
            _themes.Add("Light", new List<Uri>
            {
                new Uri("pack://application:,,,/Excalibur.Timeline;component/Themes/Light.xaml"),
                new Uri("pack://application:,,,/Excalibur.Timeline.Demo;component/Themes/Light.xaml"),
            });
            LoadResources("Light");
         
            _themes.Add("Dark", new List<Uri>
            {
                new Uri("pack://application:,,,/Excalibur.Timeline;component/Themes/Dark.xaml"),
                new Uri("pack://application:,,,/Excalibur.Timeline.Demo;component/Themes/Dark.xaml"),
            });
            LoadResources("Dark");
        }

        private void LoadResources(string name)
        {
            if (!_themes.ContainsKey(name)) return;
            var themeUris = _themes[name];

            var resources = FindExistingResources(themeUris);
            if (resources.Count == 0)
            {
                for (int i = 0; i < themeUris.Count; i++)
                {
                    try
                    {
                        resources.Add(new ResourceDictionary
                        {
                            Source = themeUris[i]
                        });
                    }
                    catch
                    {

                    }
                }
            }
            else if (_activeTheme == null)
            {
                _activeTheme = name;
            }
            _themesResources.Add(name, resources);
            _availableThemes.Add(name);
        }

        private List<ResourceDictionary> FindExistingResources(List<Uri> uris)
        {
            var result = new List<ResourceDictionary>();
            foreach (var d in Application.Current.Resources.MergedDictionaries)
            {
                if (d.Source != null && uris.Contains(d.Source))
                {
                    result.Add(d);
                }
            }

            return result;
        }

        public void SetTheme(string name)
        {
            if (!_themesResources.ContainsKey(name))
            {
                return;
            }

            if (_themesResources.TryGetValue(name, out var resources))
            {
                foreach (var res in resources)
                {
                    Application.Current.Resources.MergedDictionaries.Add(res);
                }

                if (_activeTheme != null)
                {
                    foreach (var res in _themesResources[_activeTheme])
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(res);
                    }
                }

                _activeTheme = name;
            }
        }

        private void OnChangeThemeCommandExecute(object obj)
        {
            if (_activeTheme != null)
            {
                var i = _availableThemes.IndexOf(_activeTheme);
                var next = i + 1 == _themes.Count ? 0 : i + 1;

                SetTheme(_availableThemes[next]);
            }
            else if (_availableThemes.Count > 0)
            {
                SetTheme(_availableThemes[0]);
            }
        }
        #endregion
    }
}
