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

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);
			_grid = FindViewById<GridLayout>(Resource.Id.gridLayout);

			NewGame(9);
		}

		async void NewGame(int size)
		{
			_mines = new List<Mine>();
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
			int row = Convert.ToInt32(Math.Ceiling((double)((position + 1) / numberOfColumns)));
			int column = position - (row * numberOfColumns);
			var neighbors = Helpers.CountNeighbors(_mines, column, row);

			var tile = new Tile { X = column, Y = row, Neighbors = neighbors };

			if(_mines.Exists(m => m.X == column && m.Y == row))
			{
				// You lose :(
				tileLayout.SetBackgroundColor(Color.Red);
			}
			else
			{
				tileLayout.SetBackgroundColor(Color.LightSlateGray);
				tileLayout.Click -= TileClick;

				if(neighbors == 0)
					ClearNeighbors(tile);
				else
					DisplayNeighborCount(tileLayout, neighbors);
			}
		}

		void ClearNeighbors(Tile tile)
		{
		}

		void DisplayNeighborCount(LinearLayout tile, int count)
		{
			var text = new TextView(this);
			text.TextSize = 22;
			text.TextAlignment = TextAlignment.Center;
			text.Text = count.ToString();
			tile.AddView(text);
		}
	}
}


