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
    [Activity(Label = "ChiPhiKhacActivity")]
    public class ChiPhiKhacActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<ChiPhiKhac> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ChiPhiKhac);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.CPK_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.ChiPhiKhac_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.ChiPhiKhac_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.ChiPhiKhac_listView);
            ChiPhiKhacDefaut();
            LoadContactsInList();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemChiPhiKhacActivity));
                StartActivity(activityAddEdit);

            };

            btnSearch.Click += delegate
            {
                LoadContactsInList();
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
        private void ChiPhiKhacDefaut()
        {
            ChiPhiKhacDbHelper db = new ChiPhiKhacDbHelper(this,MainActivity.dataname);
            db.AddNewChiPhiKhac(new ChiPhiKhac { MaChiPhiKhac = "CPK01", TenChiPhiKhac = "Tiền Điện", DVTChiPhiKhac = "KWh" });
            db.AddNewChiPhiKhac(new ChiPhiKhac { MaChiPhiKhac = "CPK02", TenChiPhiKhac = "Giấy", DVTChiPhiKhac = "Thùng" });

        }

        private void LoadContactsInList()
        {
            ChiPhiKhacDbHelper dbVals = new ChiPhiKhacDbHelper(this,MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllChiPhiKhac();
            }
            else
            {
                listItsms = dbVals.GetChiPhiKhacBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new ChiPhiKhacAdapter(this, listItsms,MainActivity.dataname);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ChiPhiKhac o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemChiPhiKhacActivity));
            activityAddEdit.PutExtra("MaChiPhiKhac", o.MaChiPhiKhac);
            activityAddEdit.PutExtra("TenChiPhiKhac", o.TenChiPhiKhac);
            StartActivity(activityAddEdit);
        }
    }
}