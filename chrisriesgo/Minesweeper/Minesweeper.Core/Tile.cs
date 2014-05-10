using System;

namespace Minesweeper.Core
{
	public class Tile
	{
		public Tile() {	}

		public int X { get; set; }

		public int Y { get; set; }

		public int Neighbors { get; set; }

		public int Position { get; set; }

		public bool Flagged { get; set; }
	}
}

