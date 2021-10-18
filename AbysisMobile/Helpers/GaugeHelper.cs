using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Gauges.SfCircularGauge;

namespace AbysisMobile.Helpers
{
    public static class GaugeHelper
    {
        public enum GaugeType
        {
            gtpercentage,
            gtvalue
        };
        private static int CalculateMax(double intervalf)
        {
            string intervalStr = Math.Round(intervalf, 0).ToString();
            int digitCount = intervalStr.Length;
            string tmp = "";
            while (tmp.Length < digitCount)
            {
                tmp += "9";
            }
            return Convert.ToInt32(tmp) + 1;
        }
        public static SfCircularGauge GetGaugeForLine(SfCircularGauge result,Authentication.ConsumptionLine Line, GaugeType type)
        {
            //SfCircularGauge result = new SfCircularGauge(context);
            bool isConsumption = (Line.FORMULA != "MAX");
            float value = (float)Math.Round(Line.AMOUNT, 3);
            float endindex = (float)Math.Round(Line.END_INDEX, 3);
            int MaxValue = CalculateMax(value);
            int mult = 1;
            if (MaxValue > 100) mult = MaxValue / 1000;
            MaxValue = MaxValue / mult;


            Header header = new Header
            {
                Text = Line.INDEX_INFO,
                TextSize = 15,
                TextColor = Color.Black
            };
            result.Headers.Add(header);
            if (type == GaugeType.gtvalue)
            {
                ObservableCollection<CircularScale> scales = new ObservableCollection<CircularScale>();
                CircularScale scale = new CircularScale
                {
                    StartValue =0 ,
                    EndValue = (float)MaxValue,
                    EnableAutoInterval=true
                };
                scales.Add(scale);
                //Adding range
                CircularRange range = new CircularRange
                {
                    StartValue = 0,
                    EndValue = (float)MaxValue / 3,
                    Color = Color.Green
                };

                CircularRange range2 = new CircularRange
                {
                    StartValue= (float)MaxValue / 3,
                    EndValue = (float)MaxValue * 2 / 3,
                    Color = Color.Orange
                };
                CircularRange range3 = new CircularRange
                {
                    StartValue  = (float)MaxValue * 2 / 3,
                    EndValue = (float)MaxValue,
                    Color = Color.Red
                };
                scale.CircularRanges.Add(range);
                scale.CircularRanges.Add(range2);
                scale.CircularRanges.Add(range3);

                //Adding needle pointer
                NeedlePointer needlePointer = new NeedlePointer
                {
                    Value = value / mult
                };
                scale.CircularPointers.Add(needlePointer);

                //Adding range pointer
                RangePointer rangePointer = new RangePointer
                {
                    Value = value / mult
                };
                scale.CircularPointers.Add(rangePointer);
                result.CircularScales = scales;

            }
            else
            {
                string strFormula = Line.FORMULA.Replace("%", "");
                if (strFormula == "") strFormula = "0";
                int intFormula = Convert.ToInt32(strFormula);

                ObservableCollection<CircularScale> scales = new ObservableCollection<CircularScale>();
                CircularScale scale = new CircularScale
                {
                    StartValue = 0,
                    EndValue = 100
                };
                scales.Add(scale);
                //Adding range
                CircularRange range = new CircularRange
                {
                    StartValue = 0,
                    EndValue = intFormula,
                    Color = Color.Green
                };

                CircularRange range2 = new CircularRange
                {
                    StartValue = intFormula,
                    EndValue = 100,
                    Color = Color.Red
                };
              
                scale.CircularRanges.Add(range);
                scale.CircularRanges.Add(range2);

                //Adding needle pointer
                NeedlePointer needlePointer = new NeedlePointer
                {
                    Value = (float)Convert.ToDouble(Line.STATE.Replace("%", ""))
                };
                scale.CircularPointers.Add(needlePointer);

                //Adding range pointer
                RangePointer rangePointer = new RangePointer
                {
                    Value = (float)Convert.ToDouble(Line.STATE.Replace("%", ""))
                };
                scale.CircularPointers.Add(rangePointer);
                result.CircularScales = scales;

                /*


                ((ICircularGauge)selected.Gauges[0]).Scales[0].Value = (float)Convert.ToDouble(Line.STATE.Replace("%", ""));
                ((ICircularGauge)selected.Gauges[0]).Labels[0].Text = Line.FORMULA;
                ((ICircularGauge)selected.Gauges[0]).Labels[1].Text = "%" + Convert.ToDouble(Line.STATE.Replace("%", "")).ToString("0.00");
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[0].StartValue = 0;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[0].EndValue = intFormula;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[1].StartValue = intFormula;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[1].EndValue = 100;*/
            }

            return result;
        }

    }

}