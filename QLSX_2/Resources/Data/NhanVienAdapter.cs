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
    public class NhanVienAdapter : BaseAdapter<NhanVien>
    {
        IList<NhanVien> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public NhanVienAdapter(Context context,
                                                IList<NhanVien> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count
        {
            get { return contactListArrayList.Count; }
        }

        public override NhanVien this[int position]
        {
            get
            {
                return contactListArrayList[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView btnDelete;
            ContactsViewHolder holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_NV, null);
                holder = new ContactsViewHolder();

                holder.txtFullName = convertView.FindViewById<TextView>(Resource.Id.lr_TenNV);
                holder.txtMobile = convertView.FindViewById<TextView>(Resource.Id.lr_SoDTNV);
                holder.txtEmail = convertView.FindViewById<TextView>(Resource.Id.lr_emailNV);
                holder.txtDiachi = convertView.FindViewById<TextView>(Resource.Id.lr_DiaChiNV);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_BtndeleteNV);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaNV.ToString();
                        string fname = contactListArrayList[poldel].TenNV;

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_BtndeleteNV);
                holder = convertView.Tag as ContactsViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtFullName.Text = contactListArrayList[position].TenNV;
            holder.txtMobile.Text = contactListArrayList[position].SoDTNV;
            holder.txtEmail.Text = contactListArrayList[position].EmailNV;
            holder.txtDiachi.Text = contactListArrayList[position].DiachiNV;

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

        public IList<NhanVien> GetAllDataKH()
        {
            return contactListArrayList;
        }

        public class ContactsViewHolder : Java.Lang.Object
        {
            public TextView txtFullName { get; set; }
            public TextView txtMobile { get; set; }
            public TextView txtEmail { get; set; }
            public TextView txtDiachi { get; set; }
        }

        private void DeleteSelectedContact(string contactId)
        {
            NhanVienDbHelper _db = new NhanVienDbHelper(activity, MainActivity.dataname);
            _db.DeleteNhanVien(contactId);
        }
    }
}