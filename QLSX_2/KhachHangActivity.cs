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
    [Activity(Label = "KhachHangActivity")]
    public class KhachHangActivity : Activity
    {      
            Button btnAdd, btnSearch;
            EditText txtSearch;
            ListView lv;
            IList<KhachHang> listItsms = null;
       
        protected override void OnCreate(Bundle savebundle)
        {
            base.OnCreate(savebundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.KhachHang);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetDisplayShowTitleEnabled(false);
            btnAdd = FindViewById<Button>(Resource.Id.KH_btnAdd);
            btnSearch = FindViewById<Button>(Resource.Id.KH_btnSearch);
            txtSearch = FindViewById<EditText>(Resource.Id.KH_txtSearch);
            lv = FindViewById<ListView>(Resource.Id.KH_listView);
          
            KhachHangDefaut();
            LoadContactsInList();
            btnAdd.Click += delegate
            {
                var activityAddEdit = new Intent(this, typeof(ThemKHActivity));
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
        private void LoadContactsInList()
        {
            KhachHangDbHelper db = new KhachHangDbHelper(this,MainActivity.dataname);
            if (txtSearch.Text.Trim().Length < 1)
            {
                listItsms = db.GetAllKhachHang();
            }
            else
            {
                listItsms = db.GetKhachHangBySearchName(txtSearch.Text.Trim());
            }

            lv.Adapter = new KHListBaseAdapter(this, listItsms, MainActivity.dataname);

            lv.ItemLongClick += lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            KhachHang o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(ThemKHActivity));
            activityAddEdit.PutExtra("MaKH", o.MaKH);
            activityAddEdit.PutExtra("TenKH", o.TenKH);
            StartActivity(activityAddEdit);
        }
        private void KhachHangDefaut()
        {
            KhachHangDbHelper db = new KhachHangDbHelper(this,MainActivity.dataname);
            db.AddNewKhachHang(new KhachHang { MaKH = "KH01", TenKH = "Phạm Tấn Phát",  Diachi = "12/8, Phạm Hùng, Quận 12", SoDT = "0969852369", Email = "phat12@gmail.com"});
            db.AddNewKhachHang(new KhachHang { MaKH = "KH02", TenKH = "Phạm Thanh Sang",  Diachi = "241, Phan Đình Phùng, Quận 1 ", SoDT = "0978216325", Email = "sang10@gmail.com" });
            db.AddNewKhachHang(new KhachHang { MaKH = "KH03", TenKH = "Nguyễn Văn Tiến",  Diachi = "55, 3 Tháng 2, Quận 7 ", SoDT = "01238522687", Email = "Tien71@gmail.com" });

        }

    }
}