using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Annotation;

using Android.Util;
using Android.Views;
using Android.Widget;

using AbysisMobile.Adapters;
namespace AbysisMobile.Fragments
{
    public class TabFragment : Fragment
    {
        public static TabLayout tabLayout;
        public static ViewPager viewPager;
        public static int int_items = 5;
        List<Fragment> MyFragments { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        FragmentManager MyFragmentManager { get; set; }
        public TabFragment(FragmentManager fragmentManager) : base()
        {
            this.MyFragmentManager = fragmentManager;
        }
        public TabFragment(FragmentManager fragmentManager,List<Fragment> fragments) : base()
        {
            this.MyFragmentManager = fragmentManager;
            this.MyFragments = fragments;
        }
        [Nullable]
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.tab_fragment_layout, null);
            // set up stuff.
            tabLayout = (TabLayout)view.FindViewById(Resource.Id.tabs);
            viewPager = (ViewPager)view.FindViewById(Resource.Id.viewpager);

            // create a new adapter for our pageViewer. This adapters returns child fragments as per the positon of the page Viewer.
            //viewPager.setAdapter(new MyAdapter(getChildFragmentManager()));
            viewPager.Adapter = new TabFragmentAdapter(ChildFragmentManager, MyFragments);

            // this is a workaround
            tabLayout.SetupWithViewPager(viewPager);
                //to preload the adjacent tabs. This makes transition smooth.
            viewPager.OffscreenPageLimit = 2;

        return view;
        }
    }
}