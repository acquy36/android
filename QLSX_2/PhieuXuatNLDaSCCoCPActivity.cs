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
    [Activity(Label = "PhieuXuatNLDaSCCoCPActivity")]
    public class PhieuXuatNLDaSCCoCPActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<PhieuXuatNLDaSCCoCP> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PhieuXuatNLDaSCCP);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);

            btnAdd = FindViewById<Button>(Resource.Id.PXNLDaSCCP_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.PXNLDaSCCP_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.PXNLDaSCCP_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.PXNLDaSCCP_listView);
            PXNLDaSCCoCPDefaut();
            LoadContactsInList();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemPXNLDaSCCoCPActivity));
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
        private void PXNLDaSCCoCPDefaut()
        {
            PhieuXuatNLDaSCCPDbHelper db = new PhieuXuatNLDaSCCPDbHelper(this, MainActivity.dataname);
            db.AddNewPhieuXuatNLDaSCCP(new PhieuXuatNLDaSCCoCP { MaPhieuXuatNLDaSC = "PX001", MaChiPhiKhac = "CPK02", SLXuatNLDaSCChiPhi = 5, DGXuatNLDaSCChiPhi = 15000 });
        }

        private void LoadContactsInList()
        {
            PhieuXuatNLDaSCCPDbHelper dbVals = new PhieuXuatNLDaSCCPDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllPhieuXuatNLDaSCCP();
            }
            else
            {
                listItsms = dbVals.GetPhieuXuatNLDaSCCPBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new PhieuXuatNLDaSCCoCPAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuXuatNLDaSCCoCP o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemPXNLDaSCCoCPActivity));
            activityAddEdit.PutExtra("MaPhieuXuatNLDaSC", o.MaPhieuXuatNLDaSC);
            activityAddEdit.PutExtra("MaChiPhiKhac", o.MaChiPhiKhac);
            StartActivity(activityAddEdit);
        }
    }
}