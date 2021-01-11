using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using baUHInia.Playground.Model.Utility;
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
            if (isAvailable)
            {
                Console.WriteLine("");
                foreach (var VARIABLE in gameObjects)
                {
                    Console.WriteLine(
                        $"{VARIABLE.TileObject.Name}, price {VARIABLE.Price}, val {VARIABLE.ChangeValue}");
                }
            }
            this.isAvailable = isAvailable;
            ObjectsGrid = objectsGrid;
            GameObjectsList = new List<AdminInGridClickableObject>(gameObjects.Length);
            var copy = gameObjects.Select(gameObject => new GameObject(gameObject.TileObject, gameObject.ChangeValue, gameObject.Price)).ToList();
            foreach (var gameObject in copy)
            {
                GameObjectsList.Add(new AdminInGridClickableObject(gameObject, isAvailable, adminOnClickObject));
            }
        }

        public void CreateGrid()
        {
            ObjectsGrid.Children.Clear();

            int cols = 12;

            int row = 0;
            int col = 0;
            foreach (var gameObject in GameObjectsList)
            {
                var subGrid = gameObject.ClickableGameObject;
                Grid.SetRow(subGrid, row);
                Grid.SetColumn(subGrid, col);
                ObjectsGrid.Children.Add(subGrid);
                
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
                    var subGrid = GameObjectsList[i].ClickableGameObject;
                    Grid.SetRow(subGrid, row);
                    Grid.SetColumn(subGrid, col);
                    ObjectsGrid.Children.Add(subGrid);

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

        public void ChangeAvailability(GameObject gameObject)
        {
            GameObjectsList.Find(
                x => x.GameObject.TileObject.Name == gameObject.TileObject.Name
            ).IsAvailable ^= true;
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