using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AbysisMobile.Authentication;
using AbysisMobile.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace AbysisMobile
{
    [Activity(
        Label = "@string/app_name", 
        Theme = "@style/AppLoginTheme.NoActionBar",
         ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        LaunchMode = Android.Content.PM.LaunchMode.SingleTop,
        MainLauncher =true
        )]
    public class LoginActivity : AppCompatActivity
    {
        LinearLayout linearLoginInput;
        ImageView abysisLogo;
        Animation animFromBottom;
        Animation animFromTop;
        EditText etUsername;
        EditText etPassword;
        Button btnLogin;
        CheckBox chckRemmeberMe;
#pragma warning disable CS0618 // Type or member is obsolete
        ProgressDialog pd;
#pragma warning restore CS0618 // Type or member is obsolete
        void ManageUIs()
        {
            linearLoginInput = FindViewById<LinearLayout>(Resource.Id.linearLoginInput);
            abysisLogo = FindViewById<ImageView>(Resource.Id.ivLoginAbysisLogo);
            etUsername = FindViewById<EditText>(Resource.Id.etLoginUserName);
            etPassword = FindViewById<EditText>(Resource.Id.etLoginPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLoginLogin);
            chckRemmeberMe = FindViewById<CheckBox>(Resource.Id.chckRememberMe);
            btnLogin.Click += BtnLogin_Click;
        }
        void ManageAnimations()
        {
            animFromBottom = AnimationUtils.LoadAnimation(this, Resource.Animation.frombottom);
            animFromTop = AnimationUtils.LoadAnimation(this, Resource.Animation.fromtop);
            linearLoginInput.Animation = animFromBottom;
            abysisLogo.Animation = animFromTop;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if(Session.IsLogin)
                StartActivity(new Intent(this, typeof(MainActivity)));
            SetContentView(Resource.Layout.activity_login);

            //UI Kısmı
            ManageUIs();
            //Animasyoun Kısmı
            ManageLoginConfig();
            ManageAnimations();
            // onlyDebug();
            //
#pragma warning disable CS0618 // Type or member is obsolete
            pd = new ProgressDialog(this);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private void ManageLoginConfig()
        {
            if(LoginConfig.ConfigFileExist())
            {
                var configContent = LoginConfig.GetLoginInfo().Split('|');
                if (!string.IsNullOrEmpty(configContent[0]))
                    etUsername.Text = configContent[0];
                if (!string.IsNullOrEmpty(configContent[1]))
                    etPassword.Text = configContent[1];
            }
        }

        void onlyDebug()
        {
            etUsername.Text = "1.A015";
            etPassword.Text = "1.A015";
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {

            pd.SetMessage("Giriş Yapılıyor");
            pd.SetCancelable(false);
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);
            pd.Progress = 0;
            pd.Max = 11;
            pd.Show();

            if(chckRemmeberMe.Checked)
            {
                if(!LoginConfig.ConfigFileExist())
                    LoginConfig.CreteConfigFile();
                LoginConfig.SetLoginInfo(etUsername.Text + "|" + etPassword.Text);
            }
            new Thread(() => LoginThread()).Start();

        }
        private void LoginThread()
        {
            if (etUsername.Text != "" && etPassword.Text != "")
            {
                RunOnUiThread(() =>
                {
                    //LoginInfoTextView.Text = "Giriş Yapılıyor...";
                    //LoginButton.Enabled = false;
                    //LoginProgressBar.Visibility = ViewStates.Visible;
                    pd.SetMessage("Giriş Yapılıyor...");
                });
                try
                {

                    var loginData = Services.authenticationClient.Login(etUsername.Text, etPassword.Text, "");
                    ManageLogin(data: loginData);
                }
                catch (Exception ex)
                {
                    RunOnUiThread(() => {
                        pd.SetMessage("Hata \n" + ex.Message);
                        pd.SetCancelable(true);
                    });
                }
            }
            else 
                RunOnUiThread(() => {
                    
                    pd.SetMessage("Kullanıcı adınızı ve şifrenizi girmelisiniz.");
                    pd.SetCancelable(true);
                }); 
        }

        private void ManageLogin(AuthenticationResult data)
        {

            if (data != null)
            {
                switch (data.Result.Id)
                {
                    case 0:
                        if (data.AuthorizedMeters.Count() > 0)
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Giriş Başarılı", ToastLength.Short).Show();
                                ProgressDialogSetMessage("Giriş Başarılı",false);
                            });
                            //var authMeters = data.AuthorizedMeters.OrderBy(x => x.Id).ToList();
                            
                            var authMeters = data.AuthorizedMeters.OrderBy(x => x.Id).Select(x=>x).ToList();
                            data.AuthorizedMeters = authMeters.ToArray();
                            Session.LoginData = data;
                            var SelectedMeter = authMeters[0];
                            Session.SelectedMeter = SelectedMeter;

                            Session.MeterList.Clear();
                            foreach (var item in authMeters)
                            {
                                Session.MeterList.Add(new Models.AuthMeters(item));
                            }
                            //Task.Run(()=> SessionHelper.GetInvoicesForSelectedDepartment());
                            Session.IsLogin = true;
                            StartActivity(new Intent(this, typeof(MainActivity)));
                        }
                        else
                        {
                            RunOnUiThread(() =>
                            {
                                Toast.MakeText(this, "Kullanıcınız için yetkilendirilmiş bir sayaç bulunamadı.", ToastLength.Short).Show();
                                ProgressDialogSetMessage("Kullanıcınız için yetkilendirilmiş bir sayaç bulunamadı.",true);
                                btnLogin.Enabled = true;
                            });

                        }
                        break;
                    case 1:
                        //RunOnUiThread(() => { });
                        RunOnUiThread(() =>
                        {
                            ProgressDialogSetMessage("Kullanıcı adı veya parolanız hatalı.",true);
                        });
                        break;
                    default:
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, data.Result.Info, ToastLength.Short).Show();
                            ProgressDialogSetMessage(data.Result.Info,true);
                            btnLogin.Enabled = true;
                        });
                        break;
                }
            }
            RunOnUiThread(() =>
            {
                btnLogin.Enabled = true;
                pd.Dismiss();
            });
        }
        void ProgressDialogSetMessage(string message,bool cancelable)
        {
            pd.SetMessage(message);
            pd.SetCancelable(cancelable);
        }
    }
}