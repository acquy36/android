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
    public partial class TPAdapter : BaseAdapter<ThanhPham>
    {
        IList<ThanhPham> TPArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        private string DataName;
        public TPAdapter(Context context,IList<ThanhPham> results, string DataName)
        {
            this.DataName = DataName;
            this.activity = context;
            TPArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count
        {
            get { return TPArrayList.Count; }
        }

        public override ThanhPham this[int position]
        {
            get
            {
                return TPArrayList[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView btnDelete;
            ThanhPhamHolder holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_TP, null);
                holder = new ThanhPhamHolder();
                holder.txtMaTP = convertView.FindViewById<TextView>(Resource.Id.lr_MaTP);
                holder.txtTenTP = convertView.FindViewById<TextView>(Resource.Id.lr_TenTP);
                holder.txtDVTTP = convertView.FindViewById<TextView>(Resource.Id.lr_DVTTP);
               
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtnTP);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = TPArrayList[poldel].MaTP.ToString();
                        string fname = TPArrayList[poldel].TenTP;

                        TPArrayList.RemoveAt(poldel);

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtnTP);
                holder = convertView.Tag as ThanhPhamHolder;
                btnDelete.Tag = position;
            }
            holder.txtMaTP.Text = TPArrayList[position].MaTP;
            holder.txtTenTP.Text = TPArrayList[position].TenTP;
            holder.txtDVTTP.Text = TPArrayList[position].DVTTP;
           

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

        public IList<ThanhPham> GetAllDataTP()
        {
            return TPArrayList;
        }

        public class ThanhPhamHolder : Java.Lang.Object
        {
            public TextView txtMaTP { get; set; }
            public TextView txtTenTP { get; set; }
            public TextView txtDVTTP { get; set; }

        }

        private void DeleteSelectedContact(string contactId)
        {
            ThanhPhamDBHelper _db = new ThanhPhamDBHelper(activity, DataName);
            _db.DeleteThanhPham(contactId);
        }
    }
}