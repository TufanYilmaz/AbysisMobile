using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AbysisMobile.Authentication;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using AbysisMobile.Helpers;
using System.Threading;
using Android.App;
//using Java.Lang;

namespace AbysisMobile.Fragments
{
    public class FragmentVoltageGraph : Fragments.FragmentTriFaze
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);

        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            QueryButton.Click += QueryButton_Click;
            return view;
        }
        List<Indexes.indexInfo> IndexList = new List<Indexes.indexInfo>();
        //ProgressDialog pd;
        private void QueryButton_Click(object sender, EventArgs e)
        {

        }
    }
}