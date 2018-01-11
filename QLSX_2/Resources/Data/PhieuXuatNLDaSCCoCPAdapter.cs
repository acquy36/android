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
    class PhieuXuatNLDaSCCoCPAdapter : BaseAdapter<PhieuXuatNLDaSCCoCP>
    {
        IList<PhieuXuatNLDaSCCoCP> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuXuatNLDaSCCoCPAdapter(Context context, IList<PhieuXuatNLDaSCCoCP> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override PhieuXuatNLDaSCCoCP this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PXNLDaSCCoCP, null);
                holder = new ViewHolder();

                holder.txtPhieu = convertView.FindViewById<TextView>(Resource.Id.lr_MaPhieuXuatNLDaSC);
                holder.txtChiPhi = convertView.FindViewById<TextView>(Resource.Id.lr_MaCPK_PXNLDaSCCP);
                holder.txtSL = convertView.FindViewById<TextView>(Resource.Id.lr_SLXuatNLDaSCCoCP);
                holder.txtDG = convertView.FindViewById<TextView>(Resource.Id.lr_DGXuatNLDaSCCoCP);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXNLDaSCCoCP);

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
                        string fname = contactListArrayList[poldel].MaChiPhiKhac;

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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXNLDaSCCoCP);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtPhieu.Text = contactListArrayList[position].MaPhieuXuatNLDaSC;
            holder.txtChiPhi.Text = contactListArrayList[position].MaChiPhiKhac;
            holder.txtSL.Text = contactListArrayList[position].SLXuatNLDaSCChiPhi.ToString();
            holder.txtSL.Text = contactListArrayList[position].DGXuatNLDaSCChiPhi.ToString();

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

        public IList<PhieuXuatNLDaSCCoCP> GetAllData()
        {
            return contactListArrayList;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtPhieu { get; set; }
            public TextView txtChiPhi { get; set; }
            public TextView txtSL { get; set; }
            public TextView txtDG { get; set; }

        }

        private void DeleteSelectedContact(string contactId, string Chiphi)
        {
            PhieuXuatNLDaSCCPDbHelper _db = new PhieuXuatNLDaSCCPDbHelper(activity, MainActivity.dataname);
            _db.DeletePhieuXuatNLDaSCCP(contactId, Chiphi);


        }
    }
}