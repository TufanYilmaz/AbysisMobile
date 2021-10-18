using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbysisMobile.Authentication;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AbysisMobile.Fragments
{
    public class FragmentDefault : Fragment
    {
        private int meterPosition;
        private Authentication.meterInfo meter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);
            if (Arguments != null)
               meterPosition = Arguments.GetInt("Position");
            meter = Session.LoginData.AuthorizedMeters[meterPosition];
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view= inflater.Inflate(Resource.Layout.fragment_default, container, false);
            ListView lvMeterGauge;
            if (meter.DepartmentId != 5)
            {
                lvMeterGauge = view.FindViewById<ListView>(Resource.Id.lvMeterLineGauge);
                //List<Authentication.ConsumptionLine> lines = meter.LastValues.Where(l => l.IndexInfo.WebVisible == true).Select(l => l.IndexInfo.).ToList();
                List<int> webb = new List<int>();
                List<ConsumptionLine> res = new List<ConsumptionLine>();
                foreach (var row in meter.Consumptions)
                {
                    webb.Clear();
                    webb = meter.LastValues.Where(x => x.IndexInfo.Code == row.INDEX_CODE & x.IndexInfo.WebMainPage == true).Select(x => x.Id).ToList();
                    if (webb.Count < 1)
                        continue;
                    res.Add(row);
                }
                Adapters.MeterLineGaugeAdapter adap = new Adapters.MeterLineGaugeAdapter(Context, res);
                lvMeterGauge.Adapter = adap;
            }
            return view;
        }
    }
}