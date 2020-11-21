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

            int[,] tileGrid = new int[2,2];
            tileGrid[0, 0] = 1;
            tileGrid[1, 0] = 2;
            tileGrid[0, 1] = 3;
            tileGrid[1, 1] = 1;
            bool[,] placableGrid = new bool[2, 2];
            placableGrid[0, 0] = true;
            placableGrid[1, 0] = true;
            placableGrid[0, 1] = true;
            placableGrid[1, 1] = false;

            Dictionary<int, string> indexer = new Dictionary<int, string>();

            indexer.Add(1, "some1/path");
            indexer.Add(2, "some2/path");
            indexer.Add(3, "some3/path");

            Map map = new Map(123,123,"somename",tileGrid,placableGrid,indexer,null,5000,null);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.WriteLine(i+","+j+": " + indexer[tileGrid[i,j]] + "\n");
                }
            }

            target.Children.Add(GameMapManager.GetGameLoadGrid());
            target.Children.Clear();
            target.Children.Add(GameMapManager.GetMapLoadGrid());
        }
    }
}
