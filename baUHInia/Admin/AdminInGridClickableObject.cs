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
        public List<Button> ClickableGameObject { get; }
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

        private List<Button> CreateButton(GameObject gameObject)
        {
            Button button = new Button
            {
                Content = new Image {Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[0]]},
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(1),
                Padding = new Thickness(-1.2)
            };
            button.Click += OnClick;
            return new List<Button> {button};
        }

        private List<Button> CreateOfButtons(GameObject gameObject)
        {
            var list = new List<Button>();
            foreach (var offset in gameObject.TileObject.Config.Offsets)
            {
                Button button = new Button
                {
                    Content = new Image {Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[offset.I]]},
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Margin = new Thickness(1),
                    Padding = new Thickness(-1.2)
                };
                button.Click += OnClick;
                list.Add(button);
            }

            return list;
        }

        private void OnClick(object obj, RoutedEventArgs routedEventArgs)
        {
            _iAdminOnClickObject.OnObjectClick(this);
        }
    }
}