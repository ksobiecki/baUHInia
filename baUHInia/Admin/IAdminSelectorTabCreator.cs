using System.Windows.Controls;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public interface IAdminSelectorTabCreator
    {
        Grid GetAdminSelectorTableGrid(ITileBinder iTileBinder);
        GameObject[] GetModifiedAvailableObjects();
        int GetBudget();
    }
}