using System;

namespace AbysisMobile
{
    public static class Services
    {
        //public static string ServiceEndPoint = CheckEnding("http://192.168.2.222:90/Services");
        //public static string ServiceEndPoint = CheckEnding("http://elk.mimarsinanosb.org.tr:90/Services");//MOSB
        //public static string ServiceEndPoint = CheckEnding("http://213.74.195.138:90/Services");//KOSBİ
        public static string ServiceEndPoint = CheckEnding("http://95.6.50.139:90/Services");//Akhisar


        public static Authentication.Authentication authenticationClient = new Authentication.Authentication()
        {  
            Url = ServiceEndPoint + "Authentication.asmx",
            Timeout=50000
        };
        public static Indexes.Indexes indexesClient = new Indexes.Indexes()
        {
            Url = ServiceEndPoint + "Indexes.asmx",
            Timeout = 20000
        };
        public static Invoices.Invoices invoicesClient = new Invoices.Invoices()
        {
            Url = ServiceEndPoint + "Invoices.asmx",
            Timeout = 30000
        };

        private static string CheckEnding(string ServiceEndPoint)
        {
            if (!ServiceEndPoint.EndsWith("/"))
                ServiceEndPoint += "/";
            if (!ServiceEndPoint.EndsWith("Services/"))
                ServiceEndPoint += "Services/";
            return ServiceEndPoint;
        }

        static Services()
        {
            var test = authenticationClient.Url;
        }
    }
}