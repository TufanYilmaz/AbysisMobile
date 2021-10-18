using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AbysisMobile.Authentication;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Gauges.SfCircularGauge;
using Syncfusion;

namespace AbysisMobile.Adapters
{
    class MeterLineGaugeAdapter : BaseAdapter<ConsumptionLine>
    {

        Context context;
        List<ConsumptionLine> consumptionLines;
        public MeterLineGaugeAdapter(Context context)
        {
            this.context = context;
        }
        public MeterLineGaugeAdapter(Context context,List<ConsumptionLine> consumptionLines)
        {
            this.context = context;
            this.consumptionLines = consumptionLines;
        }
        public MeterLineGaugeAdapter(Context context, meterInfo meter)
        {
            this.context = context;
            this.consumptionLines = meter.Consumptions.ToList();
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {


            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.list_meter_gauge, null, false);
            }
            TextView label2 = row.FindViewById<TextView>(Resource.Id.tvLabel2);
            TextView label3 = row.FindViewById<TextView>(Resource.Id.tvLabel3);
            TextView label4 = row.FindViewById<TextView>(Resource.Id.tvLabel4);
            TextView label5 = row.FindViewById<TextView>(Resource.Id.tvLabel5);
            if (consumptionLines[position].INDEX_CODE == "3023")
            {
                label2.Text = "Toplam Atık Su Debisi";
            }
            else
            {
                label2.Text = consumptionLines[position].INDEX_INFO;
            }
            label3.Text = string.Format("Son Endeks : {0}", ((float)Math.Round(consumptionLines[position].END_INDEX, 3)).ToString("n2"));
            label4.Text = string.Format("Tuketim: {0} {1}", ((float)Math.Round(consumptionLines[position].AMOUNT, 3)).ToString("n2"), consumptionLines[position].UNIT);
            //LabelComponent dateText = null;
            if (consumptionLines[position].USE_MULTIPLIER)
            {
                //dateText = ((ICircularGauge)selected.Gauges[0]).Labels[6];
                label5.Text = string.Format("Çarpan: {0}", consumptionLines[position].MULTIPLIER.ToString());
            }
            //SfCircularGauge circularGauge = row.FindViewById<SfCircularGauge>(Resource.Id.meterLineGauge);
            if (consumptionLines[position].INDEX_CODE == "5.8.0" || consumptionLines[position].INDEX_CODE == "8.8.0")
                 GetGaugeForLine(row, consumptionLines[position], GaugeType.gtpercentage);
            else
                 GetGaugeForLine(row, consumptionLines[position], GaugeType.gtvalue);

            return row;

        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return consumptionLines.Count;
            }
        }

        public override ConsumptionLine this[int position] => consumptionLines[position];
        public enum GaugeType
        {
            gtpercentage,
            gtvalue
        };
        private  int CalculateMax(double intervalf)
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
        public  void GetGaugeForLine(View view, ConsumptionLine Line, GaugeType type)
        {
            
            SfCircularGauge result = view.FindViewById<SfCircularGauge>(Resource.Id.meterLineGauge);
            bool isConsumption = (Line.FORMULA != "MAX");
            float value = (float)Math.Round(Line.AMOUNT, 3);
            float endindex = (float)Math.Round(Line.END_INDEX, 3);
            int MaxValue = CalculateMax(value);
            int mult = 1;
            if (MaxValue > 100) mult = MaxValue / 1000;
            MaxValue = MaxValue / mult;
            
            result.Headers.Clear();
            
                Header label1 = new Header
                {
                    Text = Line.INDEX_CODE,
                    TextSize = 25,
                    TextColor = Color.Black,
                    Position = new PointF((float)0.50, (float)0.25)
                };
                result.Headers.Add(label1);
            
            if (type == GaugeType.gtvalue)
            {
                ObservableCollection<CircularScale> scales = new ObservableCollection<CircularScale>();
                CircularScale scale = new CircularScale
                {
                    StartValue = 0,
                    EndValue = (float)MaxValue,
                    EnableAutoInterval = true
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
                    StartValue = (float)MaxValue / 3,
                    EndValue = (float)MaxValue * 2 / 3,
                    Color = Color.Orange
                };
                CircularRange range3 = new CircularRange
                {
                    StartValue = (float)MaxValue * 2 / 3,
                    EndValue = (float)MaxValue,
                    Color = Color.Red
                };
                scale.CircularRanges.Add(range);
                scale.CircularRanges.Add(range2);
                scale.CircularRanges.Add(range3);

                //Adding needle pointer
                NeedlePointer needlePointer = new NeedlePointer
                {
                    Value = value / mult,
                    EnableAnimation =false
                };
                scale.CircularPointers.Add(needlePointer);

                //Adding range pointer
                RangePointer rangePointer = new RangePointer
                {
                    Value = value / mult,
                    EnableAnimation = false
                };
                scale.CircularPointers.Add(rangePointer);
                result.CircularScales = scales;

                Header label0 = new Header
                {
                    Text = "x" + mult.ToString(),
                    TextSize = 15,
                    TextColor = Color.Black,
                };
                result.Headers.Add(label0);

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
                    Value = (float)Convert.ToDouble(Line.STATE.Replace("%", "")),
                    EnableAnimation=false
                };
                scale.CircularPointers.Add(needlePointer);

                //Adding range pointer
                RangePointer rangePointer = new RangePointer
                {
                    Value = (float)Convert.ToDouble(Line.STATE.Replace("%", "")),
                    EnableAnimation = false
                };
                scale.CircularPointers.Add(rangePointer);
                result.CircularScales = scales;
                Header label0 = new Header
                {
                    Text = Line.FORMULA,
                    TextSize = 15,
                    TextColor = Color.Black,
                };
                result.Headers.Add(label0);
                result.Headers[0].Text= "%" + Convert.ToDouble(Line.STATE.Replace("%", "")).ToString("0.00");
                /*


                ((ICircularGauge)selected.Gauges[0]).Scales[0].Value = (float)Convert.ToDouble(Line.STATE.Replace("%", ""));
                ((ICircularGauge)selected.Gauges[0]).Labels[0].Text = Line.FORMULA;
                ((ICircularGauge)selected.Gauges[0]).Labels[1].Text = "%" + Convert.ToDouble(Line.STATE.Replace("%", "")).ToString("0.00");
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[0].StartValue = 0;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[0].EndValue = intFormula;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[1].StartValue = intFormula;
                ((ICircularGauge)selected.Gauges[0]).Scales[0].Ranges[1].EndValue = 100;*/
            }

            //return result;
        }
    }

}