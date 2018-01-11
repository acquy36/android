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
using QLSX.Resources.Model;
using QLSX_2.Resources.Data;
using Android.Database;

namespace QLSX_2
{
    [Activity(Label = "ThemChiPhiKhacActivity")]
    public class ThemChiPhiKhacActivity : Activity
    {
        EditText txtMaPhieu, txtTen, txtDVT, txtGia;
        TextView txtNgay;
        Spinner spNgay;
        ListView lv;
        Button btnSave, btnCancel, btnChiTiet, btnHuy, btnLuu, btnNgay;
        IList<ChiPhiKhacChiTiet> listItsms;
        IList<Ngay> list_Ngay;
        bool Edit = false;
        bool ThemCTtruoc = false;
        Dialog dialog;
        ArrayAdapter arrNgay;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemChiPhiKhac);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaPhieu = FindViewById<EditText>(Resource.Id.MaChiPhiKhac_edit_text);
            txtTen = FindViewById<EditText>(Resource.Id.TenCPK_edit_text);
            txtDVT = FindViewById<EditText>(Resource.Id.DVTCPK_edit_text);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuChiPhiKhac);
            btnCancel = FindViewById<Button>(Resource.Id.btnHuyChiPhiKhac);
            btnChiTiet = FindViewById<Button>(Resource.Id.btnThemNgayCPK);
            lv = FindViewById<ListView>(Resource.Id.lsNgayGia);

            btnChiTiet.Click += ThemChiTiet;
            btnSave.Click += buttonSave_Click;
            btnCancel.Click += delegate
            {
                if (Edit == false)
                {
                    ChiPhiKhacDbHelper db = new ChiPhiKhacDbHelper(this, MainActivity.dataname);
                    db.DeleteChiPhiKhac(txtMaPhieu.Text);
                }
                Intent intent = new Intent(this, typeof(ChiPhiKhacActivity));
                StartActivity(intent);
            };
            var editId = Intent.GetStringExtra("MaChiPhiKhac") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaPhieu.Text = editId;
                LoadDataForEdit(editId);
                LoadChiPhiKhacChiTiet();
                txtMaPhieu.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string editId)
        {
            ChiPhiKhacDbHelper db = new ChiPhiKhacDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getChiPhiKhacById(editId);
            if (cData.MoveToFirst())
            {
                txtMaPhieu.Text = cData.GetString(cData.GetColumnIndex("MaChiPhiKhac"));
                txtTen.Text = cData.GetString(cData.GetColumnIndex("TenChiPhiKhac"));
                txtDVT.Text = cData.GetString(cData.GetColumnIndex("DVTChiPhiKhac"));
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ChiPhiKhacDbHelper db = new ChiPhiKhacDbHelper(this, MainActivity.dataname);
            if (txtMaPhieu.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ma Chi Phí Khác.", ToastLength.Short).Show();
                return;
            }
            if (txtTen.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Tên Chi Phí Khác.", ToastLength.Short).Show();
                return;
            }
            if (txtTen.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Đơn Vị tính Chi Phí Khác.", ToastLength.Short).Show();
                return;
            }
           
            ChiPhiKhac ab = new ChiPhiKhac();
            ab.MaChiPhiKhac = txtMaPhieu.Text;
            ab.TenChiPhiKhac = txtTen.Text;
            ab.DVTChiPhiKhac = txtDVT.Text;
            try
            {
                if (Edit == false)
                {                   
                    if (ThemCTtruoc==false)
                    {
                        IList<ChiPhiKhac> list = db.GetAllChiPhiKhac();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].MaChiPhiKhac == txtMaPhieu.Text)
                            {
                                Toast.MakeText(this, "Mã Chi Phí Khác Đã Tồn Tại.", ToastLength.Short).Show();
                                return;
                            }
                        }
                        db.AddNewChiPhiKhac(ab);
                    }
                    else
                    {                       
                        db.UpdateChiPhiKhac(ab);
                    }

                    Toast.MakeText(this, "Thêm Chi Phí Khác Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdateChiPhiKhac(ab);
                    Toast.MakeText(this, "Cập Nhật Thành Công.", ToastLength.Short).Show();

                }

                Finish();
                //Go to main activity after save/edit
                var PTActivity = new Intent(this, typeof(ChiPhiKhacActivity));
                StartActivity(PTActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LoadNgay()
        {
            dialog = new Dialog(this);
            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View post = inflater.Inflate(Resource.Layout.ThemChiPhiKhacChiTiet, null);

            txtGia = post.FindViewById<EditText>(Resource.Id.GiaCPK_edit_text);
            txtNgay = post.FindViewById<TextView>(Resource.Id.txtNgayCPK);
            spNgay = post.FindViewById<Spinner>(Resource.Id.spMaNgay_CPKCT);
            btnNgay = post.FindViewById<Button>(Resource.Id.btnThemNgay_ChiPhiKhac);
            btnLuu = post.FindViewById<Button>(Resource.Id.LuuCPKCT);
            btnHuy = post.FindViewById<Button>(Resource.Id.HuyCPKCT);
            dialog.SetContentView(post);
            //Spinner Ngay
            NgayDbHelper db = new NgayDbHelper(this, MainActivity.dataname);
            list_Ngay = db.GetAllNgay();
            List<string> ls_TP = new List<string>();
            ls_TP.Add("Ngày");
            for (int i = 0; i < list_Ngay.Count; i++)
            {
                ls_TP.Add(list_Ngay[i].NgayChiPhiKhac);
            }
            arrNgay = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ls_TP);
            arrNgay.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spNgay.Adapter = arrNgay;
            dialog.Show();
            btnNgay.Click += delegate
             {
                 DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                 {
                     txtNgay.Text = time.ToShortDateString();
                 });
                 frag.Show(FragmentManager, DatePickerFragment.TAG);
                 Ngay d = new Ngay();
                 d.NgayChiPhiKhac = txtNgay.Text;

                 db.AddNewNgay(d);
                 Toast.MakeText(this, "Thêm Mới Ngày Chi Phí Khác Thành Công.", ToastLength.Long).Show();
             };
            btnHuy.Click += delegate
            {
                dialog.Cancel();
            };
        }

        private void LoadChiPhiKhacChiTiet()
        {
            ChiPhiKhacCTDbHelper dbVals = new ChiPhiKhacCTDbHelper(this, MainActivity.dataname);
            listItsms = dbVals.GetCPKChiTietBySearchName(txtMaPhieu.Text);
            lv.Adapter = new ChiPhiKhacChiTietAdapter(this, listItsms, MainActivity.dataname);
            lv.ItemLongClick += lv_ItemLongClick;

        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ChiPhiKhacChiTiet o = listItsms[e.Position];
            LoadNgay();
            int position = arrNgay.GetPosition(o.NgayChiPhiKhac);
            spNgay.SetSelection(position);
            txtGia.Text = o.GiaChiPhiKhac.ToString();
            btnLuu.Click += delegate
            {
                ChiPhiKhacCTDbHelper h = new ChiPhiKhacCTDbHelper(this, MainActivity.dataname);
                ChiPhiKhacChiTiet p = new ChiPhiKhacChiTiet();
                if (txtNgay.Text.Trim().Length < 1)
                {
                    if (spNgay.SelectedItem.ToString().Trim().Equals("Ngày"))
                    {
                        Toast.MakeText(this, "Vui lòng chọn Ngày Hoặc Thêm Mới Ngày.", ToastLength.Long).Show();
                        return;
                    }
                    else { p.NgayChiPhiKhac = spNgay.SelectedItem.ToString(); }
                }
                else
                {
                    p.NgayChiPhiKhac = txtNgay.Text;
                }
                             
                if (txtGia.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Giá Chi Phí.", ToastLength.Short).Show();
                    return;
                }
                if (float.Parse(txtGia.Text.Trim()) < 10000)
                {
                    Toast.MakeText(this, "Enter Giá Trên 10000.", ToastLength.Short).Show();
                    return;
                }
              
                p.MaChiPhiKhac = txtMaPhieu.Text;               
                p.GiaChiPhiKhac = float.Parse( txtGia.Text);
                h.UpdateCPKChiTiet(p);
                LoadChiPhiKhacChiTiet();
                dialog.Cancel();
                Toast.MakeText(this, "Cập Nhật Chi Tiết Thành Công.", ToastLength.Short).Show();
            };
        }
     
        private void ThemChiTiet(object sender, EventArgs e)
        {
            if(Edit == false)
            {
                if (txtMaPhieu.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Ma Chi Phi Khac.", ToastLength.Short).Show();
                    return;
                }
                ChiPhiKhacDbHelper a = new ChiPhiKhacDbHelper(this,MainActivity.dataname);
                ChiPhiKhac ab = new ChiPhiKhac();
                IList<ChiPhiKhac> list = a.GetAllChiPhiKhac();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].MaChiPhiKhac == txtMaPhieu.Text)
                    {
                        Toast.MakeText(this, "Mã Chi Phí Khác Đã Tồn Tại.", ToastLength.Short).Show();
                        return;
                    }
                }
                ab.MaChiPhiKhac = txtMaPhieu.Text;
                ab.TenChiPhiKhac = txtTen.Text;
                ab.DVTChiPhiKhac = txtDVT.Text;
                a.AddNewChiPhiKhac(ab);          
                ThemCTtruoc = true;           
            }
             LoadNgay();
            
                btnLuu.Click += delegate
                {
                    ChiPhiKhacCTDbHelper db = new ChiPhiKhacCTDbHelper(this, MainActivity.dataname);
                    ChiPhiKhacChiTiet o = new ChiPhiKhacChiTiet();
                    if (txtNgay.Text.Trim().Length < 1)
                    {
                        if (spNgay.SelectedItem.ToString().Trim().Equals("Ngày"))
                        {
                            Toast.MakeText(this, "Vui lòng chọn Ngày Hoặc Thêm Mới Ngày.", ToastLength.Long).Show();
                            return;
                        }
                        else { o.NgayChiPhiKhac = spNgay.SelectedItem.ToString(); }
                    }
                    else
                    {
                        o.NgayChiPhiKhac = txtNgay.Text;
                    }

                    if (txtGia.Text.Trim().Length < 1)
                    {
                        Toast.MakeText(this, "Enter Giá Chi Phí.", ToastLength.Short).Show();
                        return;
                    }

                    if (float.Parse(txtGia.Text.Trim()) < 10000)
                    {
                        Toast.MakeText(this, "Enter Đơn Giá Trên 10000.", ToastLength.Short).Show();
                        return;
                    }

                    o.MaChiPhiKhac = txtMaPhieu.Text;
                    o.GiaChiPhiKhac = float.Parse(txtGia.Text);
                    db.AddNewCPKChiTiet(o);
                    LoadChiPhiKhacChiTiet();
                    dialog.Cancel();
                    Toast.MakeText(this, "Thêm Giá Thành Công.", ToastLength.Short).Show();
                };
            
            
            
            
        }
    }
}