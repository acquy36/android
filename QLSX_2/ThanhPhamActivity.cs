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
    [Activity(Label = "ThanhPhamActivity")]
    public class ThanhPhamActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<ThanhPham> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ThanhPham);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.TP_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.TP_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.TP_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.TP_listView);
            ThanhPhamDefaut();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemTPActivity));
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
            ThanhPhamDBHelper dbVals = new ThanhPhamDBHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllThanhPham();
            }
            else
            {
                listItsms = dbVals.GetThanhPhamBySearchName(txtSearch.Text.Trim());
            }

           
            lv.Adapter = new TPAdapter(this,listItsms, MainActivity.dataname);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ThanhPham o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemTPActivity));
            activityAddEdit.PutExtra("MaTP", o.MaTP);
            activityAddEdit.PutExtra("TenTP", o.TenTP);
            StartActivity(activityAddEdit);
        }
        private void ThanhPhamDefaut()
        {
            ThanhPhamDBHelper db = new ThanhPhamDBHelper(this, MainActivity.dataname);
            db.AddNewThanhPham(new ThanhPham { MaTP = "TP01", TenTP = "Thit Cá Đông Lạnh", DVTTP = "Gói 500g " });
            db.AddNewThanhPham(new ThanhPham { MaTP = "TP02", TenTP = "Thịt Cá Kho", DVTTP = "Hộp 200g " });
            db.AddNewThanhPham(new ThanhPham { MaTP = "TP03", TenTP = "Cá phi lê", DVTTP = "Gói 500g " });

        }
    }
}