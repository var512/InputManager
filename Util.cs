using System.Windows.Forms;
using System.Windows.Input;

namespace InputManager
{
    public class Util
    {
        // [System.Windows.Input.Key] to [System.Windows.Forms.Keys]
        public static Keys InputKeyToFormsKeys(Key k)
        {
            return (Keys) KeyInterop.VirtualKeyFromKey(k);
        }

        // [System.Windows.Forms.Keys] to [System.Windows.Input.Key]
        public static Key FormsKeysToInputKey(Keys k)
        {
            return KeyInterop.KeyFromVirtualKey((int) k);
        }
    }
}
