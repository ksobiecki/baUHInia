using System.Windows.Controls;
using baUHInia.Playground.Model;

namespace baUHInia.Playground.Logic.Creators
{
    public interface ISelectionWindowCreator
    {
        void UpdateSelectionWindow(Grid displayGrid, GameObject gameObject);
    }
}