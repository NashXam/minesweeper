using System;
using System.Collections.Generic;
using System.Linq;

namespace Minesweeper.Core
{
	public class Helpers
	{
		public static Func<List<Mine>, int, int, int> CountNeighbors = ((mines, x, y) =>
		{
			return mines.Where(m => 
				m.X == x-1 && m.Y == y-1 ||
				m.X == x-1 && m.Y == y ||
				m.X == x-1 && m.Y == y+1 ||
				m.X == x && m.Y == y-1 ||
				m.X == x && m.Y == y+1 ||
				m.X == x+1 && m.Y == y-1 ||
				m.X == x+1 && m.Y == y ||
				m.X == x+1 && m.Y == y+1
			).Count();
		});

		public static IList<Tile> Neighbors (Tile tile, int size)
		{
			var neighbors = new List<Tile>();
			// Top Left
			neighbors.Add(new Tile { X = tile.X - 1, Y = tile.Y - 1, Position = tile.Position - (size + 1) });
			// Left
			neighbors.Add(new Tile { X = tile.X - 1, Y = tile.Y, Position = tile.Position - 1 });
			// Bottom Left
			neighbors.Add(new Tile { X = tile.X - 1, Y = tile.Y + 1, Position = tile.Position + (size - 1) });
			// Top
			neighbors.Add(new Tile { X = tile.X, Y = tile.Y - 1, Position = tile.Position - size });
			// Bottom
			neighbors.Add(new Tile { X = tile.X, Y = tile.Y + 1, Position = tile.Position + size });
			// Top Right
			neighbors.Add(new Tile { X = tile.X + 1, Y = tile.Y - 1, Position = tile.Position - (size - 1) });
			// Right
			neighbors.Add(new Tile { X = tile.X + 1, Y = tile.Y, Position = tile.Position + 1 });
			// Bottom Right
			neighbors.Add(new Tile { X = tile.X + 1, Y = tile.Y + 1, Position = tile.Position + (size + 1) });
			neighbors.RemoveAll(t => t.X < 0 || t.Y < 0 || t.X >= size || t.Y >= size);
			return neighbors;
		}
	}
}

