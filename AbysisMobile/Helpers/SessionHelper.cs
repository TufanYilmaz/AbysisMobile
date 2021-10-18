using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbysisMobile.Indexes;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AbysisMobile.Helpers
{
    public static class SessionHelper
    {
        private  static List<int> ValidDataTypes = new List<int> { 0, 8 };
        public static List<indexInfo> LoadIndexList()
        {
            string selectedMeter = Session.SelectedMeter.Id.ToString();
            if (!Session.IndexList.ContainsKey(selectedMeter))
            {
                try
                {

                    var indexlist = Services.indexesClient.GetIndexList(
                        Session.LoginData.Value.ToString(),
                        Session.SelectedMeter.DepartmentId
                        ).Value.ToList();
                    indexlist = indexlist.ToList().FindAll(i => ValidDataTypes.Contains(i.DataType) && i.WebVisible).ToList();
                    Session.IndexList.Add(Session.SelectedMeter.Id.ToString(), indexlist);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return  Session.IndexList[selectedMeter];
        }
        public static List<Invoices.invoice> GetInvoicesForSelectedDepartment()
        {
            List<Invoices.invoice> result = new List<Invoices.invoice>();
            try
            {
                if (Session.InvoicesResults.Keys.Contains(Session.SelectedMeter.DepartmentId))
                    return Session.InvoicesResults[Session.SelectedMeter.DepartmentId];
                DateTime now = DateTime.Now;
                DateTime start = new DateTime(now.Year - 5, 1, 1);
                result = Services.invoicesClient.GetInvoiceBySubscriber(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, 1, start, now, Session.SelectedMeter.SubscriberId).Value.ToList();
                Session.InvoicesResults.Add(Session.SelectedMeter.DepartmentId, result);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;

        }
        public  static void GetInvoicesData()
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year - 5, 1, 1);
            List<Invoices.invoice> result = new List<Invoices.invoice>();
            //Parallel.ForEach((Session.LoginData.AuthorizedMeters), (meter) =>
            //{
            //    try
            //    {
            //        if (!Session.InvoicesResults.Keys.Contains(meter.DepartmentId))
            //        {
            //            result = Services.invoicesClient.GetInvoiceBySubscriber(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, 1, start, now, Session.SelectedMeter.SubscriberId).Value.ToList();
            //            Session.InvoicesResults.Add(Session.SelectedMeter.DepartmentId, result);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //});
            foreach (var meter in Session.LoginData.AuthorizedMeters)
            {
                try
                {
                    if (Session.InvoicesResults.Keys.Contains(meter.DepartmentId))
                        continue;
                    result = Services.invoicesClient.GetInvoiceBySubscriber(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, 1, start, now, Session.SelectedMeter.SubscriberId).Value.ToList();
                    Session.InvoicesResults.Add(Session.SelectedMeter.DepartmentId, result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}