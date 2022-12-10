﻿using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2022
{
    public class Day07 : Day
    {
        public override void Solve()
        {
            FilesystemDirectory rootDir = new FilesystemDirectory
            {
                Name = "/",
                Parent = null
            };
            FilesystemDirectory currentDir = rootDir;

            foreach (string s in Input)
            {
                string[] splitted = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted[0].Equals("$"))
                {
                    // command input
                    if (splitted[1].Equals("cd"))
                    {
                        if (splitted[2].Equals("/"))
                        {
                            currentDir = rootDir;
                        }
                        else if (splitted[2].Equals(".."))
                        {
                            currentDir = currentDir.Parent;
                        }
                        else
                        {
                            currentDir = currentDir.dirs[splitted[2]];
                        }
                    }
                }
                else if (splitted[0].Equals("dir"))
                {
                    FilesystemDirectory dir = new()
                    {
                        Name = splitted[1],
                        Parent = currentDir
                    };

                    currentDir.dirs.Add(splitted[1], dir);
                }
                else
                {
                    FilesystemFile file = new();
                    file.Name = splitted[1];
                    file.Size = int.Parse(splitted[0]);
                    currentDir.files.Add(file);
                    AddUpSizeToAllParents(currentDir, file.Size);
                }
            }

            Part1Solution = FindMaxTotalSize(rootDir, 100000).ToString();

            Part2Solution = FindClosestTotalSize(rootDir, 30000000 - (70000000 - rootDir.TotalSize), rootDir.TotalSize).ToString();
        }

        public void AddUpSizeToAllParents(FilesystemDirectory dir, int filesize)
        {
            dir.TotalSize += filesize;
            if (dir.Parent != null)
            {
                AddUpSizeToAllParents(dir.Parent, filesize);
            }
        }

        public int FindMaxTotalSize(FilesystemDirectory dir, int filesize)
        {
            int totalsize = 0;

            if (dir.TotalSize < filesize)
            {
                totalsize = dir.TotalSize;
            }

            foreach(FilesystemDirectory d in dir.dirs.Values)
            {
                totalsize += FindMaxTotalSize(d, filesize);
            }

            return totalsize;
        }

        public int FindClosestTotalSize(FilesystemDirectory dir, int filesize, int closestSize)
        {
            if ((dir.TotalSize > filesize) && (dir.TotalSize < closestSize))
            {
                closestSize = dir.TotalSize;
            }

            foreach (FilesystemDirectory d in dir.dirs.Values)
            {
                closestSize = FindClosestTotalSize(d, filesize, closestSize);
            }

            return closestSize;
        }
    }

    public class FilesystemFile
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class FilesystemDirectory
    {
        public Dictionary<string, FilesystemDirectory> dirs = new();
        public List<FilesystemFile> files = new();
        public string Name;
        public FilesystemDirectory Parent;
        public int TotalSize = 0;
    }
}
