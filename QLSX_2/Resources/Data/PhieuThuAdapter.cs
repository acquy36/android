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
    class PhieuThuAdapter : BaseAdapter<PhieuThu>
    {
        IList<PhieuThu> PTArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuThuAdapter(Context context,  IList<PhieuThu> results)
        {
            this.activity = context;
            PTArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override int Count
        {
            get
            {
                 return PTArrayList.Count; 
            }
        }

        public override PhieuThu this[int position]
        {
            get
            {
                return PTArrayList[position];
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView btnDelete;
            Phieu holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_PT, null);
                holder = new Phieu();
                holder.txtMaPhieu = convertView.FindViewById<TextView>(Resource.Id.lr_MaPhieu);
                holder.txtNgay = convertView.FindViewById<TextView>(Resource.Id.lr_Ngay);
                holder.txtSotien = convertView.FindViewById<TextView>(Resource.Id.lr_Sotien);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtnPT);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = PTArrayList[poldel].MaPhieuThu.ToString();
                        

                        PTArrayList.RemoveAt(poldel);

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtnPT);
                holder = convertView.Tag as Phieu;
                btnDelete.Tag = position;
            }

            holder.txtMaPhieu.Text = PTArrayList[position].MaPhieuThu;
            holder.txtNgay.Text = PTArrayList[position].NgayPhieuThu;
            holder.txtSotien.Text = PTArrayList[position].SoTienThu.ToString();

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
        public IList<PhieuThu> GetAllDataPT()
        {
            return PTArrayList;
        }

        public class Phieu : Java.Lang.Object
        {
            public TextView txtMaPhieu { get; set; }
            public TextView txtNgay { get; set; }
            public TextView txtSotien{ get; set; }
           
        }

        private void DeleteSelectedContact(string contactId)
        {
            PhieuThuDbHelper _db = new PhieuThuDbHelper(activity, MainActivity.dataname);
            _db.DeletePhieuThu(contactId);
        }
    }
}