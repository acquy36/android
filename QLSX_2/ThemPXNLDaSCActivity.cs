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
    [Activity(Label = "ThemPXNLDaSCActivity")]
    public class ThemPXNLDaSCActivity : Activity
    {
        EditText txtMaPhieu, txtSL, txtDG;
        TextView txtNgay, txtMaNV;
        Spinner spNV, spNL;
        ListView lv;
        Button btnSave, btnCancel, btnChonNgay, btnNLDaSCChiTiet, btnHuy, btnLuu;
        IList<PhieuXuatNLDaSCChiTiet> listItsms;
        IList<NhanVien> list_NV;
        IList<NguyenLieuDaSoche> list_NL;
        bool Edit = false;
        bool themtruoc=false;
        Dialog dialog;
        ArrayAdapter arrNL, arrNV;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemPhieuXNLDaSoChe);

            txtMaPhieu = FindViewById<EditText>(Resource.Id.MaPhieuXuatNLDaSC_edit_text);
            txtNgay = FindViewById<TextView>(Resource.Id.date_PXNLDaSC);
            txtMaNV = FindViewById<TextView>(Resource.Id.tvMaNV_PXNL);
            spNV = FindViewById<Spinner>(Resource.Id.PXNLDaSC_spNV);
            btnChonNgay = FindViewById<Button>(Resource.Id.btnChon_PXNLDaSC);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuPhieuxuatNLDaSC);
            btnCancel = FindViewById<Button>(Resource.Id.btnHuyPhieuxuatNLDaSC);
            btnNLDaSCChiTiet = FindViewById<Button>(Resource.Id.btnThemDSNguyenLieuDaSC);
            lv = FindViewById<ListView>(Resource.Id.lsPhieuXuatNLDaSCChitiet);

            btnChonNgay.Click += DateSelect_OnClick;
            btnNLDaSCChiTiet.Click += ThemNLDaSCChiTiet;

            //Spinner Nhân Viên
            NhanVienDbHelper nv = new NhanVienDbHelper(this, MainActivity.dataname);
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
            btnCancel.Click += delegate
            {
                //if (Edit == false)
                //{
                //    PhieuXuatNLDaSCDbHelper db = new PhieuXuatNLDaSCDbHelper(this);
                //    ICursor c = db.getPhieuXuatNLDaSCById(txtMaPhieu.Text);
                //    if (c == null)
                //    {
                //        db.DeletePhieuXuatNLDaSC(txtMaPhieu.Text);
                //    }
                //}
                Intent intent = new Intent(this, typeof(PhieuXuatNLDaSCActivity));
                StartActivity(intent);
            };
            var editId = Intent.GetStringExtra("MaPhieuXuatNLDaSC") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaPhieu.Text = editId;
                LoadDataForEdit(editId);
                LoadPhieuXuatNLDaSCChiTiet();
                txtMaPhieu.Enabled = false;
                Edit = true;
            }
        }

        private void LoadDataForEdit(string editId)
        {
            PhieuXuatNLDaSCDbHelper db = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getPhieuXuatNLDaSCById(editId);
            if (cData.MoveToFirst())
            {
                txtNgay.Text = cData.GetString(cData.GetColumnIndex("NgayPhieuXuatNLDaSC"));
                txtMaNV.Text = cData.GetString(cData.GetColumnIndex("MaNV"));
                ////NhanVienDbHelper n = new NhanVienDbHelper(this, MainActivity.dataname);
                ////var NV = n.getNhanVienById(txtMaNV.Text);

                ////var ten = NV.GetString(NV.GetColumnIndex("TenNV"));
                ////int positionNV = arrNV.GetPosition(ten);
                ////spNV.SetSelection(positionNV);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            PhieuXuatNLDaSCDbHelper db = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            if (txtMaPhieu.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ma Phiếu Xuất Nguyên liệu Đã Sơ chế.", ToastLength.Short).Show();
                return;
            }
            if (txtNgay.Text == "Ngày Xuất")
            {
                Toast.MakeText(this, "Enter Ngày Xuất Nguyên liệu Đã Sơ chế.", ToastLength.Short).Show();
                return;
            }
            if (spNV.SelectedItem.ToString().Trim().Equals("Nhân Viên"))
            {
                Toast.MakeText(this, "Vui lòng chọn Nhân Viên", ToastLength.Long).Show();
                return;
            }

            PhieuXuatNLDaSoChe ab = new PhieuXuatNLDaSoChe();
            ab.MaPhieuXuatNLDaSC = txtMaPhieu.Text;
            ab.MaNV = spNV.SelectedItem.ToString();
            ab.NgayPhieuXuatNLDaSC = txtNgay.Text;
            try
            {
                if (Edit == false)
                {             
                    if (themtruoc == false)
                    {
                        IList<PhieuXuatNLDaSoChe> list = db.GetAllPhieuXuatNLDaSC();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].MaPhieuXuatNLDaSC == txtMaPhieu.Text)
                            {
                                Toast.MakeText(this, "Mã Phiếu Đã Tồn Tại.", ToastLength.Short).Show();
                                return;
                            }
                        }
                        db.AddNewPhieuXuatNLDaSC(ab);
                    }
                    else { db.UpdatePhieuXuatNLDaSC(ab); }

                    Toast.MakeText(this, "Thêm Phiếu Xuất Nguyên Liệu Đã Sơ Chế Thành Công.", ToastLength.Short).Show();
                }
                else
                {
                    db.UpdatePhieuXuatNLDaSC(ab);
                    Toast.MakeText(this, "Cập Nhật thành công.", ToastLength.Short).Show();

                }

                Finish();
                //Go to main activity after save/edit
                var PTActivity = new Intent(this, typeof(PhieuXuatNLDaSCActivity));
                StartActivity(PTActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void LoadPhieuXuatNLDaSCChiTiet()
        {
            PhieuXuatNLDaSCChiTietDbHelper dbVals = new PhieuXuatNLDaSCChiTietDbHelper(this, MainActivity.dataname);
            listItsms = dbVals.GetPhieuXuatNLDaSCChiTietByName(txtMaPhieu.Text);
            lv.Adapter = new PhieuXuatNLDaSCChiTietAdapter(this, listItsms);
            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuXuatNLDaSCChiTietDbHelper db = new PhieuXuatNLDaSCChiTietDbHelper(this, MainActivity.dataname);
            PhieuXuatNLDaSCChiTiet o = listItsms[e.Position];
            LoadNguyenlieuDaSC();
            int positionTP = arrNL.GetPosition(o.MaNLDaSC);
            spNL.SetSelection(positionTP);
            txtSL.Text = o.SLXuatNLSoChe.ToString();
            txtDG.Text = o.DGXuatNLSoChe.ToString();
            btnLuu.Click += delegate
            {
                if (spNL.SelectedItem.ToString().Trim().Equals("Nguyên Liệu"))
                {
                    Toast.MakeText(this, "Vui lòng chọn Nguyên Liệu", ToastLength.Long).Show();
                    return;
                }

                if (txtSL.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Số Lượng.", ToastLength.Short).Show();
                    return;
                }

                if (txtDG.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Đơn Giá.", ToastLength.Short).Show();
                    return;
                }
                if (float.Parse(txtDG.Text.Trim()) < 10000)
                {
                    Toast.MakeText(this, "Enter Đơn Giá Trên 10000.", ToastLength.Short).Show();
                    return;
                }

                PhieuXuatNLDaSCChiTiet p = new PhieuXuatNLDaSCChiTiet();
                p.MaPhieuXuatNLDaSC = txtMaPhieu.Text;
                p.MaNLDaSC = spNL.SelectedItem.ToString();
                p.SLXuatNLSoChe = float.Parse(txtSL.Text);
                p.DGXuatNLSoChe = float.Parse(txtDG.Text);
                db.UpdatePhieuXuatNLDaSCChiTiet(p);
                LoadPhieuXuatNLDaSCChiTiet();
                dialog.Cancel();
                Toast.MakeText(this, "Cập nhật Nguyên Liệu Đã Sơ Chế Thành Công.", ToastLength.Short).Show();
            };
        }

        private void LoadNguyenlieuDaSC()
        {
            dialog = new Dialog(this);
            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View post = inflater.Inflate(Resource.Layout.ThemPhieuXuatNLDaSCChiTiet, null);

            txtSL = post.FindViewById<EditText>(Resource.Id.SLXuatNLSoche_edit_text);
            txtDG = post.FindViewById<EditText>(Resource.Id.DGXuatNLSoche_edit_text);
            spNL = post.FindViewById<Spinner>(Resource.Id.spMaNLDaSC_PXNLDaSCCT);
            btnLuu = post.FindViewById<Button>(Resource.Id.LuuPXNLDaSCCT);
            btnHuy = post.FindViewById<Button>(Resource.Id.HuyPXNLDaSCCT);
            dialog.SetContentView(post);
            //Spinner Nguyên liệu
            NguyenLieuDaSCDbHelper db = new NguyenLieuDaSCDbHelper(this, MainActivity.dataname);
            list_NL = db.GetAllNLDaSC();
            List<string> ls_NL = new List<string>();
            ls_NL.Add("Nguyên Liệu Đã Sơ Chế");
            for (int i = 0; i < list_NL.Count; i++)
            {
                ls_NL.Add(list_NL[i].MaNLDaSC);
            }
            arrNL = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ls_NL);
            arrNL.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spNL.Adapter = arrNL;
            dialog.Show();
            btnHuy.Click += delegate
            {
                dialog.Cancel();
            };
        }

        private void ThemNLDaSCChiTiet(object sender, EventArgs e)
        {
            if (Edit == false)
            {
                if (txtMaPhieu.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Ma Phieu.", ToastLength.Short).Show();
                    return;
                }
                PhieuXuatNLDaSCDbHelper a = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
                PhieuXuatNLDaSoChe ab = new PhieuXuatNLDaSoChe();
                IList<PhieuXuatNLDaSoChe> list = a.GetAllPhieuXuatNLDaSC();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].MaPhieuXuatNLDaSC == txtMaPhieu.Text)
                    {
                        Toast.MakeText(this, "Mã Phiếu Xuất Đã Tồn Tại.", ToastLength.Short).Show();
                        return;
                    }
                }
                ab.MaPhieuXuatNLDaSC = txtMaPhieu.Text;
                ab.MaNV = spNV.SelectedItem.ToString();
                ab.NgayPhieuXuatNLDaSC = txtNgay.Text;
                a.AddNewPhieuXuatNLDaSC(ab);
                themtruoc = true;
            }            
            LoadNguyenlieuDaSC();
            btnLuu.Click += delegate
            {
                PhieuXuatNLDaSCChiTietDbHelper db = new PhieuXuatNLDaSCChiTietDbHelper(this, MainActivity.dataname);
                if (spNL.SelectedItem.ToString().Trim().Equals("Nguyên Liệu"))
                {
                    Toast.MakeText(this, "Vui lòng chọn Nguyên Liệu", ToastLength.Long).Show();
                    return;
                }

                if (txtSL.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Số Lượng.", ToastLength.Short).Show();
                    return;
                }

                if (txtDG.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Đơn Giá.", ToastLength.Short).Show();
                    return;
                }
                if (float.Parse(txtDG.Text.Trim()) < 10000)
                {
                    Toast.MakeText(this, "Enter Đơn Giá Trên 10000.", ToastLength.Short).Show();
                    return;
                }

                PhieuXuatNLDaSCChiTiet p = new PhieuXuatNLDaSCChiTiet();
                p.MaPhieuXuatNLDaSC = txtMaPhieu.Text;
                p.MaNLDaSC = spNL.SelectedItem.ToString();
                p.SLXuatNLSoChe = float.Parse(txtSL.Text);
                p.DGXuatNLSoChe = float.Parse(txtDG.Text);
                db.AddNewPhieuXuatNLDaSCChiTiet(p);
                LoadPhieuXuatNLDaSCChiTiet();
                dialog.Cancel();
                Toast.MakeText(this, "Thêm Nguyên Liệu Đã Sơ Chế Thành Công.", ToastLength.Short).Show();
            };
        }

        private void DateSelect_OnClick(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtNgay.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}