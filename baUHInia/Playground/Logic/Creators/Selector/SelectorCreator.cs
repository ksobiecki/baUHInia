using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public Button CreateSelectorTile(string elementName, BitmapImage image, (string cat, string subCat) tag)
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
            Style = Application.Current.FindResource("SquareButton") as Style,
            Tag = elementName
        };

        //============================ SELECTOR BEHAVIOUR ============================//

        private void OnSelectorMouseClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Button senderButton = sender as Button;
            (string category, string subCategory) = ((string, string)) senderButton.Tag;
            
            TileObject tileObject = _tileCategories
                .First(c => c.Name == category).TileObjects
                .First(o => o.Tag.subCategory == subCategory);
            
            _currentSelection.AssignSelection(tileObject);
            _currentSelection.ChangeState(State.Place);
        }
    }
}