using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AbysisMobile.Indexes;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.SfPicker;
using Syncfusion.SfDataGrid;
using static Android.App.DatePickerDialog;
using static Android.App.TimePickerDialog;

namespace AbysisMobile.Fragments
{
    public class FragmentMeterQuery : Fragment, IOnDateSetListener, IOnTimeSetListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);



            // Create your fragment here
        }
        public Spinner spIndices;
        public Button btnDate1;
        public Button btnDate2;
        public Button btnQuery;
        public SfDataGrid indicesGrid;
        private bool beginDateSelection = false;
        private bool endDateSelection = false;
        private DateTime d1 = new DateTime();
        private DateTime d2 = new DateTime();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.fragment_meter_query, container, false);
            ManageUIs(view);
            LoadIndicies();
            LoadSessionDates();

            return view;
        }
        //List<int> ValidDataTypes = new List<int> { 0, 8 };
        private void LoadIndicies()
        {
            try
            {
                //var list = Services.indexesClient.GetIndexList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId);
                //List<indexInfo> indices = list.Value.ToList().FindAll(i => ValidDataTypes.Contains(i.DataType) && i.WebVisible);
                var indices = Helpers.SessionHelper.LoadIndexList();
                Adapters.IndexListAdapter adap = new Adapters.IndexListAdapter(Context, indices);
                spIndices.Adapter = adap;
            }
            catch (Exception)
            {
                Toast.MakeText(Context, "Index List Hata", ToastLength.Short).Show();
            }
        }

        private void ManageUIs(View view)
        {
            spIndices = view.FindViewById<Spinner>(Resource.Id.spnQueryIndexes);
            btnDate1 = view.FindViewById<Button>(Resource.Id.btnQueryDate1);
            btnDate1.Click += BtnDate1_Click;
            btnDate2 = view.FindViewById<Button>(Resource.Id.btnQueryDate2);
            btnDate2.Click += BtnDate2_Click;
            btnQuery = view.FindViewById<Button>(Resource.Id.btnMQuery);
            btnQuery.Click += BtnQuery_Click;
            indicesGrid = view.FindViewById<SfDataGrid>(Resource.Id.sfDGMQindices);
            indicesGrid.ColumnSizer = ColumnSizer.Star;
            indicesGrid.RowHeight = 30;
            indicesGrid.AllowPullToRefresh = false;
            indicesGrid.AutoGeneratingColumn += IndicesGrid_AutoGeneratingColumn;
            indicesGrid.AllowSorting = true;
            indicesGrid.AllowTriStateSorting = true;
            indicesGrid.OverScrollMode = OverScrollMode.Never;
            indicesGrid.SortColumnDescriptions.Add(new SortColumnDescription()
            {
                ColumnName = "dateTime",
                SortDirection = System.ComponentModel.ListSortDirection.Descending
            });
        }

        private void IndicesGrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.MappingName == "RawValue")
            {
                e.Column.HeaderText = "Okunan Değer";
                e.Column.Format = "n3";
                //e.Column.w
            }
            else if (e.Column.MappingName == "dateTime")
            {
                e.Column.HeaderText = "Tarih";
                e.Column.Format = "dd/MM/yyyy HH:mm";
            }
            else if (e.Column.MappingName == "Multiplier")
            {
                e.Column.HeaderText = "Çarpan";
            }
            else if (e.Column.MappingName == "FixedValue")
            {
                e.Column.HeaderText = "Endeks değeri";
                e.Column.Format = "n2";
            }
            else
            {
                e.Column.IsHidden = true;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            //if (spIndices.SelectedItemPosition > -1)
            //{
            //    //if (Session.SelectedName.ContainsKey(SelectedName))
            //    //    Session.SelectedName[SelectedName] = selectedindex;
            //    //else
            //    //    Session.SelectedName.Add(SelectedName, selectedindex);
            //    var ResultTable =Services.indexesClient.GetIndexValueList(
            //        Session.LoginData.Value.ToString(), 
            //        Session.SelectedMeter.DepartmentId, 
            //        d1, 
            //        d2, 
            //        Session.SelectedMeter.SubscriberId,
            //        Session.SelectedMeter.Id, 
            //        (int)spIndices.GetItemIdAtPosition(spIndices.SelectedItemPosition));
            //    var result = ResultTable.Value.ToList();
                
            //    //if (Session.ResultName.ContainsKey(ResultName))
            //    //    Session.ResultName[ResultName] = ResultTable;
            //    //else
            //    //    Session.ResultName.Add(ResultName, ResultTable);
            //    //indexValuesResultTables = new List<MeterQueryIndexResultTable>();
            //    //foreach (var item in ResultTable.Value)
            //    //{
            //    //    indexValuesResultTables.Add(new MeterQueryIndexResultTable(item));
            //    //}
            //    var orderedByDate = result.OrderByDescending(r => r.dateTime).ToList();
            //    indicesGrid.ItemsSource = orderedByDate;
            //}
        }
        public void BtnQueryAction()
        {
            if (spIndices.SelectedItemPosition > -1)
            {
                //if (Session.SelectedName.ContainsKey(SelectedName))
                //    Session.SelectedName[SelectedName] = selectedindex;
                //else
                //    Session.SelectedName.Add(SelectedName, selectedindex);
                var ResultTable = Services.indexesClient.GetIndexValueList(
                    Session.LoginData.Value.ToString(),
                    Session.SelectedMeter.DepartmentId,
                    d1,
                    d2,
                    Session.SelectedMeter.SubscriberId,
                    Session.SelectedMeter.Id,
                    (int)spIndices.GetItemIdAtPosition(spIndices.SelectedItemPosition));
                var result = ResultTable.Value.ToList();

                //if (Session.ResultName.ContainsKey(ResultName))
                //    Session.ResultName[ResultName] = ResultTable;
                //else
                //    Session.ResultName.Add(ResultName, ResultTable);
                //indexValuesResultTables = new List<MeterQueryIndexResultTable>();
                //foreach (var item in ResultTable.Value)
                //{
                //    indexValuesResultTables.Add(new MeterQueryIndexResultTable(item));
                //}
                var orderedByDate = result.OrderByDescending(r => r.dateTime).ToList();
                indicesGrid.ItemsSource = orderedByDate;
            }
        }

        private void BtnDate2_Click(object sender, EventArgs e)
        {
            endDateSelection = true;
            beginDateSelection = false;
            Android.App.TimePickerDialog timePicker = new Android.App.TimePickerDialog(Context, this, d2.Hour, d2.Minute, true);
            timePicker.Show();
            Android.App.DatePickerDialog datepicker = new Android.App.DatePickerDialog(Context, this, d2.Year, d2.Month-1, d2.Day);
            datepicker.Show();
        }

        private void BtnDate1_Click(object sender, EventArgs e)
        {
            endDateSelection = false;
            beginDateSelection = true;
            Android.App.TimePickerDialog timePicker = new Android.App.TimePickerDialog(Context, this, d1.Hour, d1.Minute, true);
            timePicker.Show();
            Android.App.DatePickerDialog datepicker = new Android.App.DatePickerDialog(Context, this, d1.Year, d1.Month-1, d1.Day);
            datepicker.Show();


        }
        private void LoadSessionDates()
        {
            DateTime now = DateTime.Now;
            d1 = new DateTime(now.Year, now.Month, now.Day);
            d1 = d1.AddDays(-7);
            d2 = now;
            if (Session.Date1.Year > 1990) d1 = Session.Date1;
            if (Session.Date2.Year > 1990) d2 = Session.Date2;
            btnDate1.Text = d1.ToString();
            btnDate2.Text = d2.ToString();
        }
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (beginDateSelection)
            {
                d1 = new DateTime(year, month+1, dayOfMonth, d1.Hour, d1.Minute, 0);
                btnDate1.Text = d1.ToString();
                Session.Date1 = d1;
            }
            if (endDateSelection)
            {
                d2 = new DateTime(year, month+1, dayOfMonth, d2.Hour, d2.Minute, d2.Second);
                btnDate2.Text = d2.ToString();
                Session.Date2 = d2;
            }
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            if (beginDateSelection)
            {
                d1 = new DateTime(d1.Year, d1.Month, d1.Day, hourOfDay, minute, 0);
                btnDate1.Text = d1.ToString();
                Session.Date1 = d1;
            }
            if (endDateSelection)
            {
                d2 = new DateTime(d2.Year, d2.Month, d2.Day, hourOfDay, minute, 0);
                btnDate2.Text = d2.ToString();
                Session.Date2 = d2;
            }
        }
    }
}