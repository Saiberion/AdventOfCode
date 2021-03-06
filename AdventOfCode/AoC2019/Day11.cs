using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace AoC2019
{
    public class Day11 : Day
    {
        override public void Solve()
        {
            Queue<long> input = new Queue<long>();
            Queue<long> output = new Queue<long>();
            IntCodeComputer ic = new IntCodeComputer(Input[0]);
            PaintingRobot pr = new PaintingRobot();
            Dictionary<Point, long> hull = new Dictionary<Point, long>();

            ic.ExecuteAsync(input, output);

            while(!ic.IsProgramHalted())
            {
                if (!hull.ContainsKey(pr.Position))
                {
                    hull.Add(new Point(pr.Position.X, pr.Position.Y), 0);
                }

                input.Enqueue(hull[pr.Position]);

                while (output.Count < 2)
                {
                    if (ic.IsProgramHalted())
                    {
                        break;
                    }
                }
                if (output.Count == 2)
                {
                    hull[pr.Position] = output.Dequeue();
                    pr.Turn(output.Dequeue());
                    pr.Move();
                }
            }
            Part1Solution = hull.Count.ToString();

            hull.Clear();
            input.Clear();
            output.Clear();
            pr.Position = new Point(0, 0);
            ic.ExecuteAsync(input, output);
            hull.Add(new Point(0, 0), 1);
            while (!ic.IsProgramHalted())
            {
                if (!hull.ContainsKey(pr.Position))
                {
                    hull.Add(new Point(pr.Position.X, pr.Position.Y), 0);
                }

                input.Enqueue(hull[pr.Position]);

                while (output.Count < 2)
                {
                    if (ic.IsProgramHalted())
                    {
                        break;
                    }
                }
                if (output.Count == 2)
                {
                    hull[pr.Position] = output.Dequeue();
                    pr.Turn(output.Dequeue());
                    pr.Move();
                }
            }
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach(Point p in hull.Keys)
            {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }

            List<List<int>> disp = new List<List<int>>();
            for (int x = minX; x <= maxX; x++)
            {
                List<int> line = new List<int>();
                for (int y = maxY - 1; y > minY + 1; y--) 
                {
                    Point p = new Point(x, y);
                    if (hull.ContainsKey(p) && (hull[new Point(x, y)] == 1))
                    {
                        line.Add(1);
                    }
                    else
                    {
                        line.Add(0);
                    }
                }
                disp.Add(line);
            }

            int[,] display = new int[disp[0].Count, disp.Count];
            for (int y = 0; y < disp.Count; y++)
            {
                for(int x = 0; x < disp[y].Count; x++)
                {
                    display[x, y] = disp[y][x];
                }
            }

            Part2Solution = DotmatrixToString.Render(display);
        }
    }

    class PaintingRobot
    {
        public Point Position { get; set; }
        public int Direction { get; set; }

        public PaintingRobot()
        {
            Position = new Point();
            Direction = 0;
        }

        public void Turn(long dir)
        {
            if (dir == 0)
            {
                if (Direction == 0)
                {
                    Direction = 3;
                }
                else
                {
                    Direction--;
                }
            }
            else
            {
                if (Direction == 3)
                {
                    Direction = 0;
                }
                else
                {
                    Direction++;
                }
            }
        }

        public void Move()
        {
            switch(Direction)
            {
                case 0:
                    Position = new Point(Position.X, Position.Y - 1);
                    break;
                case 1:
                    Position = new Point(Position.X + 1, Position.Y);
                    break;
                case 2:
                    Position = new Point(Position.X, Position.Y + 1);
                    break;
                case 3:
                    Position = new Point(Position.X - 1, Position.Y);
                    break;
            }
        }
    }
}
