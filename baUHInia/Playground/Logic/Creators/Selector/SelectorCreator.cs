using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class SelectorCreator
    {
        private readonly Selection _currentSelection;
        private readonly List<TileCategory> _tileCategories;


        public SelectorCreator(Selection currentSelection)
        {
            _currentSelection = currentSelection;
            //TODO: Check
            _tileCategories = ResourceHolder.Get.Terrain;
        }

        public Button CreateSelectorTile(string elementName, BitmapImage image, (int category, int subCategory) tag)
        {
            Button button = CreateSelector(elementName, image);
            button.Click += OnSelectorMouseClick;
            button.Tag = tag;
            return button;
        }
        
        private static Button CreateSelector(string elementName, BitmapImage image) => new Button
        {
            Content = new Image {Source = image},
            Background = Brushes.Transparent,
            BorderBrush = Brushes.Transparent,
            Margin = new Thickness(1),
            Padding = new Thickness(-1.2),
            Tag = elementName
        };
        
        //============================ SELECTOR BEHAVIOUR ============================//
        
        private void OnSelectorMouseClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Button senderButton = sender as Button;
            (int category, int subCategory) = ((int, int)) senderButton.Tag;
            TileObject tileObject = _tileCategories[category].TileObjects[subCategory];
            _currentSelection.TileObject = tileObject;
        }
    }
}