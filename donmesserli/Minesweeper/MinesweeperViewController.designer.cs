// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Minesweeper
{
	[Register ("MinesweeperViewController")]
	partial class MinesweeperViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton cheatButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel currentScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView flagImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel highScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel newGameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (currentScoreLabel != null) {
				currentScoreLabel.Dispose ();
				currentScoreLabel = null;
			}

			if (flagImage != null) {
				flagImage.Dispose ();
				flagImage = null;
			}

			if (highScoreLabel != null) {
				highScoreLabel.Dispose ();
				highScoreLabel = null;
			}

			if (cheatButton != null) {
				cheatButton.Dispose ();
				cheatButton = null;
			}

			if (newGameLabel != null) {
				newGameLabel.Dispose ();
				newGameLabel = null;
			}
		}
	}
}
