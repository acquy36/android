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

namespace QLSX_2
{
    [Activity(Label = "BaoCaoXuatTPActivity")]
    public class BaoCaoXuatTPActivity : Activity
    {
        TextView txtSLTon, txtTong;
        Spinner spTP;
        Button btnLoad;
        ListView lv;
        IList<ThanhPham> list_TP;
        protected override void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.BaoCaoXuatTP);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnLoad = FindViewById<Button>(Resource.Id.btnLoad_BCXuatTP);
            txtSLTon = FindViewById<TextView>(Resource.Id.txtSLTonTP);
            txtTong = FindViewById<TextView>(Resource.Id.txtTongXuatTP);
            spTP = FindViewById<Spinner>(Resource.Id.sp_TPBCXTP);
            lv = FindViewById<ListView>(Resource.Id.BCXuatTP_listView);
            //Spinner Thành Phẩm
            ThanhPhamDBHelper db = new ThanhPhamDBHelper(this, MainActivity.dataname);
            list_TP = db.GetAllThanhPham();
            List<string> ls_NL = new List<string>();
            for (int i = 0; i < list_TP.Count; i++)
            {
                ls_NL.Add(list_TP[i].MaTP);
            }
            ArrayAdapter arrNL = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ls_NL);
            arrNL.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spTP.Adapter = arrNL;
            //Spinner Nguyên liệu          
            btnLoad.Click += delegate
            {
                LoadBC();
            };
        }

        private void LoadBC()
        {
            float SLTon, SoN, SoX;
            PhieuNhapTPChiTietDbHelper db = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
            PhieuXuatTPChiTietDbHelper d = new PhieuXuatTPChiTietDbHelper(this, MainActivity.dataname);
            SoN = db.getSoLuongNhapByTp(spTP.SelectedItem.ToString());
            SoX = d.getSoLuongXuatByTp(spTP.SelectedItem.ToString());
            txtTong.Text = SoX.ToString();
            SLTon = SoN - SoX;
            txtSLTon.Text = SLTon.ToString();
            IList<PhieuXuatTPChiTiet> list;
            list = d.GetPhieuXuatTPCTBySearchName(spTP.SelectedItem.ToString());
            lv.Adapter = new BaoCaoTPXuatAdapter(this, list);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Intent inte = new Intent(this, typeof(MainActivity));
                StartActivity(inte);
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}