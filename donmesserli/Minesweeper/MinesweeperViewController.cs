using System;
using System.Drawing;
using System.Timers;
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

		private static int SHORTPRESS = 0;
		private static int LONGPRESS = 1;

		private MineSweeperGame game;
		private Timer pressTimer;
		private int pressType;
		private int pressTag;

		private int highScore = 0;

		private UIButton[,] tile = new UIButton[NUMCOLS, NUMROWS];

		private UIImage blankTile;
		private UIImage mineTile;
		private UIImage flag;
		private UIImage coveredTile;
		private UIImage flaggedTile;
		private UIImage[] numberTiles = new UIImage[9];

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
			flag = UIImage.FromBundle ("flag.png");
			coveredTile = UIImage.FromBundle ("covered.png");
			flaggedTile = UIImage.FromBundle ("flagged.png");

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
					var frame = new RectangleF (STARTX + (col * (TILESIZE + GAPSIZE)), STARTY + (row * (TILESIZE + GAPSIZE)), TILESIZE, TILESIZE);
					UIButton iv = UIButton.FromType(UIButtonType.Custom);
					iv.Frame = frame;
					tile [col, row] = iv;
					//tile [col, row].Image = mineTile;
					iv.BackgroundColor = UIColor.White;
					View.Add (iv);
					iv.UserInteractionEnabled = true;
					iv.Tag = TagForTile (col, row);

					iv.TouchDown += (sender, ea) => {
						pressTag = ((UIButton)sender).Tag;
						flagImage.Image = flag;
						pressTimer = new Timer(400);
						pressTimer.Elapsed += OnTimerElapsed;
						pressTimer.Start ();
						pressType = SHORTPRESS;
					};

					iv.TouchUpInside += (sender, ea) => {
						InvokeOnMainThread (() => flagImage.Image = blankTile);
						if (pressTimer != null) {
							pressTimer.Close();
							pressTimer.Dispose();
						}
						HandlePress();
					};

					//iv.TouchesCancelled += (sender, ea) => {
					//
					//};

					iv.TouchUpOutside += (sender, ea) => {
						InvokeOnMainThread (() => flagImage.Image = blankTile);
						if (pressTimer != null) {
							pressTimer.Close();
							pressTimer.Dispose();
						}
					};

				}		 
			}

			// (UIGestureRecognizer r, UITouch t, Object o) 
			StartGame (); 
		}
			
		private void OnTimerElapsed (object o, EventArgs e)
		{
			InvokeOnMainThread (() => flagImage.Image = blankTile);
			pressType = LONGPRESS;
			pressTimer.Close();
			pressTimer.Dispose();
			pressTimer = null;
		}

		private void HandlePress()
		{
			if (pressType == SHORTPRESS) {
				Point p = TileForTag (pressTag);
				game.FlagTile (p.X, p.Y);
			} else { // LONGPRESS
				Point p = TileForTag (pressTag);
				game.UncoverTile (p.X, p.Y);
			}
		}

		private void StartGame()
		{
			game = new MineSweeperGame (NUMCOLS, NUMROWS, NUMMINES, (MineSweeperGame g) => {
				ShowScore(g);

				for (int col = 0; col < NUMCOLS; col++) {
					for (int row = 0; row < NUMROWS; row++) {
						int value = g.GetTileValue(col, row);

						MineSweeperGame.DrawType draw = g.GetDrawType(col, row);

						switch (draw) {
						case MineSweeperGame.DrawType.Covered:
							tile [col, row].SetImage(coveredTile, UIControlState.Normal);
							break;

						case MineSweeperGame.DrawType.Flagged:
							tile [col, row].SetImage(flaggedTile, UIControlState.Normal);
							break;

						case MineSweeperGame.DrawType.Value:
							tile [col, row].SetImage(numberTiles [value], UIControlState.Normal);
							break;

						case MineSweeperGame.DrawType.Mine:
							tile [col, row].SetImage(mineTile, UIControlState.Normal);
							break;
						}
					}
				}

				if (g.isGameOver()) {
					ShowAlert();
				}

				return true;
			});
		}

		private void ShowAlert()
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Bang!";
			alert.AddButton("Ok");
			alert.Message = "Game Over!";
			alert.Show();

			// TODO: Update view to start over
		}

		private void ShowScore(MineSweeperGame g)
		{
			currentScoreLabel.Text = g.GetCurrentScore().ToString ();
			highScoreLabel.Text = highScore.ToString ();
		}
			
		private int TagForTile(int col, int row)
		{
			return ((col << 8) + row);
		}

		private Point TileForTag(int tag)
		{
			int col = (tag & 0xFF00) >> 8;
			int row = tag & 0xFF;

			return new Point (col, row);
		}
	}
}

