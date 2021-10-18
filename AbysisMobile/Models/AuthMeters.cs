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

namespace AbysisMobile.Models
{
    public class AuthMeters
    {
        public int Image { get; set; }
        public int Id { get; set; }
        public string Mark { get; set; }
        public string DepartmentCode { get; set; }
        public string Code { get; set; }
        public int DepartmentId { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int DepartmentType { get; set; }
        public AuthMeters(Authentication.meterInfo MeterInfo)
        {
            Id = MeterInfo.Id;
            Mark = MeterInfo.Mark;
            DepartmentCode = MeterInfo.DepartmentCode;
            Code = MeterInfo.Code;
            DepartmentId = MeterInfo.DepartmentId;
            FirstDate = MeterInfo.FirstDate;
            LastDate = MeterInfo.LastDate;
            DepartmentType = MeterInfo.Subscriber.Department.departmentType;
            if (DepartmentId == 1)
                Image = Resource.Drawable.light;
            else if (DepartmentId == 2)
                Image = Resource.Drawable.drop;
            else if (DepartmentId == 3)
                Image = Resource.Drawable.gas;
            else if (DepartmentType == 4)
                Image = Resource.Drawable.comminicate;
            else
                Image = Resource.Drawable.ic_launcher;
        }
        public override string ToString()
        {
            return DepartmentCode + "\n" +
                Mark + " " + Code;// + "\n" +
                //"İlk Okuma= " + FirstDate.ToShortDateString() + "\n" +
                //"Sons Okuma= " + LastDate.ToShortDateString();
        }
    }
}