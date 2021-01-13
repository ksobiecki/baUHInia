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
        
        
        public Score(ITileBinder tileBinder, int boardDensity) {
            this.simulation = new Simulation(tileBinder, boardDensity);
            
        }
            
        
        //METHOD CALCULATE SCORE
        public int SimulationScore() {

            
            simulation.Sim();
            float airTemperature = simulation.AirTemperature;
            
            averageResult = 0;
            scoreFinal = 0;
            scoreTmp = 0;
             

            for (int i = 0; i < simulation.avgFieldsTemp.Count; i++)
            {
      
                float tmpValueTemp = (simulation.avgFieldsTemp[i] - airTemperature) < 0 ? 0 : (simulation.avgFieldsTemp[i] - airTemperature);

                
              if (tmpValueTemp <= 0.4)
                {
                    scoreFinal += 4;
                    scoreTmp += 4;
                }
                else if (tmpValueTemp > 0.4 && tmpValueTemp <= 0.8)
                {
                    scoreFinal += 3;
                    scoreTmp += 3;
                }
                else if (tmpValueTemp > 0.8 && tmpValueTemp <= 1.5)
                {
                    scoreFinal += 2;
                    scoreTmp += 2;
                }
                else if (tmpValueTemp > 1.5 && tmpValueTemp <= 2.5)
                {
                    scoreFinal += 1;
                    scoreTmp += 1;
                }
                else if (tmpValueTemp > 2.5)
                {
                    scoreFinal += 0;
                    scoreTmp += 0;
                }
                  

                averageResult += tmpValueTemp;
            }


            if (simulation.avgFieldsTemp.Count / 200 > 0 && (averageResult - airTemperature) < 3.0) 
            {
                scoreFinal += simulation.avgFieldsTemp.Count / 200 * 10;
            }

            
            if (scoreFinal > 10000)
                scoreFinal = 10000;
            
            
            Console.WriteLine("ScoreFinal: " + scoreFinal);
            

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