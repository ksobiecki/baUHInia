using baUHInia.MapLogic.Model;
using baUHInia.Playground.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace baUHInia.MapLogic.Manager
{
    class GameMapManager : IGameMapManager
    {
        private string choice;

        public GameMapManager()
        {
            this.choice = "";
        }

        public Grid GetGameLoadGrid()
        {
            if (choice.Length == 0) {
                throw new Exception("Invalid game selected.");
            }

            throw new NotImplementedException();
        }

        public Grid GetMapLoadGrid()
        {
            if (choice.Length == 0)
            {
                throw new Exception("Invalid map selected.");
            }

            throw new NotImplementedException();
        }

        public bool GetMapSaveGrid()
        {
            throw new NotImplementedException();
        }

        public Game LoadGame(string name)
        {
            throw new NotImplementedException();
        }

        public Map LoadMap(string name)
        {
            throw new NotImplementedException();
        }

        public bool SaveGame(ITileBinder tileBinder)
        {
            throw new NotImplementedException();
        }

        public bool SaveMap(ITileBinder tileBinder)
        {
            throw new NotImplementedException();
        }
    }
}
