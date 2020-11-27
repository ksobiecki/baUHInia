﻿using System.Collections.Generic;
using System.Windows.Controls;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public interface IAdminSelectorTabCreator
    {
        Grid GetAdminSelectorTableGrid();
        List<GameObject> GetModifiedAvailableObjects();
        int GetBudget();
    }
}