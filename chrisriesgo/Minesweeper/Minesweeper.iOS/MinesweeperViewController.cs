using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Minesweeper.iOS
{
	partial class MinesweeperViewController : UIViewController
	{
		public MinesweeperViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = new UIColor(0.161f, 0.6f, 0.525f, 1f);

			NewGameButton.TouchUpInside += (sender, e) => 
			{
				foreach(var c in ChildViewControllers)
				{
					var controller = c as MineGridViewController;
					if (c != null)
					{
						controller.NewGame();
						return;
					}
				}
			};

			SetUpViews();
		}

		// TODO: Potentially pass data to the MineGridViewController to initialize it
//		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
//		{
//			base.PrepareForSegue (segue, sender);
//
//			var mineGridController = segue.DestinationViewController as MineGridViewController;
//
//			if (mineGridController != null) {
//				var u = 1;
//			}
//		}

		#region "Initialize Views"
		private void SetUpViews()
		{
			NewGameButton.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			NewGameButton.TitleLabel.TextAlignment = UITextAlignment.Center;
		}
		#endregion
	}
}
