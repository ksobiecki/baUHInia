using System.Collections.Generic;
using System.Linq;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators.Selector
{
    public class AdminSelectorGridCreator : VerticalSelectorGridCreator
    {
        public AdminSelectorGridCreator(ITileBinder binder) : base(binder) { }

        public override void CreateSelectionPanel(TileCategory tileCategory, ITileBinder tileBinder)
        {
            ClearSelectorGrid();
            int gridRowIndex = 0;
            
            IEnumerable<string> groups = GetGroups(tileCategory);
            TileObject[] otherObjects = GetStandaloneObjects(tileCategory);
            CreateGroups(groups, tileCategory.TileObjects, ref gridRowIndex);
            CreateStandaloneObjects(otherObjects, ref gridRowIndex);
        }

        private static IEnumerable<string> GetGroups(TileCategory tileCategory) => tileCategory.TileObjects
            .Where(o => o.Group != null)
            .Select(o => o.Group)
            .Distinct()
            .ToArray();
        
        private static TileObject[] GetStandaloneObjects(TileCategory tileCategory) => tileCategory.TileObjects
            .Where(o => o.Group == null)
            .ToArray();
    }
}