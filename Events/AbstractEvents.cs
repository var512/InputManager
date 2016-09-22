using System.Threading.Tasks;
using System.Windows.Input;

namespace InputManager
{
    public abstract class AbstractEvents
    {
        public AbstractEvents()
        {
            Log.Message("Events init");
        }

        public void OnExit()
        {
            Log.Message("Events onExit()");
        }

        public abstract Task UseSkill(Key key);
    }
}
