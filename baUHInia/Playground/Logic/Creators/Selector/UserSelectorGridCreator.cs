using System.Collections.Generic;
using System.Linq;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    //TODO: NOT TESTED !!!
    public class UserSelectorGridCreator : VerticalSelectorGridCreator
    {
        public UserSelectorGridCreator(ITileBinder binder) : base(binder) { }

        public override void CreateSelectionPanel(TileCategory tileCategory, ITileBinder tileBinder)
        {
            ClearSelectorGrid();
            int gridRowIndex = 0;
            List<TileObject> tileObjects = GetTileObjects(tileBinder.AvailableObjects);
            tileObjects = Filter(tileObjects, tileCategory);
            
            IEnumerable<string> groups = GetGroups(tileObjects);
            TileObject[] otherObjects = GetStandaloneObjects(tileObjects);
            CreateGroups(groups, tileCategory.TileObjects, ref gridRowIndex);
            CreateStandaloneObjects(otherObjects, ref gridRowIndex);
        }

        private static List<TileObject> GetTileObjects(IEnumerable<GameObject> gameObjects) =>
            gameObjects.Select(g => g.TileObject).ToList();

        private static List<TileObject> Filter(IEnumerable<TileObject> tileObjects, TileCategory tileCategory) =>
            tileObjects.Where(o => tileCategory.TileObjects.Contains(o)).ToList();
        
        private static IEnumerable<string> GetGroups(IEnumerable<TileObject> tileObjects) => tileObjects
            .Where(o => o.Group != null)
            .Select(o => o.Group)
            .Distinct()
            .ToArray();
        
        private static TileObject[] GetStandaloneObjects(IEnumerable<TileObject> tileObjects) => tileObjects
            .Where(o => o.Group == null)
            .ToArray();
    }
}