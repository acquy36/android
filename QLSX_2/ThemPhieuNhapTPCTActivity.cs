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
    [Activity(Label = "ThemPhieuNhapTPCTActivity")]
    public class ThemPhieuNhapTPCTActivity : Activity
    {
        EditText txtSL, txtDG;
        Spinner spPhieu, spTP;
        Button btnSave, btnHuy;
        bool Edit = false;
        IList<ThanhPham> list_TP;
        IList<PhieuXuatNLDaSoChe> list_Phieu;
        protected override void OnCreate(Bundle savedInstanceState)
        {           
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemPhieuNhapTPchitiet);
            ActionBar.SetDisplayShowTitleEnabled(false);
            spPhieu = FindViewById<Spinner>(Resource.Id.spMaPhieuXuatNLDaSC);
            spTP = FindViewById<Spinner>(Resource.Id.spMaTP_PNTP);
            txtSL = FindViewById<EditText>(Resource.Id.SLNhapTP_edit);
            txtDG = FindViewById<EditText>(Resource.Id.DGNhapTP_edit);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuPhieuNhapTP);
            btnHuy = FindViewById<Button>(Resource.Id.btnHuyPhieuNhapTP);

            //Spinner Thành Phẩm
            ThanhPhamDBHelper nv = new ThanhPhamDBHelper(this, MainActivity.dataname);
            list_TP = nv.GetAllThanhPham();
            List<string> lsNV = new List<string>();
            lsNV.Add("Thành Phẩm");
            for (int i = 0; i < list_TP.Count; i++)
            {
                lsNV.Add(list_TP[i].MaTP);
            }
            ArrayAdapter arrNV = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsNV);
            arrNV.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTP.Adapter = arrNV;

            //Spinner Phiếu
            PhieuXuatNLDaSCDbHelper phieu = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            list_Phieu = phieu.GetAllPhieuXuatNLDaSC();
            List<string> lsphieu = new List<string>();
            lsphieu.Add("Phiếu Xuất NL Đã SC");
            for (int i = 0; i < list_Phieu.Count; i++)
            {
                lsphieu.Add(list_Phieu[i].MaPhieuXuatNLDaSC);
            }
            ArrayAdapter arrphieu = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsphieu);
            arrphieu.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spPhieu.Adapter = arrphieu;

            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            var editId = Intent.GetStringExtra("MaPhieuXuatNLDaSC") ?? string.Empty;
            var editTP = Intent.GetStringExtra("MaTP") ?? string.Empty;
            if (editId.Trim().Length > 0)
            {
                int positionP = arrphieu.GetPosition(editId);
                spPhieu.SetSelection(positionP);
                spPhieu.Enabled = false;
                int positionTP = arrphieu.GetPosition(editTP);
                spTP.SetSelection(positionTP);
                spTP.Enabled = false;
                LoadDataForEdit(editId, editTP);              
                Edit = true;
            }
        }
        private void LoadDataForEdit(string editId, string editTP)
        {
            PhieuNhapTPChiTietDbHelper db = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getPhieuNhapTPCTById(editId, editTP);
            if (cData.MoveToFirst())
            {
                txtSL.Text = cData.GetString(cData.GetColumnIndex("SLNhapTP"));
                txtDG.Text = cData.GetString(cData.GetColumnIndex("DGNhapTP"));
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {

            PhieuNhapTPChiTietDbHelper db = new PhieuNhapTPChiTietDbHelper(this,MainActivity.dataname);
            if (spTP.SelectedItem.ToString().Trim().Equals("Thành Phẩm"))
            {
                Toast.MakeText(this, "Vui lòng chọn Thành Phẩm", ToastLength.Long).Show();
                return;
            }
            if (spPhieu.SelectedItem.ToString().Trim().Equals("Phiếu Xuất NL Đã SC"))
            {
                Toast.MakeText(this, "Vui lòng chọn Phiếu Xuất NL Đã SC", ToastLength.Long).Show();
                return;
            }
            if (txtSL.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter So luong.", ToastLength.Short).Show();
                return;
            }

            if (txtDG.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Don gia.", ToastLength.Short).Show();
                return;
            }
            
            PhieuNhapTPCT ab = new PhieuNhapTPCT();
            ab.MaPhieuXuatNLDaSC = spPhieu.SelectedItem.ToString();
            ab.MaTP = spTP.SelectedItem.ToString();
            ab.SLNhapTP = float.Parse(txtSL.Text);
            ab.DGNhapTP = float.Parse( txtDG.Text);

            try
            {
                if (Edit == false)
                {
                    IList<PhieuNhapTPCT> list = db.GetAllPhieuNhapTPCT();
                    for(int i = 0; i < list.Count; i++)
                    {
                        if(list[i].MaPhieuXuatNLDaSC==spPhieu.SelectedItem.ToString() && list[i].MaTP==spTP.SelectedItem.ToString())
                        {
                        Toast.MakeText(this, "Mã Phiếu Nhập Đã tồn tại.", ToastLength.Short).Show();
                        return;   
                        }
                    }
                        db.AddNewPhieuNhapTPCT(ab);
                        Toast.MakeText(this, "Thêm Phiếu Nhập Thành Phẩm Chi Tiết Thành Công.", ToastLength.Short).Show();
                                        
                }
                else
                {
                    db.UpdatePhieuNhapTPCT(ab);
                    Toast.MakeText(this, "Cập Nhật thành công.", ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var NVActivity = new Intent(this, typeof(PhieuNhapTPChiTietActivity));
                StartActivity(NVActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      
    }
}