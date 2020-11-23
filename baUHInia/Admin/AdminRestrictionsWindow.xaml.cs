using System.Collections.Generic;
using System.Linq;
using baUHInia.Playground.Model.Resources;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;

namespace baUHInia.Admin
{
    public partial class AdminRestrictionsWindow : IAdminOnClickObject
    {
        private AdminGridObjectsCreator AllGameObjects;
        private AdminSelectedObjectDetails ObjectDetails;
        public AdminRestrictionsWindow()
        {
            InitializeComponent();
            AllGameObjects = new AdminGridObjectsCreator(
                InitializeGameObjects(),
                false,
                AllGameObjectsGrid,
                this
                );
            AllGameObjects.CreateGrid(GetCategoryBreakLineIndex());
            ObjectDetails = new AdminSelectedObjectDetails(SelectedGameObjectDetails);
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

        public void OnObjectClick(AdminInGridClickableObject selectedObject)
        {
            ObjectDetails.Display(selectedObject.GameObject);
        }
    }
}