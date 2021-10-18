using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;


namespace AbysisMobile.Fragments
{
    
    public class FragmentChangePassword : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.fragment_change_password, null);
            ManageUIs(view);
            //(InputManager)Context.GetSystemService(Context.Inp)
            return view;
        }
        EditText etPassword;
        EditText etNewPassword;
        EditText etNewPasswordAgain;
        Button btnChangePassword;
        TextView tvPasswordChangeInfo;
        private void ManageUIs(View view)
        {
            etPassword = view.FindViewById<EditText>(Resource.Id.etPreviousPassword);
            etNewPassword = view.FindViewById<EditText>(Resource.Id.etNewPassword);
            etNewPasswordAgain = view.FindViewById<EditText>(Resource.Id.etNewPasswordAgain);
            btnChangePassword = view.FindViewById<Button>(Resource.Id.btnChangePassword);
            btnChangePassword.Click += BtnChangePassword_Click;
            tvPasswordChangeInfo = view.FindViewById<TextView>(Resource.Id.tvPasswordChangeInfo);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            tvPasswordChangeInfo.SetTextColor(new Android.Graphics.Color(180, 50, 50));
            if (string.IsNullOrEmpty(etPassword.Text) || string.IsNullOrEmpty(etNewPassword.Text) || string.IsNullOrEmpty(etNewPasswordAgain.Text))
            {
                tvPasswordChangeInfo.Text = "Bütün Alanlar Zorunludur";
                return;
            }
            if (!etNewPassword.Text.Equals(etNewPasswordAgain.Text))
            {
                tvPasswordChangeInfo.Text = "Yeni parola ile tekrarı eşleşmiyor";
                return;
            }
            try
            {
                //var result=Services.authenticationClient.SetPassword(Session.SessionId, etPassword.Text, etNewPassword.Text);
            }
            catch (Exception ex)
            {
                tvPasswordChangeInfo.Text = "Hata\n" + ex.Message;
            }
        }
    }
}