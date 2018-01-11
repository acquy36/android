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
    class NguyenLieuDaSCAdapter : BaseAdapter<NguyenLieuDaSoche>
    {
        IList<NguyenLieuDaSoche> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public NguyenLieuDaSCAdapter(Context context,
                                                IList<NguyenLieuDaSoche> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override int Count
        {
            get { return contactListArrayList.Count; }
        }

        public override NguyenLieuDaSoche this[int position]
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
            Nlieu holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_NLDaSC, null);
                holder = new Nlieu();

                holder.txtMaNL = convertView.FindViewById<TextView>(Resource.Id.lr_MaNLDaSC);
                holder.txtFullName = convertView.FindViewById<TextView>(Resource.Id.lr_TenNLDaSC);
                holder.txtDVT = convertView.FindViewById<TextView>(Resource.Id.lr_DVTNLDaSC);

                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_NLDaSC);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaNLDaSC.ToString();
                        string fname = contactListArrayList[poldel].TenNLDaSC;

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_NLDaSC);
                holder = convertView.Tag as Nlieu;
                btnDelete.Tag = position;
            }

            holder.txtMaNL.Text = contactListArrayList[position].MaNLDaSC;
            holder.txtFullName.Text = contactListArrayList[position].TenNLDaSC;
            holder.txtDVT.Text = contactListArrayList[position].DVTNLDaSC;


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

        public IList<NguyenLieuDaSoche> GetAllDataKH()
        {
            return contactListArrayList;
        }

        public class Nlieu : Java.Lang.Object
        {
            public TextView txtMaNL { get; set; }
            public TextView txtFullName { get; set; }
            public TextView txtDVT { get; set; }
        }

        private void DeleteSelectedContact(string contactId)
        {
            NguyenLieuDaSCDbHelper _db = new NguyenLieuDaSCDbHelper(activity, MainActivity.dataname);
            _db.DeleteNLDaSC(contactId);
        }
    }
}