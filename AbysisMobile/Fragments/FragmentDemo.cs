using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AbysisMobile.Fragments
{
    public class FragmentDemo : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }


        TextView tv;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.FragmentDemo, null);
            tv= view.FindViewById<TextView>(Resource.Id.tvFragmentDemo);
            if (Arguments != null)
                tv.Text = Arguments.GetString("Title");
            return view;
        }
    }
}