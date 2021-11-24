﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode._2019
{
    public class Day08 : Day
    {
        public Day08()
        {
            Load("2019/inputs/day08.txt");
        }

        override public void Solve()
        {
            SIF sif = new SIF(Input[0], 25, 6);
            int layerIndexLeastZeroes = sif.GetLayerWithLeastDigitCount('0');
            Part1Solution = sif.GetCheckMultiply(layerIndexLeastZeroes, '1', '2').ToString();
            int[,] message = sif.Render();
            
            for (int h = 0; h < sif.Height; h++)
            {
                for (int w = 0; w < sif.Width; w++)
                {
                    if (message[w, h] == 1)
                    {
                        System.Diagnostics.Debug.Write("#"); 
                    }
                    else
                    {
                        System.Diagnostics.Debug.Write(" ");
                    }
                }
                System.Diagnostics.Debug.WriteLine("");
            }
            Part2Solution = "See debug output";
        }
    }

    class SIF
    {
        public List<SIFLayer> Layers { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SIF(string fileContent, int width, int height)
        {
            Layers = new List<SIFLayer>();
            int fileIndex = 0;
            do
            {
                Layers.Add(new SIFLayer(fileContent.Substring(fileIndex, width * height)));
                fileIndex += width * height;
            } while (fileIndex < fileContent.Length);
            Width = width;
            Height = height;
        }

        public int GetLayerWithLeastDigitCount(char digit)
        {
            int minLayerNumber = -1;
            int minDigitCount = int.MaxValue;
            for (int i = 0; i < Layers.Count; i++)
            {
                if (Layers[i].Statistics[digit] < minDigitCount)
                {
                    minDigitCount = Layers[i].Statistics[digit];
                    minLayerNumber = i;
                }
            }
            return minLayerNumber;
        }

        public int GetCheckMultiply(int layerIndex, char dig1, char dig2)
        {
            return Layers[layerIndex].Statistics[dig1] * Layers[layerIndex].Statistics[dig2];
        }

        public int[,] Render()
        {
            int[,] result = new int[Width, Height];
            int idx = 0;

            for(int h = 0; h < Height; h++)
            {
                for (int w = 0; w < Width; w++)
                {
                    result[w, h] = Layers[0].Content[idx++] - 0x30;
                }
            }

            for (int l = 1; l < Layers.Count; l++)
            {
                idx = 0;
                for (int h = 0; h < Height; h++)
                {
                    for (int w = 0; w < Width; w++)
                    {
                        if (result[w, h] == 2)
                        {
                            result[w, h] = Layers[l].Content[idx] - 0x30;
                        }
                        idx++;
                    }
                }
            }

            return result;
        }
    }

    class SIFLayer
    {
        public string Content { get; set; }
        public Dictionary<char, int> Statistics { get; set; }

        public SIFLayer(string content)
        {
            Statistics = new Dictionary<char, int>
            {
                { '0', 0 },
                { '1', 0 },
                { '2', 0 }
            };
            Content = content;
            foreach (char c in Content)
            {
                if (Statistics.ContainsKey(c))
                {
                    Statistics[c]++;
                }
                else
                {
                    Statistics.Add(c, 1);
                }
            }
        }
    }
}
