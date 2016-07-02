
using System;

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
		private readonly int mapWidth;

		private readonly int mapHeight;

		private readonly bool[][] landCellsWidthByHeight;

		public CellMapCoastlineCalculator(int mapWidth, int mapHeight)
		{
			this.mapHeight = mapHeight;
			this.mapWidth = mapWidth;
			landCellsWidthByHeight = new bool[mapWidth][];
			for (int x = 0; x < mapWidth; x++)
			{
				landCellsWidthByHeight[x] = new bool[mapHeight];
			}
		}

		public void MarkLandAt(int cellX, int cellY)
		{
			landCellsWidthByHeight[cellX][cellY] = true;
		}
		
		public int GetPerimeter()
		{
			var perimeter = 0;
			for (int x = 0; x < mapWidth; x++)
			{
				for (int y = 0; y < mapHeight; y++)
				{
					perimeter += SeaNeighbourPerimeter(x, y);
				}
			}
			
			return perimeter;
		}

		private bool IsLake(int cellX, int cellY)
		{
			return IsLand(cellX - 1, cellY) &&
			IsLand(cellX, cellY - 1) &&
			IsLand(cellX + 1, cellY) &&
			IsLand(cellX, cellY + 1);
		}
		
		private int SeaNeighbourPerimeter(int cellX, int cellY)
		{
			if (!IsLand(cellX, cellY))
			{
				return 0;
			}
			
			var perimeter = 0;
			if (IsSea(cellX - 1, cellY))
			{
				perimeter++;
			}
			if (IsSea(cellX, cellY - 1))
			{
				perimeter++;
			}
			if (IsSea(cellX + 1, cellY))
			{
				perimeter++;
			}
			if (IsSea(cellX, cellY + 1))
			{
				perimeter++;
			}
		
			return perimeter;
		}
		
		private bool IsSea(int cellX, int cellY)
		{
			if (IsOutOfMap(cellX, cellY))
			{
				return true;
			}
			
			if (IsLake(cellX, cellY))
			{
				return false;
			}
			
			return !landCellsWidthByHeight[cellX][cellY];
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