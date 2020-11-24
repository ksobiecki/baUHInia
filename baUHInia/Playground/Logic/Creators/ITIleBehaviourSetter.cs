using System.Windows.Input;

namespace baUHInia.Playground.Logic.Creators
{
    public interface ITileBehaviourSetter
    {
        void OnTileMouseClick(object sender, MouseEventArgs mouseEventArgs);

        void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs);

        void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs);
    }
}