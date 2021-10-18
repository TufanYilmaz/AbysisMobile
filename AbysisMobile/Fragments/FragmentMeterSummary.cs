using AbysisMobile.Helpers.MyObjects;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Syncfusion.SfDataGrid;
using System.Collections.Generic;
using System.Linq;

namespace AbysisMobile.Fragments
{
    public class FragmentMeterSummary : Fragment
    {
        public static FragmentMeterSummary NewInstance(string title)
        {
            var b = new Bundle();
            b.PutString("Title", title);
            var smmry = new FragmentMeterSummary { Arguments = b };
            return smmry;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        SfDataGrid summaryGrid;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.meter_summary_grid, container, false);

            ManageUIs(view);
            ManageData();
            return view;
        }

        private void ManageData()
        {
            //GridTextColumn lineIdColumn = new GridTextColumn();
            //lineIdColumn.MappingName = "INDEX_ID";
            //lineIdColumn.HeaderText = "ID";
            ////lineIdColumn.Width = 0;

            //GridTextColumn lineIndexColumn = new GridTextColumn();
            //lineIndexColumn.MappingName = "INDEX_CODE";
            //lineIndexColumn.HeaderText = "Endex Kodu";

            //summaryGrid.Columns.Add(lineIdColumn);
            //summaryGrid.Columns.Add(lineIndexColumn);
        }

        private void ManageUIs(View view)
        {
            summaryGrid = view.FindViewById<SfDataGrid>(Resource.Id.sfDGMeterConsumption);
            summaryGrid.RowHeight = 30;
            summaryGrid.ColumnSizer = ColumnSizer.Auto;
            summaryGrid.ItemsSource = Session.SelectedMeter.Consumptions.ToList();
            //summaryGrid.FrozenColumnsCount = 2;
            summaryGrid.AllowPullToRefresh = false;
            summaryGrid.VerticalOverScrollMode = VerticalOverScrollMode.None;
            summaryGrid.ScrollingMode = ScrollingMode.PixelLine;
            summaryGrid.AutoGeneratingColumn += SummaryGrid_AutoGeneratingColumn;
            summaryGrid.FrozenColumnsCount = 4;
            //summaryGrid.ItemsSourceChanged
           
            //List<BasicHelperClass> mechanicItems = new List<BasicHelperClass>();
            //if (Session.SelectedMeter.Mechanics != null && Session.SelectedMeter.Mechanics.Length > 0 && Session.SelectedMeter.DepartmentId == 3)
            //{
            //    mechanicItems.Add(new BasicHelperClass { Code = "Hat No", Value = Session.SelectedMeter.Mechanics[0].LineNo.ToString() });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Mekanik Seri No", Value = Session.SelectedMeter.Mechanics[0].Serial });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Tip", Value = Session.SelectedMeter.Mechanics[0].Type });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Sınıf", Value = Session.SelectedMeter.Mechanics[0].Class });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Marka", Value = Session.SelectedMeter.Mechanics[0].Mark });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Model", Value = Session.SelectedMeter.Mechanics[0].Model });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Mühür No", Value = Session.SelectedMeter.Mechanics[0].SealNumber });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Üretim Yılı", Value = Session.SelectedMeter.Mechanics[0].ProdYear });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Son Kalibrasyon", Value = Session.SelectedMeter.Mechanics[0].CalibrationDate.ToString("dd.MM.yyyy") });
            //    mechanicItems.Add(new BasicHelperClass { Code = "QMin", Value = Session.SelectedMeter.Mechanics[0].QMin.ToString() });
            //    mechanicItems.Add(new BasicHelperClass { Code = "QMax", Value = Session.SelectedMeter.Mechanics[0].QMax.ToString() });
            //    mechanicItems.Add(new BasicHelperClass { Code = "Basınç Değeri", Value = Session.SelectedMeter.Mechanics[0].PressValue.ToString() });

            //    //mechanicGrid = view.FindViewById<SfDataGrid>(Resource.Id.sfDGMeterMEchanics);
            //    //mechanicGrid.RowHeight = 30;
            //    //mechanicGrid.ColumnSizer = ColumnSizer.LastColumnFill;
            //    //mechanicGrid.ItemsSource = mechanicItems;
            //    //mechanicGrid.AllowPullToRefresh = false;
            //    //mechanicGrid.VerticalOverScrollMode = VerticalOverScrollMode.None;
            //    //mechanicGrid.ScrollingMode = ScrollingMode.PixelLine;
            //    //mechanicGrid.Visibility = ViewStates.Visible;
            //}
           
        }



        private void SummaryGrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.MappingName == "INDEX_ID")
            {
                e.Column.IsHidden = true;

            }
            else if (e.Column.MappingName == "INDEX_CODE")
            {
                e.Column.HeaderText = "Endeks Kodu";
            }
            else if (e.Column.MappingName == "INDEX_INFO")
            {
                e.Column.HeaderText = "Açıklama";
            }
            else if (e.Column.MappingName == "BEGIN_INDEX")
            {
                e.Column.HeaderText = "İlk endeks";
                e.Column.Format = "n2";
            }
            else if (e.Column.MappingName == "END_INDEX")
            {
                e.Column.HeaderText = "Son endeks";
                e.Column.Format = "n2";
            }
            else if (e.Column.MappingName == "INDEX_AMOUNT")
            {
                e.Column.HeaderText = "Endeks Farkı";
                e.Column.Format = "n2";
            }
            else if (e.Column.MappingName == "MULTIPLIER")
            {
                e.Column.HeaderText = "Çarpan";
                e.Column.Format = "n1";
            }
            else if (e.Column.MappingName == "ADDITIONAL_AMOUNT")
            {
                e.Column.HeaderText = "Ek tüketim";
                e.Column.Format = "n2";
            }
            else if (e.Column.MappingName == "AMOUNT")
            {
                e.Column.HeaderText = "Tüketim";
                e.Column.Format = "n2";
            }
            else if (e.Column.MappingName == "FORMULA")
            {
                e.Column.HeaderText = "Yasal Sınır";
            }
            else if (e.Column.MappingName == "STATE")
            {
                e.Column.HeaderText = "Durum";
            }
            else
            {
                e.Column.IsHidden = true;

            }
            //else if (e.Column.MappingName == "ORDERBY")
            //{
            //    e.Column.IsHidden = true;
            //}
            //else if (e.Column.MappingName == "UNIT")
            //{
            //    e.Column.IsHidden = true;
            //}
            //else if (e.Column.MappingName == "USE_MULTIPLIER")
            //{
            //    e.Column.IsHidden = true;
            //}
            //else if (e.Column.MappingName == "DATETIME_")
            //{
            //    e.Column.IsHidden = true;
            //}


        }
    }
}