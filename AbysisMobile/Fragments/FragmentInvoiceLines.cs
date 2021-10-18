using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfDataGrid;

namespace AbysisMobile.Fragments
{
    public class FragmentInvoiceLines : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static FragmentInvoiceLines NewInstance(string title)
        {
            var b = new Bundle();
            b.PutString("Title", title);
            return new FragmentInvoiceLines() { Arguments = b };
        }
        SfDataGrid sfInvoiceLines;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =inflater.Inflate(Resource.Layout.invoice_lines, container, false);
            sfInvoiceLines = view.FindViewById<SfDataGrid>(Resource.Id.sfInvoiceLines);
            sfInvoiceLines.RowHeight = 30;
            sfInvoiceLines.ItemsSource = Session.SelectedInvoice.Lines;
            //sfInvoiceLines.FrozenColumnsCount =4;
            //sfInvoiceLines.AutoGeneratingColumn += SfInvoiceLines_AutoGeneratingColumn;
            sfInvoiceLines.AutoGenerateColumns = false;
            string[] mappingnames = new string[] {
                "Info", "BeginIndex", "EndIndex", "Multiplier", "FixedAmount", "AdditionalAmount1",
                "AdditionalAmount2","Amount","Price","Total" };
            string[] headernames = new string[] {
                "Açıklama", "İlk Endex", "Son Endex",Session.SelectedMeter.DepartmentId != 3 ? "Çarpan":"AOBE", "Tüketim", "Bakır Kaybı",
                "+/- Tüketim","Toplam Tüketim","Birim Fiyat (TL)","Tutar (TL)" };
            GridTextColumn Column;
            for (int i = 0; i < mappingnames.Length; i++)
            {
                if (Session.SelectedMeter.DepartmentId == 3 && mappingnames[i] == "AdditionalAmount1")
                    continue;
                Column = new GridTextColumn
                {
                    MappingName = mappingnames[i],
                    HeaderText = headernames[i]
                };
                sfInvoiceLines.Columns.Add(Column);
            }
            sfInvoiceLines.FrozenColumnsCount = 1;
            //Total//Price//Amount//AdditionalAmount2//AdditionalAmount1//FixedAmount//Multiplier//EndIndex//BeginIndex//Info

            //Column = new GridTextColumn
            //{
            //    MappingName = "Multiplier",
            //    HeaderText = "Çarpan"
            //};
            //sfInvoiceLines.Columns.Add(Column);
            //sfInvoiceLines.co
            //return base.OnCreateView(inflater, container, savedInstanceState);
            return view;
        }


    }
}