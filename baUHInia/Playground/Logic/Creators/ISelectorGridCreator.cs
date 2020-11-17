using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Tiles;

namespace baUHInia.Playground.Logic.Creators
{
    public interface ISelectorGridCreator
    {
        void CreateSelectionPanel(TileCategory tileCategory, ITileBinder tileBinder);
    }
}