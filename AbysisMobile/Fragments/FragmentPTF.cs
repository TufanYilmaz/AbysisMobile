using AbysisMobile.Helpers;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using Syncfusion.Data;
using Syncfusion.SfDataGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using static Android.App.DatePickerDialog;

namespace AbysisMobile.Fragments
{
    public class FragmentPTF : Fragment, IOnDateSetListener
    {
        DateTime datePTF;
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);
            datePTF = DateTime.Now;


            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.fragment_ptf, container, false);
            ManageUIs(view);
            DisplayPTF();
            btnChooseDate.Text = datePTF.ToShortDateString();
            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
        Button btnPreviousDay;
        Button btnNextDay;
        Button btnToday;
        Button btnChooseDate;
        SfChart sfPtfChart;
        SfDataGrid sfPtfGrid;
        ImageButton btnChangeDisplay;
        private void ManageUIs(View view)
        {
            btnPreviousDay = view.FindViewById<Button>(Resource.Id.btnPtfPrevious);
            btnPreviousDay.Click += (s, e) =>
            {
                datePTF = datePTF.AddDays(-1);
                btnChooseDate.Text = datePTF.ToShortDateString();
                DisplayPTF();
            };
            btnNextDay = view.FindViewById<Button>(Resource.Id.btnPtfNext);
            btnNextDay.Click += (s, e) =>
            {
                datePTF = datePTF.AddDays(1);
                btnChooseDate.Text = datePTF.ToShortDateString();
                DisplayPTF();
            };
            btnToday = view.FindViewById<Button>(Resource.Id.btnPtfToday);
            btnToday.Click += (s, e) =>
            {
                datePTF = DateTime.Today;
                btnChooseDate.Text = datePTF.ToShortDateString();
                DisplayPTF();
            };
            btnChooseDate = view.FindViewById<Button>(Resource.Id.btnPtfDate);
            btnChooseDate.Click += BtnChooseDate_Click;
            btnChangeDisplay = view.FindViewById<ImageButton>(Resource.Id.btnPtfChangeDisplayMode);
            btnChangeDisplay.SetImageResource(Resource.Drawable.bluechartlinea);
            //btnChangeDisplay.scale
            btnChangeDisplay.Click += BtnChangeDisplay_Click;
            sfPtfChart = view.FindViewById<SfChart>(Resource.Id.sfPtfChart);
            sfPtfGrid = view.FindViewById<SfDataGrid>(Resource.Id.sfPtfGrid);
            sfPtfChart.Visibility = ViewStates.Gone;
            sfPtfGrid.Visibility = ViewStates.Visible;
            sfPtfGrid.VerticalOverScrollMode = VerticalOverScrollMode.None;
            sfPtfGrid.AllowPullToRefresh = false;
            //sfPtfGrid.CellRenderers.Remove("TableSummary");
            //sfPtfGrid.CellRenderers.Add("TableSummary", new GridTableSummaryCellRendererExt());

            sfPtfGrid.AutoGeneratingColumn += SfPtfGrid_AutoGeneratingColumn;
            sfPtfGrid.ScrollingMode = ScrollingMode.PixelLine;
            sfPtfGrid.ColumnSizer = ColumnSizer.Star;

            sfPtfGrid.GridStyle = new CustomGridStyle();
            sfPtfGrid.RowHeight = 30;
            sfPtfGrid.OverScrollMode = OverScrollMode.Never;
            GridTableSummaryRow bottomSummaryRow = new GridTableSummaryRow();
            bottomSummaryRow.Position = Position.Bottom;
            bottomSummaryRow.Title = "Fiyat Ortalaması = {AveragePrice}";
            bottomSummaryRow.ShowSummaryInRow = true;
            bottomSummaryRow.SummaryColumns.Add(new GridSummaryColumn()
            {
                Name = "AveragePrice",
                MappingName = "Price",
                Format = "{Average:n2}",
                SummaryType = SummaryType.DoubleAggregate,
            });
            sfPtfGrid.TableSummaryRows.Add(bottomSummaryRow);

            ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
            {
                SelectionZoomingEnabled = true,
                ZoomMode = ZoomMode.X

            };
            ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior
            {
                ShowLabel = true,
                ShowLine = true
            };

            sfPtfChart.Behaviors.Clear();
            sfPtfChart.Behaviors.Add(trackballBehavior);
            sfPtfChart.Behaviors.Add(zoomPanBehavior);
            sfPtfChart.PrimaryAxis = new DateTimeAxis() { IntervalType = DateTimeIntervalType.Hours, Interval = 1 };

            sfPtfChart.Title.Text = "PTF Fiyatları";
            sfPtfChart.Title.TextSize = 10;
            sfPtfChart.PrimaryAxis.LabelRotationAngle = 90;

        }

        private void SfPtfGrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.MappingName == "DateTime")
            {
                e.Column.HeaderText = "Tarih";
                e.Column.Format = "dd/MM/yyyy HH:mm";
            }
            else if (e.Column.MappingName == "Price")
            {
                e.Column.HeaderText = "Fiyat";
                e.Column.Format = "n2";
                //e.Column.w
            }

            else
            {
                e.Column.IsHidden = true;
            }
        }

        bool disState = false;
        private void BtnChangeDisplay_Click(object sender, EventArgs e)
        {
            try
            {

                sfPtfChart.Visibility = disState ? ViewStates.Gone : ViewStates.Visible;
                sfPtfGrid.Visibility = disState ? ViewStates.Visible : ViewStates.Gone;
                disState = !disState;
                if (disState)
                    btnChangeDisplay.SetImageResource(Resource.Drawable.bluegrida);
                else
                    btnChangeDisplay.SetImageResource(Resource.Drawable.bluechartlinea);

            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.Message, ToastLength.Short).Show();
            }
        }

        private void BtnChooseDate_Click(object sender, EventArgs e)
        {
            Android.App.DatePickerDialog datepicker = new Android.App.DatePickerDialog(Context, this, datePTF.Year, datePTF.Month - 1, datePTF.Day);
            datepicker.Show();
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            datePTF = new DateTime(year, month + 1, dayOfMonth);
            btnChooseDate.Text = datePTF.ToShortDateString();
            DisplayPTF();
        }

        private void SetPtfChart(List<Invoices.PTFPrice> prices)
        {

            //SfChart chart = new SfChart(Context);
            double max = (double)prices.Max(x => x.Price);
            sfPtfChart.SecondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = max + 100, Interval=25 };
            sfPtfChart.Series.Clear();
            var lineSeries = new FastLineSeries()
            {
                ItemsSource = prices,
                XBindingPath = "DateTime",
                YBindingPath = "Price",
            };
            lineSeries.DataMarker.ShowLabel = true;
            lineSeries.DataMarker.LabelStyle.Angle = 90;
            lineSeries.DataMarker.LabelStyle.StrokeColor = Color.Orange;

            lineSeries.DataMarker.ConnectorLineStyle.ConnectorHeight = 25;
            lineSeries.DataMarker.ConnectorLineStyle.ConnectorRotationAngle = 90;
            lineSeries.DataMarker.ConnectorLineStyle.StrokeColor = Color.DarkOrange;
            lineSeries.DataMarker.ConnectorLineStyle.StrokeWidth = 2;
            lineSeries.DataMarker.ConnectorLineStyle.PathEffect = new DashPathEffect(new float[] { 3, 3 }, 3);

            lineSeries.DataMarker.MarkerStrokeColor = Color.Orange;
            lineSeries.DataMarker.MarkerColor = Color.OrangeRed;

            sfPtfChart.Series.Add(lineSeries);
        }
        private void SetPtfGrid(List<Invoices.PTFPrice> prices)
        {
            sfPtfGrid.ItemsSource = prices;

        }
        private void DisplayPTF()
        {
            DateTime beginDate = new DateTime(datePTF.Year, datePTF.Month, datePTF.Day, 0, 0, 0);
            DateTime endDate = new DateTime(datePTF.Year, datePTF.Month, datePTF.Day, 23, 30, 0);
            var prices = Services.invoicesClient.GetPTFPrices(Session.LoginData.Value.ToString(), beginDate, endDate).Value.ToList();
            if (prices.Count < 1)
            {
                Toast.MakeText(Context, "Gösterilecek veri bulunamadı", ToastLength.Short);
                sfPtfChart.Series.Clear();
                sfPtfGrid.ItemsSource = null;
                return;

            }
            SetPtfChart(prices);
            SetPtfGrid(prices);
        }
    }
}