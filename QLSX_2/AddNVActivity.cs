using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Database;
using System.Text.RegularExpressions;
using QLSX.Resources.Model;
using QLSX_2.Resources.Data;
using System.Collections.Generic;

namespace QLSX_2
{
    [Activity(Label = "AddNVActivity")]
    public class AddNVActivity : Activity
    {
        EditText txtMaNV, txtTenNV, txtSoDTNV, txtDiaChiNV, txtEmailNV;

        Button btnSave, btnHuy;
        string editId;
        bool Edit = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemNhanVien);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaNV = FindViewById<EditText>(Resource.Id.MaNV_edit_text);
            txtTenNV = FindViewById<EditText>(Resource.Id.HoTenNV_edit_text);
            txtSoDTNV = FindViewById<EditText>(Resource.Id.SoDTNV_edit_text);
            txtDiaChiNV = FindViewById<EditText>(Resource.Id.DiachiNV_edit_text);
            txtEmailNV = FindViewById<EditText>(Resource.Id.EmailNV_edit_text);
          
            btnSave = FindViewById<Button>(Resource.Id.ThemNV);
            btnHuy = FindViewById<Button>(Resource.Id.HuyNV);

            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate {
                Intent intent = new Intent(this, typeof(NhanVienActivity));
                StartActivity(intent);
            };
            editId = Intent.GetStringExtra("MaNV") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaNV.Text = editId;
                LoadDataForEdit(editId);
                txtMaNV.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string ediId)
        {
            NhanVienDbHelper db = new NhanVienDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getNhanVienById(ediId);
            if (cData.MoveToFirst())
            {
                txtTenNV.Text = cData.GetString(cData.GetColumnIndex("TenNV"));
                txtSoDTNV.Text = cData.GetString(cData.GetColumnIndex("SoDTNV"));
                txtDiaChiNV.Text = cData.GetString(cData.GetColumnIndex("DiaChiNV"));
                txtEmailNV.Text = cData.GetString(cData.GetColumnIndex("EmailNV"));
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            NhanVienDbHelper db = new NhanVienDbHelper(this, MainActivity.dataname);
            if (txtTenNV.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ten Nhan Vien.", ToastLength.Short).Show();
                return;
            }

            if (txtSoDTNV.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Mobile Number.", ToastLength.Short).Show();
                return;
            }

            if (txtEmailNV.Text.Trim().Length > 0)
            {
                string EmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (!Regex.IsMatch(txtEmailNV.Text, EmailPattern, RegexOptions.IgnoreCase))
                {
                    Toast.MakeText(this, "Invalid email address.", ToastLength.Short).Show();
                    return;
                }
            }
           
            NhanVien ab = new NhanVien();

            if (txtMaNV.Text.Trim().Length > 0)
            {
                ab.MaNV = txtMaNV.Text;
            }
            ab.TenNV = txtTenNV.Text;
            ab.SoDTNV = txtSoDTNV.Text;
            ab.DiachiNV = txtDiaChiNV.Text;
            ab.EmailNV = txtEmailNV.Text;

            try
            {

                if (Edit == false)
                {
                    IList<NhanVien> list = db.GetAllNhanVien();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaNV == txtMaNV.Text)
                        {
                            Toast.MakeText(this, "Mã Nhân Viên Đã tồn tại.", ToastLength.Short).Show();
                            return;
                        }
                    }                   
                    db.AddNewNhanVien(ab);
                    Toast.MakeText(this, "Thêm Nhân viên Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdateNhanVien(ab);
                    Toast.MakeText(this, "Cập nhật thành công.", ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var NVActivity = new Intent(this, typeof(NhanVienActivity));
                StartActivity(NVActivity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}