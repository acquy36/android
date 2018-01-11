using System;
using System.Collections.Generic;
using System.Linq;
using QLSX_2.Resources.Data;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using QLSX.Resources.Model;
using Android.Widget;
using System.IO;
using Android.Database.Sqlite;

namespace QLSX_2
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {

        EditText txtusername;
        EditText txtPassword;
        Button btncreate;
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Newuser);
            // Create your application here
           
            btncreate = FindViewById<Button>(Resource.Id.btn_reg_create);
            txtusername = FindViewById<EditText>(Resource.Id.txt_reg_username);
            txtPassword = FindViewById<EditText>(Resource.Id.txt_reg_password);

            btncreate.Click += Btncreate_Click;
        }

        private void Btncreate_Click(object sender, EventArgs e)
        {

            try
            {
                DataBase db = new DataBase(this, txtusername.Text);
                LoginTable tbl = new LoginTable();
                tbl.username = txtusername.Text;
                tbl.password = txtPassword.Text;
                ContentValues vals = new ContentValues();
                vals.Put("Username", tbl.username);
                vals.Put("Password", tbl.password);
                db.WritableDatabase.Insert("LOGINTABLE", null, vals);
                
                Toast.MakeText(this, "Đắng Ký Thành Công...,", ToastLength.Short).Show();
                MainActivity.dataname = txtusername.Text;
                var TPActivity = new Intent(this, typeof(MainActivity));
                StartActivity(TPActivity);
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
    }
}