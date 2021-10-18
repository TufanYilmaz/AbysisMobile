using AbysisMobile.Adapters;
using AbysisMobile.Fragments;
using AbysisMobile.Helpers;
using AbysisMobile.Models;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfDataGrid;
using System;
using System.Collections.Generic;
using System.Threading;
using Fragment = Android.Support.V4.App.Fragment;

namespace AbysisMobile
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.NoActionBar",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        LaunchMode = Android.Content.PM.LaunchMode.SingleTop
       )]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        LinearLayout navigationHeader;
        TextView navigationHeaderUserName;
        public static TextView navigationHeaderSelectedMeter;
        FloatingActionButton fab;
        DrawerLayout drawer;
        ActionBarDrawerToggle toggle;
        NavigationView navigationView;
        Android.Support.V7.Widget.Toolbar toolbar;
        ExpandableListView expandableList;
        ExpandableListAdapter menuAdapter;
        List<ExpandedMenuModel> listDataHeader;
        Dictionary<ExpandedMenuModel, List<string>> listDataChild;
        //Android.Support.V4.App.FragmentManager fragmentManager=
#pragma warning disable CS0618 // Type or member is obsolete
        static ProgressDialog waitingDialog;
#pragma warning restore CS0618 // Type or member is obsolete
        [Obsolete]
        private ProgressDialog GetProgressDialog()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            waitingDialog = new ProgressDialog(this);
#pragma warning restore CS0618 // Type or member is obsolete
            waitingDialog.SetMessage("Lütfen Bekleyiniz");
            waitingDialog.SetCancelable(false);
            waitingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            waitingDialog.Progress = 0;
            waitingDialog.Max = 1;
            return waitingDialog;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            //waitingDialog = new ProgressDialog(this);
            //waitingDialog.SetMessage("Lütfen Bekleyiniz");
            //waitingDialog.SetCancelable(false);
            //waitingDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            //waitingDialog.Progress = 0;
            //waitingDialog.Max = 1;
            //waitingDialog.Show();
            ////SessionHelper.GetInvoicesData();
            //waitingDialog.Hide();
            //new Thread(() =>
            //{
            //    SessionHelper.GetInvoicesData();
            //}).Start();
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetHomeButtonEnabled(true);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Visibility = ViewStates.Gone;
            fab.Click += FabOnClick;

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();



            PrepareMeterListData();
            expandableList = FindViewById<ExpandableListView>(Resource.Id.navigationmenu);
            menuAdapter = new ExpandableListAdapter(this, listDataHeader, listDataChild, expandableList);
            expandableList.SetAdapter(menuAdapter);
            expandableList.ChildClick += ExpandableList_ChildClick;


            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationHeader = (LinearLayout)navigationView.GetHeaderView(0);

            ManageNavigation();
            SetSelectedMeterToText();
            Bundle b = new Bundle();
            b.PutInt("Position", 0);
            FragmentDefault fragment = new FragmentDefault
            {
                Arguments = b
            };
            SetFragmentToScreen(fragment);
            //SelectFragment((char)MenuItemShortCut.Meters);
            //drawer
            //new Thread(() =>
            //{
            //    SessionHelper.GetInvoicesForSelectedDepartment();
            //}).Start();
            //var progressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleLarge);
            //DrawerLayout.LayoutParams params = new DrawerLayout.LayoutParams(100, 100);
            //params

        }

        private void ExpandableList_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {

            //Console.WriteLine(e.ChildPosition);
            Bundle b = new Bundle();
            b.PutInt("Position", e.ChildPosition);
            FragmentDefault fragment = new FragmentDefault
            {
                Arguments = b
            };
            Session.SelectedMeter = Session.LoginData.AuthorizedMeters[e.ChildPosition];
            SetSelectedMeterToText();
            expandableList.CollapseGroup(0);
            //new Thread(() =>
            //{
            //    SessionHelper.GetInvoicesForSelectedDepartment();
            //}).Start();
            RunOnUiThread(() =>
            {
                drawer.CloseDrawers();
                drawer.CloseDrawer(GravityCompat.Start);
            });
            new Thread(() => SetFragmentToScreen(fragment)).Start();
            //try
            //{
            //    DateTime now = DateTime.Now;
            //    DateTime start = new DateTime(now.Year - 5, 1, 1);
            //    List<Invoices.invoice> result = new List<Invoices.invoice>();
            //    result = Services.invoicesClient.GetInvoiceBySubscriber(Session.LoginData.Value.ToString(), Session.SelectedMeter.DepartmentId, 1, start, now, Session.SelectedMeter.SubscriberId).Value.ToList();

            //}
            //catch (Exception ex)
            //{
            //    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            //}
            //waitingDialog.Show();
        }

        private void PrepareMeterListData()
        {
            listDataHeader = new List<ExpandedMenuModel>();
            listDataChild = new Dictionary<ExpandedMenuModel, List<string>>();

            ExpandedMenuModel item1 = new ExpandedMenuModel
            {
                Name = "Sayaçlar",
                Image = Resource.Drawable.speedometer
            };
            listDataHeader.Add(item1);
            List<string> heading1 = new List<string>();
            foreach (var item in Session.LoginData.AuthorizedMeters)
            {
                heading1.Add(item.DepartmentId + item.Code);
            }



            listDataChild.Add(listDataHeader[0], heading1);// Header, Child data

        }
        List<NavigationMenuItem> menuItem = NavigationMenuItem.GetMenuItems();
        void ManageNavigation()
        {
            int g = 0;
            navigationView.SetNavigationItemSelectedListener(this);
            //Sol Açılır Menu Listesi
            foreach (var m in menuItem)
            {
                navigationView.Menu
                    .Add(0, m.Id, g, m.Name)
                    .SetIcon(m.Icon)
                    .SetVisible(m.Visiblity)
                    .SetNumericShortcut((char)m.ItemPosition);
            }

            //navigationView.Menu.AddSubMenu("Yan Servisler");

            //sol açılır menü itemi seçildiğinde
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            //açılır menu başlığı

            navigationHeaderUserName = navigationHeader.FindViewById<TextView>(Resource.Id.navigationHeaderUsername);
            navigationHeaderSelectedMeter = navigationHeader.FindViewById<TextView>(Resource.Id.navigationHeaderSelectedMeter);
            navigationHeaderUserName.Text = Session.LoginData.User.Info;
            navigationHeaderUserName.Text += string.IsNullOrEmpty(Session.LoginData.User.Code) ? "" : "\n" + Session.LoginData.User.Code;
            navigationHeaderUserName.Text += string.IsNullOrEmpty(Session.LoginData.User.Windows_User) ? "" : "\n" + Session.LoginData.User.Windows_User;
            //ElektricDepartmentMenu(false);
        }
        void MenuVisiblityByDepartmentId(int department)
        {

            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.Current).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId);
            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.Voltage).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId);
            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.PTF).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId);
            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.MeterSummary).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId || 2 == Session.SelectedMeter.DepartmentId || 3 == Session.SelectedMeter.DepartmentId);
            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.PTF).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId);
            navigationView.Menu.FindItem(menuItem.Find(x => x.Id == (int)MenuItemShortCutInt.PTF).Id)
                .SetVisible(1 == Session.SelectedMeter.DepartmentId);
                //.SetVisible(false);//PTF olmayan OSB ler
        }
        void SetSelectedMeterToText()
        {
            navigationHeaderSelectedMeter.Text = string.IsNullOrEmpty(Session.SelectedMeter.DepartmentCode) ? "" : Session.SelectedMeter.DepartmentCode + "\n";
            navigationHeaderSelectedMeter.Text += string.IsNullOrEmpty(Session.SelectedMeter.Code) ? "" : Session.SelectedMeter.Code;
            MenuVisiblityByDepartmentId(Session.SelectedMeter.DepartmentId);
        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            Console.WriteLine(e.MenuItem);
            OnNavigationItemSelected(e.MenuItem);
            //e.MenuItem.
            //e.MenuItem.
        }

        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder alertDialogBuilder = new Android.App.AlertDialog.Builder(this);
            alertDialogBuilder.SetMessage(Resource.String.AskClose);
            alertDialogBuilder.SetCancelable(true);
            alertDialogBuilder.SetPositiveButton(Resource.String.Close, delegate
            {
                Session.ClearSession();
                this.Finish();
            });
            alertDialogBuilder.SetNegativeButton(Resource.String.Cancel, delegate
            {
                alertDialogBuilder.Dispose();
            });
            alertDialogBuilder.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menu_change_password)
            {
                SetFragmentToScreen(new FragmentChangePassword());
            }
            else if (id == Resource.Id.menu_exit)
            {
                OnBackPressed();
            }
            else
            {
                Toast.MakeText(this, "Hatalı Seçim", ToastLength.Short).Show();
            }

            return base.OnOptionsItemSelected(item);
        }
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
        int oldPosition = -1;
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            expandableList.CollapseGroup(0);
            int id = item.ItemId;
            char position = item.NumericShortcut;

            oldPosition = position;

            SelectFragment(item.ItemId);

            RunOnUiThread(() => drawer.CloseDrawer(GravityCompat.Start));

            return true;
        }
        private void SelectFragment(int position)
        {
            switch (position)
            {
                case (int)MenuItemShortCutInt.Meters:
                    SetFragmentToScreen(FragmentMeters.NewInstance());
                    break;
                case (int)MenuItemShortCutInt.MeterSummary:
                     List<Fragment> summaryFragments = new List<Fragment>
                    {
                        FragmentMeter.NewInstance("Sayaç Bilgileri"),
                        FragmentMeterSummary.NewInstance("Sayaç Detayları"),
                    };
                    if (Session.SelectedMeter.DepartmentId == 3)
                        summaryFragments.Add(FragmentMechanicMeterInfo.NewInstance("Mekanik Sayaç Bilgileri"));
                    SetFragmentToScreen(
                        new TabFragment(
                            SupportFragmentManager,
                            summaryFragments
                        ));
                    break;
                case (int)MenuItemShortCutInt.MeterQuery:
                    var fragmentMeterQuery = new FragmentMeterQuery();
                    SetFragmentToScreen(fragmentMeterQuery);
                    new Thread(() =>
                    {
                        Thread.Sleep(500);
                        fragmentMeterQuery.btnQuery.Click += (s, e) =>
                        {
                            RunOnUiThread(() =>
                            {
                                fragmentMeterQuery.btnDate1.Enabled = false;
                                fragmentMeterQuery.btnDate2.Enabled = false;
                                fragmentMeterQuery.btnQuery.Enabled = false;
                                fragmentMeterQuery.spIndices.Enabled = false;
                            });
                            var mHandler = new Handler(Looper.MainLooper);
                            mHandler.Post(() =>
                            {
                                fragmentMeterQuery.BtnQueryAction();
                                RunOnUiThread(() =>
                                {
                                    fragmentMeterQuery.btnDate1.Enabled = true;
                                    fragmentMeterQuery.btnDate2.Enabled = true;
                                    fragmentMeterQuery.btnQuery.Enabled = true;
                                    fragmentMeterQuery.spIndices.Enabled = true;
                                });
                            });

                        };

                    }).Start();
                    break;
                case (int)MenuItemShortCutInt.Fixed:

                    var fragmentConsGraphFixed = new FragmentConsGraphFixed();
                    SetFragmentToScreen(fragmentConsGraphFixed);
                    new Thread(() =>
                    {
                        Thread.Sleep(1000);//todo:kesinlikle event olması gerekiyor
                        fragmentConsGraphFixed.btnQuery.Click += (s, e) =>
                        {
                            RunOnUiThread(() =>
                            {
                                fragmentConsGraphFixed.spIndices.Enabled = false;
                                fragmentConsGraphFixed.spPeriodYear.Enabled = false;
                                fragmentConsGraphFixed.btnQuery.Enabled = false;
                                fragmentConsGraphFixed.btnClear.Enabled = false;
                            });
                            var mHandler = new Handler(Looper.MainLooper);
                            mHandler.Post(() =>
                            {
                                fragmentConsGraphFixed.BtnQueryAction();
                                RunOnUiThread(() =>
                                {
                                    fragmentConsGraphFixed.spIndices.Enabled = true;
                                    fragmentConsGraphFixed.spPeriodYear.Enabled = true;
                                    fragmentConsGraphFixed.btnQuery.Enabled = true;
                                    fragmentConsGraphFixed.btnClear.Enabled = true;
                                });
                            });
                        };
                    }).Start();

                    break;
                case (int)MenuItemShortCutInt.FixedYear:

                    var fragments = GetSlidingTabFragments(
                        new string[]
                        {
                        "Title 1",
                        "Title 2",
                        "Title3"
                        });


                    var tabbedFragment =
                    SetFragmentToScreen(
                        new TabFragment(
                            SupportFragmentManager,
                            fragments
                            ));
                    break;
                case (int)MenuItemShortCutInt.Voltage:

                    var fragmentVoltageGraph = new FragmentVoltageGraph();
                    SetFragmentToScreen(fragmentVoltageGraph);
                    new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        fragmentVoltageGraph.QueryButton.Click += (s, e) =>
                        {
                            RunOnUiThread(() =>
                            {
                                fragmentVoltageGraph.LoadingLayout.Visibility = ViewStates.Visible;
                                fragmentVoltageGraph.QueryButton.Enabled = false;
                                fragmentVoltageGraph.TrifazeChart.Enabled = false;
                                fragmentVoltageGraph.btnDate1.Enabled = false;
                                fragmentVoltageGraph.btnDate2.Enabled = false;

                            });
                            var mHandler = new Handler(Looper.MainLooper);
                            mHandler.Post(() =>
                            {
                                var IndexList = SessionHelper.LoadIndexList().FindAll(i => i.Code == "32.7.0" || i.Code == "52.7.0" || i.Code == "72.7.0");
                                fragmentVoltageGraph.TrifazeChart.GetTrifaze(IndexList, fragmentVoltageGraph.Date1, fragmentVoltageGraph.Date2, "Voltaj Grafiği"); //= Helpers.ChartHelper.GetTriFazeGraph(Context, IndexList, Date1, Date2, "Voltaj Grafiği");
                                Thread.Sleep(100);
                                RunOnUiThread(() =>
                                {
                                    fragmentVoltageGraph.LoadingLayout.Visibility = ViewStates.Gone;
                                    fragmentVoltageGraph.QueryButton.Enabled = true;
                                    fragmentVoltageGraph.TrifazeChart.Enabled = true;
                                    fragmentVoltageGraph.btnDate1.Enabled = true;
                                    fragmentVoltageGraph.btnDate2.Enabled = true;


                                });
                            });

                        };
                    }).Start();

                    break;
                case (int)MenuItemShortCutInt.Current:

                    var fragmentCurrentGraph = new FragmentCurrentGraph();
                    SetFragmentToScreen(fragmentCurrentGraph);
                    new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        fragmentCurrentGraph.QueryButton.Click += (s, e) =>
                        {
                            RunOnUiThread(() =>
                            {
                                fragmentCurrentGraph.LoadingLayout.Visibility = ViewStates.Visible;
                                fragmentCurrentGraph.QueryButton.Enabled = false;
                                fragmentCurrentGraph.TrifazeChart.Enabled = false;
                                fragmentCurrentGraph.btnDate1.Enabled = false;
                                fragmentCurrentGraph.btnDate2.Enabled = false;

                            });
                            var mHandler = new Handler(Looper.MainLooper);
                            mHandler.Post(() =>
                            {
                                var IndexList = SessionHelper.LoadIndexList().FindAll(i => i.Code == "31.7.0" || i.Code == "51.7.0" || i.Code == "71.7.0");
                                fragmentCurrentGraph.TrifazeChart.GetTrifaze(IndexList, fragmentCurrentGraph.Date1, fragmentCurrentGraph.Date2, "Akım Grafiği"); //= Helpers.ChartHelper.GetTriFazeGraph(Context, IndexList, Date1, Date2, "Voltaj Grafiği");

                                Thread.Sleep(100);
                                RunOnUiThread(() =>
                                {
                                    fragmentCurrentGraph.LoadingLayout.Visibility = ViewStates.Gone;
                                    fragmentCurrentGraph.QueryButton.Enabled = true;
                                    fragmentCurrentGraph.TrifazeChart.Enabled = true;
                                    fragmentCurrentGraph.btnDate1.Enabled = true;
                                    fragmentCurrentGraph.btnDate2.Enabled = true;


                                });
                            });

                        };
                    }).Start();

                    break;
                case (int)MenuItemShortCutInt.Invoice:

                    var fragmentInvoices = new FragmentInvoices();
                    SetFragmentToScreen(fragmentInvoices);
                    new Thread(() =>
                    {
                        Thread.Sleep(1000);//Todo: Evente Çevir
                        var gridInvoices = fragmentInvoices.View.FindViewById<SfDataGrid>(Resource.Id.sfAllInvoices);
                        var invoices = SessionHelper.GetInvoicesForSelectedDepartment();
                        RunOnUiThread(() =>
                        {
                            gridInvoices.ItemsSource = invoices;
                            gridInvoices.ScrollingMode = ScrollingMode.PixelLine;
                            fragmentInvoices.progressBar.Visibility = ViewStates.Gone;
                            gridInvoices.Visibility = ViewStates.Visible;
                            fragmentInvoices.tvHelper.Visibility = ViewStates.Visible;

                        });
                    }).Start();

                    break;
                case (int)MenuItemShortCutInt.NotFound:

                    break;
                case (int)MenuItemShortCutInt.PTF:
                    SetFragmentToScreen(new FragmentPTF());
                    break;
                default:
                    break;
            }

        }


        public List<Fragment> GetSlidingTabFragments(string[] args = null)
        {
            var result = new List<Fragment>();

            foreach (var item in Session.LoginData.AuthorizedMeters)
            {
                result.Add(FragmentMeter.NewInstance(item));
            }
            return result;
        }
        bool SetFragmentToScreen(Fragment fragment)
        {
            RunOnUiThread(() => drawer.CloseDrawers());
            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.frameContent, fragment)
                .Commit();
            return false;

        }
    }
}

