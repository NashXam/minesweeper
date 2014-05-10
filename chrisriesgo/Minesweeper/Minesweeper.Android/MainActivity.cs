using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Graphics;
using Minesweeper.Core;
using System.Linq;
using System.Threading.Tasks;

namespace Minesweeper.Android
{
	[Activity(Label = "Minesweeper", MainLauncher = true)]
	public class MainActivity : Activity
	{
		List<Mine> _mines;
		GridLayout _grid;
		LinearLayout[] _subViews;
		List<int> _checked;
		int _size;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);
			_grid = FindViewById<GridLayout>(Resource.Id.gridLayout);
			_size = 9;

			NewGame(_size);
		}

		async void NewGame(int size)
		{
			_mines = new List<Mine>();
			_subViews = new LinearLayout[size * size];
			_checked = new List<int>();
			for(int i = 0; i < size; i++)
			{
				int x;
				int y;
				do
				{
					x = new Random().Next(size);
					y = new Random().Next(size);
				} while (_mines.Exists(m => m.X == x && m.Y == y));
				_mines.Add(new Mine { X = x, Y = y });
			}

			await DrawBoard(size * size);
		}

		async Task DrawBoard(int tileCount)
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					for(var i = 0; i < tileCount; i++)
					{
						var tile = new LinearLayout(this);
						tile.SetBackgroundColor(Color.LightGray);
						var p = new GridLayout.LayoutParams() { Height = 100, Width = 100 };
						p.SetMargins(4, 4, 4, 4);
						tile.LayoutParameters = p;
						RunOnUiThread(() =>
						{
							tile.Click += TileClick;
							_grid.AddView(tile);
							_subViews[_grid.IndexOfChild(tile)] = tile;
						});
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
				}
			});
		}

		void TileClick (object sender, EventArgs e)
		{
			var tileLayout = (LinearLayout)sender;

			var position = _grid.IndexOfChild(tileLayout);
			int numberOfColumns = _grid.ColumnCount;
			int row = Convert.ToInt32(Math.Floor((double)(position/numberOfColumns)));
			int column = position - (row * numberOfColumns);
			var neighbors = Helpers.CountNeighbors(_mines, column, row);

			var tile = new Tile { X = column, Y = row, Neighbors = neighbors, Position = position };
			_checked.Add(position);

			if(_mines.Exists(m => m.X == column && m.Y == row))
			{
				// You lose :(
				tileLayout.SetBackgroundColor(Color.Red);
			}
			else
			{
				if(neighbors == 0)
					ClearNeighbors(tileLayout, tile);
				else
					DisplayNeighborCount(tileLayout, neighbors);
			}
		}

		void ClearNeighbors(LinearLayout tileLayout,Tile tile)
		{
			Flip(tileLayout);

			foreach(var n in Helpers.Neighbors(tile, _size))
			{
				if(_checked.Contains(n.Position)) continue;
				_checked.Add(n.Position);
				n.Neighbors = Helpers.CountNeighbors(_mines, n.X, n.Y);
				var layout = _subViews[n.Position];
				if(n.Neighbors == 0)
					ClearNeighbors(layout, n);
				else
					DisplayNeighborCount(layout, n.Neighbors);
			}
		}

		void DisplayNeighborCount(LinearLayout tileLayout, int count)
		{
			Flip(tileLayout);

			var text = new TextView(this);
			text.TextSize = 22;
			text.TextAlignment = TextAlignment.Center;
			text.Text = count.ToString();
			tileLayout.AddView(text);
		}

		void Flip(LinearLayout tileLayout)
		{
			tileLayout.SetBackgroundColor(Color.LightSlateGray);
			tileLayout.Click -= TileClick;
		}
	}
}


