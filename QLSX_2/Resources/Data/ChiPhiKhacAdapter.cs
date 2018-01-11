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
    class ChiPhiKhacAdapter : BaseAdapter<ChiPhiKhac>
    {
        IList<ChiPhiKhac> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        private string DataName;
        public ChiPhiKhacAdapter(Context context, IList<ChiPhiKhac> results,string DataName)
        {
            this.DataName = DataName;
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override ChiPhiKhac this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_CPK, null);
                holder = new ViewHolder();

                holder.txtMa = convertView.FindViewById<TextView>(Resource.Id.lr_MaChiPhiKhac);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_TenChiPhiKhac);
                holder.txtDonVi = convertView.FindViewById<TextView>(Resource.Id.lr_DVTChiPhiKhac);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btndelete_ChiPhiKhac);

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
                        string fname = contactListArrayList[poldel].TenChiPhiKhac;

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btndelete_ChiPhiKhac);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtMa.Text = contactListArrayList[position].MaChiPhiKhac;
            holder.txtName.Text = contactListArrayList[position].TenChiPhiKhac;
            holder.txtDonVi.Text = contactListArrayList[position].DVTChiPhiKhac;

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

        public IList<ChiPhiKhac> GetAllData()
        {
            return contactListArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtMa{ get; set; }
            public TextView txtName { get; set; }
            public TextView txtDonVi { get; set; }

        }

        private void DeleteSelectedContact(string contactId)
        {
            ChiPhiKhacDbHelper _db = new ChiPhiKhacDbHelper(activity, DataName);
            ChiPhiKhacCTDbHelper d = new ChiPhiKhacCTDbHelper(activity, DataName);
            ICursor pnnl = d.WritableDatabase.RawQuery("select * from CHIPHIKHACCT where MaChiPhiKhac='" + contactId + "'", null);
            if (pnnl != null)
            {
                Toast.MakeText(activity, "Yều cầu xóa hết các Chi Phí Khác Chi Tiết trước", ToastLength.Short).Show();
                return;
            }
            else { _db.DeleteChiPhiKhac(contactId); }
        }
    }
}