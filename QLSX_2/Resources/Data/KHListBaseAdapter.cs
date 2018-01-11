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
    [Activity(Label = "ContactListBaseAdapter")]
    public partial class KHListBaseAdapter : BaseAdapter<KhachHang>
    {
        IList<KhachHang> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        private string DataName;
        public KHListBaseAdapter(Context context, IList<KhachHang> results,string DataName)
        {
            this.DataName = DataName;
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count
        {
            get { return contactListArrayList.Count; }
        }

        public override KhachHang this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_contact_list, null);
                holder = new ContactsViewHolder();

                holder.txtFullName = convertView.FindViewById<TextView>(Resource.Id.lr_fullName);
                holder.txtMobile = convertView.FindViewById<TextView>(Resource.Id.lr_SoDT);
                holder.txtEmail = convertView.FindViewById<TextView>(Resource.Id.lr_email);
                holder.txtDiachi = convertView.FindViewById<TextView>(Resource.Id.lr_DiaChi);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaKH.ToString();
                        string fname = contactListArrayList[poldel].TenKH;

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);
                holder = convertView.Tag as ContactsViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtFullName.Text = contactListArrayList[position].TenKH;
            holder.txtMobile.Text = contactListArrayList[position].SoDT;
            holder.txtEmail.Text = contactListArrayList[position].Email;
            holder.txtDiachi.Text = contactListArrayList[position].Diachi;

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

        public IList<KhachHang> GetAllDataKH()
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
            KhachHangDbHelper _db = new KhachHangDbHelper(activity, DataName);
            _db.DeleteKhachHang(contactId);
        }

    }
}