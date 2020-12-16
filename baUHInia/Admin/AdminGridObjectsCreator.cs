using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            this.isAvailable = isAvailable;
            ObjectsGrid = objectsGrid;
            GameObjectsList = new List<AdminInGridClickableObject>(gameObjects.Length);
            foreach (var gameObject in gameObjects)
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
                if (gameObject.ClickableGameObject.Count == 1)
                {
                    var button = gameObject.ClickableGameObject[0];
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    ObjectsGrid.Children.Add(button);
                }
                else
                {
                    var subGrid = new Grid();
                    (int width, int height) = gameObject.GameObject.TileObject.Sprite.SpriteWidthHeight();
                    (sbyte x, sbyte y) = gameObject.GameObject.TileObject.Sprite.SpriteMinCoordinates();
                    
                    for (var i = 0; i <= height; i++) subGrid.RowDefinitions.Add(new RowDefinition());
                    for (var i = 0; i <= width; i++) subGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    for (var i = 0; i < gameObject.ClickableGameObject.Count; i++)
                    {
                        var button = gameObject.ClickableGameObject[i];
                        button.Margin = new Thickness(-1, 0, -1, 0);
                        button.Padding = new Thickness(-1.1);
                        Grid.SetColumn(button, gameObject.GameObject.TileObject.Config.Offsets[i].X - x);
                        Grid.SetRow(button, height - gameObject.GameObject.TileObject.Config.Offsets[i].Y + y);
                     
                        subGrid.Children.Add(button);
                    }
                    Grid.SetRow(subGrid, row);
                    Grid.SetColumn(subGrid, col);
                    ObjectsGrid.Children.Add(subGrid);
                }


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
                    //todo do funkcji
                    if (GameObjectsList[i].ClickableGameObject.Count == 1)
                    {
                        var button = GameObjectsList[i].ClickableGameObject[0];
                        Grid.SetRow(button, row);
                        Grid.SetColumn(button, col);
                        ObjectsGrid.Children.Add(button);
                    }
                    else
                    {
                        var subGrid = new Grid();
                        (int width, int height) = GameObjectsList[i].GameObject.TileObject.Sprite.SpriteWidthHeight();
                        (sbyte x, sbyte y) = GameObjectsList[i].GameObject.TileObject.Sprite.SpriteMinCoordinates();
                    
                        for (var j = 0; j <= height; j++) subGrid.RowDefinitions.Add(new RowDefinition());
                        for (var j = 0; j <= width; j++) subGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        for (var j = 0; j < GameObjectsList[i].ClickableGameObject.Count; j++)
                        {
                            var button = GameObjectsList[i].ClickableGameObject[j];
                            button.Margin = new Thickness(-1, 0, -1, 0);
                            button.Padding = new Thickness(-1.1);
                            Grid.SetColumn(button, GameObjectsList[i].GameObject.TileObject.Config.Offsets[j].X - x);
                            Grid.SetRow(button, height - GameObjectsList[i].GameObject.TileObject.Config.Offsets[j].Y + y);
                     
                            subGrid.Children.Add(button);
                        }
                        Grid.SetRow(subGrid, row);
                        Grid.SetColumn(subGrid, col);
                        ObjectsGrid.Children.Add(subGrid);
                    }
                    
                    
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