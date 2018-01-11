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
    class PhieuXuatNLDaSCAdapter : BaseAdapter<PhieuXuatNLDaSoChe>
    {
        IList<PhieuXuatNLDaSoChe> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuXuatNLDaSCAdapter(Context context, IList<PhieuXuatNLDaSoChe> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override PhieuXuatNLDaSoChe this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PXNLDaSC, null);
                holder = new ViewHolder();

                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_MaPhieuXuatNLDaSC);
                holder.txtNgay = convertView.FindViewById<TextView>(Resource.Id.lr_NgayXuatNLDaSC);
                holder.txtNhanvien = convertView.FindViewById<TextView>(Resource.Id.lr_MaNV_PXNLDaSC);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXNLDaSC);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaPhieuXuatNLDaSC.ToString();
                        string fname = contactListArrayList[poldel].NgayPhieuXuatNLDaSC;

                        contactListArrayList.RemoveAt(poldel);

                        DeleteSelectedContact(id);
                        NotifyDataSetChanged();

                        Toast.MakeText(activity, "Contact Deeletd Successfully", ToastLength.Short).Show();
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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXNLDaSC);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtName.Text = contactListArrayList[position].MaPhieuXuatNLDaSC;
            holder.txtNgay.Text = contactListArrayList[position].NgayPhieuXuatNLDaSC;
            holder.txtNhanvien.Text = contactListArrayList[position].MaNV;

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

        public IList<PhieuXuatNLDaSoChe> GetAllData()
        {
            return contactListArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtName { get; set; }
            public TextView txtNgay { get; set; }
            public TextView txtNhanvien { get; set; }
        }

        private void DeleteSelectedContact(string contactId)
        {
            PhieuXuatNLDaSCDbHelper _db = new PhieuXuatNLDaSCDbHelper(activity, MainActivity.dataname);
            ICursor pnnl = _db.WritableDatabase.RawQuery("select * from PHIEUXUATNGUYENLIEUDASOCHECT where MaPhieuXuatNLDaSC='" + contactId + "'", null);
            if (pnnl != null)
            {
                Toast.MakeText(activity, "Yều cầu xóa hết các Phiếu Xuất Nguyên Liệu Đã Sơ Chế Chi Tiết trước", ToastLength.Short).Show();
                return;
            }
            else
            {
                _db.DeletePhieuXuatNLDaSC(contactId);
            }
        }
    }
}