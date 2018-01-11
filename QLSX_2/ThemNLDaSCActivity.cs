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
using QLSX.Resources.Model;

namespace QLSX_2
{
    [Activity(Label = "ThemNLDaSCActivity")]
    public class ThemNLDaSCActivity : Activity
    {
        EditText txtMaNL, txtTenNL, txtDVT;
        Button btnSave, btnHuy;
        bool Edit = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemNguyenLieuDaSC);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaNL = FindViewById<EditText>(Resource.Id.MaNLDaSC_edit_text);
            txtTenNL = FindViewById<EditText>(Resource.Id.TenNLDaSC_edit_text);
            txtDVT = FindViewById<EditText>(Resource.Id.DVTNLDaSC_edit_text);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuNLDaSC);
            btnHuy = FindViewById<Button>(Resource.Id.btnHuyNLDaSC);

            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate
            {
                Intent intent = new Intent(this, typeof(NguyenLieuDaSCActivity));
                StartActivity(intent);
            };
            var editId = Intent.GetStringExtra("MaNLDaSC") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaNL.Text = editId;
                LoadDataForEdit(editId);
                txtMaNL.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string editId)
        {
            NguyenLieuDaSCDbHelper db = new NguyenLieuDaSCDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getNLDaSCById(editId);
            if (cData.MoveToFirst())
            {
                txtMaNL.Text = cData.GetString(cData.GetColumnIndex("MaNLDaSC"));
                txtTenNL.Text = cData.GetString(cData.GetColumnIndex("TenNLDaSC"));
                txtDVT.Text = cData.GetString(cData.GetColumnIndex("DVTNLDaSC"));
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            NguyenLieuDaSCDbHelper db = new NguyenLieuDaSCDbHelper(this, MainActivity.dataname);
            if (txtMaNL.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Mã Nguyên Liệu Đã Sơ Chế.", ToastLength.Short).Show();
                return;
            }
            if (txtTenNL.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Tên Nguyên Liệu Đã Sơ Chế.", ToastLength.Short).Show();
                return;
            }
            if (db.getNLDaSCById(txtMaNL.Text) != null)
            {
                Toast.MakeText(this, "Mã Nguyên Liệu Đã Sơ Chế Đã tồn tại.", ToastLength.Short).Show();
                return;
            }
            NguyenLieuDaSoche ab = new NguyenLieuDaSoche();

            ab.MaNLDaSC = txtMaNL.Text;
            ab.TenNLDaSC = txtTenNL.Text;
            ab.DVTNLDaSC = txtDVT.Text;

            try
            {
                if (Edit == false)
                {
                    IList<NguyenLieuDaSoche> list = db.GetAllNLDaSC();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaNLDaSC == txtMaNL.Text)
                        {
                            Toast.MakeText(this, "Mã Nguyên Liệu Đã tồn tại.", ToastLength.Short).Show();
                            return;
                        }
                    }
                    db.AddNewNLDaSC(ab);
                    Toast.MakeText(this, "Thêm Nguyên Liệu Đã Sơ Chế Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdateNLDaSC(ab);
                    Toast.MakeText(this, "Cập Nhật thành công.", ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var PTActivity = new Intent(this, typeof(NguyenLieuDaSCActivity));
                StartActivity(PTActivity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}