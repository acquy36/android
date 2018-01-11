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
    [Activity(Label = "PhieuXuatNLDaSCActivity")]
    public class PhieuXuatNLDaSCActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<PhieuXuatNLDaSoChe> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PhieuXuatNLDaSoChe);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.PXNLDaSC_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.PXNLDaSC_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.PXNLDaSC_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.PXNLDaSC_listView);
            PXNLDaSCDefaut();
            LoadContactsInList();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemPXNLDaSCActivity));
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
        private void PXNLDaSCDefaut()
        {
            PhieuXuatNLDaSCDbHelper db = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            db.AddNewPhieuXuatNLDaSC(new PhieuXuatNLDaSoChe { MaPhieuXuatNLDaSC = "PX001", MaNV = "NV03", NgayPhieuXuatNLDaSC = "2/12/2016" });
            db.AddNewPhieuXuatNLDaSC(new PhieuXuatNLDaSoChe { MaPhieuXuatNLDaSC = "PX002", MaNV = "NV03", NgayPhieuXuatNLDaSC = "15/12/2016" });

        }

        private void LoadContactsInList()
        {
            PhieuXuatNLDaSCDbHelper dbVals = new PhieuXuatNLDaSCDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllPhieuXuatNLDaSC();
            }
            else
            {
                listItsms = dbVals.GetPhieuXuatNLDaSCBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new PhieuXuatNLDaSCAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuXuatNLDaSoChe o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemPXNLDaSCActivity));
            activityAddEdit.PutExtra("MaPhieuXuatNLDaSC", o.MaPhieuXuatNLDaSC);
            activityAddEdit.PutExtra("NgayPhieuXuatNLDaSC", o.NgayPhieuXuatNLDaSC);
            StartActivity(activityAddEdit);
        }
    }
}