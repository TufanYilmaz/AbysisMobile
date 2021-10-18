using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbysisMobile.Indexes;
using AbysisMobile.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AbysisMobile
{
    public static class Session
    {
        
        public static bool IsLogin = false;
        public static string SessionId = new Guid().ToString();
        public static Authentication.AuthenticationResult LoginData = new Authentication.AuthenticationResult();
        public static Authentication.meterInfo SelectedMeter = new Authentication.meterInfo();
        public static List<AuthMeters> MeterList = new List<AuthMeters>();
        public static Dictionary<int, List<Invoices.invoice>> InvoicesResults = new Dictionary<int, List<Invoices.invoice>>();
        #region METER QUERY
        public static Dictionary<string, IndexValueListResult> ResultName = new Dictionary<string, IndexValueListResult>();
        public static Dictionary<string, List<indexInfo>> IndexList = new Dictionary<string, List<indexInfo>>();
        public static Dictionary<string, int> SelectedName = new Dictionary<string, int>();

        public static DateTime Date1 = new DateTime();
        public static DateTime Date2 = new DateTime();
        #endregion
        public static Invoices.invoice SelectedInvoice = new Invoices.invoice();
        public static void ClearSession()
        {
            IsLogin = false;
            SessionId = new Guid().ToString();
            LoginData = new Authentication.AuthenticationResult();
            SelectedMeter = new Authentication.meterInfo();
            MeterList = new List<AuthMeters>();
            Date1 = new DateTime();
            Date2 = new DateTime();

            ResultName = new Dictionary<string, IndexValueListResult>();
            IndexList = new Dictionary<string, List<indexInfo>>();
            SelectedName = new Dictionary<string, int>();
        }
    }
}