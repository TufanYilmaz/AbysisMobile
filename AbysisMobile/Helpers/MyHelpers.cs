using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Syncfusion.SfDataGrid;
using Color = Android.Graphics.Color;

namespace AbysisMobile.Helpers
{
    class MyHelpers
    {
    }
    public class GridTableSummaryCellRendererExt : GridTableSummaryCellRenderer
    {
        public GridTableSummaryCellRendererExt() { }

        public override void OnInitializeDisplayView(DataColumnBase dataColumn, TextView view)
        {
            base.OnInitializeDisplayView(dataColumn, view);
            view.SetTextColor(Android.Graphics.Color.White);
            view.TextSize = 20;
            view.TextAlignment = TextAlignment.Center;
            view.Typeface = Typeface.Create("GillSans-Italic", TypefaceStyle.BoldItalic);
            view.SetBackgroundColor(Android.Graphics.Color.DarkBlue);

        }
    }
    public class CustomGridStyle : DataGridStyle
    {
        public CustomGridStyle()
        {
        }

        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.Gray;
        }
    }
    public class Dark : DataGridStyle
    {
        public Dark()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.Black;
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.White;
        }

        public override Color GetRecordBackgroundColor()
        {
            return Color.DarkGray;
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.White;
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.DarkCyan;
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.White;
        }

        public override Color GetCaptionSummaryRowBackgroundColor()
        {
            return Color.Black;
        }
        [Obsolete]
        public override Color GetCaptionSummaryRowForeGroundColor()
        {
            return Color.White;
        }

        public override Color GetBorderColor()
        {
            return Color.LightGray;
        }

        public override Color GetLoadMoreViewBackgroundColor()
        {
            return Color.WhiteSmoke;
        }

        public override Color GetLoadMoreViewForegroundColor()
        {
            return Color.DarkSlateGray;
        }

        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.Cyan;
        }
    }
}