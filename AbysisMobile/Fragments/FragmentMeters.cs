using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbysisMobile.Models;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
namespace AbysisMobile.Fragments
{
    public class FragmentMeters : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static FragmentMeters NewInstance()
        {
            var deflt = new FragmentMeters { Arguments = new Bundle() };
            return deflt;
        }
        Adapters.MeterListAdapter MeterAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view= inflater.Inflate(Resource.Layout.fragment_meterlist,null);

            ManageUIs(view);
            MeterAdapter = new Adapters.MeterListAdapter(Context, Session.MeterList);
            lvMeters.Adapter = MeterAdapter;
            tvMeterInfo = view.FindViewById<TextView>(Resource.Id.tvMeterInfoDefault);
            tvMeterInfo.Text = "Seçili Sayaç : " +
                (new AuthMeters(Session.SelectedMeter)).ToString();
            lvMeters.ItemClick += (s, e) =>
            {
                var item = Session.LoginData.AuthorizedMeters.Where(x => x.Id == Session.MeterList[e.Position].Id).Select(x => x).FirstOrDefault();
                //foreach (var item in Session.LoginData.AuthorizedMeters)
                //{
                //    if (item.Id == Session.MeterList[e.Position].Id)
                //    {
                Session.SelectedMeter = item;
                tvMeterInfo.Text = "Seçili Sayaç : " + (new AuthMeters(item)).ToString();
                //MeterSelected.Invoke(this, new OnMeterSelectedEventArgs(item.DepartmentId, item.Subscriber.Department.departmentType));
                MainActivity.navigationHeaderSelectedMeter.Text = "Seçili Sayaç : " + (new AuthMeters(item)).ToString();
                //}
                //}

            };
            return view;
        }
        TextView tvMeterInfo;
        ListView lvMeters;
        private void ManageUIs(View view)
        {
            tvMeterInfo = view.FindViewById<TextView>(Resource.Id.tvMeterInfoDefault);
            lvMeters = view.FindViewById<ListView>(Resource.Id.lvMeterList);
        }
    }
}