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

		private UIImageView[,] tile = new UIImageView[NUMCOLS, NUMROWS];

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
			
			for (int col = 0; col < NUMCOLS; col++) {
				for (int row = 0; row < NUMROWS; row++) {
					tile [col, row] = (UIImageView)this.View.ViewWithTag (CoordsToTag(col, row));
				}
			}

		}

		private int CoordsToTag(int col, int row)
		{
			return (col * 10) + row;
		}
	}
}

