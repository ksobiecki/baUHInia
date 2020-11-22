using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using baUHInia.Playground.Model;

namespace baUHInia.Admin
{
    public class AdminSelectedObjectDetails
    {
        private Grid selectedObjectDetails;
        public AdminSelectedObjectDetails(Grid selectedObjectDetails)
        {
            this.selectedObjectDetails = selectedObjectDetails;
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition{Height = new GridLength(100)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition{Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition{Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition{Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition{Height = new GridLength(20)});
            selectedObjectDetails.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(85)} );
            selectedObjectDetails.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(115)} );


        }

        public void Display(GameObject gameObject)
        {
            selectedObjectDetails.Children.Clear();
            
            var image = new Button
            {
                Content = new Image{Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[0]]},
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(-30,0,0,0)
            };
            Grid.SetRow(image,0);
            Grid.SetColumnSpan(image,2);
            
            var nameLabel = new TextBlock {Text = "Nazwa: "};
            Grid.SetRow(nameLabel,1);
            Grid.SetColumn(nameLabel,0);
            var name = new TextBlock {Text = gameObject.TileObject.Name};
            Grid.SetRow(name,1);
            Grid.SetColumn(name,1);
            
            var priceLabel = new TextBlock {Text = "Cena: "};
            Grid.SetRow(priceLabel,2);
            Grid.SetColumn(priceLabel,0);
            var price = new TextBox {Text = gameObject.Price.ToString()};
            Grid.SetRow(price,2);
            Grid.SetColumn(price,1);
            
            var ratioLabel = new TextBlock {Text = "Wpływ na temp: "};
            Grid.SetRow(ratioLabel,3);
            Grid.SetColumn(ratioLabel,0);
            var ratio = new TextBox {Text = gameObject.ChangeValue.ToString()};
            Grid.SetRow(ratio,3);
            Grid.SetColumn(ratio,1);
            
            var save = new Button
            {
                Content = "Zapisz",
                Background = new SolidColorBrush(Color.FromRgb(0x00,0x30,0x49)),
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(-30,0,0,0)
            };
            Grid.SetRow(save,4);
            Grid.SetColumnSpan(save,2);
            
            selectedObjectDetails.Children.Add(image);
            selectedObjectDetails.Children.Add(name);
            selectedObjectDetails.Children.Add(price);
            selectedObjectDetails.Children.Add(ratio);
            selectedObjectDetails.Children.Add(nameLabel);
            selectedObjectDetails.Children.Add(priceLabel);
            selectedObjectDetails.Children.Add(ratioLabel);
            selectedObjectDetails.Children.Add(save);
            
        }
    }
}