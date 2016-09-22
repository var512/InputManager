using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GlobalHotKey;
using ReadWriteMemory;

namespace InputManager
{
    public class GlobalHookEvents : AbstractEvents
    {
        private readonly HotKeyManager hotKeyManager;

        private ProcessMemory memory;
        public ProcessMemory Memory { get { return memory; } }

        public GlobalHookEvents()
        {
            // TODO
            this.memory = new ProcessMemory(@"todo", 0);
            memory.StartProcess();

            this.hotKeyManager = new HotKeyManager();
            hotKeyManager.KeyPressed += OnKeyPressed;

            hotKeyManager.Register(Key.F1, ModifierKeys.None);
            hotKeyManager.Register(Key.F2, ModifierKeys.None);
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

        private void OnKeyPressed(object sender, GlobalHotKey.KeyPressedEventArgs e)
        {
            if (!Window.IsValidTarget()) return;

            Log.Message("Window.GetActiveWindowTitle(): " + Window.GetActiveWindowTitle());
            Log.Message("Window.IsValidTarget(): " + Window.IsValidTarget());

            if (!Settings.SpamKeys.ContainsKey(e.HotKey.Key)) return;

            UseSkill(Settings.SpamKeys[e.HotKey.Key]);
        }

        public void OnExit()
        {
            ProcessMemory.CloseHandle(memory.ProcessHandle);
            memory = null;

            hotKeyManager.KeyPressed -= OnKeyPressed;
            hotKeyManager.Dispose();
        }
    }
}
