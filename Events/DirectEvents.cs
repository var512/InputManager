using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReadWriteMemory;

namespace InputManager
{
    public class DirectInput
    {

    }

    public class DirectEvents : AbstractEvents
    {
        private readonly DirectInput input;

        private ProcessMemory memory;
        public ProcessMemory Memory { get { return memory; } }

        public DirectEvents()
        {
            // TODO
            this.memory = new ProcessMemory(@"todo", 0);
            memory.StartProcess();

            this.input = new DirectInput();

            // var boxSpecialKey = new System.Windows.Forms.TextBox();
        }

        public async Task UseSkill(Interceptor.Keys key)
        {
            Log.Message("UseSkill: " + key);

            System.Windows.Forms.Keys eventKey = (System.Windows.Forms.Keys) key;

            memory.SendKey(eventKey);
            await Task.Delay(1);
            memory.SendKey(System.Windows.Forms.Keys.LButton);
        }

        public override async Task UseSpecialMacro1()
        {
            Log.Message("UseSpecialMacro1: " + Settings.SpecialKey);

            memory.SendKey(System.Windows.Forms.Keys.F5);
        }

        public override async Task UseSpecialMacro2()
        {
            Log.Message("UseSpecialMacro2: " + Settings.SpecialKey2);

            memory.SendKey(System.Windows.Forms.Keys.F6);

            /*
            input.SendKey(Settings.SpecialKey2, KeyState.Down);
            await Task.Delay(1);
            input.SendKey(Settings.SpecialKey2, KeyState.Up);
            */
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (!Window.IsValidTarget()) return;

            Interceptor.Keys keyPress = (Interceptor.Keys) e.Key;

            Log.Message("Window.GetActiveWindowTitle(): " + Window.GetActiveWindowTitle());
            Log.Message("Window.IsValidTarget(): " + Window.IsValidTarget());

            if (!Settings.SpamKeys.ContainsKey(keyPress)) return;

            if (e.State == KeyState.Down)
            {
                UseSkill(Settings.SpamKeys[keyPress]);
            }

            if (e.State == KeyState.Up) {}

            e.Handled = true;
        }

        public void OnExit()
        {
            ReadWriteMemory.ProcessMemory.CloseHandle(memory.ProcessHandle);
            memory = null;
        }
    }
}
