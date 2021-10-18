using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AbysisMobile.Authentication;
using AbysisMobile.Helpers;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using static Android.App.DatePickerDialog;
using static Android.App.TimePickerDialog;

namespace AbysisMobile.Fragments
{
    public class FragmentTriFaze : Fragment, IOnDateSetListener, IOnTimeSetListener
    {

        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public SfChart TrifazeChart { get; set; }
        public Button QueryButton { get; set; }
        public LinearLayout LinearLayoutDates { get; set; }
        public LinearLayout LoadingLayout { get; set; }
        public ProgressBar LoadingProgressBar { get; set; }
        public TextView tvServiceInfo { get; set; }
        public Button btnDate1;
        public Button btnDate2;
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);
            Date2 = DateTime.Now;
            Date1 = Date2.AddDays(-1);// new DateTime(Date2.Year, Date2.Month, 1);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view=  inflater.Inflate(Resource.Layout.trifaze_layout, container, false);
            ManageUIs(view);
            return view;
        }

        private void ManageUIs(View view)
        {
            //LoadingLayout = view.FindViewById<LinearLayout>(Resource.Id.trifazeLoadingLayout);
            LoadingLayout = view.FindViewById<LinearLayout>(Resource.Id.trifazeLoadingLayout);
            LoadingLayout.Visibility = ViewStates.Gone;
            LoadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progressTrifazeAnswer);
            LinearLayoutDates = view.FindViewById<LinearLayout>(Resource.Id.layoutTrifazeDateSelection);
            btnDate1 = view.FindViewById<Button>(Resource.Id.btnTrifazeDate1);
            btnDate1.Text = Date1.ToString();
            btnDate1.Click += BtnDate1_Click;

            btnDate2 = view.FindViewById<Button>(Resource.Id.btnTrifazeDate2);
            btnDate2.Text = Date2.ToString();
            btnDate2.Click += BtnDate2_Click;
            QueryButton = view.FindViewById<Button>(Resource.Id.btnTrifazeQuery);
            TrifazeChart = view.FindViewById<SfChart>(Resource.Id.sfTriFazeGraph);

        }

        private void BtnDate2_Click(object sender, EventArgs e)
        {
            endDateSelection = true;
            beginDateSelection = false;
            Android.App.TimePickerDialog timePicker = new Android.App.TimePickerDialog(Context, this, Date2.Hour, Date2.Minute, true);
            timePicker.Show();
            Android.App.DatePickerDialog datepicker = new Android.App.DatePickerDialog(Context, this, Date2.Year, Date2.Month - 1, Date2.Day);
            datepicker.Show();
        }

        private void BtnDate1_Click(object sender, EventArgs e)
        {
            endDateSelection = false;
            beginDateSelection = true;
            Android.App.TimePickerDialog timePicker = new Android.App.TimePickerDialog(Context, this, Date1.Hour, Date1.Minute, true);
            timePicker.Show();
            Android.App.DatePickerDialog datepicker = new Android.App.DatePickerDialog(Context, this, Date1.Year, Date1.Month - 1, Date1.Day);
            datepicker.Show();
        }

        private bool beginDateSelection = false;
        private bool endDateSelection = false;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (beginDateSelection)
            {
                Date1 = new DateTime(year, month + 1, dayOfMonth, Date1.Hour, Date1.Minute, 0);
                btnDate1.Text = Date1.ToString();
                //Session.Date1 = Date1;
            }
            if (endDateSelection)
            {
                Date2 = new DateTime(year, month + 1, dayOfMonth, Date2.Hour, Date2.Minute, Date2.Second);
                btnDate2.Text = Date2.ToString();
               // Session.Date2 = Date2;
            }
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            if (beginDateSelection)
            {
                Date1 = new DateTime(Date1.Year, Date1.Month, Date1.Day, hourOfDay, minute, 0);
                btnDate1.Text = Date1.ToString();
               // Session.Date1 = Date1;
            }
            if (endDateSelection)
            {
                Date2 = new DateTime(Date2.Year, Date2.Month, Date2.Day, hourOfDay, minute, 0);
                btnDate2.Text = Date2.ToString();
                //Session.Date2 = Date2;
            }
        }
    }
}