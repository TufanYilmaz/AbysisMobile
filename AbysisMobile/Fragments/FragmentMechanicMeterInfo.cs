using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbysisMobile.Helpers.MyObjects;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfDataGrid;

namespace AbysisMobile.Fragments
{
    class FragmentMechanicMeterInfo : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static FragmentMechanicMeterInfo NewInstance(string title)
        {
            var b = new Bundle();
            b.PutString("Title", title);
            var smmry = new FragmentMechanicMeterInfo { Arguments = b };
            return smmry;
        }

        SfDataGrid summaryGrid;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.meter_summary_grid, container, false);
            ManageUIs(view);
            //ManageData();
            return view;
        }
        List<BasicHelperClass> mechanicItems = new List<BasicHelperClass>();

        private void ManageUIs(View view)
        {
            summaryGrid = view.FindViewById<SfDataGrid>(Resource.Id.sfDGMeterConsumption);

            if (Session.SelectedMeter.Mechanics != null && Session.SelectedMeter.Mechanics.Length > 0 && Session.SelectedMeter.DepartmentId == 3)
            {
                mechanicItems.Add(new BasicHelperClass { Code = "Hat No", Value = Session.SelectedMeter.Mechanics[0].LineNo.ToString() });
                mechanicItems.Add(new BasicHelperClass { Code = "Mekanik Seri No", Value = Session.SelectedMeter.Mechanics[0].Serial });
                mechanicItems.Add(new BasicHelperClass { Code = "Tip", Value = Session.SelectedMeter.Mechanics[0].Type });
                mechanicItems.Add(new BasicHelperClass { Code = "Sınıf", Value = Session.SelectedMeter.Mechanics[0].Class });
                mechanicItems.Add(new BasicHelperClass { Code = "Marka", Value = Session.SelectedMeter.Mechanics[0].Mark });
                mechanicItems.Add(new BasicHelperClass { Code = "Model", Value = Session.SelectedMeter.Mechanics[0].Model });
                mechanicItems.Add(new BasicHelperClass { Code = "Mühür No", Value = Session.SelectedMeter.Mechanics[0].SealNumber });
                mechanicItems.Add(new BasicHelperClass { Code = "Üretim Yılı", Value = Session.SelectedMeter.Mechanics[0].ProdYear });
                mechanicItems.Add(new BasicHelperClass { Code = "Son Kalibrasyon", Value = Session.SelectedMeter.Mechanics[0].CalibrationDate.ToString("dd.MM.yyyy") });
                mechanicItems.Add(new BasicHelperClass { Code = "QMin", Value = Session.SelectedMeter.Mechanics[0].QMin.ToString() });
                mechanicItems.Add(new BasicHelperClass { Code = "QMax", Value = Session.SelectedMeter.Mechanics[0].QMax.ToString() });
                mechanicItems.Add(new BasicHelperClass { Code = "Basınç Değeri", Value = Session.SelectedMeter.Mechanics[0].PressValue.ToString() });

            }
            summaryGrid.ItemsSource = mechanicItems;
            summaryGrid.RowHeight = 30;
            summaryGrid.ColumnSizer = ColumnSizer.LastColumnFill;
            //summaryGrid.FrozenColumnsCount = 2;
            summaryGrid.AllowPullToRefresh = false;
            summaryGrid.VerticalOverScrollMode = VerticalOverScrollMode.None;
            summaryGrid.ScrollingMode = ScrollingMode.PixelLine;
            summaryGrid.AutoGeneratingColumn += SummaryGrid_AutoGeneratingColumn;
            //summaryGrid.ItemsSourceChanged

          
        }

        private void SummaryGrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.MappingName == "Code")
            {
                e.Column.HeaderText = "Bilgi";
            }
            if (e.Column.MappingName == "Value")
            {
                e.Column.HeaderText = "Değer";
            }
        }
    }
    
}