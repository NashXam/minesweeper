using System;
using MonoTouch.UIKit;

namespace Minesweeper.iOS
{
	public class MineGridViewControllerCellLayout : UICollectionViewFlowLayout
	{
		public MineGridViewControllerCellLayout() : base()
		{
			var width = UIScreen.MainScreen.Bounds.Width;
			var height = UIScreen.MainScreen.Bounds.Height;
			var spacing = 3.0f;
			var totalMargin = width - CollectionView.Bounds.Width;
			var numberCols = 8.0f;
			var cellsize = ((width - totalMargin) / numberCols) - spacing;
			//var margin = totalMargin / 2;
			//var topMargin = 40 / 2;

			MinimumInteritemSpacing = spacing;
			MinimumLineSpacing = spacing;
			ItemSize = new System.Drawing.SizeF(cellsize, cellsize);
		}
	}
}

