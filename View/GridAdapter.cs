using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MuzikaClasses
{
    public class GridAdapter : BaseAdapter
    {
        public List<List<View>> Rows { get; set; } = new List<List<View>>();
        public int Columns { get; set; }

        public override int Count => Rows.Count * Columns;

        public override Java.Lang.Object GetItem(int position)
        {
            if (position >= 0)
            {
                int row = (int)System.Math.Floor((decimal)position / Columns);
                return Rows[row].ElementAt(position-(row*Columns));
            }
            else
            {
                throw new System.Exception("Negative position received");
            }
        }

        public override long GetItemId(int position)
        {
            throw new NotImplementedException();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}