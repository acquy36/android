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
    [Activity(Label = "NhanVienActivity")]
    public class NhanVienActivity : Activity
    {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<NhanVien> listItsms = null;
            
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.NhanVien);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.NhanVien_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.NhanVien_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.NhanVien_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.NhanVien_listView);
            NhanVienDefaut();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(AddNVActivity));
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
            NhanVienDbHelper dbVals = new NhanVienDbHelper(this, MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = dbVals.GetAllNhanVien();
            }
            else
            {
                listItsms = dbVals.GetNhanVienBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new NhanVienAdapter(this, listItsms);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            NhanVien o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(AddNVActivity));
            activityAddEdit.PutExtra("MaNV", o.MaNV);
            activityAddEdit.PutExtra("TenNV", o.TenNV);
            StartActivity(activityAddEdit);
        }
        private void NhanVienDefaut()
        {
            NhanVienDbHelper db = new NhanVienDbHelper(this, MainActivity.dataname);
            db.AddNewNhanVien(new NhanVien { MaNV = "NV01", TenNV = "Lê Tiến Dũng", SoDTNV = "01234854754", DiachiNV = "47,30 tháng 4, Quận Ninh Kiều", EmailNV = "Dung47@gmail.com" });
            db.AddNewNhanVien(new NhanVien { MaNV = "NV02", TenNV = "Trần Quang Nguyên", SoDTNV = "0162478627", DiachiNV = "49, Mậu Thân, Quận Ninh Kiều", EmailNV = "Nguyen7@gmail.com" });
            db.AddNewNhanVien(new NhanVien { MaNV = "NV03", TenNV = "Lê Hòa Phát", SoDTNV = "0975213114", DiachiNV = "47,Phạm Hùng, Quận Cái Răng", EmailNV = "phat21@gmail.com" });

        }
    }
}