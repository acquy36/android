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
    [Activity(Label = "PhieuXuatTPActivity")]
    public class PhieuXuatTPActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<PhieuXuatTP> listItsms = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.PhieuXuatTP);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.PXTP_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.PXTP_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.PXTP_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.PXTP_listView);
            PhieuXuatTPDefaut();
            LoadContactsInList();
            btnAdd.Click += delegate
            {
                var activityAdd = new Intent(this, typeof(ThemPhieuXuatTPActivity));
                StartActivity(activityAdd);
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
        

        private void LoadContactsInList()
        {
            PhieuXuatThanhPhamDbHelper dbVals = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllPhieuXuatTP();
            }
            else
            {
                listItsms = dbVals.GetPhieuXuatTPBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new PhieuXuatTPAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            PhieuXuatTP o = listItsms[e.Position];
            var activityAddEdit = new Intent(this, typeof(ThemPhieuXuatTPActivity));
            activityAddEdit.PutExtra("MaPhieuXuatTP", o.MaPhieuXuatTP);
            activityAddEdit.PutExtra("NgayPhieuXuatTP", o.NgayXuatTP);
            StartActivity(activityAddEdit);
           
        }
        private void PhieuXuatTPDefaut()
        {
            PhieuXuatThanhPhamDbHelper db = new PhieuXuatThanhPhamDbHelper(this, MainActivity.dataname);
            db.AddNewPhieuXuatTP(new PhieuXuatTP { MaPhieuXuatTP = "P001", MaNV = "NV01", MaKH = "KH01", NgayXuatTP = "20/2/2016" });
            db.AddNewPhieuXuatTP(new PhieuXuatTP { MaPhieuXuatTP = "P002", MaNV = "NV03", MaKH = "KH03", NgayXuatTP = "15/3/2016" });
            db.AddNewPhieuXuatTP(new PhieuXuatTP { MaPhieuXuatTP = "P003", MaNV = "NV01", MaKH = "KH02", NgayXuatTP = "4/4/2016" });


        }
    }
}