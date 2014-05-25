
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper.iOS
{
	public partial class MineGridViewControllerCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("MineGridViewControllerCell");
		public MineGridViewControllerCell(IntPtr handle) : base(handle)
		{
		}

		[Export ("initWithFrame:")]
		MineGridViewControllerCell (RectangleF frame) : base (frame)
		{
//			// create an image view to use in the cell
////			imageView = new UIImageView (new RectangleF (0, 0, 100, 100)); 
////			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
//
//			// populate the content view
////			ContentView.AddSubview (imageView);
//
//			// scale the content view down so that the background view is visible, effecively as a border
////			ContentView.Transform = CGAffineTransform.MakeScale (0.9f, 0.9f);
//
//			frame.Height = 30.0f;
//			frame.Width = 30.0f;
//
//			// background view displays behind content view and selected background view
//			BackgroundView = new UIView{BackgroundColor = UIColor.White};
//
//			// selected background view displays over background view when cell is selected
//			SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Yellow};
		}
	}
}

