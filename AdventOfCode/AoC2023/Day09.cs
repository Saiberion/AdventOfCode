﻿using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2023
{
    public class Day09 : Day
    {
        public override void Solve()
        {
            long sumP1 = 0;
            List<List<long>> histories = new();

            foreach(string line in Input)
            {
                List<long> history = new();
                string[] splitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in splitted)
                {
                    history.Add(long.Parse(s));
                }
                histories.Add(history);
            }

            foreach(List<long> hist in histories)
            {
                List<List<long>> differenceTree = new() { hist };
                bool allZeroes = true;

                do
                {
                    allZeroes = true;
                    List<long> differences = new();
                    for (int i = 1; i < differenceTree[^1].Count; i++)
                    {
                        long diff = differenceTree[^1][i] - differenceTree[^1][i - 1];
                        if (diff != 0)
                        {
                            allZeroes = false;
                        }
                        differences.Add(diff);
                    }
                    differenceTree.Add(differences);
                } while (allZeroes == false);

                long newExtrapolate = 0;
                differenceTree[^1].Add(newExtrapolate);
                for (int t = differenceTree.Count - 2; t >= 0; t--)
                {
                    differenceTree[t].Add(differenceTree[t][^1] + differenceTree[t + 1][^1]);
                }
                sumP1 += differenceTree[0][^1];
            }

            Part1Solution = sumP1.ToString();

            Part2Solution = "TBD";
        }
    }
}
