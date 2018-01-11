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
using Android.Database;

namespace QLSX_2.Resources.Data
{
    class PhieuXuatTPAdapter : BaseAdapter<PhieuXuatTP>
    {
        IList<PhieuXuatTP> ArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuXuatTPAdapter(Context context,  IList<PhieuXuatTP> results)
        {
            this.activity = context;
            ArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override PhieuXuatTP this[int position]
        {
            get
            {
                return ArrayList[position];
            }
        }

        public override int Count
        {
            get
            {
                return ArrayList.Count;
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PXTP, null);
                holder = new ViewHolder();

                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_MaPhieuXTP);
                holder.txtNgay = convertView.FindViewById<TextView>(Resource.Id.lr_NgayPhieuXuatTP);
                holder.txtKH = convertView.FindViewById<TextView>(Resource.Id.lr_MaKH_PXTP);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXTP);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = ArrayList[poldel].MaPhieuXuatTP;
                        string fname = ArrayList[poldel].NgayXuatTP;
                        PhieuXuatThanhPhamDbHelper _db = new PhieuXuatThanhPhamDbHelper(activity, MainActivity.dataname);
                        ICursor pnnl = _db.WritableDatabase.RawQuery("select * from PHIEUXUATTHANHPHAMCT where MaPhieuXuatTP='" + id + "'", null);
                        if (pnnl != null)
                        {
                            Toast.MakeText(activity, "Yều cầu xóa hết các Phiếu Xuất Thành Phẩm Chi Tiết trước", ToastLength.Short).Show();
                            return;
                        }
                        else
                        {
                            _db.DeletePhieuXuatTP(id);
                            ArrayList.RemoveAt(poldel);
                            NotifyDataSetChanged();

                            Toast.MakeText(activity, "Phieu Xuat Thanh Pham Deleted Successfully", ToastLength.Short).Show();
                        }
                       
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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXTP);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtName.Text = ArrayList[position].MaPhieuXuatTP;
            holder.txtNgay.Text = ArrayList[position].NgayXuatTP;
            holder.txtKH.Text = ArrayList[position].MaKH;      

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

        public IList<PhieuXuatTP> GetAllData()
        {
            return ArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtName { get; set; }
            public TextView txtNgay { get; set; }
            public TextView txtKH { get; set; }
         
        }
    }
}