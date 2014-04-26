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
	}
}

