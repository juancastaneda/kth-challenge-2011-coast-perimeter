
using System;
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
