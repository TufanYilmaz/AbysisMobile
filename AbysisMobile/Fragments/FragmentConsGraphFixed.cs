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
using Com.Syncfusion.Charts;

namespace AbysisMobile.Fragments
{
    public class FragmentConsGraphFixed : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjI5MzNAMzEzNjJlMzQyZTMwVUgxUG12dWlBNGlpL3FTVkRKeitLb3B2QjR5OUdUTlNnOEdXS1BhYmowYz0=");
            base.OnCreate(savedInstanceState);
            IndexList = Helpers.SessionHelper.LoadIndexList();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view =inflater.Inflate(Resource.Layout.fragment_consgraphfixed, container, false);

            ManageUIs(view);
            LoadData();
            return view;
        }

        public Spinner spPeriodYear;
        public Spinner spIndices;
        public Button btnQuery;
        public Button btnClear;
        public SfChart chart;
        private void ManageUIs(View view)
        {
            spPeriodYear=view.FindViewById<Spinner>(Resource.Id.spCGPeriodYear);
            spIndices   =view.FindViewById<Spinner>(Resource.Id.spCGIndicies);
            btnQuery = view.FindViewById<Button>(Resource.Id.btnCGQuery);
            btnQuery.Click += BtnQuery_Click;
            btnClear = view.FindViewById<Button>(Resource.Id.btnCGClear);
            btnClear.Click += BtnClear_Click;
            chart = view.FindViewById<SfChart>(Resource.Id.sfConsGraphFixed);
        }
        int selectedIndex = -1;
        private void BtnClear_Click(object sender, EventArgs e)
        {
            chart.Series.Clear();
            aMax = 0;
            first = true;
            selectedIndexLines.Clear(); 
        }

        DataTable Data = null;
        bool first = true;
        double aMax = 0;
        List<Indexes.indexInfo> IndexList;
        List<string> selectedIndexLines = new List<string>();

        public void BtnQueryAction()
        {
            int selectedyear = Convert.ToInt32(spPeriodYear.SelectedItem.ToString());
            int selectedIndexLineId = Convert.ToInt32(spIndices.SelectedItemId);
            DateTime date = new DateTime(selectedyear, 1, 1, 0, 0, 0);
            string selectedDateAndIndexLineIdPair = selectedyear.ToString() + selectedIndexLineId.ToString();
            if(selectedIndexLines.Contains(selectedDateAndIndexLineIdPair))
            {
                Toast.MakeText(Context, "Seçilen Endex değeri grafikte vardır.", ToastLength.Long).Show();
                return;
            }
            string[] fields = new string[]
            {
                "Invoice.INVOICE_DATE",
                "InvoiceLine.INDEX_CODE",
                "InvoiceLine.AMOUNT",
                "Invoice.PERIOD_CODE",
                "InvoiceLine.INDEX_LINE_ID",
                "DATEPART(MONTH,Invoice.INVOICE_DATE) AS MONTH_"
            };
            try
            {
                Data = Services.invoicesClient.GetInvoiceFieldsForSubscriber(
                    Session.LoginData.Value.ToString(),
                    Session.SelectedMeter.DepartmentId,
                    Session.SelectedMeter.SubscriberId,
                    1,
                    date,
                    date.AddYears(1),
                    fields).Value;
                //Data=(from item in Data
                //     where item["INDEX_LINE_ID"]==Convert.ToInt32(spIndicies.SelectedItemId)
                //     select item).c
                //Data = Data.Select("INDEX_LINE_ID=" + spIndicies.SelectedItemId.ToString();
                //-----------------------------------------------------------------------------
                var qData = (from row in Data.Select().AsEnumerable()
                             where Convert.ToInt32(row["INDEX_LINE_ID"]) == selectedIndexLineId
                             select row).Distinct().ToList();
                //-----------------------------------------------------------------------------

                var pData = Data.Select()
                    .AsEnumerable()
                    .Where(r => Convert.ToInt32(r["INDEX_LINE_ID"]) == selectedIndexLineId)
                    .Select(x => new { Month = Convert.ToInt32(x["MONTH_"]), Amount = Convert.ToDouble(x["AMOUNT"]) }).Distinct().ToList();
                if (!(pData.Count > 0))
                {
                    Toast.MakeText(Context, "Gösterilecek Kayıt Bulunamadı", ToastLength.Short);
                    return;
                }
                if (pData[0].Month != 1)
                {
                    int m = pData[0].Month;
                    for (int i = 1; i < pData[0].Month; i++)
                    {
                        pData.Insert(i - 1, new { Month = i, Amount = 0.0d });
                    }
                }
                string Unit = (from u in IndexList where u.Id == selectedIndexLineId select u.Unit).FirstOrDefault<string>();
                


                double sMax = pData.Max(x => x.Amount);
                //chart.PrimaryAxis = new DateTimeAxis();
                if (first || selectedIndex != selectedIndexLineId)
                {
                    selectedIndexLines.Clear();
                    aMax = sMax;
                    ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
                    {
                        SelectionZoomingEnabled = true
                    };
                    chart.Behaviors.Add(zoomPanBehavior);
                    chart.Series.Clear();
                    chart.PrimaryAxis = new CategoryAxis() { Interval = 1, LabelPlacement = LabelPlacement.BetweenTicks };
                    chart.PrimaryAxis.Title.Text = "Ay";
                    chart.PrimaryAxis.LabelRotationAngle = 90;
                    chart.SecondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = sMax + 1000, Interval = 1000 };
                    chart.SecondaryAxis.Title.Text = $"Tüketim({Unit})";
                    chart.Title.Text = "Tüketim Grafiği";
                    first = false;
                    selectedIndex = Convert.ToInt32(Convert.ToInt32(spIndices.SelectedItemId));
                }
                else if (aMax < sMax)
                {
                    ((NumericalAxis)chart.SecondaryAxis).Maximum = sMax;
                    chart.SecondaryAxis.Title.Text = $"Tüketim{Unit}";
                    chart.Title.Text = "Tüketim Grafiği";
                    aMax = sMax;
                }

                selectedIndexLines.Add(selectedDateAndIndexLineIdPair);

            ColumnSeries columnSeries = new ColumnSeries()
                {
                    ItemsSource = pData,
                    XBindingPath = "Month",
                    YBindingPath = "Amount"
                };
                columnSeries.DataMarker.ShowLabel = true;
                columnSeries.Label = spPeriodYear.SelectedItem.ToString();
                columnSeries.DataMarker.LabelStyle.LabelPosition = DataMarkerLabelPosition.Inner;
                columnSeries.DataMarker.LabelStyle.Angle = 90;
                columnSeries.TooltipEnabled = true;
                chart.Series.Add(columnSeries);
                chart.Legend.Visibility = Visibility.Visible;

                var v = chart.Visibility;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.ToString(), ToastLength.Short).Show();
            }
        }
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            /*
             int selectedyear = Convert.ToInt32(spPeriodYear.SelectedItem.ToString());
            DateTime date = new DateTime(selectedyear, 1, 1, 0, 0, 0);
            string[] fields = new string[]
            {
                "Invoice.INVOICE_DATE",
                "InvoiceLine.INDEX_CODE",
                "InvoiceLine.AMOUNT",
                "Invoice.PERIOD_CODE",
                "InvoiceLine.INDEX_LINE_ID",
                "DATEPART(MONTH,Invoice.INVOICE_DATE) AS MONTH_"
            };
            try
            {
                Data =Services.invoicesClient.GetInvoiceFieldsForSubscriber(
                    Session.LoginData.Value.ToString(), 
                    Session.SelectedMeter.DepartmentId, 
                    Session.SelectedMeter.SubscriberId, 
                    1, 
                    date, 
                    date.AddYears(1), 
                    fields).Value;
                //Data=(from item in Data
                //     where item["INDEX_LINE_ID"]==Convert.ToInt32(spIndicies.SelectedItemId)
                //     select item).c
                //Data = Data.Select("INDEX_LINE_ID=" + spIndicies.SelectedItemId.ToString();
                //-----------------------------------------------------------------------------
                var qData = (from row in Data.Select().AsEnumerable()
                             where Convert.ToInt32(row["INDEX_LINE_ID"]) == Convert.ToInt32(spIndices.SelectedItemId)
                             select row).Distinct().ToList();
                //-----------------------------------------------------------------------------

                var pData = Data.Select()
                    .AsEnumerable()
                    .Where(r => Convert.ToInt32(r["INDEX_LINE_ID"]) == Convert.ToInt32(spIndices.SelectedItemId))
                    .Select(x => new { Month = Convert.ToInt32(x["MONTH_"]), Amount = Convert.ToDouble(x["AMOUNT"]) }).Distinct().ToList();
                if (!(pData.Count > 0))
                {
                    Toast.MakeText(Context, "Gösterilecek Kayıt Bulunamadı", ToastLength.Short);
                    return;
                }
                if(pData[0].Month!=1)
                {
                    int m = pData[0].Month;
                    for (int i = 1; i < pData[0].Month; i++)
                    {
                        pData.Insert(i - 1, new { Month = i, Amount = 0.0d });
                    }
                }
                string Unit = (from u in IndexList where u.Id == Convert.ToInt32(spIndices.SelectedItemId) select u.Unit).FirstOrDefault<string>();
                //List<FixedConsumptionData> ResultPlotData = new List<FixedConsumptionData>();
                //foreach (var item in pData)
                //{
                //    ResultPlotData.Add(new FixedConsumptionData() { Amount = item.Amount, Month_ = item.Month });
                //}
                //plotConsGraphFixed.Model = CreatePlotModel(ResultPlotData);

                ////var pData = Data.Select().GroupBy(x => new { col1 = x["MONTH_"] }).Select(g => g.ToList()).ToList();
                //chartConsGraphFixed.Chart = CreateConsShart(GetEntries(qData));
                double sMax = pData.Max(x => x.Amount);
                //chart.PrimaryAxis = new DateTimeAxis();
                if (first || selectedIndex != Convert.ToInt32(Convert.ToInt32(spIndices.SelectedItemId)))
                {
                    aMax = sMax;
                    ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
                    {
                        SelectionZoomingEnabled = true
                    };
                    chart.Behaviors.Add(zoomPanBehavior);
                    chart.Series.Clear();
                    chart.PrimaryAxis = new CategoryAxis() { Interval = 1, LabelPlacement = LabelPlacement.BetweenTicks };
                    chart.PrimaryAxis.Title.Text = "Ay";
                    chart.PrimaryAxis.LabelRotationAngle = 90;
                    chart.SecondaryAxis = new NumericalAxis() { Minimum = 0, Maximum = sMax + 1000, Interval = 2000 };
                    chart.SecondaryAxis.Title.Text = $"Tüketim({Unit})";
                    chart.Title.Text = "Tüketim Grafiği";
                    first = false;
                    selectedIndex = Convert.ToInt32(Convert.ToInt32(spIndices.SelectedItemId));
                }
                else if (aMax < sMax)
                {
                    ((NumericalAxis)chart.SecondaryAxis).Maximum = sMax;
                    chart.SecondaryAxis.Title.Text = "Tüketim(KwH)";
                    chart.Title.Text = "Tüketim Grafiği";
                    aMax = sMax;
                }
                

                ColumnSeries columnSeries = new ColumnSeries()
                {
                    ItemsSource = pData,
                    XBindingPath = "Month",
                    YBindingPath = "Amount"
                };
                columnSeries.DataMarker.ShowLabel = true;
                columnSeries.Label = spPeriodYear.SelectedItem.ToString();
                columnSeries.DataMarker.LabelStyle.LabelPosition = DataMarkerLabelPosition.Inner;
                columnSeries.DataMarker.LabelStyle.Angle = 90;
                columnSeries.TooltipEnabled = true;
                chart.Series.Add(columnSeries);
                chart.Legend.Visibility = Visibility.Visible;

                var v = chart.Visibility;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Context, ex.ToString(), ToastLength.Short).Show();
            }
             */
        }

        private void LoadData()
        {
            var indices = Helpers.SessionHelper.LoadIndexList();
            Adapters.IndexListAdapter adap = new Adapters.IndexListAdapter(Context, indices);
            spIndices.Adapter = adap;

            List<string> periodList = new List<string>();
            for (int i = DateTime.Today.Year; i >= 2013; i--)
            {
                periodList.Add(i.ToString());
            }
            spPeriodYear.Adapter = new ArrayAdapter(Context, Resource.Layout.support_simple_spinner_dropdown_item, periodList);
        }
    }
}