using baUHInia.Authorisation;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;



namespace baUHInia.Simulation
{
    public class Simulation : ISimulate
    {
        //wartosci stale w projekcie takie jak: 
        private const float airTemperature = 22.0f;
        private const float shadowLength = 0.58f; //stale zacienie
        private const float dimensionField = 5.0f; //metry
        private const float dimensionFieldSquare = dimensionField*dimensionField;
       

        //Parametry Sloneczne
        private const float azimuthSun = 32.63f;  //stopnie
        private const float declinationSun = 21.56f; //stopnie
        private const float solarRadiationDirect = 1128.60f; //W/m^2
        private const float solarRadiationDiffuse = 193.55f; 


        //Wartości obiektów:
        //wysokości
        private const float heightBigTree = 10.0f;
        private const float heightSmallTree = 5.0f;
        private const float heightSmallHouse = 4.5f;
        private const float heightHouse = 8.0f;
        private const float mediumBlock = 20.0f;
        private const float heightBlock = 30.0f;

        //wsolczynniki
        private const float asphaltTempValue = 0.93f;
        private const float dirtTempValue = 0.55f;
        private const float treeTempValue = 0.6f;
       
        private List<(int, int)> shadowedTiles1 = new List<(int, int)>();
        private List<(int, int)> warmerFields1 = new List<(int, int)>();
       // private List<(int, int, float)> zmienna = new List<(int, int, float)>();

        private List<float> avgFieldsTemp = new List<float>();


        //private List<(int, int,int)> shadowedTiles = new List<(int,int,int)>();
        
        
        //zmienne kto
        static private List<Placement> placements ;
        private Tile[,] plytki; 
        //private Placement plac;
        
        int ile = 0;
        float temperatures = 0;
        public Simulation() { }
       

        public void Sim(ITileBinder iTileBinder, int boardDensity)
        {

            List<(int, int)> shadowedTiles = new List<(int, int)>();
            List<(int, int)> warmerFields = new List<(int, int)>();
            List<float> avgFieldsTemp = new List<float>();

            placements = iTileBinder.PlacedObjects;
            ile = 0;
  
            foreach(Placement it in iTileBinder.PlacedObjects)
            {
               
               
                //slonce od strony lewej
                if (it.Position == (it.Position.x + 1, it.Position.y)) {
                    continue;
                }
                else {
                    int iterator = 1;
                    int tmpShadowFieldsTemp = (int)((int)(heightBigTree * shadowLength) % dimensionField) > 5 ?
                        (int)(((heightBigTree * shadowLength) / dimensionField) + 1) : 
                        (int)((heightBigTree * shadowLength)/ dimensionField);   

                    Console.WriteLine("To jest tmp shadow: " + tmpShadowFieldsTemp);
                    while ((iterator <= tmpShadowFieldsTemp) && (it.Position.x+iterator < boardDensity)) {

                        Console.WriteLine("Pozycja x: " +( it.Position.x+iterator).ToString());
                        Console.WriteLine("Density: " + boardDensity);
                        Console.WriteLine("Shadow tmp: " + tmpShadowFieldsTemp);
                        shadowedTiles.Add(((it.Position.x+iterator),it.Position.y));
                        iterator++;
                        
                    }
                }
              
            }

            //SORTOWANIE OOBIEKTOW (Od lewego gornego do prawego dolnego)
            placements.Sort((x, y) => {
                int result = x.Position.Item1.CompareTo(y.Position.Item1);
                return result == 0 ? x.Position.Item2.CompareTo(y.Position.Item2) : result;
            });

            foreach (Placement it in placements) {
                Console.WriteLine("Posortowane: " + it.Position);
          
            }

           
            //OBSZARY GORACA 
            for (int it=0; it < placements.Count; it++) {
                int distance = 5;

                //avgFieldsTemp.Add(placements[it].GameObject.ChangeValue);
                //dla drzewa
                //z
                //avgFieldsTemp.Add((placements[it].Position.x, 
                //    placements[it].Position.y,
                //    airTemperature*treeTempValue + airTemperature));

                for (int it2 = it + 1; it2 < placements.Count; it2++)
                {
                    int distanceObjects = 0;

                    if (Math.Pow((placements[it].Position.x) - placements[it2].Position.x, 2) 
                     + Math.Pow((placements[it].Position.y) - placements[it2].Position.y, 2) <= Math.Pow((distance + 1), 2))
                    {
                        int xIle = Math.Abs(placements[it].Position.x - placements[it2].Position.x) !=0 ?
                            Math.Abs(placements[it].Position.x - placements[it2].Position.x)-1 :
                            Math.Abs(placements[it].Position.x - placements[it2].Position.x);

                        int yIle = Math.Abs(placements[it].Position.y - placements[it2].Position.y) != 0 ? 
                           ( Math.Abs(placements[it].Position.y - placements[it2].Position.y)-1) :
                           ( Math.Abs(placements[it].Position.y - placements[it2].Position.y));

                        while (xIle > 0 || yIle > 0)
                        {
                            //zmienic by wykrywalo dystans
                            if (!(placements.Any(field => field.Position == (placements[it].Position.x + xIle, placements[it].Position.y + yIle))))
                            {
                                warmerFields.Add((placements[it].Position.x + xIle, placements[it].Position.y + yIle));
                                //Console.WriteLine("Pole o podwyzszonej temperaturze: " + (placements[it].Position.x + xIle) + " " + (placements[it].Position.y + yIle));
                            }
                            if (xIle > 0)
                            {
                                xIle--;
                            }

                            if (yIle > 0)
                            {
                                yIle--;
                            }

                        }
                    }
                }
                        
            }

           

            //sortowanie i usuwanie powtorzen
            warmerFields.Sort((x, y) => {
                int result = x.Item1.CompareTo(y.Item1);
                return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
            });

            warmerFields = warmerFields.Distinct().ToList();



            /*
            for (int i = 0; i < warmerFields.Count; i++)
            {
                Console.WriteLine(warmerFields[i].ToString());
            }*/


            for (int i = 0; i < boardDensity; i++) {
                for (int j = 0; j < boardDensity; j++) {

                    float tempTemperature = 22.0f;

                    foreach (Placement it in placements)
                    {
                        if (it.Position == (i, j) && it.GameObject.TileObject.Name == "Summer Oak")
                        {
                            Console.WriteLine("Jestem");
                            tempTemperature = airTemperature * treeTempValue + airTemperature;
                        }
                    }


                    if (iTileBinder.TileGrid[i,j].GetName().Contains("Asphalt") ||
                        iTileBinder.TileGrid[i, j].GetName().Contains("Road"))
                    {
                        Console.WriteLine(iTileBinder.TileGrid[i,j].GetName());
                        //zmienna.Add((i,j, (airTemperature*asphaltTempValue+airTemperature)));
                        tempTemperature = airTemperature * asphaltTempValue + airTemperature;
                    }

                    if (iTileBinder.TileGrid[i, j].GetName().Contains("Dirt"))
                    {
                        //zmienna.Add((i, j, dirtTempValue*airTemperature + airTemperature));
                        tempTemperature = airTemperature * dirtTempValue + airTemperature;
                    }

                    //ZMIENIC NA WARTOSC Z LITERATURY
                    for (int a = 0; a < warmerFields.Count; a++) {
                        if (warmerFields[a].Item1 == j && warmerFields[a].Item2 == i) {
                            tempTemperature = tempTemperature * 1.8f;
                        }
                    }

                    //ZMIENIC NA WARTOSC Z LITERATURY
                    for (int a = 0; a < shadowedTiles.Count; a++) {
                        if (shadowedTiles[a].Item1 == j && shadowedTiles[a].Item2 == i) {
                            tempTemperature = tempTemperature - 2.0f;
                        }
                    }
                    

                    avgFieldsTemp.Add(tempTemperature);
                    
                }
            }
            /*
            for (int i = 0; i < shadowedTiles.Count; i++) {

                Console.WriteLine("Wyswietlenie pol zacienionych: " + shadowedTiles[i].ToString());
            }

            for (int i = 0; i < zmienna.Count; i++) {
                foreach ((int, int) it in shadowedTiles) {
                    if (it.Item1 == zmienna[i].Item1 && it.Item2 == zmienna[i].Item2) {
                        //zmienna[i] = (it.Item1, it.Item2);
                    }
                }
                Console.WriteLine("Wyswietlenie zwyklych plytek: " + zmienna[i].ToString());
            }
           */
            float Result = 0;
           

            foreach (float it in avgFieldsTemp) {
                Result += it;
            }

            Result = ((Result / avgFieldsTemp.Count) + airTemperature) / 2;
            Console.WriteLine("wynik: " + Result);

        }

        public string returnScoreTemperature()
        {
            throw new NotImplementedException();
        }
    }
}


