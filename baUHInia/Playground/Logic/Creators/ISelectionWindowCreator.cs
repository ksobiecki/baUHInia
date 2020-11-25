using System.Windows.Controls;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Playground.Logic.Creators
{
    public interface ISelectionWindowCreator
    {
        void UpdateSelectionWindow(Grid displayGrid, GameObject gameObject);
    }
}