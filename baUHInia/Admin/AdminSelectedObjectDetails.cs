using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public class AdminSelectedObjectDetails
    {
        private Grid selectedObjectDetails;
        private IAdminChangeObjectDetails iAdminChangeObjectDetails;

        public AdminSelectedObjectDetails(Grid selectedObjectDetails,
            IAdminChangeObjectDetails iAdminChangeObjectDetails)
        {
            this.selectedObjectDetails = selectedObjectDetails;
            this.iAdminChangeObjectDetails = iAdminChangeObjectDetails;
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition {Height = new GridLength(100)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition {Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition {Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition {Height = new GridLength(20)});
            selectedObjectDetails.RowDefinitions.Add(new RowDefinition {Height = new GridLength(20)});
            selectedObjectDetails.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(105)});
            selectedObjectDetails.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(195)});
        }

        public void Display(GameObject gameObject)
        {
            selectedObjectDetails.Children.Clear();

            var image = new System.Windows.Controls.Button
            {
                Content = new Image {Source = gameObject.TileObject[gameObject.TileObject.Sprite.Names[0]]},
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Margin = new Thickness(-30, 0, 0, 0)
            };
            Grid.SetRow(image, 0);
            Grid.SetColumnSpan(image, 2);

            var nameLabel = new TextBlock {Text = "Nazwa: "};
            Grid.SetRow(nameLabel, 1);
            Grid.SetColumn(nameLabel, 0);
            var name = new TextBlock {Text = shorten(gameObject.TileObject.Name, 20)};
            Grid.SetRow(name, 1);
            Grid.SetColumn(name, 1);
            nameLabel.Padding = new Thickness(10, 0, 0, 5);
            name.Margin = new Thickness(-50, 0, 40, 3);
            


            var priceLabel = new TextBlock {Text = "Cena: "};
            Grid.SetRow(priceLabel, 2);
            Grid.SetColumn(priceLabel, 0);
            var price = new System.Windows.Controls.TextBox { Text = gameObject.Price.ToString()};
            Grid.SetRow(price, 2);
            Grid.SetColumn(price, 1);
            price.PreviewTextInput += Int_PreviewTextInput;
            priceLabel.Padding = new Thickness(10, 0, 0, 5);
            price.Margin = new Thickness(0, 0, 40, 3); ;

            var ratioLabel = new TextBlock {Text = "Wpływ na temp: "};
            Grid.SetRow(ratioLabel, 3);
            Grid.SetColumn(ratioLabel, 0);
            var ratio = new System.Windows.Controls.TextBox { Text = gameObject.ChangeValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)};
            Grid.SetRow(ratio, 3);
            Grid.SetColumn(ratio, 1);
            ratio.PreviewTextInput += Decimal_PreviewTextInput;
            ratioLabel.Padding = new Thickness(10, 0, 0, 5);
            ratio.Margin = new Thickness(0, 0, 40, 3);

            var save = new System.Windows.Controls.Button
            {
                Content = "Zapisz",
                Background = new SolidColorBrush(Color.FromRgb(0x00, 0x30, 0x49)),
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(-30, 0, 0, 0)
            };
            save.Click += (sender, args) => SaveChanges(
                price, ratio, sender, args
                );
            
            Grid.SetRow(save, 4);
            Grid.SetColumnSpan(save, 2);

            selectedObjectDetails.Children.Add(image);
            selectedObjectDetails.Children.Add(name);
            selectedObjectDetails.Children.Add(price);
            selectedObjectDetails.Children.Add(ratio);
            selectedObjectDetails.Children.Add(nameLabel);
            selectedObjectDetails.Children.Add(priceLabel);
            selectedObjectDetails.Children.Add(ratioLabel);
            selectedObjectDetails.Children.Add(save);
        }

        private void Decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveChanges(System.Windows.Controls.TextBox price, System.Windows.Controls.TextBox ratio, object obj, RoutedEventArgs routedEventArgs)
        {
            int priceInt;
            float ratioFloat;
            string parsedRatio = ratio.Text.Replace(".", ",");
            if (int.TryParse(price.Text, out priceInt) && float.TryParse(parsedRatio, out ratioFloat))
            {
                iAdminChangeObjectDetails.SubmitChanges(priceInt, ratioFloat);
            }
            else
            {
                System.Windows.MessageBox.Show("Prosze wpisać poprawne wartości", "Błąd wpisanych wartości",
                (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                price.Text = "0";
                ratio.Text = "0.00";
            }
        }
        private string shorten(string input, int trimmedLength)
        {
            string output = input;
            if (input.Length > trimmedLength)
            {
                output = "";
                for (int i = 0; i < trimmedLength; i++) {
                    output += input[i];
                };
            }
            return output;
        }


    }
}