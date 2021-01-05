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
            _allGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
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
            _availableForUserGameObjects.InitializeGridDefinitions();
            _availableForUserGameObjects.CreateGrid();
            _allGameObjects.InitializeGridDefinitions();
            _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            _objectDetails = new AdminSelectedObjectDetails(SelectedGameObjectDetails, this);
        }

        //todo zmien Terrain na Foliage, gdy cos tam juz bedzie
        private GameObject[] InitializeGameObjects()
        {
            List<GameObject> allGameObjects = new List<GameObject>();
            //ResourceHolder.Get.ChangeResourceType(ResourceType.Terrain);
            ResourceHolder.Get.ChangeResourceType(ResourceType.Foliage);
            List<TileCategory> categoryList = ResourceHolder.Get.GetSelectedCategories();


            foreach (var category in categoryList)
            {
                foreach (var tileObject in category.TileObjects)
                {
                    allGameObjects.Add(
                        new GameObject(tileObject, 0.0F, 0)
                    );
                }
            }

            return allGameObjects.ToArray();
        }

        private List<int> GetCategoryBreakLineIndex()
        {
            List<TileCategory> categoryList = ResourceHolder.Get.Terrain;
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
                _allGameObjects.ChangeAvailability(_selectedObject);
                _availableForUserGameObjects.RemoveObject(_selectedObject);
                _availableForUserGameObjects.CreateGrid();
                _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            }
            else
            {
                _allGameObjects.ChangeAvailability(_selectedObject);
                AdminInGridClickableObject
                    copy = new AdminInGridClickableObject(_selectedObject.GameObject, true, this);
                OnObjectClick(copy);
                _availableForUserGameObjects.AddObject(copy);
                _availableForUserGameObjects.CreateGrid();
                _allGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
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