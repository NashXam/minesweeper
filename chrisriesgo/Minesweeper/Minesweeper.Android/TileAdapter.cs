using System;
using Android.Content;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace Minesweeper.Android
{
	public class TileAdapter : BaseAdapter<Button>
	{
		Context context;
		IList<Button> _buttons = new List<Button>{ };

		public TileAdapter(Context c, IList<Button> buttons)
		{
			context = c;
			_buttons = buttons;
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			Button button;
			if(convertView == null)
			{
				button = new Button(context);
				button.LayoutParameters = new GridView.LayoutParams(125, 125);
				button.SetPadding(8, 8, 8, 8);
			}
			else
			{
				button = (Button)convertView;
			}

			return button;
		}

		public override int Count
		{
			get
			{
				return _buttons.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override Button this[int index]
		{
			get
			{
				return _buttons[index];
			}
		}

		#endregion
	}

//	public class Tile
//	{
//		public Tile()
//		{
//		}
//	}
}

