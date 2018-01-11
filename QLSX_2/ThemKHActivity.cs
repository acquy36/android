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
using Android.Database;
using System.Text.RegularExpressions;
using QLSX.Resources.Model;

namespace QLSX_2
{
    [Activity(Label = "ThemKHActivity")]
    public class ThemKHActivity : Activity
    {
        EditText txtMaKH, txtTenKH, txtSoDT, txtDiaChi, txtEmail;
        Button btnSave,btnHuy;
        string editId;
        bool Edit = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddEditKhachHang);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaKH = FindViewById<EditText>(Resource.Id.MaKH_edit);
            txtTenKH = FindViewById<EditText>(Resource.Id.HoTen_edit);
            txtSoDT = FindViewById<EditText>(Resource.Id.SoDT_edit);
            txtDiaChi = FindViewById<EditText>(Resource.Id.Diachi_edit);
            txtEmail = FindViewById<EditText>(Resource.Id.Email_edit);
            btnSave = FindViewById<Button>(Resource.Id.btnLuu);
            btnHuy = FindViewById<Button>(Resource.Id.btnHuy);

            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate {
                Intent intent = new Intent(this, typeof(KhachHangActivity));
                StartActivity(intent);
            };
             editId = Intent.GetStringExtra("MaKH") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaKH.Text = editId;
                LoadDataForEdit(editId);
                txtMaKH.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string contactId)
        {
            KhachHangDbHelper db = new KhachHangDbHelper(this,MainActivity.dataname);
            ICursor cData = db.getKhachHangById(contactId);
            if(cData.MoveToFirst())
            {
                txtTenKH.Text = cData.GetString(cData.GetColumnIndex("TenKH")); 
                txtSoDT.Text = cData.GetString(cData.GetColumnIndex("SoDTKH")); 
                txtDiaChi.Text= cData.GetString(cData.GetColumnIndex("DiaChiKH"));
                txtEmail.Text = cData.GetString(cData.GetColumnIndex("EmailKH")); 
            }
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            KhachHangDbHelper db = new KhachHangDbHelper(this,MainActivity.dataname);
            if (txtMaKH.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ten Ma Khach Hang.", ToastLength.Short).Show();
                return;
            }
            if (txtTenKH.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ten Khach Hang.", ToastLength.Short).Show();
                return;
            }

            if (txtSoDT.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Mobile Number.", ToastLength.Short).Show();
                return;
            }

            if (txtEmail.Text.Trim().Length > 0)
            {
                string EmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (!Regex.IsMatch(txtEmail.Text, EmailPattern, RegexOptions.IgnoreCase))
                {
                    Toast.MakeText(this, "Invalid email address.", ToastLength.Short).Show();
                    return;
                }
            }
           
            KhachHang ab = new KhachHang();         
            ab.MaKH = txtMaKH.Text;            
            ab.TenKH = txtTenKH.Text;
            ab.SoDT = txtSoDT.Text;
            ab.Diachi = txtDiaChi.Text;
            ab.Email = txtEmail.Text;

            try
            {             
                if (Edit==false)
                {
                    IList<KhachHang> list = db.GetAllKhachHang();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaKH == txtMaKH.Text.Trim())
                        {
                            Toast.MakeText(this, "Mã Khách Hàng Đã tồn tại.", ToastLength.Short).Show();
                            return;
                        }
                    }
                    db.AddNewKhachHang(ab);
                     Toast.MakeText(this, "Thêm Khách Hàng Thành Công.", ToastLength.Short).Show(); 
                    
                }
                else
                {
                    db.UpdateKhachHang(ab);
                     Toast.MakeText(this, "Cập nhật thành công.", ToastLength.Short).Show();
                    
                }

                Finish();

                //Go to main activity after save/edit
                var KHActivity = new Intent(this, typeof(KhachHangActivity));
                StartActivity(KHActivity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
