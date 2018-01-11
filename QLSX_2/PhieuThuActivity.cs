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
using System.Globalization;

namespace QLSX_2
{
    [Activity(Label = "PhieuThuActivity")]
    public class PhieuThuActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<PhieuThu> listItsmsPT = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PhieuThu);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);

            btnAdd = FindViewById<Button>(Resource.Id.PhieuThu_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.PhieuThu_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.Phieuthu_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.PhieuThu_listView);
            PhieuThudefault();
            LoadPhieuThu();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemPhieuThuActivity));
                StartActivity(activityAddEdit);

            };

            btnSearch.Click += delegate
            {
                LoadPhieuThu();
            };
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
        private void LoadPhieuThu()
        {
            PhieuThuDbHelper dbVals = new PhieuThuDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsmsPT = dbVals.GetAllPhieuThu();
            }
            else
            {
                listItsmsPT = dbVals.GetPhieuThuBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new PhieuThuAdapter(this,listItsmsPT);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuThu o = listItsmsPT[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemPhieuThuActivity));
            activityAddEdit.PutExtra("MaPhieuThu", o.MaPhieuThu);
            activityAddEdit.PutExtra("NgayPhieuThu", o.NgayPhieuThu);
            StartActivity(activityAddEdit);
        }
        private void PhieuThudefault()
        {
            PhieuThuDbHelper db = new PhieuThuDbHelper(this, MainActivity.dataname);
            db.AddNewPhieuThu(new PhieuThu { MaPhieuThu = "MPT01", MaKH = "KH01", MaNV = "NV02", NgayPhieuThu = "24/01/2017", SoTienThu = 1000000 });
            db.AddNewPhieuThu(new PhieuThu { MaPhieuThu = "MPT02", MaKH = "KH01", MaNV = "NV01", NgayPhieuThu = "02/2/2017", SoTienThu = 5000000 });
            db.AddNewPhieuThu(new PhieuThu { MaPhieuThu = "MPT03", MaKH = "KH03", MaNV = "NV03", NgayPhieuThu = "15/3/2017", SoTienThu = 3000000 });

        }
    }
}