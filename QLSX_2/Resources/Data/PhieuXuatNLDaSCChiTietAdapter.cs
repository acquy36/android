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
    class PhieuXuatNLDaSCChiTietAdapter : BaseAdapter<PhieuXuatNLDaSCChiTiet>
    {
        IList<PhieuXuatNLDaSCChiTiet> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuXuatNLDaSCChiTietAdapter(Context context, IList<PhieuXuatNLDaSCChiTiet> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override PhieuXuatNLDaSCChiTiet this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PXNLDaSCCT, null);
                holder = new ViewHolder();

                holder.txtMa = convertView.FindViewById<TextView>(Resource.Id.lr_MaNLDaSC_PXNLDaSCCT);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_TenNLDaSC_PXNLDaSCCT);
                holder.txtSL = convertView.FindViewById<TextView>(Resource.Id.lr_SLXuatNLDaSC);
                holder.txtDG = convertView.FindViewById<TextView>(Resource.Id.lr_DGXuatNLDaSC);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn_PXNLDaSCCT);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaPhieuXuatNLDaSC;
                        string fname = contactListArrayList[poldel].MaNLDaSC;

                        contactListArrayList.RemoveAt(poldel);

                        DeleteSelectedContact(id, fname);
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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn_PXNLDaSCCT);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }
            NguyenLieuDaSCDbHelper _db = new NguyenLieuDaSCDbHelper(activity, MainActivity.dataname);
            ICursor c = _db.getNLDaSCById(contactListArrayList[position].MaNLDaSC);
            if (c.MoveToFirst())
            {
                holder.txtName.Text = c.GetString(c.GetColumnIndex("TenNLDaSC"));
            }
            holder.txtMa.Text = contactListArrayList[position].MaNLDaSC;
            holder.txtSL.Text = contactListArrayList[position].SLXuatNLSoChe.ToString();
            holder.txtDG.Text = contactListArrayList[position].DGXuatNLSoChe.ToString();


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

        public IList<PhieuXuatNLDaSCChiTiet> GetDataTP()
        {
            return contactListArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtMa { get; set; }
            public TextView txtName { get; set; }
            public TextView txtSL { get; set; }
            public TextView txtDG { get; set; }


        }
        private void DeleteSelectedContact(string Id, string tp)
        {
            PhieuXuatNLDaSCChiTietDbHelper _db = new PhieuXuatNLDaSCChiTietDbHelper(activity, MainActivity.dataname);
            _db.DeletePhieuXuatNLDaSCChiTiet(Id, tp);
        }
    }
}