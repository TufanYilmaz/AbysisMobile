using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AbysisMobile.Helpers;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfDataGrid;

namespace AbysisMobile.Fragments
{
    public class FragmentInvoices : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static FragmentInvoices NewInstance()
        {
            return new FragmentInvoices();
        }
        public SfDataGrid gridInvoices;
        public ProgressBar progressBar;
        public TextView tvHelper;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.fragment_invoices, container, false);
            tvHelper = view.FindViewById<TextView>(Resource.Id.tvInoiceDetailHelper);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressInvoice);
            //progressBar.SystemUiVisibilityChange += (s, e) =>
            //{
            //    if (e.Visibility == StatusBarVisibility.Visible)
            //};
            gridInvoices = view.FindViewById<SfDataGrid>(Resource.Id.sfAllInvoices);
            gridInvoices.ColumnSizer = ColumnSizer.Star;
            gridInvoices.ScrollingMode = ScrollingMode.Pixel;

            //var invoices = SessionHelper.GetInvoicesForSelectedDepartment();
            //gridInvoices.ItemsSource = invoices;
            //gridInvoices.Visibility = ViewStates.Gone;
            //gridInvoices.ScrollingMode = ScrollingMode.PixelLine;

            gridInvoices.VerticalOverScrollMode = VerticalOverScrollMode.None;
            gridInvoices.AllowPullToRefresh = false;
            gridInvoices.AutoGeneratingColumn += GridInvoices_AutoGeneratingColumn;
            gridInvoices.GridLongPressed += GridInvoices_GridLongPressed;
            gridInvoices.AllowSorting = true;
            gridInvoices.SortColumnDescriptions.Add(new SortColumnDescription()
            {
                ColumnName = "Date",
                SortDirection = System.ComponentModel.ListSortDirection.Descending
            });
            return view;
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Task.Run(() =>
            {
                //var invoices = SessionHelper.GetInvoicesForSelectedDepartment();
                //gridInvoices.ItemsSource = invoices;
                //progressBar.Visibility = ViewStates.Gone;
                //gridInvoices.Visibility = ViewStates.Visible;
            });
            base.OnViewCreated(view, savedInstanceState);
        }
        
        public  void GridInvoices_GridLongPressed(object sender, GridLongPressedEventArgs e)
        {
            var data = (Invoices.invoice)e.RowData;
            Bundle b = new Bundle();
            b.PutInt("InvoiceId", data.Id);
            b.PutInt("PeriodId", data.Period.Id);
            var dialogInvoice = new DialogInvoice() { Arguments=b };
            dialogInvoice.SetStyle((int)Android.App.DialogFragmentStyle.NoTitle, Android.Resource.Style.ThemeMaterialLightNoActionBarFullscreen);
            dialogInvoice.Show(ChildFragmentManager, "");
            //Toast.MakeText(Context,e.RowData)
        }



        private void GridInvoices_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.MappingName == "Id")
            {
                e.Column.IsHidden = true;

            }
            else if (e.Column.MappingName == "Date")
            {
                e.Column.HeaderText = "Tarih";
            }
            else if (e.Column.MappingName == "InvoiceNumber")
            {
                e.Column.HeaderText = "Fatura No";
            }
            else if (e.Column.MappingName == "InvoiceTotal")
            {
                e.Column.HeaderText = "Tutar";
                e.Column.Format = "n2";
            }
            else
            {
                e.Column.IsHidden = true;
            }

        }

        public  List<Invoices.invoice> GetData()
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year-5, 1, 1);
            List<Invoices.invoice> result = new List<Invoices.invoice>();
            try
            {

                result = Services.invoicesClient.GetInvoiceBySubscriber(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, 1, start, now, Session.SelectedMeter.SubscriberId).Value.ToList();

            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.Message, ToastLength.Short).Show();
            }
            //foreach (var item in Invoices)
            //{
            //    item.Period.Info = "<a href=\"" + "ShowInvoice.aspx?id=" + item.Id.ToString() + "&pid=" + item.Period.Id.ToString() + "\" target=\"_blank\">Fatura Göster<a/>";
            //}
            return result;
        }
    }
}