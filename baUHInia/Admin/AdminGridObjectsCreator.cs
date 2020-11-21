using System;
using System.Windows.Controls;
using baUHInia.Playground.Model;

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
            int cols = 8;
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
            foreach (var gameObject in GameObjectsList)
            {
                Button button = gameObject.ClickableGameObject;
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