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
    [Activity(Label = "ThemPhieuXuatTPActivity")]
    public class ThemPhieuXuatTPActivity : Activity
    {
        EditText txtMaPhieu, txtSL, txtDG;
        TextView txtNgay, txtMaKH, txtMaNV;
        Spinner spKH, spNV, spTP;       
        ListView lv;
        Button btnSave, btnCancel, btnChonNgay, btnTPChiTiet, btnHuy, btnLuu;
        IList<PhieuXuatTPChiTiet> listItsms ;
        IList<KhachHang> list_KH;
        IList<NhanVien> list_NV;
        bool Edit = false;
        bool themCTtruoc = false;
        IList<ThanhPham> list_TP;
        Dialog dialog;
        ArrayAdapter arrTP, arrNV, arrKH;
        string dataname = MainActivity.dataname;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThemPhieuXuatTP);
            ActionBar.SetDisplayShowTitleEnabled(false);
            txtMaPhieu = FindViewById<EditText>(Resource.Id.MaPhieuXuatTP_edit_text);
            txtNgay = FindViewById<TextView>(Resource.Id.date_PhieuxuatTP);
            txtMaKH = FindViewById<TextView>(Resource.Id.tvMaKH_PXTP);
            txtMaNV = FindViewById<TextView>(Resource.Id.tvMaNV_PXTP);
            spNV = FindViewById<Spinner>(Resource.Id.PXTP_spNV);
            spKH = FindViewById<Spinner>(Resource.Id.PXTP_spKH);
            btnChonNgay = FindViewById<Button>(Resource.Id.btnChon_PhieuxuatTP);
            btnSave = FindViewById<Button>(Resource.Id.btnLuuPhieuxuatTP);
            btnCancel = FindViewById<Button>(Resource.Id.btnHuyPhieuxuatTP);
            btnTPChiTiet = FindViewById<Button>(Resource.Id.btnThemDSThanhPham);
            lv = FindViewById<ListView>(Resource.Id.lvPhieuxuatTPChitiet);

            btnChonNgay.Click += DateSelect_OnClick;
            btnTPChiTiet.Click += ThemThanhPhamCT;

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

            //Spinner Khách Hàng
            KhachHangDbHelper kh = new KhachHangDbHelper(this, MainActivity.dataname);
            list_KH = kh.GetAllKhachHang();
            List<string> lsKH = new List<string>();
            lsKH.Add("Khách Hàng");
            for (int i = 0; i < list_KH.Count; i++)
            {
                lsKH.Add(list_KH[i].TenKH);
            }
            arrKH = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, lsKH);
            arrKH.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spKH.Adapter = arrKH;

            // Thêm hoặc cập nhật
            btnSave.Click += buttonSave_Click;
            
            var editId = Intent.GetStringExtra("MaPhieuXuatTP") ?? string.Empty;

            if (editId.Trim().Length > 0)
            {
                txtMaPhieu.Text = editId;
                LoadDataForEdit(editId);
                LoadPhieuXuatTPChiTiet();
                txtMaPhieu.Enabled = false;
                Edit = true;
            }
            btnCancel.Click += delegate
            {
                if ((Edit == false)&& txtMaPhieu.Text.Trim() != null)
                {
                    PhieuXuatThanhPhamDbHelper db = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
                    ICursor c = db.getPhieuXuatTPById(txtMaPhieu.Text);
                    if (c != null)
                    {
                        db.DeletePhieuXuatTP(txtMaPhieu.Text);
                    }
                }
                Intent intent = new Intent(this, typeof(PhieuXuatTPActivity));
                StartActivity(intent);
            };

        }
        private void LoadThanhPham()
        {
            
            dialog = new Dialog(this);
            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            View post = inflater.Inflate(Resource.Layout.ThemPhieuXuatTPChiTiet, null);

             txtSL = post.FindViewById<EditText>(Resource.Id.SLXuatTP_edit_text);
             txtDG = post.FindViewById<EditText>(Resource.Id.DGXuatTP_edit_text);
             spTP = post.FindViewById<Spinner>(Resource.Id.spMaTP_PXTPCT);
             btnLuu = post.FindViewById<Button>(Resource.Id.LuuPXTPCT);
             btnHuy = post.FindViewById<Button>(Resource.Id.HuyPXTPCT);
            dialog.SetContentView(post);
            //Spinner Thanh Pham
            ThanhPhamDBHelper db = new ThanhPhamDBHelper(this, MainActivity.dataname);
            list_TP = db.GetAllThanhPham();
            List<string> ls_TP = new List<string>();
            ls_TP.Add("Thành Phẩm");
            for (int i = 0; i < list_TP.Count; i++)
            {
                ls_TP.Add(list_TP[i].MaTP);
            }
            arrTP = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ls_TP);
            arrTP.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTP.Adapter = arrTP;
            dialog.Show();
            btnHuy.Click += delegate
            {
                dialog.Cancel();
            };
        }
        private void LoadDataForEdit(string editId)
        {
            PhieuXuatThanhPhamDbHelper db = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
            ICursor cData = db.getPhieuXuatTPById(editId);
            if (cData.MoveToFirst())
            {
                txtNgay.Text = cData.GetString(cData.GetColumnIndex("NgayPhieuXuatTP"));
                txtMaNV.Text = cData.GetString(cData.GetColumnIndex("MaNV"));
                txtMaKH.Text = cData.GetString(cData.GetColumnIndex("MaKH"));

            }
            //NhanVienDbHelper n = new NhanVienDbHelper(this, MainActivity.dataname);
            //var NV = n.getNhanVienById(txtMaNV.Text);
            //int positionNV = arrNV.GetPosition(NV.GetString(NV.GetColumnIndex("TenNV")));
            //spNV.SetSelection(positionNV);

            //KhachHangDbHelper k = new KhachHangDbHelper(this, MainActivity.dataname);
            //var KH = k.getKhachHangById(txtMaKH.Text);
            //var tenKh = KH.GetString(KH.GetColumnIndex("TenKH"));
            //int positionKH = arrKH.GetPosition(tenKh);
            //spKH.SetSelection(positionKH);
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            PhieuXuatThanhPhamDbHelper db = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
            if (txtMaPhieu.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ma Phieu Xuat Thanh Pham.", ToastLength.Short).Show();
                return;
            }
            if (txtNgay.Text.Trim().Length < 1)
            {
                Toast.MakeText(this, "Enter Ngay Xuat Thanh Pham.", ToastLength.Short).Show();
                return;
            }          
            if (spNV.SelectedItem.ToString().Trim().Equals("Nhân Viên"))
            {
                Toast.MakeText(this, "Vui lòng chọn Nhân Viên", ToastLength.Long).Show();
                return;
            }
            if (spKH.SelectedItem.ToString().Trim().Equals("Khách Hàng"))
            {
                Toast.MakeText(this, "Vui lòng chọn Khách Hàng", ToastLength.Long).Show();
                return;
            }
            PhieuXuatTP ab = new PhieuXuatTP();
            ab.MaPhieuXuatTP = txtMaPhieu.Text;           
            ab.MaNV = txtMaNV.Text;
            ab.MaKH = txtMaKH.Text;
            ab.NgayXuatTP = txtNgay.Text;
            try
            {
                if (Edit == false)
                {
                    if(themCTtruoc==false)
                    {
                        IList<PhieuXuatTP> list = db.GetAllPhieuXuatTP();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].MaPhieuXuatTP == txtMaPhieu.Text)
                            {
                                Toast.MakeText(this, "Mã Phiếu Xuất Đã Tồn Tại.", ToastLength.Short).Show();
                                return;
                            }
                        }
                        db.AddNewPhieuXuatTP(ab);
                    }   
                    Toast.MakeText(this, "Thêm Phiếu Xuất Thành Phẩm Thành Công.", ToastLength.Short).Show();              
                }
                else
                {
                    db.UpdatePhieuXuatTP(ab);
                    Toast.MakeText(this, "Cập Nhật thành công.", ToastLength.Short).Show();
                    
                }

                Finish();
                //Go to main activity after save/edit
                var PTActivity = new Intent(this, typeof(PhieuXuatTPActivity));
                StartActivity(PTActivity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private void DateSelect_OnClick(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtNgay.Text = time.ToShortDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
        private void LoadPhieuXuatTPChiTiet()
        {
            PhieuXuatTPChiTietDbHelper dbVals = new PhieuXuatTPChiTietDbHelper(this, dataname);
            listItsms = dbVals.GetPhieuXuatTPCTBySearchPhieu(txtMaPhieu.Text);
            lv.Adapter = new PhieuXuatTPChiTietAdapter(this, listItsms);
            lv.ItemLongClick += lv_ItemLongClick;
        }
        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuXuatTPChiTiet o = listItsms[e.Position];
            LoadThanhPham();
            int positionTP = arrTP.GetPosition(o.MaTP);
            spTP.SetSelection(positionTP);
            txtSL.Text = o.SLXuatTP.ToString();
            txtDG.Text = o.DGXuatTP.ToString();
            btnLuu.Click += delegate
            {
                PhieuXuatTPChiTietDbHelper h = new PhieuXuatTPChiTietDbHelper(this, MainActivity.dataname);
                if (spTP.SelectedItem.ToString().Trim().Equals("Thành Phẩm"))
                {
                    Toast.MakeText(this, "Vui lòng chọn Thành Phẩm", ToastLength.Long).Show();
                    return;
                }
                PhieuNhapTPChiTietDbHelper nhap = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
                float SoN = nhap.getSoLuongNhapByTp(spTP.SelectedItem.ToString());
                float SoX = h.getSoLuongXuatByTp(spTP.SelectedItem.ToString());
                float S = SoN - SoX;
                if (S <= 0)
                {
                    Toast.MakeText(this, "Đã Hết Thành Phẩm" + spTP.SelectedItem.ToString(), ToastLength.Short).Show();
                    return;
                }

                if (float.Parse(txtSL.Text.Trim()) > S)
                {
                    Toast.MakeText(this, "Enter Số Lượng Nhỏ Hơn " + S, ToastLength.Short).Show();
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

                PhieuXuatTPChiTiet p = new PhieuXuatTPChiTiet();
                p.MaPhieuXuatTP = txtMaPhieu.Text;
                p.MaTP = spTP.SelectedItem.ToString();
                p.SLXuatTP = float.Parse(txtSL.Text);
                p.DGXuatTP = float.Parse(txtDG.Text);

                h.UpdatePhieuXuatTPCT(p);
                LoadPhieuXuatTPChiTiet();
                dialog.Cancel();
                Toast.MakeText(this, "Cập nhật Thành phẩm Thành Công.", ToastLength.Short).Show();
            };
        }
        private void ThemThanhPhamCT(object sender, EventArgs e)
        {
            if (Edit == false)
            {
            
                if (txtMaPhieu.Text.Trim().Length < 1)
                {
                    Toast.MakeText(this, "Enter Ma Phieu Xuat Thanh Pham.", ToastLength.Short).Show();
                    return;
                }
            PhieuXuatThanhPhamDbHelper a = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
            IList<PhieuXuatTP> list = a.GetAllPhieuXuatTP();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MaPhieuXuatTP == txtMaPhieu.Text)
                {
                    Toast.MakeText(this, "Mã Phiếu Xuất Đã Tồn Tại.", ToastLength.Short).Show();
                    return;
                }
                else
                {
                    PhieuXuatTP ab = new PhieuXuatTP();
                    ab.MaPhieuXuatTP = txtMaPhieu.Text;
                    ab.MaNV = txtMaNV.Text;
                    ab.MaKH = txtMaKH.Text;
                    ab.NgayXuatTP = txtNgay.Text;
                    a.AddNewPhieuXuatTP(ab);
                    themCTtruoc = true;
                }

            }
            }
                
            LoadThanhPham();
            btnLuu.Click += delegate
            {
                PhieuXuatTPChiTietDbHelper db = new PhieuXuatTPChiTietDbHelper(this, dataname);
                if (spTP.SelectedItem.ToString().Trim().Equals("Thành Phẩm"))
                {
                    Toast.MakeText(this, "Vui lòng chọn Thành Phẩm", ToastLength.Long).Show();
                    return;
                }
                PhieuNhapTPChiTietDbHelper nhap = new PhieuNhapTPChiTietDbHelper(this,dataname);
                float SoN = nhap.getSoLuongNhapByTp(spTP.SelectedItem.ToString());
                float SoX = db.getSoLuongXuatByTp(spTP.SelectedItem.ToString());
                var S = SoN - SoX;
                if (S <= 0)
                {
                    Toast.MakeText(this, "Đã Hết Thành Phẩm" + spTP.SelectedItem.ToString(), ToastLength.Short).Show();
                    return;
                }

                if (S< float.Parse(txtSL.Text))
                {
                    Toast.MakeText(this, "Enter Số Lượng Nhỏ Hơn " + S + ".", ToastLength.Short).Show();
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

                PhieuXuatTPChiTiet o = new PhieuXuatTPChiTiet();
                o.MaPhieuXuatTP = txtMaPhieu.Text;
                o.MaTP = spTP.SelectedItem.ToString();
                o.SLXuatTP = float.Parse(txtSL.Text);
                o.DGXuatTP = float.Parse(txtDG.Text);
                db.AddNewPhieuXuatTPCT(o);
                LoadPhieuXuatTPChiTiet();
                dialog.Cancel();
                Toast.MakeText(this, "Thêm Thành Phẩm Thành Công.", ToastLength.Short).Show();
            };
        }
    }
}

