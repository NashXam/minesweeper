using System;
using System.Drawing;
using System.Timers;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PerpetualEngine.Storage;
using SysSound.Extensions;
using MonoTouch.AudioToolbox;

namespace Minesweeper
{
	public partial class MinesweeperViewController : UIViewController
	{
		// To turn cheating on, flag and unflag a tile 3 times each
		private int cheatCounter = 0;
		private int lastFlagged = 0xFFFF;
		private bool cheating = true;

		private static int NUMROWS = 8;
		private static int NUMCOLS = 8;

		private static int STARTX = 20;
		private static int STARTY = 170;
		private static int TILESIZE = 33;
		private static int GAPSIZE = 2;

		private static int NUMMINES = 10;

		// SHORT PRESS (TAP) to uncover tile.
		// LONG PRESS tp place flag;
		private static int SHORTPRESS = 0;
		private static int LONGPRESS = 1;

		private SimpleStorage preferences = SimpleStorage.EditGroup("Minesweeper");
		private static string HIGHSCOREKEY = "highscore";

		private MineSweeperGame game;
		private Timer pressTimer;
		private int pressType;
		private int pressedTag;
		private	bool gameOver;

		private SystemSound bombSound;
		private SystemSound winSound;

		private int highScore = 0;

		private UIButton[,] tiles = new UIButton[NUMCOLS, NUMROWS];

		private UIImage blankTile;
		private UIImage mineTile;
		private UIImage flag;
		private UIImage coveredTile;
		private UIImage flaggedTile;
		private UIImage cheatTile;
		private UIImage redmineTile;
		private UIImage flaggedCheatTile;
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

			//enable audio
			AudioSession.Initialize();

			bombSound = SystemSound.FromFile("Bomb.wav");
			winSound = SystemSound.FromFile("success.wav");

			highScore = Convert.ToInt32(preferences.Get(HIGHSCOREKEY) ?? "0");

			blankTile = UIImage.FromBundle ("tile.png");
			mineTile = UIImage.FromBundle ("mine.png");
			flag = UIImage.FromBundle ("flag.png");
			coveredTile = UIImage.FromBundle ("covered.png");
			flaggedTile = UIImage.FromBundle ("flagged.png");
			cheatTile = UIImage.FromBundle ("cheat.png");
			redmineTile = UIImage.FromBundle ("redmine.png");
			flaggedCheatTile = UIImage.FromBundle ("flaggedcheat.png");

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
					tiles [col, row] = iv;
					iv.BackgroundColor = UIColor.White;
					View.Add (iv);
					iv.UserInteractionEnabled = true;
					iv.Tag = TagForTile (col, row);

					iv.TouchDown += (sender, ea) => {
						if (!gameOver) {
							pressedTag = ((UIButton)sender).Tag;
							pressTimer = new Timer(400);
							pressTimer.Elapsed += OnTimerElapsed;
							pressTimer.Start ();
							pressType = SHORTPRESS;
						}
					};

					iv.TouchUpInside += (sender, ea) => {
						if (gameOver) {
							StartGame ();
						} else {
							InvokeOnMainThread (() => flagImage.Image = blankTile);
							if (pressTimer != null) {
								pressTimer.Close();
								pressTimer.Dispose();
							}
							HandlePress();
						}
					};

					//iv.TouchesCancelled += (sender, ea) => {
					//
					//};

					iv.TouchUpOutside += (sender, ea) => {
						if (gameOver) {
							StartGame ();
						} else {
							InvokeOnMainThread (() => flagImage.Image = blankTile);
							if (pressTimer != null) {
								pressTimer.Close();
								pressTimer.Dispose();
							}
						}
					};

				}		 
			}

			// (UIGestureRecognizer r, UITouch t, Object o) 
			StartGame (); 
		}
			
		private void OnTimerElapsed (object o, EventArgs e)
		{
			InvokeOnMainThread (() => flagImage.Image = flag);
			pressType = LONGPRESS;
			pressTimer.Close();
			pressTimer.Dispose();
			pressTimer = null;
		}

		private void HandlePress()
		{
			Point p = TileForTag (pressedTag);

			if (pressType == SHORTPRESS) {
				// Reset cheating state
				cheatCounter = 0;
				game.UncoverTile (p.X, p.Y);
			} else { // LONGPRESS
				// Figure out cheating state
				if (lastFlagged == pressedTag) {
					cheatCounter++;
					if (cheatCounter == 6) {
						cheating = !cheating;
						cheatCounter = 0;
					}
				} else {
					// Reset cheating state
					cheatCounter = 0;
				}

				lastFlagged = pressedTag;
				game.FlagTile (p.X, p.Y);
			}
		}

		private void StartGame()
		{
			gameOver = false;
			newGameLabel.Hidden = true;

			game = new MineSweeperGame (NUMCOLS, NUMROWS, NUMMINES, (MineSweeperGame g) => {

				Point p = TileForTag (pressedTag);
				UIButton lastPressed = tiles [p.X, p.Y];

				if (g.isGameOver()) {
					gameOver = true;
					newGameLabel.Hidden = false;
					if (g.wasGameWon ()) {
						winSound.PlaySystemSound ();
					} else {
						bombSound.PlaySystemSound ();
					}
					//ShowAlert ();
				}

				ShowScore(g);

				for (int col = 0; col < NUMCOLS; col++) {
					for (int row = 0; row < NUMROWS; row++) {
						UIButton tile = tiles [col, row];
						Console.WriteLine("Tile = {0:X} LastPressed = {1:X}", tile, lastPressed);
						int value = g.GetTileValue(col, row);

						MineSweeperGame.DrawType draw = g.GetDrawType(col, row);

						switch (draw) {
						case MineSweeperGame.DrawType.Covered:
							if (cheating  || gameOver) {
								if (value == 0xFFFF) {
									if (cheating && !gameOver) {
										tile.SetImage(cheatTile, UIControlState.Normal);
									} else {
										tile.SetImage(mineTile, UIControlState.Normal);
									}
								} else {
									tile.SetImage(coveredTile, UIControlState.Normal);
								}
							} else {
								tile.SetImage(coveredTile, UIControlState.Normal);
							}
							break;

						case MineSweeperGame.DrawType.Flagged:
							if (cheating && value == 0xFFFF) {
								tile.SetImage(flaggedCheatTile, UIControlState.Normal);
							} else {
								tile.SetImage(flaggedTile, UIControlState.Normal);
							}
							break;

						case MineSweeperGame.DrawType.Value:
							tile.SetImage(numberTiles [value], UIControlState.Normal);
							break;

						case MineSweeperGame.DrawType.Mine:
							if (gameOver && tile == lastPressed) {
								tile.SetImage(redmineTile, UIControlState.Normal);
							} else {
								tile.SetImage(mineTile, UIControlState.Normal);
							}
							break;
						}
					}
				}

				return true;
			});
		}
			
		private void ShowAlert()
		{
			UIAlertView alert = new UIAlertView();
			if (game.wasGameWon ()) {
				alert.Title = "Congratulations!";
				alert.Message = "You won!";
			} else {
				alert.Title = "Bang!";
				alert.Message = "Game Over!";
			}

			alert.AddButton("OK");

			//alert.Clicked += (s, b) => {
			//	StartGame ();
			//};
			alert.Show();

			// TODO: Update view to start over
		}

		private void ShowScore(MineSweeperGame g)
		{
			int currentScore = g.GetCurrentScore ();
			if (currentScore > highScore) {
				highScore = currentScore;
				preferences.Put(HIGHSCOREKEY, highScore.ToString());
			}

			currentScoreLabel.Text = currentScore.ToString ();
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

		#region respond to shaking (OS3+)
		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.BecomeFirstResponder();
		}

		public override void ViewWillDisappear (bool animated)
		{
			this.ResignFirstResponder();
			base.ViewWillDisappear (animated);
		}

		public override void MotionEnded (UIEventSubtype motion, UIEvent evt)
		{
			if (motion ==  UIEventSubtype.MotionShake)
			{
				StartGame();   
			}
		}
		#endregion
	}
}

