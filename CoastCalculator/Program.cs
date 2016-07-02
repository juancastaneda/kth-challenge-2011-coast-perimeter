
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

		private readonly int[] coastPointX;

		private readonly int[] coastPointY;

		private int lastCoastPointIndex = -1;

		public CellMapCoastlineCalculator(int mapWidth, int mapHeight)
		{
			this.mapHeight = mapHeight;
			this.mapWidth = mapWidth;
			coastPointX = new int[mapHeight + 1];
			coastPointY = new int[mapWidth + 1];
		}

		public void MarkLandAt(int cellX, int cellY)
		{lastCoastPointIndex++;
			//AddPointsForCell(cellX, cellY);
		}

		public int GetPerimeter()
		{
			if (lastCoastPointIndex >= 0)
			{
				return 4;
			}
			
			return 0;
		}

		private void AddPointForCell(int cellX, int cellY, Corner corner)
		{
			lastCoastPointIndex++;
			switch (corner)
			{
				case Corner.UpperLeft:
					coastPointX[lastCoastPointIndex] = cellX;
					coastPointY[lastCoastPointIndex] = cellY;
					break;
				case Corner.UpperRight:
					coastPointX[lastCoastPointIndex] = cellX;
					coastPointY[lastCoastPointIndex] = cellY + 1;
					break;
				case Corner.LowerLeft:
					coastPointX[lastCoastPointIndex] = cellX + 1;
					coastPointY[lastCoastPointIndex] = cellY;
					break;
				case Corner.LowerRigth:
					coastPointX[lastCoastPointIndex] = cellX + 1;
					coastPointY[lastCoastPointIndex] = cellY + 1;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		private enum Corner
		{
			UpperLeft,
			UpperRight,
			LowerLeft,
			LowerRigth
		}
	}

}