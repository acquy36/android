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
    [Activity(Label = "BaoCaoTPActivity")]
    public class BaoCaoTPActivity : Activity
    {
        TextView txtSLTon,txtTong;
        Spinner spTP;
        Button btnLoad;
        ListView lv;
        IList<ThanhPham> list_TP;
        PhieuNhapTPChiTietDbHelper p;
        PhieuXuatTPChiTietDbHelper d;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.BaoCaoTP);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnLoad = FindViewById<Button>(Resource.Id.btnLoad_BC);
            txtSLTon = FindViewById<TextView>(Resource.Id.txtSoLuongTP);
            txtTong = FindViewById<TextView>(Resource.Id.txtTongNhapTP);
            spTP = FindViewById<Spinner>(Resource.Id.sp_TPBC);
            lv = FindViewById<ListView>(Resource.Id.BCTP_listView);
            p = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
            d = new PhieuXuatTPChiTietDbHelper(this, MainActivity.dataname);
            //Spinner Nguyên liệu
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
            btnLoad.Click += LoadBC;
        }

        private void LoadBC(object sender, EventArgs e)
        {
            float SLTon, SoN, SoX;
            
            SoN = p.getSoLuongNhapByTp(spTP.SelectedItem.ToString());
            SoX = d.getSoLuongXuatByTp(spTP.SelectedItem.ToString());
            txtTong.Text = SoN.ToString();
            SLTon = SoN - SoX;
            txtSLTon.Text = SLTon.ToString();
            IList<PhieuNhapTPCT> list;
            list = p.GetPhieuNhapTPCTBySearchName(spTP.SelectedItem.ToString());
            lv.Adapter = new BaoCaoTPNhapAdapter(this, list);      
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