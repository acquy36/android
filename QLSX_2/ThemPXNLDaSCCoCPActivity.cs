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
    [Activity(Label = "ThemPXNLDaSCCoCPActivity")]
    public class ThemPXNLDaSCCoCPActivity : Activity
    {
        EditText txtSL, txtDG;
        Spinner spPhieu, spCPK;
        Button btnSave, btnHuy;
        private bool Edit = false;
        IList<PhieuXuatNLDaSoChe> list_Phieu;
        IList<ChiPhiKhac> list_CPK;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemPXNLDaSCCoCP);
            ActionBar.SetDisplayShowTitleEnabled(false);
            spPhieu = FindViewById<Spinner>(Resource.Id.PXNLDaSCCoCP_spPhieuXNLDaSC);
            spCPK = FindViewById<Spinner>(Resource.Id.PXNLDaSCCoCP_spPhieuXNLDaSC);
            txtSL = FindViewById<EditText>(Resource.Id.SLXuatNLDaSCCP_edit_text);
            txtDG = FindViewById<EditText>(Resource.Id.DGXuatNLDaSCCP_edit_text);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuPhieuXNLDaSCCP);
            btnHuy = FindViewById<Button>(Resource.Id.btnHuyPhieuXNLDaSCCP);

            //Spinner Phiếu Xuất Nguyên Liệu Đã Sơ Chế
            PhieuXuatNLDaSCDbHelper nv = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            list_Phieu = nv.GetAllPhieuXuatNLDaSC();
            List<string> lsPhieu = new List<string>();
            lsPhieu.Add("Phiếu");
            for (int i = 0; i < list_Phieu.Count; i++)
            {
                lsPhieu.Add(list_Phieu[i].MaPhieuXuatNLDaSC);
            }
            ArrayAdapter arr = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsPhieu);
            arr.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spPhieu.Adapter = arr;

            //Spinner Chi Phí Khác
            ChiPhiKhacDbHelper cp = new ChiPhiKhacDbHelper(this, MainActivity.dataname);
            list_CPK = cp.GetAllChiPhiKhac();
            List<string> lsCPK = new List<string>();
            lsCPK.Add("Chi phí");
            for (int i = 0; i < list_CPK.Count; i++)
            {
                lsCPK.Add(list_CPK[i].MaChiPhiKhac);
            }
            ArrayAdapter arrcp = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsCPK);
            arrcp.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spCPK.Adapter = arrcp;
            // Thêm Cập Nhật
            btnSave.Click += buttonSave_Click;
            btnHuy.Click += delegate {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
            var editPhieu = Intent.GetStringExtra("MaPhieuXuatNLDaSC") ?? string.Empty;
            var editCPK = Intent.GetStringExtra("MaChiPhiKhac") ?? string.Empty;
            if (editPhieu.Trim().Length > 0)
            {
                LoadDataForEdit(editPhieu, editCPK);
                Edit = true;
            }
        }

        private void LoadDataForEdit(string editId, string editTP)
        {
            PhieuXuatNLDaSCCPDbHelper db = new PhieuXuatNLDaSCCPDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getPhieuXuatNLDaSCCPById(editId, editTP);
            if (cData.MoveToFirst())
            {
                spPhieu.SetSelection(cData.GetColumnIndex("MaPhieuXuatNLDaSC"));
                spCPK.SetSelection(cData.GetColumnIndex("MaChiPhiKhac"));                
                txtSL.Text = cData.GetString(cData.GetColumnIndex("SLXuatNLDaSCChiPhi"));
                txtDG.Text = cData.GetString(cData.GetColumnIndex("DGXuatNLDaSCChiPhi"));
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            PhieuXuatNLDaSCCPDbHelper db = new PhieuXuatNLDaSCCPDbHelper(this, MainActivity.dataname);
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

            PhieuXuatNLDaSCCoCP ab = new PhieuXuatNLDaSCCoCP();
            ab.MaPhieuXuatNLDaSC = spPhieu.SelectedItem.ToString();
            ab.MaChiPhiKhac = spCPK.SelectedItem.ToString();
            ab.SLXuatNLDaSCChiPhi = float.Parse(txtSL.Text);
            ab.DGXuatNLDaSCChiPhi = float.Parse(txtDG.Text);

            try
            {

                if (Edit == false)
                {
                    IList<PhieuXuatNLDaSCCoCP> list = db.GetAllPhieuXuatNLDaSCCP();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].MaPhieuXuatNLDaSC == spPhieu.SelectedItem.ToString() && list[i].MaChiPhiKhac== spCPK.SelectedItem.ToString())
                        {
                            Toast.MakeText(this, "Phiếu Xuất Đã Tồn Tại.", ToastLength.Short).Show();
                            return;
                        }
                    }
                    db.AddNewPhieuXuatNLDaSCCP(ab);
                    Toast.MakeText(this, "Thêm Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdatePhieuXuatNLDaSCCP(ab);
                    Toast.MakeText(this, "Cập nhật thành công.", ToastLength.Short).Show();
                }

                Finish();
                //Go to main activity after save/edit
                var NVActivity = new Intent(this, typeof(PhieuXuatNLDaSCCoCPActivity));
                StartActivity(NVActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}