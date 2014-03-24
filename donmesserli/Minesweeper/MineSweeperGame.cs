using System;
using System.Drawing;

namespace Minesweeper
{
	public class MineSweeperGame
	{
		public enum DrawType {
			Covered,
			Flagged,
			Questioned,
			Value,
			Mine
		};

		private Tile[,] tiles;
		private int currentScore = 0;
		private Random random = new Random();

		public delegate void DrawTiles();
		private Func<MineSweeperGame,bool> drawer;
		private bool gameOver;
		private bool gameWon;
		private int columns, rows;

		public MineSweeperGame (int columns, int rows, int mines, Func<MineSweeperGame,bool> adrawer)
		{
			this.columns = columns;
			this.rows = rows;

			drawer = adrawer;
			tiles = new Tile[columns, rows];

			for (int col = 0; col < columns; col++) {
				for (int row = 0; row < rows; row++) {
					tiles [col, row] = new Tile (col, row);
				}
			}

			// Place the mines
			int minesplaced = 0;

			do {
				Point p = RandomTile ();

				int col = p.X;
				int row = p.Y;

				if (!tiles [col, row].isMine()) {
					tiles [col, row].PlaceMine();
					minesplaced++;
				}
			} while (minesplaced < 10);

			// Calculate numbers for each tile
			for (int col = 0; col < columns; col++) {
				for (int row = 0; row < rows; row++) {
					Tile tile = tiles [col, row];
					if (!tile.isMine()) {
						tile.Value = CalculateTileValue (tile);
					}
				}
			}

			// Reset score
			currentScore = 0;

			drawer (this);
		}

		public bool isGameOver()
		{
			return gameOver;
		}

		public bool wasGameWon()
		{
			return gameWon;
		}

		public DrawType GetDrawType(int col, int row) {
			Tile tile = tiles [col, row];

			DrawType draw = DrawType.Covered;

			if (tile.isFlagged ()) {
				draw = DrawType.Flagged;
			} else if (tile.isCovered ()) {
				draw = DrawType.Covered;
			} else if (tile.isQuestioned ()) {
				draw = DrawType.Questioned;
			} else { // uncovered
				if (tile.isMine ()) {
					draw = DrawType.Mine;
				} else {
					draw = DrawType.Value;
				}
			}

			return draw;
		}

		public int GetTileValue(int col, int row)
		{
			return tiles [col, row].Value;
		}

		public int GetCurrentScore()
		{
			return currentScore;
		}

		public void FlagTile(int col, int row)
		{
			if (tiles [col, row].isCovered ()) {
				tiles [col, row].Flag ();
				drawer (this);
			} else if (tiles [col, row].isFlagged ()) {
				tiles [col, row].Question ();
				drawer (this);
			} else if (tiles [col, row].isQuestioned ()) {
				tiles [col, row].Flag ();
				drawer (this);
			}
		}

		public void UncoverTile(int col, int row)
		{
			Tile tile = tiles [col, row];

			if (tile.isCovered() || tile.isQuestioned () ) {
				tile.Uncover();

				if (tile.isMine()) {
					gameOver = true;
				} else {
					DoUncover (tile);
				}

				if (CheckForWin ())
					gameOver = true;

				drawer (this);
			}
		}

		// NOTE: All tiles except for mines must be uncovered
		private bool CheckForWin()
		{
			bool bWon = true;

			for (int col = 0; col < columns; col++) {
				for (int row = 0; row < rows; row++) {
					Tile tile = tiles [col, row];

					if (!tile.isMine() && !tile.isUncovered()) {
						bWon = false;
						break;
					}
				}
			}

			gameWon = bWon;
			return bWon;
		}

		private enum Neighbors {
			UpperLeft,
			Above,
			UpperRight,
			Left,
			Right,
			LowerLeft,
			Below,
			LowerRight
		};


		private Tile GetNeighbor(Tile target, Neighbors neighbor)
		{
			Tile tile = null;
			int col = target.col;
			int row = target.row;

			switch (neighbor) {
			case Neighbors.UpperLeft:
				if (col > 0 && row > 0) {
					tile = tiles [col - 1, row - 1];
				}
				break;

			case Neighbors.Above:
				if (row > 0) {
					tile = tiles [col, row - 1];
				}
				break;

			case Neighbors.UpperRight:
				if (col < 7 && row > 0) {
					tile = tiles [col + 1, row - 1];
				}
				break;

			case Neighbors.Left:
				if (col > 0) {
					tile = tiles [col - 1, row];
				}
				break;

			case Neighbors.Right:
				if (col < 7) {
					tile = tiles [col + 1, row];
				}
				break;

			case Neighbors.LowerLeft:
				if (col > 0 && row < 7) {
					tile = tiles [col - 1, row + 1];
				}
				break;

			case Neighbors.Below:
				if (row < 7) {
					tile = tiles [col, row + 1];
				}
				break;

			case Neighbors.LowerRight:
				if (col < 7 && row < 7) {
					tile = tiles [col + 1, row + 1];
				}
				break;
			}

			return tile;
		}

		private void DoNeighbor(Tile tile, Neighbors neighbor) {
			Tile target = GetNeighbor(tile, neighbor);

			if (target != null) {
				if (target.isCovered () && !target.isMine ()) {
					target.Uncover ();
					if (target.Value == 0) {
						DoUncover (target);
					}
				}
			}
		}

		private void DoUncover(Tile tile)
		{
			if (tile.isMine()) {
				return;
			}

			// Score 1 for this tile if it has a value of 0
			if (tile.Value == 0) {
				currentScore++;
			}

			// upper left			 
			DoNeighbor(tile, Neighbors.UpperLeft);

			// above
			DoNeighbor(tile, Neighbors.Above);

			// upper right
			DoNeighbor(tile, Neighbors.UpperRight);

			// left
			DoNeighbor(tile, Neighbors.Left);

			// right
			DoNeighbor(tile, Neighbors.Right);

			// lower left
			DoNeighbor(tile, Neighbors.LowerLeft);

			// below
			DoNeighbor(tile, Neighbors.Below);

			// lower right
			DoNeighbor(tile, Neighbors.LowerRight);
		}

		private Point RandomTile()
		{
			int col = random.Next(0, 8);
			int row = random.Next(0, 8);

			//Assert.IsTrue(col >= 0 && col < 8);
			//Assert.IsTrue(row >= 0 && row < 8);

			Point coords = new Point (col, row);

			return coords;
		}

		private int CalculateTileValue(Tile tile) {
			int value = 0;
			Tile target = null;

			// upper left
			target = GetNeighbor(tile, Neighbors.UpperLeft);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// above
			target = GetNeighbor(tile, Neighbors.Above);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// upper right
			target = GetNeighbor(tile, Neighbors.UpperRight);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// left
			target = GetNeighbor(tile, Neighbors.Left);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// right
			target = GetNeighbor(tile, Neighbors.Right);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// lower left
			target = GetNeighbor(tile, Neighbors.LowerLeft);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// below
			target = GetNeighbor(tile, Neighbors.Below);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			// lower right
			target = GetNeighbor(tile, Neighbors.LowerRight);
			if (target != null) {
				if (target.isMine()) {
					value++;
				}
			}

			return value;
		}

		private class Tile
		{
			private enum TileState {
				Flagged,
				Covered,
				Questioned,
				Uncovered
			};
				
			private  static int TileValueMine = 0xFFFF;

			public Tile(int c, int r) {
				col = c;
				row = r;
				state = TileState.Covered;
			}

			private TileState state;
			private int value { get; set; }
			public int col;
			public int row;

			public bool isMine() {
				return (value == TileValueMine);
			}

			public bool isCovered() {
				return (state == TileState.Covered);
			}

			public bool isUncovered() {
				return (state == TileState.Uncovered);
			}

			public bool isFlagged() {
				return (state == TileState.Flagged);
			}

			public bool isQuestioned() {
				return (state == TileState.Questioned);
			}

			public void Flag() {
				state = TileState.Flagged;
			}

			public void Unflag() {
				state = TileState.Covered;
			}

			public void Uncover() {
				state = TileState.Uncovered;
			}

			public void Question() {
				state = TileState.Questioned;
			}

			public void PlaceMine() {
				value = TileValueMine;
			}

			public int Value {
				get {
					return value;
				}

				set {
					this.value = value;
				}
			}
		}
	}
}

