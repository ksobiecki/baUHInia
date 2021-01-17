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
        //DATABASE
        BazaDanych database = BazaDanych.GetBazaDanych();
        
        //SIMULATION
        Simulation simulation;
        
        //VARIABLES - Score
        
        float averageResult = 0;
        int scoreFinal = 0;
        int scoreTmp = 0;
        
       // BazaDanych database = BazaDanych.GetBazaDanych();

        public Score(ITileBinder tileBinder, int boardDensity) {
            this.simulation = new Simulation(tileBinder, boardDensity);
            
        }
            
        //METHOD CALCULATE SCORE
        
         public int SimulationScore() {

            
            simulation.Sim();

            float airTemperature = simulation.AirTemperature;

            float averageResult = 0;
             scoreFinal = 0;
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

         

            for (int i = 0; i < simulation.avgFieldsTemp.Count; i++)
            {
      
                float tmpValueTemp = (simulation.avgFieldsTemp[i] - airTemperature) < 0 ? 0 : (simulation.avgFieldsTemp[i] - airTemperature);


                if (simulation.avgFieldsTemp[i] <= airTemperature)
                {
                    scoreFinal += 4;
                }

               
                
              if (tmpValueTemp <= 0.4)
                {
                    scoreFinal += 4;
                    scoreTmp += 4;
                }
                else if (tmpValueTemp > 0.4 && tmpValueTemp <= 0.6)
                {
                    scoreFinal += 3;
                    scoreTmp += 3;
                }
                else if (tmpValueTemp > 0.6 && tmpValueTemp <= 0.8)
                {
                    scoreFinal += 2;
                    scoreTmp += 2;
                }
                else if (tmpValueTemp > 0.8 && tmpValueTemp <= 1.0)
                {
                    scoreFinal += 1;
                    scoreTmp += 1;
                }
                else if (tmpValueTemp > 1.0)
                {
                    scoreFinal += 0;
                    scoreTmp += 0;
                }
                  

                    averageResult += tmpValueTemp;
            }
            

           
          
            if (scoreFinal > 10000)
                scoreFinal = 10000;
            
            
            int userID = simulation.ITileBinder.Credentials.UserID;
            
            Console.WriteLine("ScoreFinal: " + scoreFinal);
            //database.SetScoreInFinniszedGame(gameId, scoreFinal, userID)

            return scoreFinal;
            


        }
        
         
        //METHOD SEND RESULT TO DATABASE 
        public void ScoreToDatabase(int gameID)
        {
            int userID = simulation.ITileBinder.Credentials.UserID;
            database.SetScoreInFinniszedGame(gameID, scoreFinal, userID);
        }
    }
}