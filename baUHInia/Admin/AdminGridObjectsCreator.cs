using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public class AdminGridObjectsCreator
    {
        public List<AdminInGridClickableObject> GameObjectsList { get; }
        private Grid ObjectsGrid { get; }
        private Boolean isAvailable;

        public AdminGridObjectsCreator(GameObject[] gameObjects, bool isAvailable, Grid objectsGrid,
            IAdminOnClickObject adminOnClickObject)
        {
            this.isAvailable = isAvailable;
            ObjectsGrid = objectsGrid;
            GameObjectsList = new List<AdminInGridClickableObject>(gameObjects.Length);
            for (var i = 0; i < gameObjects.Length; i++)
            {
                GameObjectsList.Insert(i,
                    new AdminInGridClickableObject(gameObjects[i], isAvailable, adminOnClickObject));
            }
        }

        public void CreateGrid()
        {
            ObjectsGrid.Children.Clear();
            int cols = 12;

            int row = 0;
            int col = 0;
            for (var i = 0; i < GameObjectsList.Count; i++)
            {
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

        public void CreateGridWithCategoryBreaks(List<int> categoryBreaksLineIndex)
        {
            ObjectsGrid.Children.Clear();
            int cols = 12;

            int row = 0;
            int col = 0;
            for (var i = 0; i < GameObjectsList.Count; i++)
            {
                if (categoryBreaksLineIndex.Contains(i))
                {
                    ObjectsGrid.RowDefinitions.Add(new RowDefinition());
                    row++;
                    col = 0;
                }

                if (isAvailable == GameObjectsList[i].IsAvailable)
                {
                    Button button = GameObjectsList[i].ClickableGameObject;
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    ObjectsGrid.Children.Add(button);
                    col++;
                }

                if (col == cols)
                {
                    col = 0;
                    row++;
                }
            }
        }

        public void InitializeGridDefinitions()
        {
            int cols = 12;
            int rows = GameObjectsList.Count / cols + 1;
            for (var i = 0; i < cols; i++)
            {
                ObjectsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (var i = 0; i < rows; i++)
            {
                ObjectsGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        public void ChangeAvailability(AdminInGridClickableObject gameObject)
        {
            GameObjectsList.Find(
                    x => x.GameObject.TileObject.Name == gameObject.GameObject.TileObject.Name
                    ).IsAvailable = !gameObject.IsAvailable;
        }

        public void AddObject(AdminInGridClickableObject gameObject)
        {
            GameObjectsList.Add(gameObject);
        }

        public void RemoveObject(AdminInGridClickableObject gameObject)
        {
            GameObjectsList.Remove(gameObject);
        }

        public GameObject[] GetGameObjects() => GameObjectsList.Select(c => c.GameObject).ToArray();

    }
}