﻿using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2017
{
	public class Day13 : Day
	{
		class FirewallLayer
		{
			public int Depth { get; set; }
			private int scannerPosition;
			private int direction;

			public FirewallLayer(int depth)
			{
				this.Depth = depth;
				scannerPosition = 1;
				direction = 1;
			}

			public void MoveScanner()
			{
				scannerPosition += direction;
				if (this.scannerPosition == this.Depth)
				{
					direction = -1;
				}
				else if (this.scannerPosition == 1)
				{
					direction = 1;
				}
			}

			public void UndoMoveScanner()
			{
				if (this.scannerPosition == this.Depth)
				{
					direction = 1;
				}
				else if (this.scannerPosition == 1)
				{
					direction = -1;
				}
				scannerPosition -= direction;
			}

			public void ResetScanner()
			{
				scannerPosition = 1;
				direction = 1;
			}

			public bool Detected()
			{
				return scannerPosition == 1;
			}
		}

		public override void Solve()
		{
			Dictionary<int, FirewallLayer> firewall = new();
			int packetLayerPosition = -1;
			int severity = 0;
			int maxKeyValue = int.MinValue;
			int startDelay = 0;
			int delay = 0;

			foreach (string line in Input)
			{
				string[] s = line.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
				int keyVal = int.Parse(s[0]);
				firewall.Add(keyVal, new FirewallLayer(int.Parse(s[1])));
				if (keyVal > maxKeyValue)
				{
					maxKeyValue = keyVal;
				}
			}

			for (int i = 0; i < (maxKeyValue + 1); i++)
			{
				// Move packet to next layer
				packetLayerPosition++;
				// check if caught
				if (firewall.ContainsKey(packetLayerPosition))
				{
					if (firewall[packetLayerPosition].Detected())
					{
						severity += packetLayerPosition * firewall[packetLayerPosition].Depth;
					}
				}
				// Move all scanners
				foreach (FirewallLayer f in firewall.Values)
				{
					f.MoveScanner();
				}
			}

			packetLayerPosition = -1;
			// Reset all scanners
			foreach (FirewallLayer f in firewall.Values)
			{
				f.ResetScanner();
				packetLayerPosition = -1;
			}

			while (packetLayerPosition <= maxKeyValue)
			{
				// Move packet to next layer after delay
				if (delay > 0)
				{
					delay--;
				}
				else
				{
					packetLayerPosition++;
				}
				// check if caught
				if (firewall.ContainsKey(packetLayerPosition))
				{
					if (firewall[packetLayerPosition].Detected())
					{
						startDelay++;
						delay = 1;

						while (packetLayerPosition > 0)
						{
							// Undo scanner steps
							foreach (FirewallLayer f in firewall.Values)
							{
								f.UndoMoveScanner();
							}
							packetLayerPosition--;
						}
						packetLayerPosition--;
						continue;
					}
				}
				// Move all scanners
				foreach (FirewallLayer f in firewall.Values)
				{
					f.MoveScanner();
				}
			}

			Part1Solution = severity.ToString();
			Part2Solution = startDelay.ToString();
		}
	}
}
