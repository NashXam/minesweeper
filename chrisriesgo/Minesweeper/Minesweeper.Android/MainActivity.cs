﻿using System;
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
	[Activity (Label = "Minesweeper", MainLauncher = true, Theme = "@style/MyTheme")]
	public class MainActivity : Activity
	{
		List<Mine> _mines;
		GridLayout _grid;
		TextView _currentScore;
		LinearLayout[] _subViews;
		List<Tile> _checked;
		int _size;
		int _score = 0;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);
			_grid = FindViewById<GridLayout>(Resource.Id.gridLayout);
			_currentScore = FindViewById<TextView>(Resource.Id.currentScore);
			_size = 8;

			NewGame(_size);
		}

		void Clear()
		{
			_grid = new GridLayout(this);
			_mines = new List<Mine>();
			_subViews = new LinearLayout[0];
			_checked = new List<Tile>();
			_currentScore.Text = "0";
		}

		async void NewGame(int size)
		{

			_grid.ColumnCount = size;
			_grid.RowCount = size;
			_mines = new List<Mine>();
			_subViews = new LinearLayout[size * size];
			_checked = new List<Tile>();
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
						tile.SetBackgroundColor(Resources.GetColor(Resource.Color.white));
						var p = new GridLayout.LayoutParams() { Height = 112, Width = 112 };
						p.SetMargins(4, 4, 4, 4);
						tile.LayoutParameters = p;
						RunOnUiThread(() =>
						{
							tile.Click += TileClick;
							tile.LongClick += TileLongClick;
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
			_checked.Add(tile);

			if(_mines.Exists(m => m.X == column && m.Y == row))
			{
				// You lose :(
				tileLayout.SetBackgroundColor(Color.Red);
				foreach(var v in _subViews)
				{
					var p = _grid.IndexOfChild(v);
					if(_checked.Any(t => t.Position == p)) continue;
					v.Click -= TileClick;
					v.LongClick -= TileLongClick;
				}
			}
			else
			{
				ShowTiles(tileLayout, tile);
				UpdateScore();
			}
		}

		void UpdateScore()
		{
			RunOnUiThread(() => _currentScore.Text = string.Format("{0}", ++_score));
		}

		void TileLongClick (object sender, View.LongClickEventArgs e)
		{
			// Set/Release flag
			var tileLayout = (LinearLayout)sender;

			var position = _grid.IndexOfChild(tileLayout);
			var tile = _checked.FirstOrDefault(t => t.Position == position);
			if(tile == null)
			{
				int numberOfColumns = _grid.ColumnCount;
				int row = Convert.ToInt32(Math.Floor((double)(position/numberOfColumns)));
				int column = position - (row * numberOfColumns);
				var neighbors = Helpers.CountNeighbors(_mines, column, row);

				tile = new Tile { X = column, Y = row, Neighbors = neighbors, Position = position };
			}

			tile.Flagged = !tile.Flagged;
			//_checked.Where(t => t.Position == position).First() = tile;

			SetFlag(tile);
		}

		void ClearNeighbors(LinearLayout tileLayout,Tile tile)
		{
			Flip(tileLayout);

			foreach(var n in Helpers.Neighbors(tile, _size))
			{
				if(_checked.Any(t => t.Position == n.Position)) continue;
				_checked.Add(n);
				n.Neighbors = Helpers.CountNeighbors(_mines, n.X, n.Y);
				var layout = _subViews[n.Position];
				ShowTiles(layout, n);
			}
		}

		void ShowTiles(LinearLayout layout, Tile tile)
		{
			if(tile.Neighbors == 0)
				ClearNeighbors(layout, tile);
			else
				DisplayNeighborCount(layout, tile.Neighbors);
		}

		void DisplayNeighborCount(LinearLayout tileLayout, int count)
		{
			Flip(tileLayout);

			var text = new TextView(this);
			text.SetTextAppearance(this, Resource.Style.CommonText);
			text.Text = count.ToString();
			tileLayout.AddView(text);
		}

		void Flip(LinearLayout tileLayout)
		{
			tileLayout.SetBackgroundColor(Resources.GetColor(Resource.Color.brown_background));
			tileLayout.Click -= TileClick;
			tileLayout.LongClick -= TileLongClick;
		}

		void SetFlag(Tile tile)
		{
			var layout = _subViews[tile.Position];
			if(tile.Flagged)
			{
				if (!_checked.Any(t => t.Position == tile.Position)) _checked.Add(tile);
				layout.SetBackgroundColor(Color.LightGray);
			}
			else
			{
				if (_checked.Any(t => t.Position == tile.Position)) _checked.Remove(_checked.First(t => t.Position == tile.Position));
				layout.SetBackgroundColor(Resources.GetColor(Resource.Color.white));
			}
		}
	}
}


