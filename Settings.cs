using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using IniParser;
using IniParser.Model;

namespace InputManager
{
    public class Settings
    {
        private const string ConfigFile = "InputManager.ini";
        public static string ProcessName { get; set; }

        public static bool ShowLogs { get; set; }
        public static int TimerDelay { get; set; }

        private static readonly List<string> _targetWindows = new List<string>();
        private static readonly Dictionary<Key, Key> _spamKeys = new Dictionary<Key, Key>();
        private static readonly Dictionary<string, Key> _appKeys = new Dictionary<string, Key>();

        public static List<string> TargetWindows
        {
            get { return _targetWindows; }
        }

        public static Dictionary<Key, Key> SpamKeys
        {
            get { return _spamKeys; }
        }

        public static Dictionary<string, Key> AppKeys
        {
            get { return _appKeys; }
        }

        public Settings()
        {
            try
            {
                if (!File.Exists(ConfigFile))
                {
                    throw new Exception("Config file [" + ConfigFile + "] not found");
                }

                FileIniDataParser fileIniData = new FileIniDataParser();
                fileIniData.Parser.Configuration.CommentString = "#";

                IniData data = fileIniData.ReadFile(ConfigFile);

                ProcessName = data["Options"]["processName"];
                ShowLogs = bool.Parse(data["Options"]["showLogs"]);
                TimerDelay = int.Parse(data["Options"]["timerDelay"]);

                TimerDelay = Math.Max(TimerDelay, 1);

                foreach (var window in data["TargetWindows"])
                {
                    string title = window.KeyName;
                    bool isEnabled = bool.Parse(window.Value);

                    if (isEnabled)
                    {
                        TargetWindows.Add(title);
                    }
                }

                foreach (var spamKey in data["SpamKeys"])
                {
                    Key input = (Key) Enum.Parse(typeof(Key), spamKey.KeyName);
                    Key output = (Key) Enum.Parse(typeof(Key), spamKey.Value);

                    SpamKeys.Add(input, output);
                }

                foreach (var appKey in data["AppKeys"])
                {
                    string input = appKey.KeyName;
                    Key output = (Key) Enum.Parse(typeof(Key), appKey.Value);

                    AppKeys.Add(input, output);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
