﻿using baUHInia.Authorisation;
using baUHInia.Database;
using baUHInia.Playground.Model;
using baUHInia.Playground.Model.Selectors;
using baUHInia.Playground.Model.Tiles;
using baUHInia.Playground.Model.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;



namespace baUHInia.Simulation
{
    public class Simulation
    {
        public Simulation(ITileBinder tileBinder, int boardDensity) {
            ITileBinder = tileBinder;
            BoardDensity = boardDensity;
        }

        public ITileBinder ITileBinder { get; }
        public int BoardDensity { get; }

        public List<(int, int)> shadowedTiles = new List<(int, int)>();
        public List<(int, int, float, int)> warmerFields = new List<(int, int, float, int)>();
        public List<(int, int, float, int)> warmerFields2 = new List<(int, int, float, int)>();
        public List<(int, int, float)> coolerFields = new List<(int, int, float)>();
        public List<float> avgFieldsTemp = new List<float>();


        //wartosci stale w projekcie takie jak: 
        private const float _airTemperature = 25.0f;
        public float AirTemperature { get { return _airTemperature; } }
        private const float _shadowLength = 0.58f; //stale zacienie
        private const float _dimensionField = 5.0f; //metry


        //Parametry Sloneczne
        private const float _azimuthSun = 32.63f;  //stopnie
        private const float _declinationSun = 21.56f; //stopnie
        private const float _solarRadiationDirect = 1128.60f; //W/m^2
        private const float _solarRadiationDiffuse = 193.55f;

        static private List<Placement> placements;

        //wsolczynniki temperatury dla 'plytek'
        private const float _asphaltTempValue = 0.95f;
        private const float _dirtTempValue = 0.35f;
        private const float _railsTempValue = 0.60f;
        private const float _pavementTempValue = 0.72f;
        //############

        //obiekt postawiony, wysokosc, wspolczynnik do obliczen, czy obiekt jest obiektem naturalnym
        private List<Tuple<Placement, float, float, bool>> _placementsExtends = new List<Tuple<Placement, float, float, bool>>();
        public List<Tuple<Placement, float, float, bool>> PlacementsExtends {get;}
        
        
        public void PlacementsExtendsSet(List<Placement> places)
        {
            foreach (Placement it in places)
            {
                (byte, byte) dimObj = it.GameObject.TileObject.Sprite.SpriteWidthHeight();
                

                if(it.GameObject.TileObject.Tag.subCategory == "bushes")
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, true));
                }
                else if (it.GameObject.TileObject.Tag.category == "coniferous_trees")
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, true));
                }
                else if (it.GameObject.TileObject.Tag.category == "decidous_trees") 
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, true));
                }
                else if (it.GameObject.TileObject.Tag.category == "miscellaneous")
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, true));
                    
                }
                else if (it.GameObject.TileObject.Tag.category == "small_trees")
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, false));
                }
                else
                {
                    _placementsExtends.Add(new Tuple<Placement, float, float, bool>(it, (int)(dimObj.Item2), it.GameObject.ChangeValue, false));
                }



            }
        }
        
        //Sortowanie
        public void SortPlacements()
        {
            //SORTOWANIE OBIEKTOW (Od lewego gornego do prawego dolnego)
            _placementsExtends.Sort((x, y) => 
            {
                int result = x.Item1.Position.Item1.CompareTo(y.Item1.Position.Item1);
                return result == 0 ? x.Item1.Position.Item2.CompareTo(y.Item1.Position.Item2) : result;
            });

        }

        public void ShadowedTiles()
        {

            foreach (Tuple<Placement, float, float, bool> it in _placementsExtends)
            {

                //slonce od strony lewej
                if (it.Item1.Position == (it.Item1.Position.x + 1, it.Item1.Position.y))
                {
                    continue;
                }
                else
                {

                    float tmpHeightCurrentObject = 0;
                    tmpHeightCurrentObject = it.Item2 * _dimensionField;

                    int iterator = 1;
                    int tmpShadowFieldsTemp = (int)((int)(tmpHeightCurrentObject * _shadowLength) % _dimensionField) > 5 ?
                        (int)(((tmpHeightCurrentObject * _shadowLength) / _dimensionField) + 1) :
                        (int)((tmpHeightCurrentObject * _shadowLength) / _dimensionField);


                    while ((iterator <= tmpShadowFieldsTemp) && (it.Item1.Position.x + iterator < BoardDensity))
                    {
                        shadowedTiles.Add(((it.Item1.Position.x + iterator), it.Item1.Position.y));
                        iterator++;
                    }
                }

            }
        }


        //OBSZARY CIEPLA
        public void AreaHeat()
        {
            warmerFields.Clear();
            coolerFields.Clear();

            for (int it = 0; it < _placementsExtends.Count; it++)
            {
                int distance;
                if (_placementsExtends[it].Item4)
                { 
                    //distance = (int)(_placementsExtends[it].Item2/_dimensionField)+1;
                    distance = (int)(_placementsExtends[it].Item2/2);
                    if (distance < 0)
                        distance = 0;

                    int startX = _placementsExtends[it].Item1.Position.Item1 - distance;
                    int startY = _placementsExtends[it].Item1.Position.Item2 - distance;


                    for (int i = 2*distance+1; i>0; i--) 
                    {
                        startX = _placementsExtends[it].Item1.Position.Item1 - distance;
                        for (int j = 2 * distance + 1; j > 0; j--) 
                        {
                            if ((startX >= 0 && startY >= 0) && (startX < BoardDensity && startY < BoardDensity)) {
                                if (startX  != _placementsExtends[it].Item1.Position.Item1 ||
                                    startY != _placementsExtends[it].Item1.Position.Item2)
                                {
                                    coolerFields.Add((startX, startY, _placementsExtends[it].Item3));
                                    //coolerFields.Add((startX, startY, _placementsExtends[it].Item1.GameObject.ChangeValue));
                                   
                                }
                            }

                            startX++;
                        }
                        startY++;
                    }
                }

                else 
                {
                    distance = ((int)((int)_solarRadiationDiffuse / (_dimensionField * _dimensionField))) + ((int) (_placementsExtends[it].Item2/2));
                    Console.WriteLine(distance);
                
                    for (int it2 = it + 1; it2 < _placementsExtends.Count; it2++)
                    {
                        //int distanceObjects = 0;

                        if (Math.Pow((_placementsExtends[it].Item1.Position.x) - _placementsExtends[it2].Item1.Position.x, 2)
                         + Math.Pow((_placementsExtends[it].Item1.Position.y) - _placementsExtends[it2].Item1.Position.y, 2) <= Math.Pow((distance + 1), 2) &&
                            _placementsExtends[it2].Item4 == false)
                        {
                            int xIle = Math.Abs(_placementsExtends[it].Item1.Position.x - _placementsExtends[it2].Item1.Position.x) != 0 ?
                                (Math.Abs(_placementsExtends[it].Item1.Position.x - _placementsExtends[it2].Item1.Position.x) - 1) :
                                (Math.Abs(_placementsExtends[it].Item1.Position.x - _placementsExtends[it2].Item1.Position.x));

                            int yIle = Math.Abs(_placementsExtends[it].Item1.Position.y - _placementsExtends[it2].Item1.Position.y) != 0 ?
                               (Math.Abs(_placementsExtends[it].Item1.Position.y - _placementsExtends[it2].Item1.Position.y) - 1) :
                               (Math.Abs(_placementsExtends[it].Item1.Position.y - _placementsExtends[it2].Item1.Position.y));



                            if (_placementsExtends[it].Item1.Position.x > _placementsExtends[it2].Item1.Position.x &&
                                _placementsExtends[it].Item1.Position.y > _placementsExtends[it2].Item1.Position.y)
                            {
                                xIle = xIle * (-1);
                                yIle = yIle * (-1);
                            }

                            else if (_placementsExtends[it].Item1.Position.x > _placementsExtends[it2].Item1.Position.x &&
                              _placementsExtends[it].Item1.Position.y < _placementsExtends[it2].Item1.Position.y)
                            {
                                xIle = xIle * (-1);
                            }

                            else if (_placementsExtends[it].Item1.Position.x < _placementsExtends[it2].Item1.Position.x &&
                              _placementsExtends[it].Item1.Position.y > _placementsExtends[it2].Item1.Position.y)
                            {
                                yIle = yIle * (-1);
                            }
                            
                            
                            while (Math.Abs(xIle) > 0 || Math.Abs(yIle) > 0)
                            {

                                if (!(_placementsExtends.Any(field => field.Item1.Position == (_placementsExtends[it].Item1.Position.x + xIle,
                                _placementsExtends[it].Item1.Position.y + yIle))))
                                {
                                    warmerFields.Add((_placementsExtends[it].Item1.Position.x + xIle, _placementsExtends[it].Item1.Position.y + yIle,
                                             _placementsExtends[it].Item3,distance));
                                            
                                }


                                if (xIle < 0)
                                {
                                    xIle++;
                                }

                                if (yIle < 0)
                                {
                                    yIle++;
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
            }
        }

        
        
        //sortowanie i usuwanie powtorzen
        public void WarmCoolerFieldsSort()
        {
            
            warmerFields.Sort((x, y) => {
                int result = x.Item2.CompareTo(y.Item2);
                return result == 0 ? x.Item1.CompareTo(y.Item1) : result;
            });
            
            
            warmerFields = warmerFields.Distinct().ToList();
            
            
            for (int i = 0; i < warmerFields.Count; i++)
            {
                int distanceTmp = warmerFields[i].Item4;
                for (int kk = 0; kk < warmerFields.Count-1; kk++)
                {
                    if (warmerFields[i].Item2 == warmerFields[kk].Item2 &&
                        (warmerFields[kk].Item1 - warmerFields[i].Item1 <= distanceTmp) &&
                        (warmerFields[kk].Item1 - warmerFields[i].Item1 > 0))
                    {
                        int tmp = warmerFields[kk].Item1 - warmerFields[i].Item1;
                        while (tmp > 0)
                        {
                            warmerFields2.Add(((warmerFields[i].Item1+tmp),warmerFields[i].Item2, warmerFields[i].Item3, distanceTmp));
                            tmp--;
                           
                        }
                    
                    }
                }
                
            }
            
            warmerFields.AddRange(warmerFields2);
            
            warmerFields.Sort((x, y) => {
                int result = x.Item2.CompareTo(y.Item2);
                return result == 0 ? x.Item1.CompareTo(y.Item1) : result;
            });
            
            warmerFields = warmerFields.Distinct().ToList();
            
            
            coolerFields.Sort((x, y) => {
                int result = x.Item1.CompareTo(y.Item1);
                return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
            });
            
            coolerFields = coolerFields.Distinct().ToList();

        }


        public void CalculateHeat() 
        {
 

            for (int i = 0; i < BoardDensity; i++)
            {
                for (int j = 0; j < BoardDensity; j++)
                {
                   float tempTemperature = AirTemperature;


                    foreach (Tuple<Placement, float, float, bool> it in _placementsExtends)
                    {
                        if (it.Item1.Position == (j, i))
                        {
                            if (it.Item4)
                            {
                                tempTemperature = _airTemperature + 1.0f * it.Item3 + it.Item1.GameObject.ChangeValue / 10;
                            }

                            else 
                            {
                                tempTemperature = _airTemperature + 3.0f * it.Item3 + it.Item1.GameObject.ChangeValue / 10;
                            }
                              
                        }
                    }


                    if (ITileBinder.TileGrid[i, j].GetName().Contains("Asphalt") ||
                        ITileBinder.TileGrid[i, j].GetName().Contains("Road"))
                    {

                        tempTemperature += 1.2f * _asphaltTempValue;

                    }

                    else if (ITileBinder.TileGrid[i, j].GetName().Contains("Dirt"))
                    {

                        tempTemperature += 1.0f * _dirtTempValue;

                    }
                    else if (ITileBinder.TileGrid[i, j].GetName().Contains("Rail"))
                    {
    
                        tempTemperature += 1.0f * _railsTempValue;

                    }
                    else if (ITileBinder.TileGrid[i, j].GetName().Contains("Pavement") &&
                             ITileBinder.TileGrid[i, j].GetName().Contains("pavement"))
                    {

                        tempTemperature += 1.0f * _pavementTempValue;

                    }
                    
                    
                    for (int a = 0; a < warmerFields.Count; a++)
                    {
                        if (warmerFields[a].Item1 == j && warmerFields[a].Item2 == i)
                        {
                            tempTemperature += (2.2f * warmerFields[a].Item3);
                        }
                    }


                    for (int a = 0; a < coolerFields.Count; a++)
                    {
                        if (coolerFields[a].Item1 == j && coolerFields[a].Item2 == i)
                        {
                            tempTemperature -= (2.2f *  coolerFields[a].Item3);
                            Console.WriteLine(coolerFields[a].Item3);
                           
                        }
                    }
           
                    for (int a = 0; a < shadowedTiles.Count; a++)
                    {
                        if (shadowedTiles[a].Item1 == j && shadowedTiles[a].Item2 == i)
                        {
                            tempTemperature = tempTemperature - 0.4f;
                        }
                    }


                    if (tempTemperature == 25.0f)
                    {
                        tempTemperature = AirTemperature;
                        avgFieldsTemp.Add(tempTemperature);
                    }
                    else 
                    {
                        avgFieldsTemp.Add(tempTemperature);
                       
                    }
                    

                }
            }

        }


        public void Sim()
        {
            shadowedTiles.Clear();
            warmerFields.Clear();
            coolerFields.Clear();
            avgFieldsTemp.Clear();
            _placementsExtends.Clear();

            placements = ITileBinder.PlacedObjects;

            PlacementsExtendsSet(placements);
            SortPlacements();
            ShadowedTiles();
            AreaHeat();
            WarmCoolerFieldsSort();
            CalculateHeat();

            float Result = 0;

            
            foreach (float it in avgFieldsTemp) {
                Result += it;
            }
            
            //Console.WriteLine("Ile avg " + avgFieldsTemp.Count);
            Result = ((Result / avgFieldsTemp.Count) + _airTemperature) / 2;
            Console.WriteLine("wynik: " + Result);
           // Console.WriteLine(BoardDensity);

        }
        
    }
}




