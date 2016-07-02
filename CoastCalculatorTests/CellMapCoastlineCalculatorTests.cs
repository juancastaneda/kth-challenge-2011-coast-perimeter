
using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace CoastCalculator
{
	/// <summary>
	/// Description of RectangularCoastlineCalculatorTests.
	/// </summary>
	[TestFixture]
	public class CellMapCoastlineCalculatorTests
	{
		[Test]
		public void Can_start_with_no_coastline_0_width_height()
		{
			var fixture = new Fixture()
				.WithMapHeight(0)
				.WithMapWidth(0);
			var sut = fixture.CreateSUT();
			
			var actual = sut.GetPerimeter();
		
			const int expected = 0;
			Assert.AreEqual(expected, actual, "perimeter");
		}
		
		[TestCase(20, 50)]
		[TestCase(4, 5)]
		[TestCase(220, 450)]
		[TestCase(1, 1000)]
		[TestCase(1000, 1)]
		public void Can_start_with_no_coastline_rectangle_width_height(int width, int height)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			var actual = sut.GetPerimeter();
		
			const int expected = 0;
			Assert.AreEqual(expected, actual, "perimeter");
		}
		
		[TestCase(10, 10, 3, 4)]
		[TestCase(5, 6, 0, 0)]
		[TestCase(1, 10, 0, 9)]
		[TestCase(10, 1, 9, 0)]
		public void Can_calculate_perimeter_for_one_cell(int width, int height, int cellX, int cellY)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			sut.MarkLandAt(cellX, cellY);
			var actual = sut.GetPerimeter();
		
			const int expected = 1 * 4;
			Assert.AreEqual(expected, actual, "perimeter");
		}
		
		[TestCase(10, 10)]
		[TestCase(5, 6)]
		[TestCase(1, 10)]
		[TestCase(10, 1)]
		[TestCase(1, 1)]
		public void Can_calculate_perimeter_when_all_are_land_cells(int width, int height)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					sut.MarkLandAt(x, y);
				}
			}
			
			var actual = sut.GetPerimeter();
			var expected = (height * 2) + (width * 2);
			
			Assert.AreEqual(expected, actual, "perimeter");
		}
		
		[TestCase(5, 4, 10, 1, 1, 2, 1, 3, 1, 2, 2)]
		[TestCase(5, 4, 10, 1, 1, 2, 1, 3, 1, 1, 2)]
		[TestCase(5, 4, 14, 1, 1, 2, 1, 3, 1, 1, 2, 3, 2, 1, 3)]
		[TestCase(4, 3, 10, 1, 1, 2, 1, 3, 1, 2, 2)]
		[TestCase(4, 3, 10, 1, 1, 2, 1, 3, 1, 1, 2)]
		[TestCase(4, 4, 14, 1, 1, 2, 1, 3, 1, 1, 2, 3, 2, 1, 3)]
		[TestCase(6, 5, 24, 1, 1, 2, 1, 3, 1, 4, 1, 5, 1, 1, 2, 2, 2, 3, 2, 1, 3, 1, 4, 2, 4, 3, 4, 4, 4)]
		public void Can_calculate_perimeter_shape(int height, int width, int expectedPerimeter, params int[] shapeCells)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			foreach (var xy in shapeCells
			         .Select((n,i) => new {n,i})
			         .GroupBy(n => n.i/2)
			         .Select(g=>new {x=g.First().n,y=g.Last().n}))
			{
				sut.MarkLandAt(xy.y, xy.x);
			}
			
			var actual = sut.GetPerimeter();
			
			Assert.AreEqual(expectedPerimeter, actual, "perimeter");
		}
		
		[TestCase(6, 5, 24, 1, 1, 2, 1, 1, 2, 2, 2, 4, 1, 1, 4, 2, 4, 3, 4, 4, 4, 5, 4)]
		[TestCase(4, 4, 20, 0, 0, 1, 0, 0, 1, 1, 1, 3, 0, 2, 2, 0, 3)]
		public void Can_calculate_perimeter_on_islands(int height, int width, int expectedPerimeter, params int[] shapeCells)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			foreach (var xy in shapeCells
			         .Select((n,i) => new {n,i})
			         .GroupBy(n => n.i/2)
			         .Select(g=>new {x=g.First().n,y=g.Last().n}))
			{
				sut.MarkLandAt(xy.y, xy.x);
			}
			
			var actual = sut.GetPerimeter();
			
			Assert.AreEqual(expectedPerimeter, actual, "perimeter");
		}
		
		[TestCase(5, 4, 12, 1, 1, 2, 1, 3, 1, 1, 2, 3, 2, 1, 3, 2, 3)]
		public void Can_calculate_with_lakes(int height, int width, int expectedPerimeter, params int[] shapeCells)
		{
			var fixture = new Fixture()
				.WithMapHeight(height)
				.WithMapWidth(width);
			var sut = fixture.CreateSUT();
			
			foreach (var xy in shapeCells
			         .Select((n,i) => new {n,i})
			         .GroupBy(n => n.i/2)
			         .Select(g=>new {x=g.First().n,y=g.Last().n}))
			{
				sut.MarkLandAt(xy.y, xy.x);
			}
			
			var actual = sut.GetPerimeter();
			
			Assert.AreEqual(expectedPerimeter, actual, "perimeter");
		}
		
		private sealed class Fixture
		{
			private int mapWidth;
			private int mapHeight;
			
			public Fixture WithMapHeight(int value)
			{
				mapHeight = value;
				return this;
			}
				
			public Fixture WithMapWidth(int value)
			{
				mapWidth = value;
				return this;
			}
			
			public CellMapCoastlineCalculator CreateSUT()
			{
				return new CellMapCoastlineCalculator(mapWidth, mapHeight);
			}
		}
	}
}
