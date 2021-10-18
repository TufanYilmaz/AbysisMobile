using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;

namespace AbysisMobile.Helpers
{
    public static class ExtensionMethods
    {
        public static void GetTrifaze(this SfChart chart,List<Indexes.indexInfo> indexes, DateTime Date1, DateTime Date2, string title = "")
        {
            List<Indexes.indexValue[]> Data = new List<Indexes.indexValue[]>
            {
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[0].Id).Value),
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[1].Id).Value),
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[2].Id).Value)
            };
            var m = (from d in Data select d.Min(x => x.RawValue)).Min();
            

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

            chart.Behaviors.Clear();
            chart.Behaviors.Add(trackballBehavior);
            chart.Behaviors.Add(zoomPanBehavior);
            //var datetimeAxis = new DateTimeAxis();
            //datetimeAxis.
            chart.PrimaryAxis = new DateTimeAxis() { IntervalType = DateTimeIntervalType.Auto };
            //chart.PrimaryAxis.Title.Text = "Tarih";
            chart.PrimaryAxis.LabelRotationAngle = 90;
            chart.SecondaryAxis = new NumericalAxis() { Minimum = (m-5)<0?0:(m-5) };
            //chart.SecondaryAxis.Title.Text = "Tüketim(KwH)";
            chart.Title.Text = title;
            chart.Title.TextSize = 10;
            int i = 0;
            chart.Series.Clear();
            foreach (var data in Data)
            {
                var lineSeries = new FastLineSeries()
                {
                    ItemsSource = data,
                    XBindingPath = "dateTime",
                    YBindingPath = "RawValue",
                    Label=indexes[i++].Info
                };
                chart.Series.Add(lineSeries);
            }

            chart.Legend.Visibility = Visibility.Visible;
        }
    }
}