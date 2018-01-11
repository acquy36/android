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
    [Activity(Label = "NguyenLieuDaSCActivity")]
    public class NguyenLieuDaSCActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<NguyenLieuDaSoche> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.NguyenLieuDaSoChe);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.NLDaSC_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.NLDaSC_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.NLDaSC_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.NLDaSC_listView);
            NLDaSoCheDefaut();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemNLDaSCActivity));
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
            NguyenLieuDaSCDbHelper dbVals = new NguyenLieuDaSCDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllNLDaSC();
            }
            else
            {
                listItsms = dbVals.GetNLDaSCBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new NguyenLieuDaSCAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            NguyenLieuDaSoche o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemNLDaSCActivity));
            activityAddEdit.PutExtra("MaNLDaSC", o.MaNLDaSC);
            activityAddEdit.PutExtra("TenNLDaSC", o.TenNLDaSC);
            StartActivity(activityAddEdit);
        }

        private void NLDaSoCheDefaut()
        {
            NguyenLieuDaSCDbHelper db = new NguyenLieuDaSCDbHelper(this, MainActivity.dataname);
            db.AddNewNLDaSC(new NguyenLieuDaSoche { MaNLDaSC = "NLD01", TenNLDaSC = "Cá Phi lê", DVTNLDaSC = "gram" });
            db.AddNewNLDaSC(new NguyenLieuDaSoche { MaNLDaSC = "NLD02", TenNLDaSC = "Cá Lốc Xương", DVTNLDaSC = "gram" });

        }
    }
}