using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Logic.Utils;
using baUHInia.Playground.Model.Utility;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public class AdminInGridClickableObject
    {
        public Grid ClickableGameObject { get; }
        public GameObject GameObject { get; }
        public Boolean IsAvailable { get; set; }

        private readonly IAdminOnClickObject _iAdminOnClickObject;

        public AdminInGridClickableObject(GameObject gameObject, Boolean isAvailable,
            IAdminOnClickObject iAdminOnClickObject)
        {
            this.GameObject = gameObject;
            ClickableGameObject = gameObject.TileObject.Config != null
                ? CreateOfButtons(gameObject)
                : CreateButton(gameObject);

            IsAvailable = isAvailable;
            _iAdminOnClickObject = iAdminOnClickObject;
        }

        private Grid CreateButton(GameObject gameObject)
        {
            Grid subGrid = new Grid();
            subGrid.RowDefinitions.Add(new RowDefinition());
            subGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Button button = new Button
            {
                Content = new Image {Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[0]]},
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(1),
                Padding = new Thickness(-1.2)
            };
            button.Click += OnClick;
            Grid.SetColumn(button, 0);
            Grid.SetRow(button, 0);
            subGrid.Children.Add(button);
            return subGrid;
        }

        public Grid CreateOfButtons(GameObject gameObject)
        {
            (int width, int height) = gameObject.TileObject.Sprite.SpriteWidthHeight();
            (sbyte x, sbyte y) = gameObject.TileObject.Sprite.SpriteMinCoordinates();
            var subGrid = new Grid();
            for (var i = 0; i <= Math.Max(height, width); i++) subGrid.RowDefinitions.Add(new RowDefinition());
            //for (var i = 0; i <= height; i++) subGrid.RowDefinitions.Add(new RowDefinition());
            for (var i = 0; i <= Math.Max(height, width); i++) subGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //for (var i = 0; i <= width; i++) subGrid.ColumnDefinitions.Add(new ColumnDefinition());

            foreach (var offset in gameObject.TileObject.Config.Offsets)
            {
                Button button = new Button
                {
                    Content = new Image {Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[offset.I]]},
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    // Margin = new Thickness(-1, 0, -1, 0),
                    Padding = new Thickness(-1.2)
                };
                button.Click += OnClick;
                Grid.SetColumn(button, offset.X - x);
                Grid.SetRow(button, height - offset.Y + y);
                subGrid.Children.Add(button);
            }

            subGrid.Margin = new Thickness(1);

            return subGrid;
        }

        private void OnClick(object obj, RoutedEventArgs routedEventArgs)
        {
            _iAdminOnClickObject.OnObjectClick(this);
        }
    }
}