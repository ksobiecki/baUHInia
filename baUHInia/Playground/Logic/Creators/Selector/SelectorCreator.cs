using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class SelectorCreator
    {
        private readonly Selection _currentSelection;
        private List<TileCategory> _tileCategories;


        public SelectorCreator(Selection currentSelection, List<TileCategory> categories)
        {
            _currentSelection = currentSelection;
            _tileCategories = categories;
        }

        public void UpdateTileGroup(List<TileCategory> categories) => _tileCategories = categories;

        public Button CreateSelectorTile(string elementName, BitmapImage image, (int category, int subCategory) tag)
        {
            Button button = CreateSelector(elementName, image);
            button.Click += OnSelectorMouseClick;
            button.Tag = tag;
            return button;
        }
        
        public static Button CreateSelector(string elementName, BitmapImage image) => new Button
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
            //TODO:
            TileObject tileObject = _tileCategories[category].TileObjects[subCategory];
            _currentSelection.AssignSelection(tileObject);
            _currentSelection.ChangeState(State.Place);
        }
    }
}