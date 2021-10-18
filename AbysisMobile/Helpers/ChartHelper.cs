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
    public static class ChartHelper
    {
        public static SfChart GetTriFazeGraph(Context context, List<Indexes.indexInfo> indexes,DateTime Date1, DateTime Date2, string title="")
        {
            List<Indexes.indexValue[]> Data = new List<Indexes.indexValue[]>
            {
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[0].Id).Value),
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[1].Id).Value),
                (Services.indexesClient.GetIndexValueList(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, Date1.Date, Date2.Date, Session.SelectedMeter.SubscriberId, Session.SelectedMeter.Id, indexes[2].Id).Value)
            };



            SfChart chart = new SfChart(context);
            ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
            {
                SelectionZoomingEnabled = true,
                ZoomMode=ZoomMode.X
               
            };
            ChartTrackballBehavior trackballBehavior = new ChartTrackballBehavior
            {
                ShowLabel = true,
                ShowLine = true
            };

            chart.Behaviors.Add(trackballBehavior);

            chart.Behaviors.Add(zoomPanBehavior);
            var datetimeAxis = new DateTimeAxis();
            //datetimeAxis.
            chart.PrimaryAxis = new DateTimeAxis() { IntervalType = DateTimeIntervalType.Auto  };
            //chart.PrimaryAxis.Title.Text = "Tarih";
            chart.PrimaryAxis.LabelRotationAngle = 90;
            chart.SecondaryAxis = new NumericalAxis() { Minimum = 0};
            //chart.SecondaryAxis.Title.Text = "Tüketim(KwH)";
            chart.Title.Text =title;

            foreach(var data in Data)
            {
                LineSeries lineSeries = new LineSeries()
                {
                    ItemsSource = data,
                    XBindingPath = "dateTime",
                    YBindingPath = "RawValue"
                };
                chart.Series.Add(lineSeries);
            }

            chart.Legend.Visibility = Visibility.Visible;
            
            return chart;
        }
    }
}