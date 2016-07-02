
using System;

namespace CoastCalculator
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
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
					perimeter+=SeaNeighbourPerimeter(x,y);
				}
			}
			
			return perimeter;
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
			if (cellX < 0 || cellX >= mapWidth ||
			    cellY < 0 || cellY >= mapHeight)
			{
				return true;
			}
			
			return !landCellsWidthByHeight[cellX][cellY];
		}
		
		private bool IsLand(int cellX, int cellY)
		{
			return landCellsWidthByHeight[cellX][cellY];
		}
	}
}