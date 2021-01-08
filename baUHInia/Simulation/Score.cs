using baUHInia.Database;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baUHInia.Simulation 
{
    class Score : ISimulate
    {
        //BazaDanych database = BazaDanych.GetBazaDanych();

        public Score(ITileBinder tileBinder, int boardDensity) {
            this.simulation = new Simulation(tileBinder, boardDensity);
        }
        Simulation simulation;
        


        public int SimulationScore(ITileBinder tileBinder) {

            
            simulation.Sim();

            float airTemperature = simulation.AirTemperature;

            float averageResult = 0;
            int scoreFinal = 0;
            int scoreTmp = 0;
            int placementsReserved = 0;
            int NotPlaceable = 0;



            for (int i = 0; i < simulation.BoardDensity; i++)
            {
                for (int j = 0; j < simulation.BoardDensity; j++)
                {

                    if (!simulation.ITileBinder.TileGrid[i, j].Placeable)
                    {
                        NotPlaceable++;
                        
                        foreach (Placement it in simulation.ITileBinder.PlacedObjects)
                        {
                            if (i == it.Position.x || j == it.Position.y)
                            {
                                placementsReserved++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(placementsReserved);

            for (int i = 0; i < simulation.avgFieldsTemp.Count; i++)
            {
      
                float tmpValueTemp = (simulation.avgFieldsTemp[i] - airTemperature) < 0 ? 0 : simulation.avgFieldsTemp[i] - airTemperature;

                if (tmpValueTemp < 0)
                {
                    scoreFinal += 1;
                }
                else if (tmpValueTemp <= 0.5)
                {
                    scoreFinal += 4;
                    scoreTmp += 4;
                }
                else if (tmpValueTemp > 0.5 && tmpValueTemp <= 1.0)
                {
                    scoreFinal += 3;
                    scoreTmp += 3;
                }
                else if (tmpValueTemp > 1.0 && tmpValueTemp <= 2.0)
                {
                    scoreFinal += 2;
                    scoreTmp += 2;
                }
                else if (tmpValueTemp > 2.0 && tmpValueTemp <= 3.0)
                {
                    scoreFinal += 1;
                    scoreTmp += 1;
                }
                else if (tmpValueTemp > 3.5)
                {
                    scoreFinal += 0;
                    scoreTmp += 0;
                }
                  

                    averageResult += tmpValueTemp;
            }


            if (simulation.avgFieldsTemp.Count / 200 > 0 && (averageResult - airTemperature) < 3.0) {

                scoreFinal += simulation.avgFieldsTemp.Count / 200 * 10;
            }

         
          
            if (scoreFinal > 10000)
                scoreFinal = 10000;

            Console.WriteLine("ScoreFinal: " + scoreFinal);
 

            return scoreFinal;
            


        }

        
    }
}