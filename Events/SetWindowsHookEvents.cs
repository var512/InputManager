using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FMUtils.KeyboardHook;
using ReadWriteMemory;

namespace InputManager
{
    public class SetWindowsHookEvents : AbstractEvents
    {
        private Hook keyboardHook;

        private ProcessMemory memory;

        public ProcessMemory Memory
        {
            get { return memory; }
        }

        public Stopwatch SkillTimer = new Stopwatch();

        public SetWindowsHookEvents()
        {
            StartHook();
        }

        private void StartHook()
        {
            Log.Message("StartHook [" + Settings.ProcessName + "]");

            this.memory = new ProcessMemory(Settings.ProcessName, 0);
            memory.StartProcess();

            if (memory.CheckProcess() == false)
            {
                Log.Message("Process not found: " + Settings.ProcessName);
            }

            this.keyboardHook = new Hook("InputManager");
            keyboardHook.KeyDownEvent += OnKeyPressed;
        }

        public override async Task UseSkill(Key key)
        {
            Log.Message("UseSkill: " + key);

            memory.SendKey(key);

            await Task.Delay(1);

            memory.SendClick(ProcessMemory.WMessages.WM_LBUTTONDOWN);
            await Task.Delay(1);
            memory.SendClick(ProcessMemory.WMessages.WM_LBUTTONUP);
        }

        private void OnKeyPressed(KeyboardHookEventArgs e)
        {
            Random rnd = new Random();

            if (SkillTimer.ElapsedMilliseconds <= rnd.Next(Settings.TimerDelay, Settings.TimerDelay + 149))
            {
                Log.Message("[" + SkillTimer.ElapsedMilliseconds + "] SkillTimer.Start()");

                SkillTimer.Start();
                return;
            }

            Log.Message("[" + SkillTimer.ElapsedMilliseconds + "] Skill action");

            SkillTimer.Reset();

            Log.Message("Window.GetActiveWindowTitle(): [" + Window.GetActiveWindowTitle() + "]");
            Log.Message("Window.IsValidTarget(): [" + Window.IsValidTarget() + "]");

            var key = Util.FormsKeysToInputKey(e.Key);
            Key startHookKey;
            bool hasStartHookKey = Settings.AppKeys.TryGetValue("startHook", out startHookKey);

            if (hasStartHookKey && key == startHookKey)
            {
                StartHook();
            }

            if (!Window.IsValidTarget()) return;

            if (!Settings.SpamKeys.ContainsKey(key)) return;

            UseSkill(Settings.SpamKeys[key]);
        }

        public void OnExit()
        {
            ProcessMemory.CloseHandle(memory.ProcessHandle);
            memory = null;
        }
    }
}
