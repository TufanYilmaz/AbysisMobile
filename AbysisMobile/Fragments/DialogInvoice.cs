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

namespace AbysisMobile.Fragments
{
    public class DialogInvoice : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                InvoiceId = Arguments.GetInt("InvoiceId");
                PeriodId = Arguments.GetInt("PeriodId");
                SelectedInvoice = Services.invoicesClient.GetInvoice(
                    Session.LoginData.Value.ToString(),
                    Session.SelectedMeter.DepartmentId,
                    PeriodId,
                    InvoiceId).Value;
                Session.SelectedInvoice = SelectedInvoice;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.Message, ToastLength.Short).Show();
            }
            // Create your fragment here
        }
        int InvoiceId=-1;
        int PeriodId = -1;
        Invoices.invoice SelectedInvoice = new Invoices.invoice();
        //FrameLayout frameInvoice;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view= inflater.Inflate(Resource.Layout.invoice_master, container, false);
            //frameInvoice = view.FindViewById<FrameLayout>(Resource.Id.frameInvoiceDetails);
            //var frd = Fragments.FragmentInvoiceDetail.NewInstance("Detail");
            //var frl = Fragments.FragmentInvoiceLines.NewInstance("Lines");
            //SelectedInvoice.To
            Bundle detail = new Bundle();
            //string jsonInvoice = Newtonsoft.Json.JsonConvert.SerializeObject(SelectedInvoice);
            detail.PutString("Title", "Fatura Detay");
            //detail.PutString("InvoiceJson",jsonInvoice);
            List<Fragment> invoiceFragments = new List<Fragment>
            {
                FragmentInvoiceDetail.NewInstance("Fatura Detay"),
                //new FragmentInvoiceDetail(){ Arguments=detail},
                FragmentInvoiceLines.NewInstance("Fatura Satırları")
            };
            ManageUIs(view);
            //Activity.FragmentManager
            //SupportFragmentManager
           ChildFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.frameInvoiceDetails, new TabFragment(ChildFragmentManager,invoiceFragments) )
                .Commit();
            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
        TextView tvFirmName;
        TextView tvMeterNo;
        TextView tvInvoiceDate;
        TextView tvLastPayDate;
        TextView tvInvoiceNo;
        private void ManageUIs(View view)
        {
            tvFirmName = view.FindViewById<TextView>(Resource.Id.tvInvoiceFirmName);
            tvFirmName.Text ="Firma Adı:\n" + Session.SelectedInvoice.Subscriber.Info;
            tvMeterNo = view.FindViewById<TextView>(Resource.Id.tvInvoiceMeter);
            tvMeterNo.Text ="Sayaç No:\n" + Session.SelectedInvoice.Subscriber.MainMeter.Code;
            tvInvoiceNo = view.FindViewById<TextView>(Resource.Id.tvInvoiceNo);
            tvInvoiceNo.Text ="Fatura No:\n" + Session.SelectedInvoice.InvoiceNumber;
            tvInvoiceDate = view.FindViewById<TextView>(Resource.Id.tvInvoiceDate);
            tvInvoiceDate.Text = "Fatura Tarihi:\n" + Session.SelectedInvoice.Date.ToShortDateString();
            tvLastPayDate=view.FindViewById<TextView>(Resource.Id.tvInvoiceLastPayDate);
            tvLastPayDate.Text = "Son Ödeme Tarihi:\n" + Session.SelectedInvoice.LastPayDate.ToShortDateString();
            
        }
    }
}