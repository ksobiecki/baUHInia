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
        private AdminGridObjectsCreator _allGameObjects;
        private AdminGridObjectsCreator _availableForUserGameObjects;
        private readonly AdminSelectedObjectDetails _objectDetails;
        private List<GameObject> _savedGameObjects;
        private AdminInGridClickableObject _selectedObject;
        private int _budget;
        private int _savedBudget;

        public AdminRestrictionsWindow(ITileBinder iTileBinder)
        {
            Console.WriteLine("Init");
            _savedGameObjects = iTileBinder.AvailableObjects;
            InitializeComponent();
            _budget = iTileBinder.AvailableFounds;
            _savedBudget = iTileBinder.AvailableFounds;
            AdminBudget.Text = _budget.ToString();
            _allGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
                false,
                AllGameObjectsGrid,
                this
            );

            _availableForUserGameObjects = new AdminGridObjectsCreator(
                _savedGameObjects.ToArray(),
                true,
                AvailableForUserGameObjectsGrid,
                this
            );
            HideSelectedObjects(_savedGameObjects);
            _availableForUserGameObjects.InitializeGridDefinitions();
            _availableForUserGameObjects.CreateGrid();
            _allGameObjects.InitializeGridDefinitions();
            _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            _objectDetails = new AdminSelectedObjectDetails(SelectedGameObjectDetails, this);
            OnObjectClick(_allGameObjects.GameObjectsList[0]);
        }

        private GameObject[] InitializeGameObjects()
        {
            ResourceHolder.Get.ChangeResourceType(ResourceType.Foliage);
            List<TileCategory> categoryList = ResourceHolder.Get.GetSelectedCategories();

            List<GameObject> allGameObjects = new List<GameObject>();
            foreach (var category in categoryList)
            {
                foreach (var tileObject in category.TileObjects)
                {
                    var gameObject = _savedGameObjects.Find(x => x.TileObject == tileObject);
                    allGameObjects.Add(
                        gameObject == null
                            ? new GameObject(tileObject, 0.0F, 0)
                            : new GameObject(tileObject, gameObject.ChangeValue, gameObject.Price)
                    );
                }
            }

            return allGameObjects.ToArray();
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
            if (_selectedObject.GameObject.ChangeValue == 0 || _selectedObject.GameObject.Price == 0)
            {
                System.Windows.MessageBox.Show("Prosze najpierw zapisać zmiany", "Nie zapisano zmian",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
            }
            else
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
        }

        private void HideSelectedObjects(List<GameObject> savedGameObjects)
        {
            foreach (var gameObject in savedGameObjects)
            {
                Console.WriteLine(
                    $"HideSelectedObjects {gameObject.TileObject.Name}, price {gameObject.Price}, val {gameObject.ChangeValue}");
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
            String note = "";
            List<String> notes = new List<string>();
            Console.WriteLine("Przed zmiana");
            foreach (var gameObject in _savedGameObjects)
            {
                Console.WriteLine(
                    $"{gameObject.TileObject.Name}, price {gameObject.Price}, val {gameObject.ChangeValue}");
            }

            if (price == 0)
            {
                _selectedObject.GameObject.Price = 0;
                notes.Add("cena");
            }
            else
            {
                _selectedObject.GameObject.Price = price;
            }

            if (ratio == 0.00)
            {
                _selectedObject.GameObject.ChangeValue = (float) 0.00;
                notes.Add("wpływ na temperaturę");
            }
            else
            {
                _selectedObject.GameObject.ChangeValue = ratio;
            }

            if (notes.Count == 0)
            {
                Console.WriteLine("Po zmianie");
                foreach (var gameObject in _savedGameObjects)
                {
                    Console.WriteLine(
                        $"{gameObject.TileObject.Name}, price {gameObject.Price}, val {gameObject.ChangeValue}");
                }
            }
            else
            {
                foreach (var n in notes)
                {
                    if (notes.IndexOf(n) > 0)
                    {
                        note += ", ";
                    }

                    note += n;
                }

                System.Windows.MessageBox.Show("Niepoprawne wartości:" + note + ".", "Błąd wpisanych wartości",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
                _objectDetails.Display(_selectedObject);
            }
        }

        public Grid GetAdminSelectorTableGrid() => AdminRestrictionsGrid;


        public List<GameObject> GetModifiedAvailableObjects()
        {
            return _availableForUserGameObjects.GetGameObjects().ToList().Count < 3
                ? _availableForUserGameObjects.GetGameObjects().ToList()
                : _savedGameObjects;
        }


        public void Save(object obj, RoutedEventArgs routedEventArgs)
        {
            if (int.TryParse(AdminBudget.Text, out _budget))
            {
                _budget = int.Parse(AdminBudget.Text);
                _savedBudget = _budget;
                if (_budget == 0 || _budget > Int32.MaxValue)
                {
                    System.Windows.MessageBox.Show(
                        "Budżet musi być wartością dodatnią.\nProsze wpisać poprawną wartość",
                        "Błąd wpisanych wartości",
                        (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Prosze wpisać poprawną wartość budżetu", "Błąd wpisanych wartości",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
            }

            if (_availableForUserGameObjects.GameObjectsList.Count < 3)
            {
                System.Windows.MessageBox.Show("Prosze udostępnić użytkownikowi przynajmniej 3 obiekty",
                    "Za mało obiektów",
                    (MessageBoxButton) MessageBoxButtons.OK, (MessageBoxImage) MessageBoxIcon.Error);
            }
            else
            {
                _savedGameObjects = _availableForUserGameObjects.GetGameObjects().ToList();
            }
        }

        public void Return(object obj, RoutedEventArgs routedEventArgs)
        {
            _budget = _savedBudget;
            AdminBudget.Text = _budget.ToString();
            _allGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
                false,
                AllGameObjectsGrid,
                this
            );

            _availableForUserGameObjects = new AdminGridObjectsCreator(
                _savedGameObjects.ToArray(),
                true,
                AvailableForUserGameObjectsGrid,
                this
            );
            HideSelectedObjects(_savedGameObjects);
            _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            _availableForUserGameObjects.CreateGrid();
            OnObjectClick(_selectedObject);
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