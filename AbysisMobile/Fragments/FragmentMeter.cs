using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbysisMobile.Authentication;
using AbysisMobile.Models;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AbysisMobile.Fragments
{
    public class FragmentMeter : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SelectedMeter = Session.SelectedMeter;
            // Create your fragment here
            SelectedMeter = Session.SelectedMeter;
        }
        public static FragmentMeter NewInstance(meterInfo meter)
        {
            var b = new Bundle();
            b.PutString("Title", meter.Info);
            var smmry = new FragmentMeter { Arguments = b,SelectedMeter=meter };
            return smmry;
        }
        public static FragmentMeter NewInstance(string title)
        {
            var b = new Bundle();
            b.PutString("Title",title);
            var smmry = new FragmentMeter { Arguments = b };
            return smmry;
        }
        private TextView tvSubscriberNo;
        private TextView tvGroupCode;
        private TextView tvSubscriberType;
        private TextView tvTariffType;
        private TextView tvDeviceCode;
        private TextView tvContractForce;
        private TextView tvFirstRead;
        private TextView tvLastRead;
        private TextView etGroupCode;
        private TextView etGroupInfo;
        private TextView etSubscirberNo;
        private TextView etSubscriberInfo;
        private TextView etSubscriberType;
        private TextView etTariffCode;
        private TextView etTariffInfo;
        private TextView etDeviceCode;
        private TextView etContractForce;
        private TextView etLastRead;
        private TextView etFirstRead;
        //private ListView lvMeterConsumption;
        //private ListView lvComminicationConsumption;
        //private LinearLayout linearLayoutConsumption;
        //private LinearLayout linearLayoutComminication;
        meterInfo SelectedMeter { get; set;}
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            //return base.OnCreateView(inflater, container, savedInstanceState);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.meter_detail, null);

            MangeUIs(view);
            ManageDataBound();
            //ManageSummary();
            return view;
        }

        //private void ManageSummary()
        //{
        //    if (SelectedMeter.Subscriber.Department.departmentType != 4)
        //    {
        //        lvMeterConsumption.Visibility = ViewStates.Visible;
        //        //lvCommunicationConsumption.Visibility = ViewStates.Gone;
        //        List<MeterConsumptionLine> meterConsumptionLines = new List<MeterConsumptionLine>();
        //        foreach (var item in Session.SelectedMeter.Consumptions)
        //        {
        //            meterConsumptionLines.Add(new MeterConsumptionLine(item));
        //        }
        //        ConsumptionLineAdapter adapter = new ConsumptionLineAdapter(Context, meterConsumptionLines);
        //        lvMeterConsumption.Adapter = adapter;
        //    }
        //    else
        //    {
        //        //Haberleşme kısm
        //        lvMeterConsumption.Visibility = ViewStates.Gone;
        //        //lvCommunicationConsumption.Visibility = ViewStates.Visible;
        //    }
        //}

        private void ManageDataBound()
        {
            etGroupCode.Text = SelectedMeter.Subscriber.Group.Code;
            etGroupInfo.Text = SelectedMeter.Subscriber.Group.Info;
            etSubscirberNo.Text = SelectedMeter.Subscriber.Code;//subscriber code
            etSubscriberInfo.Text = SelectedMeter.Subscriber.Info;
            etSubscriberType.Text = SelectedMeter.Subscriber.SubscriberType.Info;
            etTariffCode.Text = SelectedMeter.Subscriber.Tariff.Code;
            etTariffInfo.Text = SelectedMeter.Subscriber.Tariff.Info;
            etDeviceCode.Text = SelectedMeter.Code;
            //-------------------------
            if (SelectedMeter.DepartmentId == 1)
            {
                etContractForce.Text = SelectedMeter.ContractPower.ToString("n0");
                //tvContractForce.Visibility = ViewStates.Visible;
                //etContractForce.Visibility = ViewStates.Visible;
            }
            else
            {
                tvContractForce.Visibility = ViewStates.Gone;
                etContractForce.Visibility = ViewStates.Gone;
                if (SelectedMeter.DepartmentId > 4)
                {
                    etFirstRead.Visibility = ViewStates.Gone;
                    tvFirstRead.Visibility = ViewStates.Gone;
                    etLastRead.Visibility = ViewStates.Gone;
                    tvLastRead.Visibility = ViewStates.Gone;
                    //lvMeterConsumption.Visibility = ViewStates.Gone;
                }
            }
            etFirstRead.Text = SelectedMeter.FirstDate.ToString("dd/MM/yyyy HH:mm");
            etLastRead.Text = SelectedMeter.LastDate.ToString("dd/MM/yyyy HH:mm");
            //if (SelectedMeter.Subscriber.Department.departmentType == 4)
            //{
            //    linearLayoutConsumption.Visibility = ViewStates.Gone;
            //    linearLayoutComminication.Visibility = ViewStates.Visible;

            //    ComminicationConsumptionLineAdapter comminicationAdapter =
            //        new ComminicationConsumptionLineAdapter(Context, ComminicationConsumptionLine.GetModelList(Session.SelectedMeter));
            //    lvComminicationConsumption.Adapter = comminicationAdapter;
            //}
        }

        private void MangeUIs(View view)
        {
            tvSubscriberNo = view.FindViewById<TextView>(Resource.Id.tvSubscriberNo);
            tvGroupCode = view.FindViewById<TextView>(Resource.Id.tvGroupCode);
            tvSubscriberType = view.FindViewById<TextView>(Resource.Id.tvSubscriberType);
            tvTariffType = view.FindViewById<TextView>(Resource.Id.tvTariffType);
            tvDeviceCode = view.FindViewById<TextView>(Resource.Id.tvDeviceCode);
            tvContractForce = view.FindViewById<TextView>(Resource.Id.tvContractForce);
            tvFirstRead = view.FindViewById<TextView>(Resource.Id.tvFirstRead);
            tvLastRead = view.FindViewById<TextView>(Resource.Id.tvLastRead);
            etGroupCode = view.FindViewById<TextView>(Resource.Id.etGroupCode);
            etGroupInfo = view.FindViewById<TextView>(Resource.Id.etGroupInfo);
            etSubscirberNo = view.FindViewById<TextView>(Resource.Id.etSubscirberNo);
            etSubscriberInfo = view.FindViewById<TextView>(Resource.Id.etSubscriberInfo);
            etSubscriberType = view.FindViewById<TextView>(Resource.Id.etSubscriberType);
            etTariffCode = view.FindViewById<TextView>(Resource.Id.etTariffCode);
            etTariffInfo = view.FindViewById<TextView>(Resource.Id.etTariffInfo);
            etDeviceCode = view.FindViewById<TextView>(Resource.Id.etDeviceCode);
            etContractForce = view.FindViewById<TextView>(Resource.Id.etContractForce);
            etFirstRead = view.FindViewById<TextView>(Resource.Id.etFirstRead);
            etLastRead = view.FindViewById<TextView>(Resource.Id.etLastRead);
            //lvMeterConsumption = view.FindViewById<ListView>(Resource.Id.lvConsumption);
            //lvComminicationConsumption = view.FindViewById<ListView>(Resource.Id.lvComminicationConsumption);
            //linearLayoutConsumption = view.FindViewById<LinearLayout>(Resource.Id.linearLayoutLVConsumption);
            //linearLayoutComminication = view.FindViewById<LinearLayout>(Resource.Id.linearLayoutLVComminication);
        }
    }
}