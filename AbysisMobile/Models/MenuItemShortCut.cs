using System;
using System.Collections.Generic;

namespace AbysisMobile.Models
{
    public class NavigationMenuItem
    {
        public string Name { get; set; }
        public int Icon { get; set; }
        public bool Visiblity { get; set; } = true;
        public MenuItemShortCut ItemPosition { get; set; } = MenuItemShortCut.NotFound;
        public int DepartmentID { get; set; }
        public int Id { get; set; }
        public static List<NavigationMenuItem> GetMenuItems()
        {
            List<NavigationMenuItem> result = new List<NavigationMenuItem>
            {
                new NavigationMenuItem() { Name = "Sayaçlar", Icon = Resource.Drawable.ic_menu_gallery, Visiblity = true,ItemPosition=MenuItemShortCut.Meters,Id=(int)MenuItemShortCutInt.Meters },
                new NavigationMenuItem() { Name = "Sayaç Özet", Icon = Resource.Drawable.ic_menu_camera, Visiblity = true,ItemPosition=MenuItemShortCut.MeterSummary ,Id=(int)MenuItemShortCutInt.MeterSummary},
                new NavigationMenuItem() { Name = "Sayaç Sorgu", Icon = Resource.Drawable.magnifier, Visiblity = true,ItemPosition=MenuItemShortCut.MeterQuery,Id=(int)MenuItemShortCutInt.MeterQuery },
                new NavigationMenuItem() { Name = "Tüketim Grafik", Icon = Resource.Drawable.barchart, Visiblity = true,ItemPosition=MenuItemShortCut.Fixed,Id=(int)MenuItemShortCutInt.Fixed },
              //new NavigationMenuItem() { Name = "Yıllara Göre Tüketim", Icon = Resource.Drawable.ic_menu_slideshow, Visiblity = true,ItemPosition=MenuItemShortCut.FixedYear,Id=(int)MenuItemShortCutInt.FixedYear },
                new NavigationMenuItem() { Name = "Voltaj Grafik", Icon = Resource.Drawable.linechart, Visiblity = true,Id=(int)MenuItemShortCutInt.Voltage },
                new NavigationMenuItem() { Name = "Akım Grafik", Icon = Resource.Drawable.linechart, Visiblity = true,Id=(int)MenuItemShortCutInt.Current },
                new NavigationMenuItem() { Name = "Faturalar", Icon = Resource.Drawable.turkishlira, Visiblity = true,Id=(int)MenuItemShortCutInt.Invoice},
                new NavigationMenuItem() { Name = "PTF", Icon = Resource.Drawable.turkishlira, Visiblity = true,ItemPosition = MenuItemShortCut.PTF,Id=(int)MenuItemShortCutInt.PTF }
            };
            return result;
        }
        NavigationMenuItem() : base() { }
        NavigationMenuItem(string name, int icon, bool visibilty)
        {
            Name = name;
            Icon = icon;
            Visiblity = visibilty;
        }
        

    }
    public enum MenuItemShortCut {
        Meters = '1',
        MeterSummary ='2',
        MeterQuery='3',
        PTF='4',
        Fixed ='5',
        FixedYear ='6',
        ChangePassword='C',
        Exit='Q',
        NotFound ='x'}
    public enum MenuItemShortCutInt
    {
        Meters = 0,
        MeterSummary ,
        MeterQuery ,
        Gas,
        Fixed ,
        FixedYear ,
        Voltage,
        Current,
        Invoice,
        PTF,
        ChangePassword ,
        Exit ,
        NotFound 
    }
}