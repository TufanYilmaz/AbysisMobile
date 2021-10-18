using System;
using System.Collections.Generic;
using AbysisMobile.Models;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace AbysisMobile.Adapters
{
    class MeterListAdapter : BaseAdapter<AuthMeters>
    {
        List<AuthMeters> AMeters;
#pragma warning disable IDE0044 // Add readonly modifier
        private Context AContext;
#pragma warning restore IDE0044 // Add readonly modifier
        public MeterListAdapter(Context context, List<AuthMeters> meters)
        {
            AMeters = meters;
            AContext = context;
        }
        public override AuthMeters this[int position]
        {
            get { return AMeters[position]; }
        }
        public override int Count
        {
            get { return AMeters.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(AContext).Inflate(Resource.Layout.list_meter_layout, null, false);
            }
            ImageView meterImage = row.FindViewById<ImageView>(Resource.Id.ivMeterPic);
            meterImage.SetImageResource(AMeters[position].Image);
            string meterInfoText = AMeters[position].ToString();
            TextView meterInfo = row.FindViewById<TextView>(Resource.Id.tvMeterMenuItem);
            meterInfo.Text = meterInfoText;
            return row;
        }

        
    }

}