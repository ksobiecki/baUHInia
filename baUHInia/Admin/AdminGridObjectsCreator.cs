using System;
using System.Collections.Generic;
using System.Windows.Controls;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public class AdminGridObjectsCreator
    {
        private AdminInGridClickableObject[] GameObjectsList { get; }
        private Grid ObjectsGrid { get; }
        private Boolean isAvailable;

        public AdminGridObjectsCreator(GameObject[] gameObjects, bool isAvailable, Grid objectsGrid, IAdminOnClickObject adminOnClickObject)
        {
            this.isAvailable = isAvailable;
            ObjectsGrid = objectsGrid;
            GameObjectsList = new AdminInGridClickableObject[gameObjects.Length];
            for (var i = 0; i < gameObjects.Length; i++)
            {
                GameObjectsList[i] = new AdminInGridClickableObject(gameObjects[i], isAvailable, adminOnClickObject);
            }
            
        }

        public void CreateGrid()
        {
            int cols = 12;
            int rows = GameObjectsList.Length / cols + 1;
            for (var i = 0; i < cols; i++)
            {
                ObjectsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var i = 0; i < rows; i++)
            {
                ObjectsGrid.RowDefinitions.Add(new RowDefinition());
            }

            int row = 0;
            int col = 0;
            for (var i = 0; i < GameObjectsList.Length; i++)
            {
                if (i == 13)
                {
                    ObjectsGrid.RowDefinitions.Add(new RowDefinition());
                    row++;
                    col = 0;
                }
                Button button = GameObjectsList[i].ClickableGameObject;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                ObjectsGrid.Children.Add(button);
                col++;
                if (col == cols)
                {
                    col = 0;
                    row++;
                }
            }
        }
        public void CreateGrid(List<int> categoryBreaksLineIndex)
        {
            int cols = 12;
            int rows = GameObjectsList.Length / cols + 1;
            for (var i = 0; i < cols; i++)
            {
                ObjectsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var i = 0; i < rows; i++)
            {
                ObjectsGrid.RowDefinitions.Add(new RowDefinition());
            }

            int row = 0;
            int col = 0;
            for (var i = 0; i < GameObjectsList.Length; i++)
            {
                if (categoryBreaksLineIndex.Contains(i))
                {
                    ObjectsGrid.RowDefinitions.Add(new RowDefinition());
                    row++;
                    col = 0;
                }
                Button button = GameObjectsList[i].ClickableGameObject;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                ObjectsGrid.Children.Add(button);
                col++;
                if (col == cols)
                {
                    col = 0;
                    row++;
                }
            }
        }
    }
    
}