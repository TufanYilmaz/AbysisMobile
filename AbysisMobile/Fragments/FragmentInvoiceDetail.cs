using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;


namespace AbysisMobile.Fragments
{
    public class FragmentInvoiceDetail : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //string jsonInvoice = Arguments.GetString("InvoiceJson");
            //try
            //{

            //}
            //catch (Exception ex)
            //{

            //}
            // Create your fragment here
        }
        //TextView
        public static FragmentInvoiceDetail NewInstance(string title)
        {
            var b = new Bundle();
            b.PutString("Title", title);
            return new FragmentInvoiceDetail() { Arguments = b };
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.invoice_detail, container, false);
            ManageUIs(view);
            //return base.OnCreateView(inflater, container, savedInstanceState);
            return view;
        }
        TextView tvInvoiceKDVLabel;
        TextView tvInvoiceSubTotal;
        TextView tvInvoiceFundTotal;
        TextView tvInvoiceRaiceAmount;
        TextView tvInvoiceVatListValue;
        TextView tvInvoiceTotal;
        private void ManageUIs(View view)
        {
            tvInvoiceKDVLabel = view.FindViewById<TextView>(Resource.Id.tvInvoiceKDVLabel);
            tvInvoiceSubTotal = view.FindViewById<TextView>(Resource.Id.tvInvoiceSubTotal);
            tvInvoiceFundTotal = view.FindViewById<TextView>(Resource.Id.tvInvoiceFundTotal);
            tvInvoiceRaiceAmount = view.FindViewById<TextView>(Resource.Id.tvInvoiceRaiceAmount);
            tvInvoiceVatListValue = view.FindViewById<TextView>(Resource.Id.tvInvoiceVatListValue);
            tvInvoiceTotal = view.FindViewById<TextView>(Resource.Id.tvInvoiceTotal);
            tvInvoiceKDVLabel.Text ="%"+ Session.SelectedInvoice.VatList[0].Rate.ToString()+" KDV";
            tvInvoiceSubTotal.Text = Session.SelectedInvoice.SubTotal1.ToString()+ "₺";
            tvInvoiceFundTotal.Text = Session.SelectedInvoice.FundTotal.ToString() + "₺";
            tvInvoiceRaiceAmount.Text = Session.SelectedInvoice.RaiseAmount.ToString() + "₺";
            tvInvoiceVatListValue.Text = Session.SelectedInvoice.VatList[0].Value.ToString() + "₺";
            tvInvoiceTotal.Text = Session.SelectedInvoice.InvoiceTotal.ToString() + "₺";
        }
    }
}