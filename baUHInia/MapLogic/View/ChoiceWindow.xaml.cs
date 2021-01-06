using baUHInia.MapLogic.Manager;
using baUHInia.MapLogic.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace baUHInia.MapLogic.View
{
    public partial class ChoiceWindow : Window
    {
        public ChoiceWindow()
        {
            // This whole thing is for debug purposes only.
            InitializeComponent();
            IGameMapManager GameMapManager = new GameMapManager();
            Grid target = this.FindName("TargetGrid") as Grid;
            target.Children.Add(GameMapManager.GetGameSaveGrid());
        }
    }
}
