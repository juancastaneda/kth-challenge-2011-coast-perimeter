
using System;
using System.Collections.Generic;

namespace CoastCalculator
{
	class Program
	{
		public static void Main(string[] args)
		{
			const char land = '1';
			CellMapCoastlineCalculator calculator = null;
			int height = 2;
			int width = 0;
			var row = -1;
			string line;
			while ((row < height) && (line = Console.ReadLine()) != null)
			{
				if (calculator == null)
				{
					string[] split = line.Split(new [] { ' ' }, StringSplitOptions.None);
					width = int.Parse(split[1]);
					height = int.Parse(split[0]);
					calculator = new CellMapCoastlineCalculator(width, height);
					row = 0;
				}
				else
				{
					for (int column = 0; column < width; column++)
					{
						if (line[column] == land)
						{
							calculator.MarkLandAt(column, row);
						}
					}
					
					row++;
				}
			}
			
			Console.WriteLine(calculator.GetPerimeter());
		}
	}
	
	/// <summary>
	/// Map by using cells, each of same size.
	/// Height and width given by the number of cells
	/// </summary>
	public class CellMapCoastlineCalculator
	{
		private readonly int[] neighboursX = new int[]{ 0, 1, 0, -1 };
		private readonly int[] neighboursY = new int[]{ 1, 0, -1, 0 };
		
		// has padding
		private readonly bool?[][] cells;
		private readonly int mapWidth;
		private readonly int mapHeight;
		public CellMapCoastlineCalculator(int mapWidth, int mapHeight)
		{
			this.mapHeight = mapHeight;
			this.mapWidth = mapWidth;
			cells = new bool?[mapWidth + 2][];
			var yLength = cells.Length;
			for (int x = 0; x < yLength; x++)
			{
				var ys = new bool?[mapHeight + 2];
				cells[x] = ys;
				var xLength = cells[x].Length;
				for (int i = 0; i < xLength; i++)
				{
					ys[i] = false;
				}
			}
		}

		public void MarkLandAt(int cellX, int cellY)
		{
			cells[cellX + 1][cellY + 1] = true;
		}
		
		public int GetPerimeter()
		{
			var q = new List<int>();
			q.Add(0);
			q.Add(0);
			cells[0][0] = null;

			var perimeter = 0;
			while (q.Count != 0)
			{
				var x = q[0];
				q.RemoveAt(0);
				var y = q[0];
				q.RemoveAt(0);
				for (int neighbour = 0; neighbour < 4; neighbour++)
				{
					int neighbourX = x + neighboursX[neighbour];
					int neighbourY = y + neighboursY[neighbour];
					if (neighbourX < 0 || neighbourX >= mapWidth + 2 ||
					    neighbourY < 0 || neighbourY >= mapHeight + 2)
					{
						continue;
					}
					
					var neighbourCell = cells[neighbourX][neighbourY];
					if (neighbourCell.HasValue && neighbourCell.Value)
					{
						perimeter++;
					}
					
					if (!neighbourCell.HasValue || neighbourCell.Value)
					{
						continue;
					}
					
					cells[neighbourX][neighbourY] = null;
					q.Add(neighbourX);
					q.Add(neighbourY);
				}
			}
			
			return perimeter;
		}
	}
}