// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Minesweeper.iOS
{
	[Register ("MinesweeperViewController")]
	partial class MinesweeperViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView GridContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton NewGameButton { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (GridContainer != null) {
				GridContainer.Dispose ();
				GridContainer = null;
			}
			if (NewGameButton != null) {
				NewGameButton.Dispose ();
				NewGameButton = null;
			}
		}
	}
}
