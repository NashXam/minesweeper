using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Minesweeper.iOS
{
	partial class MineGridViewController : UICollectionViewController
	{
		public MineGridViewController (IntPtr handle) : base (handle) { }

		float _cellsize;

		//public MineGridViewController (UICollectionViewLayout layout) : base(layout) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var width = UIScreen.MainScreen.Bounds.Width;
			var height = UIScreen.MainScreen.Bounds.Height;
			var spacing = 3.0f;
			var totalMargin = width - 300;
			var numberCols = 8;
			_cellsize = ((width - totalMargin) / numberCols) - spacing;
			var margin = totalMargin / 2;
			//var topMargin = 0 / 2;

			var layout = (UICollectionViewFlowLayout)CollectionView.CollectionViewLayout;
			//layout.SectionInset = new UIEdgeInsets(margin, margin, margin, margin);
			layout.MinimumInteritemSpacing = spacing;
			layout.MinimumLineSpacing = spacing;
			layout.ItemSize = new System.Drawing.SizeF(_cellsize, _cellsize);
			CollectionView.CollectionViewLayout = layout;

			CollectionView.BackgroundColor = UIColor.Clear;
			CollectionView.RegisterClassForCell (typeof(MineGridViewControllerCell), MineGridViewControllerCell.Key);
		}

		public void NewGame()
		{
			var alert = new UIAlertView("Minesweeper", "You win!", null, "OK", null);
			alert.Show();
		}

		public override int NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override int GetItemsCount(UICollectionView collectionView, int section)
		{
			return 64;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (MineGridViewControllerCell)collectionView.DequeueReusableCell(MineGridViewControllerCell.Key, indexPath);
//			var frame = cell.Frame;
//			frame.Height = frame.Width = _cellsize;
//			cell.Frame = frame;

			// background view displays behind content view and selected background view
			cell.BackgroundView = new UIView{BackgroundColor = UIColor.White};

			// selected background view displays over background view when cell is selected
			cell.SelectedBackgroundView = new UIView{BackgroundColor = UIColor.Yellow};

			return cell;
		}
	}
}
