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
    class PhieuNhapTPCTAdapter : BaseAdapter<PhieuNhapTPCT>
    {
        IList<PhieuNhapTPCT> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public PhieuNhapTPCTAdapter(Context context, IList<PhieuNhapTPCT> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override PhieuNhapTPCT this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PNTPCT, null);
                holder = new ViewHolder();

                holder.txtMa = convertView.FindViewById<TextView>(Resource.Id.lr_MaPhieuXuatNLDaSC_PNTPCT);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_MaTP_PNTPCT);
                holder.txtSL = convertView.FindViewById<TextView>(Resource.Id.lr_SLNhapTP);
                holder.txtDG = convertView.FindViewById<TextView>(Resource.Id.lr_DGNhapTP);
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PNTPCT);

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
                        string fname = contactListArrayList[poldel].MaTP;

                        contactListArrayList.RemoveAt(poldel);

                        DeleteSelectedContact(id,fname);
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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PNTPCT);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }

            holder.txtMa.Text = contactListArrayList[position].MaPhieuXuatNLDaSC;
            holder.txtName.Text = contactListArrayList[position].MaTP;
            holder.txtSL.Text = contactListArrayList[position].SLNhapTP.ToString();
            holder.txtDG.Text = contactListArrayList[position].DGNhapTP.ToString();


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

        public IList<PhieuNhapTPCT> GetAllData()
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

        private void DeleteSelectedContact(string contactId,string name)
        {
            PhieuNhapTPChiTietDbHelper _db = new PhieuNhapTPChiTietDbHelper(activity, MainActivity.dataname);
            
            _db.DeletePhieuNhapTPCT(contactId,name); 
        }
    }
}