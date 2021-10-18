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

namespace AbysisMobile.Helpers
{
    public class MyUIEventArgs:EventArgs
    {
        public int Id { get; set; }
    }
    public class MyEventArgs : EventArgs
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Info { get; set; }
    }
}