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
using AbysisMobile.Indexes;
namespace AbysisMobile.Adapters
{
    class IndexListAdapter : BaseAdapter<indexInfo>
    {

        Context context;
        List<indexInfo> Indices;
        public IndexListAdapter(Context context)
        {
            this.context = context;
        }
        public IndexListAdapter(Context context,List<indexInfo> indices) 
        {
            this.context = context;
            Indices = indices;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return Indices[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.index_list, null, false);
            }
            TextView Code = row.FindViewById<TextView>(Resource.Id.tvMQIndiciesCode);
            TextView Info = row.FindViewById<TextView>(Resource.Id.tvMQIndiciesInfo);
            TextView Id = row.FindViewById<TextView>(Resource.Id.tvMQIndiciesID);

            //fill in your items
            Code.Text = Indices[position].Code;
            Info.Text = Indices[position].Info;
            Id.Text = Indices[position].Id.ToString();

            return row;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return Indices.Count;
            }
        }

        public override indexInfo this[int position] => Indices[position];
    }

}