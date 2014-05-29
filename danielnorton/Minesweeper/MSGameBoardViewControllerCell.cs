using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper
{
	[Register ("MSGameBoardViewControllerCell")]
	public class MSGameBoardViewControllerCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString ("MSGameBoardViewControllerCell");

		public MSGameBoardViewControllerCell (IntPtr handle) : base (handle)
		{
		}
	}
}

