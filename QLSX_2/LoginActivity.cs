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
using QLSX_2.Resources.Data;
using QLSX.Resources.Model;
using Android.Database;

namespace QLSX_2
{
    [Activity(Label = "QLSX", MainLauncher = true, Icon = "@drawable/icon")]

    public class LoginActivity : Activity
    {
        EditText txtusername;
        EditText txtPassword;
        Button btncreate;
        Button btnsign;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);
            btnsign = FindViewById<Button>(Resource.Id.btnlogin);
            btncreate = FindViewById<Button>(Resource.Id.btnregister);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtpwd);

            btnsign.Click += Btnsign_Click;
            btncreate.Click += Btncreate_Click;
        }
        private void Btncreate_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
        private void Btnsign_Click(object sender, EventArgs e)
        {
            try
            {
                DataBase db = new DataBase(this, txtusername.Text);
                LoginTable tbl = new LoginTable();
                ICursor data = db.ReadableDatabase.Query("LOGINTABLE", new string[] { "ID", "Username", "Password" }, null, null, null, null, null);

                var contacts = new List<LoginTable>();

                while (data.MoveToNext())
                {

                    tbl.id = data.GetInt(0);
                    tbl.username = data.GetString(1);
                    tbl.password = data.GetString(2); 
                   
                }
                data.Close();
                db.Close();
                if (tbl.username == txtusername.Text && tbl.password == txtPassword.Text)
                {
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();
                    MainActivity.dataname = txtusername.Text;
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
       

    }
}