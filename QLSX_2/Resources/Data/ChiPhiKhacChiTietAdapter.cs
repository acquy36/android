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

namespace QLSX_2.Resources.Data
{
    class ChiPhiKhacChiTietAdapter : BaseAdapter<ChiPhiKhacChiTiet>
    {
        IList<ChiPhiKhacChiTiet> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        private string DataName;
        public ChiPhiKhacChiTietAdapter(Context context, IList<ChiPhiKhacChiTiet> results, string DataName)
        {
            this.DataName = DataName;
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override ChiPhiKhacChiTiet this[int position]
        {
            get
            {
                return contactListArrayList[position];
            }
        }

        public override int Count
        {
            get
            {
                return contactListArrayList.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView btnDelete;
            ViewHolder holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_CPKChiTiet, null);
                holder = new ViewHolder();

                holder.txtNgay = convertView.FindViewById<TextView>(Resource.Id.lr_NgayChiPhiKhac);
                holder.txtGia = convertView.FindViewById<TextView>(Resource.Id.lr_GiaChiPhiKhac);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btndelete_CPKCT);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaChiPhiKhac.ToString();
                        string fname = contactListArrayList[poldel].NgayChiPhiKhac;

                        contactListArrayList.RemoveAt(poldel);

                        DeleteSelectedContact(id,fname);
                        NotifyDataSetChanged();

                        Toast.MakeText(activity, "Chi Phí Khác Chi tiết Deleted Successfully", ToastLength.Short).Show();
                    });
                    confirm.SetButton2("Cancel", (s, ev) =>
                    {

                    });

                    confirm.Show();
                };

                convertView.Tag = holder;
                btnDelete.Tag = position;
            }
            else
            {
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btndelete_CPKCT);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtNgay.Text = contactListArrayList[position].NgayChiPhiKhac;
            holder.txtGia.Text = contactListArrayList[position].GiaChiPhiKhac.ToString();

            if (position % 2 == 0)
            {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector);
            }
            else
            {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector_alternate);
            }

            return convertView;
        }

        public IList<ChiPhiKhacChiTiet> GetAllData()
        {
            return contactListArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtNgay { get; set; }
            public TextView txtGia { get; set; }
        }

        private void DeleteSelectedContact(string contactId, string ngay)
        {
            ChiPhiKhacCTDbHelper _db = new ChiPhiKhacCTDbHelper(activity, DataName);
             _db.DeleteCPKChiTiet(contactId,ngay); 
        }
    }
}