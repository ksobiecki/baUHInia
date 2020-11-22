using System.Windows;
using System.Windows.Input;

namespace baUHInia.Playground.Logic.Creators
{
    public interface IPlacerBehaviourSetter
    {
        void OnTileMouseClick(object sender, RoutedEventArgs routedEventArgs);

        void OnFieldMouseEnter(object sender, MouseEventArgs mouseEventArgs);

        void OnFieldMouseLeave(object sender, MouseEventArgs mouseEventArgs);
    }
}