using System;
using System.Collections.Generic;
using System.Windows;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Admin
{
    public partial class AdminRestrictionsWindow : Window, IAdminOnClickObject
    {
        private AdminGridObjectsCreator AllGameObjects;
        public AdminRestrictionsWindow()
        {
            InitializeComponent();
            AllGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
                false,
                AllGameObjectsGrid,
                this
                );
            AllGameObjects.CreateGrid();
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


        public void OnObjectClick(AdminInGridClickableObject selectedObject)
        {
            AdminBudget.Text = selectedObject.GameObject.TileObject.Name;
        }
    }
}