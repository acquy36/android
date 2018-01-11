using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using System;
using QLSX_2.Resources.Data;
using QLSX.Resources.Model;

namespace QLSX_2
{
    [Activity(Label = "MainActivity")]

    public class MainActivity : Activity
    {
        Button btnKH, btnNL, btnTP, btnPhieuNhap, btnPhieuThu, btnPhieuXuat, btnNV, btnGT, btnBC, btnCPK;

        public static string dataname;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            ActionBar.SetDisplayShowTitleEnabled(false);

            btnKH = (Button)FindViewById(Resource.Id.button_KH);
            btnNV = (Button)FindViewById(Resource.Id.button_NV);
            btnGT = (Button)FindViewById(Resource.Id.button_GT);
            btnTP = (Button)FindViewById(Resource.Id.button_TP);
            btnNL = (Button)FindViewById(Resource.Id.button_NL);
            btnCPK = FindViewById<Button>(Resource.Id.button_CPK);
            btnBC = FindViewById<Button>(Resource.Id.button_BC);
            btnPhieuNhap = (Button)FindViewById(Resource.Id.button_PNhap);
            btnPhieuXuat = (Button)FindViewById(Resource.Id.button_PXuat);
            btnPhieuThu = (Button)FindViewById(Resource.Id.button_Pthu);
            btnKH.Click += delegate {
                Intent intent = new Intent(this, typeof(KhachHangActivity));
                StartActivity(intent);

            };
            btnCPK.Click += delegate {
                Intent intent = new Intent(this, typeof(ChiPhiKhacActivity));
                StartActivity(intent);

            };
            btnBC.Click += delegate {
                Dialog dialog = new Dialog(this);
                LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
                View fost = inflater.Inflate(Resource.Layout.layoutBaoCao, null);
                Button btnBCNhapTP = fost.FindViewById<Button>(Resource.Id.button_BCNhapTP);
                Button btnBCXuatTP = fost.FindViewById<Button>(Resource.Id.button_BCXuatTP);
                dialog.SetContentView(fost);
                dialog.Show();
                btnBCNhapTP.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(BaoCaoTPActivity));
                    StartActivity(intent);
                };
                btnBCXuatTP.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(BaoCaoXuatTPActivity));
                    StartActivity(intent);
                };

            };
            btnNV.Click += delegate
            { 

                Intent intent = new Intent(this, typeof(NhanVienActivity));
                StartActivity(intent);
            };
            btnTP.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ThanhPhamActivity));
                StartActivity(intent);
            };
            btnPhieuThu.Click += delegate
            {
                Intent intent = new Intent(this, typeof(PhieuThuActivity));
                StartActivity(intent);
            };
            btnGT.Click += delegate
            {
                Intent intent = new Intent(this, typeof(GioithieuActivity));
                StartActivity(intent);
                
            };
            btnPhieuNhap.Click += delegate
            {
                    Intent intent = new Intent(this, typeof(PhieuNhapTPChiTietActivity));
                    StartActivity(intent);

            };
            btnPhieuXuat.Click += delegate
            {
                Dialog dialog = new Dialog(this);
                LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
                View fost = inflater.Inflate(Resource.Layout.layoutPhieuXuat, null);
                Button btnPXNLDaSC = fost.FindViewById<Button>(Resource.Id.button_PXNLDaSC);
                Button btnPXNLDaSCCP = fost.FindViewById<Button>(Resource.Id.button_NPXNLDaSCCP);
                Button btnPhieuXuatTP = fost.FindViewById<Button>(Resource.Id.button_PXTP);
                dialog.SetContentView(fost);
                dialog.Show();
                btnPhieuXuatTP.Click += delegate
                {
                    Intent intent = new Intent(this, typeof(PhieuXuatTPActivity));
                    StartActivity(intent);
                };

                btnPXNLDaSC.Click += delegate
                {
                    Intent inten = new Intent(this, typeof(PhieuXuatNLDaSCActivity));
                    StartActivity(inten);
                };
                btnPXNLDaSCCP.Click += delegate
                {
                    Intent inten = new Intent(this, typeof(PhieuXuatNLDaSCCoCPActivity));
                    StartActivity(inten);
                };
            };
            btnNL.Click += delegate
            {               
                    Intent intent = new Intent(this, typeof(NguyenLieuDaSCActivity));
                    StartActivity(intent);
            };
        }
        

    }
}
