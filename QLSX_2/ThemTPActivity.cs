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
    [Activity(Label = "ThemTPActivity")]
    public class ThemTPActivity : Activity
    {
        EditText txtMaTP, txtTenTP, txtDVTTP;
        Button btnSave, btnHuy;
        string editId;
        bool Edit = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemThanhPham);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaTP = FindViewById<EditText>(Resource.Id.MaSP_edit_text);
            txtTenTP = FindViewById<EditText>(Resource.Id.TenSP_edit_text);
            txtDVTTP = FindViewById<EditText>(Resource.Id.DonvitinhSP_edit_text);         
            btnSave = FindViewById<Button>(Resource.Id.LuuSP);
            btnHuy = FindViewById<Button>(Resource.Id.HuySP);
            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate {
                Intent intent = new Intent(this, typeof(ThanhPhamActivity));
                StartActivity(intent);
            };
            editId = Intent.GetStringExtra("MaTP") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaTP.Text = editId;
                LoadDataForEdit(editId);
                txtMaTP.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string contactId)
        {
            ThanhPhamDBHelper db = new ThanhPhamDBHelper(this, MainActivity.dataname);
            ICursor cData = db.getThanhPhamById(contactId);
            if (cData.MoveToFirst())
            {
                txtTenTP.Text = cData.GetString(cData.GetColumnIndex("TenTP"));
                txtDVTTP.Text = cData.GetString(cData.GetColumnIndex("DVTTP"));            
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ThanhPhamDBHelper db = new ThanhPhamDBHelper(this, MainActivity.dataname);
            if (txtMaTP.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ma Thanh Pham.", ToastLength.Short).Show();
                return;
            }
            if (txtTenTP.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Tên Thanh Pham.", ToastLength.Short).Show();
                return;
            }

            if (txtDVTTP.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Đơn vị Tính.", ToastLength.Short).Show();
                return;
            } 

            ThanhPham ab = new ThanhPham();
            ab.MaTP = txtMaTP.Text;
            ab.TenTP = txtTenTP.Text;
            ab.DVTTP = txtDVTTP.Text;       

            try
            {
                if (Edit == false)
                {
                    IList<ThanhPham> list = db.GetAllThanhPham();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaTP == txtMaTP.Text)
                        {
                            Toast.MakeText(this, "Mã Thành Phẩm Đã Tồn Tại.", ToastLength.Short).Show();
                            return;
                        }
                    }
                    db.AddNewThanhPham(ab);
                    Toast.MakeText(this, "Thêm Thành Phẩm Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdateThanhPham(ab);
                    Toast.MakeText(this, "Cập nhật thành công.", ToastLength.Short).Show();
                }
                Finish();

                //Go to main activity after save/edit
                var TPActivity = new Intent(this, typeof(ThanhPhamActivity));
                StartActivity(TPActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}