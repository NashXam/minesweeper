using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper
{
	public partial class MinesweeperViewController : UIViewController
	{
		private static int NUMROWS = 8;
		private static int NUMCOLS = 8;

		private static int STARTX = 20;
		private static int STARTY = 170;
		private static int TILESIZE = 33;
		private static int GAPSIZE = 2;

		private static int NUMMINES = 10;

		private static int MINE = 0xFFFF;

		private int highScore = 0;
		private int currentScore = 0;

		private UIImageView[,] tile = new UIImageView[NUMCOLS, NUMROWS];
		private Boolean[,] tileCovered = new Boolean[NUMCOLS, NUMROWS];
		private Boolean[,] tileFlagged = new Boolean[NUMCOLS, NUMROWS];
		private int[,] tileState = new int[NUMCOLS, NUMROWS];

		private UIImage blankTile;
		private UIImage mineTile;
		private UIImage[] numberTiles = new UIImage[9];

		private Random random = new Random();

		UILongPressGestureRecognizer longPressGestureRecognizer;

		public MinesweeperViewController () : base ("MinesweeperViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			blankTile = UIImage.FromBundle ("tile.png");
			mineTile = UIImage.FromBundle ("bomb.png");

			numberTiles[0] = UIImage.FromBundle ("blank.png");
			numberTiles[1] = UIImage.FromBundle ("one.png");
			numberTiles[2] = UIImage.FromBundle ("two.png");
			numberTiles[3] = UIImage.FromBundle ("three.png");
			numberTiles[4] = UIImage.FromBundle ("four.png");
			numberTiles[5] = UIImage.FromBundle ("five.png");
			numberTiles[6] = UIImage.FromBundle ("six.png");
			numberTiles[7] = UIImage.FromBundle ("seven.png");
			numberTiles[8] = UIImage.FromBundle ("eight.png");

			for (int col = 0; col < NUMCOLS; col++) {
				for (int row = 0; row < NUMROWS; row++) {
					var frame = new RectangleF(STARTX + (col * (TILESIZE + GAPSIZE)), STARTY + (row * (TILESIZE + GAPSIZE)), TILESIZE, TILESIZE);
					tile [col, row] = new UIImageView(frame);
					//tile [col, row].Image = mineTile;
					tile [col, row].BackgroundColor = UIColor.White;
					View.Add (tile [col, row]);
					// Long press gesture
					tile [col, row].AddGestureRecognizer(new UILongPressGestureRecognizer((UIGestureRecognizer r, UITouch t) => 
					{
						Debug.WriteLine("Long press.");
						}));
					 

				}
			}

			StartGame (); 
		}

		private void StartGame()
		{
			// All tiles are covered
			for (int col = 0; col < NUMCOLS; col++) {
				for (int row = 0; row < NUMROWS; row++) {
					tileCovered [col, row] = true;
				}
			}

			// Place the mines
			int minesplaced = 0;

			do {
				Point p = RandomTile ();

				int col = p.X;
				int row = p.Y;

					if (tileState[col, row] != MINE) {
						tileState[col, row] = MINE;
					minesplaced++;
					tile [col, row].Image = mineTile;
				}
			} while (minesplaced < 10);

			// Calculate numbers for each tile
			for (int col = 0; col < NUMCOLS; col++) {
				for (int row = 0; row < NUMROWS; row++) {
					if (tileState [col, row] != MINE) {
						int value = CalculateTileNumber (col, row);
						tileState[col, row] = value;
						tile [col, row].Image = numberTiles [value];
					}
				}
			}

			// Reset score
			currentScore = 0;
			ShowScore ();
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

		private void ShowScore()
		{
			currentScoreLabel.Text = currentScore.ToString ();
			highScoreLabel.Text = highScore.ToString ();
		}

		private int CalculateTileNumber(int col, int row) {
			int value = 0;

			// upper left
			if (col > 0 && row > 0) {
				if (tileState [col - 1, row - 1] == MINE) {
					value++;
				}
			}

			// above
			if (row > 0) {
				if (tileState [col, row - 1] == MINE) {
					value++;
				}
			}

			// upper right
			if (col < 7 && row > 0) {
				if (tileState [col + 1, row - 1] == MINE) {
					value++;
				}
			}

			// left
			if (col > 0) {
				if (tileState [col - 1, row] == MINE) {
					value++;
				}
			}

			// right
			if (col < 7) {
				if (tileState [col + 1, row] == MINE) {
					value++;
				}
			}

			// lower left
			if (col > 0 && row < 7) {
				if (tileState [col - 1, row + 1] == MINE) {
					value++;
				}
			}

			// below
			if (row < 7) {
				if (tileState [col, row + 1] == MINE) {
					value++;
				}
			}

			// lower right
			if (col < 7 && row < 7) {
				if (tileState [col + 1, row + 1] == MINE) {
					value++;
				}
			}

			return value;
		}

		private void UncoverTile(int col, int row) {


		}

	}
}

