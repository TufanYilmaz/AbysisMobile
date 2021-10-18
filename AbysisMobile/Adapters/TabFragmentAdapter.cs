using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Runtime;
using Android.Support.V4.App;
using Java.Lang;

namespace AbysisMobile.Adapters
{
    public class TabFragmentAdapter : FragmentPagerAdapter
    {
        List<Fragment> fragments;
        public TabFragmentAdapter(FragmentManager fragmentManager)
            : base(fragmentManager)
        {

        }
        public TabFragmentAdapter(
            FragmentManager fragmentManager,
            List<Fragment> fragments)
            :base(fragmentManager)
        {
            this.fragments = fragments;
        }
        public override int Count =>  fragments.Count;

        public override Fragment GetItem(int position)
        {
            return fragments[position];
            //switch (position)
            //{
            //    case 0:
            //        var bnd = new Android.OS.Bundle();
            //        bnd.PutString("Demo", "Fragment 1");
            //        return new Fragments.FragmentDemo() { Arguments=bnd };
            //    case 1:
            //        var bnd2 = new Android.OS.Bundle();
            //        bnd2.PutString("Demo", "Fragment 2   ");
            //        return new Fragments.FragmentDemo() { Arguments = bnd2 };
            //    default: return null;
            //}
        }
        //string[] vs = new string[] { "Fragment1", "fragment2" };
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            
             return CharSequence.ArrayFromStringArray(
                 fragments.Select(x=>x.Arguments.GetString("Title")).ToArray()
                 )[position];
        }
    }
}