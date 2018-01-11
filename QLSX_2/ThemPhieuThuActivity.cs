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
    [Activity(Label = "ThemPhieuThu")]
    public class ThemPhieuThuActivity : Activity
    {
        EditText txtMaPT, txtSotien;
        Spinner spNV, spKH;
        TextView txtNgayThu, txtMaKH, txtMaNV;
        Button btnSave, btnHuy, btnChonNgay;
        private bool Edit=false;
        IList<KhachHang> list_KH;
        IList<NhanVien> list_NV;
        ArrayAdapter arrKH;
        ArrayAdapter arrNV;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.ThemPhieuThu);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaPT = FindViewById<EditText>(Resource.Id.MaPhieuThu_edit);
            txtSotien = FindViewById<EditText>(Resource.Id.SoTienthu_edit);
            txtMaKH = FindViewById<TextView>(Resource.Id.tvMaKH_PT);
            txtMaNV = FindViewById<TextView>(Resource.Id.tvMaNV_PT);
            spKH = FindViewById<Spinner>(Resource.Id.PT_spKH);
            spNV = FindViewById<Spinner>(Resource.Id.PT_spNV);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuPT);
            btnHuy = FindViewById<Button>(Resource.Id.btnHuyPT);
            txtNgayThu = FindViewById<TextView>(Resource.Id.date_display);
            btnChonNgay = FindViewById<Button>(Resource.Id.date_select_button);

            btnChonNgay.Click += DateSelect_OnClick;
            //Spinner Khách Hàng
            KhachHangDbHelper db = new KhachHangDbHelper(this,MainActivity.dataname);
            list_KH = db.GetAllKhachHang();
            List<string> lsKH = new List<string>();
          
            lsKH.Add("Khách Hàng");
            for (int i = 0; i < list_KH.Count; i++)
                {
                    lsKH.Add(list_KH[i].TenKH);
                }
               arrKH   = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsKH);
                arrKH.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spKH.Adapter = arrKH;

            //Spinner Nhân Viên
            NhanVienDbHelper nv = new NhanVienDbHelper(this,MainActivity.dataname);
            list_NV = nv.GetAllNhanVien();
            List<string> lsNV = new List<string>();
            lsNV.Add("Nhân Viên");
            for (int i = 0; i < list_NV.Count; i++)
            {
                lsNV.Add(list_NV[i].TenNV);
            }
            arrNV = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsNV);
            arrNV.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spNV.Adapter = arrNV;

            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate
            {
                Intent intent = new Intent(this, typeof(PhieuThuActivity));
                StartActivity(intent);
            };
            var editId = Intent.GetStringExtra("MaPhieuThu") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaPT.Text = editId;
                LoadDataForEdit(editId);
                txtMaPT.Enabled = false;
                Edit = true;
               
            }

        }

        private void DateSelect_OnClick(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtNgayThu.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void LoadDataForEdit(string editId)
        {

            PhieuThuDbHelper db = new PhieuThuDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getPhieuThuById(editId);
            if (cData.MoveToFirst())
            {
                txtMaPT.Text = cData.GetString(cData.GetColumnIndex("MaPhieuThu"));
                txtSotien.Text = cData.GetString(cData.GetColumnIndex("SoTienThu"));   
                txtNgayThu.Text = cData.GetString(cData.GetColumnIndex("NgayPhieuThu"));  
                txtMaNV.Text = cData.GetString(cData.GetColumnIndex("MaNV"));
                txtMaKH.Text = cData.GetString(cData.GetColumnIndex("MaKH"));
            }

            //NhanVienDbHelper v = new NhanVienDbHelper(this,MainActivity.dataname);
            //var NV = v.getNhanVienById(txtMaNV.Text);
            //var ten = NV.GetString(NV.GetColumnIndex("TenNV"));
            //int positionNV = arrNV.GetPosition(ten);
            //spNV.SetSelection(positionNV);

            //KhachHangDbHelper h = new KhachHangDbHelper(this,MainActivity.dataname);
            //var KH = h.getKhachHangById(txtMaKH.Text);
            //var tenKh = KH.GetString(KH.GetColumnIndex("TenKH"));
            //int positionKH = arrKH.GetPosition(tenKh);
            //spKH.SetSelection(positionKH);
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            PhieuThuDbHelper db = new PhieuThuDbHelper(this, MainActivity.dataname);
            if (spKH.SelectedItem.ToString().Trim().Equals("Khách Hàng"))
            {
                Toast.MakeText(this, "Vui lòng chọn Khách Hàng", ToastLength.Long).Show();
                return;
            }
            if (spNV.SelectedItem.ToString().Trim().Equals("Nhân Viên"))
            {
                Toast.MakeText(this, "Vui lòng chọn Nhân Viên", ToastLength.Long).Show();
                return;
            }

            if (txtMaPT.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Mã Phiếu Thu.", ToastLength.Short).Show();
                return;
            }
           
            if (txtNgayThu.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Chọn Ngày Thu", ToastLength.Short).Show();
                return;
            }
            if (txtSotien.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Số Tiền Thu.", ToastLength.Short).Show();
                return;
            }
            if (float.Parse(txtSotien.Text) < 10000)
            {
                Toast.MakeText(this, "Vui lòng Nhập Giá trị Số trên 10000.", ToastLength.Short).Show();
                return;
            }
            
            int postion = spNV.SelectedItemPosition;
            txtMaNV.Text = list_NV[postion].MaNV;
            int postionk = spKH.SelectedItemPosition;
            txtMaKH.Text = list_KH[postionk].MaKH;

            PhieuThu ab = new PhieuThu();
            ab.MaPhieuThu = txtMaPT.Text;
            ab.MaKH = txtMaKH.Text;
            ab.MaNV = txtMaNV.Text;
            ab.NgayPhieuThu = txtNgayThu.Text;
            ab.SoTienThu =float.Parse(txtSotien.Text);
            try
            {


                if (Edit == false)
                {
                    IList<PhieuThu> list = db.GetAllPhieuThu();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaPhieuThu == txtMaPT.Text)
                        {
                            Toast.MakeText(this, "Mã Phiếu Thu Đã tồn tại.", ToastLength.Short).Show();
                            return;
                        }
                    }
                    db.AddNewPhieuThu(ab);
                    Toast.MakeText(this, "Thêm Phiếu Thu Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdatePhieuThu(ab);
                    Toast.MakeText(this, "Cập nhật thành công.", ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var PTActivity = new Intent(this, typeof(PhieuThuActivity));
                StartActivity(PTActivity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}