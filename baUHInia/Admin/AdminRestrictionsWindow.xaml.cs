using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public partial class AdminRestrictionsWindow : IAdminOnClickObject, IAdminChangeObjectDetails,
        IAdminSelectorTabCreator
    {
        private readonly AdminGridObjectsCreator _allGameObjects;
        private readonly AdminGridObjectsCreator _availableForUserGameObjects;
        private readonly AdminSelectedObjectDetails _objectDetails;
        private AdminInGridClickableObject _selectedObject;
        private int _budget;

        public AdminRestrictionsWindow(ITileBinder iTileBinder)
        {
            InitializeComponent();
            _budget = iTileBinder.AvailableFounds;
            AdminBudget.Text = _budget.ToString();
            _allGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(iTileBinder),
                false,
                AllGameObjectsGrid,
                this
            );

            _availableForUserGameObjects = new AdminGridObjectsCreator(
                iTileBinder.AvailableObjects.ToArray(),
                true,
                AvailableForUserGameObjectsGrid,
                this
            );
            HideSelectedObjects(iTileBinder);
            _availableForUserGameObjects.InitializeGridDefinitions();
            _availableForUserGameObjects.CreateGrid();
            _allGameObjects.InitializeGridDefinitions();
            _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            _objectDetails = new AdminSelectedObjectDetails(SelectedGameObjectDetails, this);
        }

        private GameObject[] InitializeGameObjects(ITileBinder iTileBinder)
        {
            ResourceHolder.Get.ChangeResourceType(ResourceType.Foliage);
            List<TileCategory> categoryList = ResourceHolder.Get.GetSelectedCategories();

            return (from category in categoryList
                from tileObject in category.TileObjects
                let gameObject = iTileBinder.AvailableObjects.Find(x => x.TileObject == tileObject)
                select gameObject ?? new GameObject(tileObject, 0.0F, 0)).ToArray();
        }

        private List<int> GetCategoryBreakLineIndex()
        {
            ResourceHolder.Get.ChangeResourceType(ResourceType.Foliage);
            List<TileCategory> categoryList = ResourceHolder.Get.GetSelectedCategories();
            List<int> categoryBreakLineIndex = new List<int>();
            foreach (var category in categoryList)
            {
                categoryBreakLineIndex.Add(categoryBreakLineIndex.LastOrDefault() + category.TileObjects.Count);
            }

            return categoryBreakLineIndex;
        }

        private void ChangeAvailabilityOfObject(Object sender, RoutedEventArgs e)
        {
            if (_selectedObject.IsAvailable)
            {
                _allGameObjects.ChangeAvailability(_selectedObject.GameObject);
                _availableForUserGameObjects.RemoveObject(_selectedObject);
                _availableForUserGameObjects.CreateGrid();
                _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
                OnObjectClick(_allGameObjects.GameObjectsList.Find(x =>
                    x.GameObject.TileObject.Name == _selectedObject.GameObject.TileObject.Name));
            }
            else
            {
                _allGameObjects.ChangeAvailability(_selectedObject.GameObject);
                AdminInGridClickableObject
                    copy = new AdminInGridClickableObject(_selectedObject.GameObject, true, this);
                OnObjectClick(copy);
                _availableForUserGameObjects.AddObject(copy);
                _availableForUserGameObjects.CreateGrid();
                _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            }
        }

        private void HideSelectedObjects(ITileBinder iTileBinder)
        {
            foreach (var gameObject in iTileBinder.AvailableObjects)
            {
                Console.WriteLine(
                    $"{gameObject.TileObject.Name}, price {gameObject.Price}, val {gameObject.ChangeValue}");
                _allGameObjects.ChangeAvailability(gameObject);
            }
        }

        public void OnObjectClick(AdminInGridClickableObject selectedObject)
        {
            _selectedObject = selectedObject;
            _objectDetails.Display(selectedObject);
        }

        public void SubmitChanges(int price, float ratio)
        {
            _selectedObject.GameObject.Price = price;
            _selectedObject.GameObject.ChangeValue = ratio;
        }

        public Grid GetAdminSelectorTableGrid() => AdminRestrictionsGrid;


        public List<GameObject> GetModifiedAvailableObjects() => _availableForUserGameObjects.GetGameObjects().ToList();


        public void Save(object obj, RoutedEventArgs routedEventArgs)
        {
            if (int.TryParse(AdminBudget.Text, out _budget))
            {
                _budget = int.Parse(AdminBudget.Text);
            }
            else
            {
                System.Windows.MessageBox.Show("Prosze wpisać poprawne wartości", "Błąd wpisanych wartości",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
            }

            if (_availableForUserGameObjects.GameObjectsList.Count < 3)
            {
                System.Windows.MessageBox.Show("Prosze udostępnić użytkownikowi przynajmniej 3 obiekty",
                    "Za mało obiektów",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
            }
        }

        public System.Windows.Controls.Button GetReturnButton() => ReturnBtn;
        public System.Windows.Controls.Button GetApplyButton() => ApplyBtn;

        public int GetBudget() => _budget;

        //unused method, but worth to keep it here
        private void Number_PreviewTextInput(object sender, KeyPressEventArgs e)

        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Controls.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void AdminBudget_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}