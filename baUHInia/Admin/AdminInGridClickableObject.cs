using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Admin
{
    public class AdminInGridClickableObject
    {
        private Button ClickableGameObject { get; }
        private Boolean IsAvailable { get; set; }

        private readonly IAdminOnClickObject _iAdminOnClickObject;

        public AdminInGridClickableObject(GameObject gameObject, Boolean isAvailable, IAdminOnClickObject iAdminOnClickObject)
        {

            ClickableGameObject = CreateButton(gameObject);
            IsAvailable = isAvailable;
            _iAdminOnClickObject = iAdminOnClickObject;
        }

        private Button CreateButton(GameObject gameObject)
        {
            Button button = new Button
            {
                Content = gameObject.TileObject[gameObject.TileObject.Sprite.Names[0]],
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(1),
                Padding = new Thickness(-1.2)
            };
            button.Click += OnClick;
            return button;
        }

        private void OnClick(object obj, RoutedEventArgs routedEventArgs)
        {
            _iAdminOnClickObject.OnObjectClick(this);
        }
    }
}