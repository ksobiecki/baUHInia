using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public partial class AdminRestrictionsWindow : IAdminOnClickObject, IAdminChangeObjectDetails
    {
        private AdminGridObjectsCreator AllGameObjects;
        private AdminGridObjectsCreator AvailableForUserGameObjects;
        private AdminSelectedObjectDetails ObjectDetails;
        private AdminInGridClickableObject SelectedObject;

        public AdminRestrictionsWindow()
        {
            InitializeComponent();
            AllGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
                false,
                AllGameObjectsGrid,
                this
            );
            AvailableForUserGameObjects = new AdminGridObjectsCreator(
                new GameObject[0],
                true,
                AvailableForUserGameObjectsGrid,
                this
            );
            AllGameObjects.InitializeGridDefinitions();
            AllGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());

            AvailableForUserGameObjects.InitializeGridDefinitions();
            AvailableForUserGameObjects.CreateGrid();
            ObjectDetails = new AdminSelectedObjectDetails(SelectedGameObjectDetails, this);
        }

        //todo zmien Terrain na Foliage, gdy cos tam juz bedzie
        private GameObject[] InitializeGameObjects()
        {
            List<GameObject> allGameObjects = new List<GameObject>();
            List<TileCategory> categoryList = ResourceHolder.Get.Terrain;


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
            if (SelectedObject.IsAvailable)
            {
                AllGameObjects.ChangeAvailability(SelectedObject);
                AvailableForUserGameObjects.RemoveObject(SelectedObject);
                AvailableForUserGameObjects.CreateGrid();
                AllGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            }
            else
            {
                AllGameObjects.ChangeAvailability(SelectedObject);
                AdminInGridClickableObject copy = new AdminInGridClickableObject(SelectedObject.GameObject, true, this);
                OnObjectClick(copy);
                AvailableForUserGameObjects.AddObject(copy);
                AvailableForUserGameObjects.CreateGrid();
                AllGameObjects.CreateGridWithCategoryBreaks(GetCategoryBreakLineIndex());
            }
        }

        public void OnObjectClick(AdminInGridClickableObject selectedObject)
        {
            SelectedObject = selectedObject;
            ObjectDetails.Display(selectedObject.GameObject);
        }

        public void SubmitChanges(int price, float ratio)
        {
            SelectedObject.GameObject.Price = price;
            SelectedObject.GameObject.ChangeValue = ratio;
        }
    }
}