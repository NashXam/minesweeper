using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper
{
	[Register ("MSGameBoardViewControllerController")]
	public class MSGameBoardViewControllerController : UICollectionViewController
	{
		#region Xamarin initializer
		public MSGameBoardViewControllerController (IntPtr handle) : base (handle)
		{
		}

		#endregion

		#region UIViewController
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			UIView header = new UIView (new RectangleF (0.0f, 0.0f, 20.0f, 20.0f));
			header.BackgroundColor = UIColor.Green;
		}

		#endregion


		#region UICollectionViewDataSource
		public override int NumberOfSections (UICollectionView collectionView)
		{
			return 10;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return 10;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell (MSGameBoardViewControllerCell.Key, indexPath) as MSGameBoardViewControllerCell;
			cell.BackgroundColor = UIColor.Purple;
			return cell;
		}

		#endregion


		#region UICollectionViewDelegate
		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			UICollectionViewCell cell = collectionView.CellForItem (indexPath);
			cell.BackgroundColor = UIColor.Green;
		} 

		[Export ("collectionView:layout:insetForSectionAtIndex:")]
		public virtual UIEdgeInsets GetInsetForSection (UICollectionView collectionView, UICollectionViewLayout layout, int section)
		{
			float top = (section == 0)
				? 20.0f
				: 0.0f;

			return new UIEdgeInsets (top, 20.0f, 10.0f, 20.0f);
		}

		#endregion

	}
}

