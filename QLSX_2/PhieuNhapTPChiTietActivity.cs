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
    [Activity(Label = "PhieuNhapTPChiTietActivity")]
    public class PhieuNhapTPChiTietActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<PhieuNhapTPCT> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PhieuNhapTPChiTiet);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.PNTPCT_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.PNTPCT_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.PNTPCT_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.PNTPCT_listView);
            PhieuNhapTPCTDefaut();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemPhieuNhapTPCTActivity));
                StartActivity(activityAddEdit);
            };

            btnSearch.Click += delegate
            {
                LoadContactsInList();
            };
            LoadContactsInList();
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
        private void LoadContactsInList()
        {
            PhieuNhapTPChiTietDbHelper dbVals = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllPhieuNhapTPCT();
            }
            else
            {
                listItsms = dbVals.GetPhieuNhapTPCTBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new PhieuNhapTPCTAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuNhapTPCT o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemPhieuNhapTPCTActivity));
            activityAddEdit.PutExtra("MaPhieuXuatNLDaSC", o.MaPhieuXuatNLDaSC);
            activityAddEdit.PutExtra("MaTP", o.MaTP);
            StartActivity(activityAddEdit);
        }

        private void PhieuNhapTPCTDefaut()
        {
            PhieuNhapTPChiTietDbHelper db = new PhieuNhapTPChiTietDbHelper(this, MainActivity.dataname);
            db.AddNewPhieuNhapTPCT(new PhieuNhapTPCT { MaPhieuXuatNLDaSC = "PX001", MaTP = "TP01", SLNhapTP = 60, DGNhapTP = 10000 });
        }
    }
}