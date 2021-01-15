using baUHInia.Playground.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Simulation
{
    public interface ISimulate
    {  
        int SimulationScore();
        void ScoreToDatabase(int gameID);

    }
}
