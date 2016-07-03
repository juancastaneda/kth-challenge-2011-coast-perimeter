
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
			var row = 0;
			string line;	
			while ((line = Console.ReadLine()) != null && row < height - 1)
			{
				if (calculator == null)
				{
					string[] split = line.Split(new [] { ' ' }, StringSplitOptions.None);
					width = int.Parse(split[1]);
					height = int.Parse(split[0]);	
					calculator = new CellMapCoastlineCalculator(width, height);
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
		private readonly int[] neighboursX = new int[]{ -1, 0, 1, 0 };
		private readonly int[] neighboursY = new int[]{ 0, -1, 0, 1 };
		
		private readonly int mapWidth;
		private readonly int mapHeight;

		private readonly bool[][] landCellsWidthByHeight;
		private readonly bool[][] seaCellsWidthByHeight;
		bool showMap;
		
		public CellMapCoastlineCalculator(int mapWidth, int mapHeight, bool showMap = false)
		{
			this.showMap = showMap;
			this.mapHeight = mapHeight;
			this.mapWidth = mapWidth;
			landCellsWidthByHeight = new bool[mapWidth][];
			seaCellsWidthByHeight = new bool[mapWidth][];
			for (int x = 0; x < mapWidth; x++)
			{
				landCellsWidthByHeight[x] = new bool[mapHeight];
				seaCellsWidthByHeight[x] = new bool[mapHeight];
			}
		}

		public void MarkLandAt(int cellX, int cellY)
		{
			landCellsWidthByHeight[cellX][cellY] = true;
		}
		
		public int GetPerimeter()
		{
			var perimeter = 0;
			CalculateSeaCells();
			ShowMap();
			
			for (int x = 0; x < mapWidth; x++)
			{
				for (int y = 0; y < mapHeight; y++)
				{
					perimeter += SeaNeighbourPerimeter(x, y);
				}
			}
			
			return perimeter;
		}

		void ShowMap()
		{
			if (!showMap)
			{
				return;
			}
			
			Console.WriteLine("map");
			for (int x = 0; x < mapWidth; x++)
			{
				var land = new System.Text.StringBuilder();
				var sea = new System.Text.StringBuilder();
				for (int y = 0; y < mapHeight; y++)
				{
					land.Append(landCellsWidthByHeight[x][y] ? '1' : '0');
					sea.Append(seaCellsWidthByHeight[x][y] ? 'X' : '-');
				}
				
				Console.WriteLine("{0}\t{1}", land, sea);
			}
	
			Console.WriteLine();
		}

		private void CalculateSeaCells()
		{
			for (int x = 0; x < mapWidth; x++)
			{
				for (int y = 0; y < mapHeight; y++)
				{
					seaCellsWidthByHeight[x][y] = HasWayToSea(x, y);
				}
			}
		}
		
		
		private bool HasWayToSea(int x, int y)
		{
			if (IsLand(x, y))
			{
				return false;
			}
			
			if (HasOutsideNeighbours(x, y) || HasSeaNeighbours(x, y))
			{
				return true;
			}

			var q = new Queue<Tuple<int,int>>();
			q.Enqueue(Tuple.Create(x, y));
			var visited = new List<Tuple<int,int>>();
			while (q.Count != 0)
			{
				var e = q.Dequeue();
				var cellX = e.Item1;
				var cellY = e.Item2;
				if (IsLand(cellX, cellY))
				{
					continue;
				}
				
				if (seaCellsWidthByHeight[cellX][cellY])
				{
					return true;
				}

				if (HasOutsideNeighbours(cellX, cellY))
				{
					for (int i = 0; i < visited.Count; i++)
					{
						var v = visited[i];
						seaCellsWidthByHeight[v.Item1][v.Item2] = true;
					}
					
					return true;
				}

				visited.Add(e);
				for (int i = 0; i < 4; i++)
				{
					var nX = cellX + neighboursX[i];
					var nY = cellY + neighboursY[i];
					if (!IsLand(nX, nY))
					{
						var tuple = Tuple.Create(nX, nY);
						if (!visited.Contains(tuple))
						{
							q.Enqueue(tuple);
						}
					}
				}

			}
			
			return false;
		}
		
		private bool HasOutsideNeighbours(int x, int y)
		{
			for (int i = 0; i < 4; i++)
			{
				var nX = x + neighboursX[i];
				var nY = y + neighboursY[i];
				if (IsOutOfMap(nX, nY))
				{
					return true;
				}
			}
			
			return false;
		}
		
		private bool HasSeaNeighbours(int x, int y)
		{
			for (int i = 0; i < 4; i++)
			{
				var nX = x + neighboursX[i];
				var nY = y + neighboursY[i];
				if (seaCellsWidthByHeight[nX][nY])
				{
					return true;
				}
			}
			
			return false;
		}
		
		private bool IsLake(int cellX, int cellY)
		{
			return !IsLand(cellX, cellY) &&
			!seaCellsWidthByHeight[cellX][cellY];
		}
		
		private int SeaNeighbourPerimeter(int cellX, int cellY)
		{
			if (!IsLand(cellX, cellY))
			{
				return 0;
			}
			
			var perimeter = 0;
			for (int i = 0; i < 4; i++)
			{
				var nX = cellX + neighboursX[i];
				var nY = cellY + neighboursY[i];
				if (IsSea(nX, nY))
				{
					perimeter++;
				}
			}
			
			return perimeter;
		}
		
		private bool IsSea(int cellX, int cellY)
		{
			if (IsOutOfMap(cellX, cellY))
			{
				return true;
			}
			
			if (IsLand(cellX, cellY))
			{
				return false;
			}
			
			if (IsLake(cellX, cellY))
			{
				return false;
			}
			
			return true;
		}
		
		private bool IsOutOfMap(int cellX, int cellY)
		{
			return cellX < 0 || cellX >= mapWidth ||
			cellY < 0 || cellY >= mapHeight;
		}
		
		private bool IsLand(int cellX, int cellY)
		{
			if (IsOutOfMap(cellX, cellY))
			{
				return false;
			}
			
			return landCellsWidthByHeight[cellX][cellY];
		}
	}
}