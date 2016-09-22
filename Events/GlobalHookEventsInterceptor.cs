using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Interceptor;
using ReadWriteMemory;

namespace InputManager
{
    public class GlobalHookEvents : AbstractEvents
    {
        private readonly IKeyboardMouseEvents globalHook;

        private ProcessMemory memory;
        public ProcessMemory Memory { get { return memory; } }

        public GlobalHookEvents()
        {
            // TODO
            this.memory = new ProcessMemory(@"todo", 0);
            memory.StartProcess();

            this.globalHook = Hook.GlobalEvents();

            // globalHook.MouseDownExt += OnMousePressed;
            globalHook.KeyPress += OnKeyPressed;
        }

        public async Task UseSkill(Interceptor.Keys key)
        {
            Log.Message("UseSkill: " + key);

            System.Windows.Forms.Keys keyWF = System.Windows.Forms.Keys.X;

            if (key == Interceptor.Keys.F1)
            {
                keyWF = System.Windows.Forms.Keys.Z;
            }
            else if (key == Interceptor.Keys.F2)
            {
                keyWF = System.Windows.Forms.Keys.X;
            }

            memory.SendKey(keyWF);
            Console.WriteLine(keyWF);

            await Task.Delay(1);

            memory.SendClick(ProcessMemory.WMessages.WM_LBUTTONDOWN);
            await Task.Delay(1);
            memory.SendClick(ProcessMemory.WMessages.WM_LBUTTONUP);

            /*
            input.SendKey(key, KeyState.Down);
            await Task.Delay(1);
            input.SendKey(key, KeyState.Up);

            input.SendMouseEvent(MouseState.LeftDown);
            await Task.Delay(1);
            input.SendMouseEvent(MouseState.LeftUp);
            */
        }

        public override async Task UseSpecialMacro1()
        {
            Log.Message("UseSpecialMacro1: " + Settings.SpecialKey1);
            memory.SendKey(System.Windows.Forms.Keys.F5);

            /*
            input.SendKey(Settings.SpecialKey1, KeyState.Down);
            await Task.Delay(1);
            input.SendKey(Settings.SpecialKey1, KeyState.Up);
            */
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

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            /*
            if (!Window.IsValidTarget()) return;

            var key = (Interceptor.Keys) e.KeyChar;

            Log.Message("Window.GetActiveWindowTitle(): " + Window.GetActiveWindowTitle());
            Log.Message("Window.IsValidTarget(): " + Window.IsValidTarget());

            // "±" F1
            // "Æ" F2
            if (!Settings.SpamKeys.ContainsKey(e.KeyChar)) return;
            UseSkill(Settings.SpamKeys[e.KeyChar]);

            // TODO map lib Interception.Keys just like the other implementation
            if (e.KeyChar.ToString() != "±" && e.KeyChar.ToString() != "Æ")
            {
                Console.WriteLine("TRIGGERED");
                return;
            }

            if (e.KeyChar.ToString() == "±")
            {
                UseSkill(Interceptor.Keys.F1);
            }
            else if (e.KeyChar.ToString() == "Æ")
            {
                UseSkill(Interceptor.Keys.F2);
            }

            e.Handled = true;
            */
        }

        public void OnExit()
        {
            ReadWriteMemory.ProcessMemory.CloseHandle(memory.ProcessHandle);
            memory = null;

            // globalHook.MouseDownExt -= OnMousePressed;
            globalHook.KeyPress -= OnKeyPressed;

            globalHook.Dispose();
        }
    }
}
