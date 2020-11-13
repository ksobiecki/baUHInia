﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Resources;

namespace baUHInia.Playground.Logic.Loaders
{
    public class TileConfigReader
    {
        private const string ResourceDir = "pack://application:,,,/resources/";
        private readonly string[] _lines;

        public TileConfigReader(string configFileName)
        {
            StreamResourceInfo resourceStream =
                Application.GetResourceStream(new Uri(ResourceDir + configFileName));
            using (StreamReader reader = new StreamReader(resourceStream.Stream))
            {
                _lines = Regex.Split(reader.ReadToEnd(), "\r\n|\r|\n");
            }
        }

        public (byte index, sbyte x, sbyte y)[] ReadTileIndexesWithOffsets()
        {
            var tilesWithOffsets = new List<(byte index, sbyte x, sbyte y)>();
            if (byte.TryParse(_lines[0], out byte count))
            {
                for (sbyte i = 1; i <= count; i++)
                {
                    tilesWithOffsets.Add(LoadTileIndexWithOffsets(_lines[i].Normalize()));
                }
            }
            else throw new FileFormatException("Not a complex element config file.");

            return tilesWithOffsets.ToArray();
        }

        private static (byte index, sbyte x, sbyte y) LoadTileIndexWithOffsets(string line)
        {
            (byte index, sbyte x, sbyte y) tuple = (0, 0, 0);
            try
            {
                tuple.index = (byte) (byte.Parse(line.Substring(0, line.IndexOf('/'))) - 1);
                int slashIndex = line.IndexOf('/') + 1;
                tuple.x = sbyte.Parse(line.Substring(slashIndex, line.IndexOf(',') - slashIndex));
                tuple.y = sbyte.Parse(line.Substring(line.IndexOf(',') + 1));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw new FileFormatException();
            }

            return tuple;
        }
    }
}