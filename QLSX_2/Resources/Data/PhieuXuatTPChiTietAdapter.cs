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
    class PhieuXuatTPChiTietAdapter : BaseAdapter<PhieuXuatTPChiTiet>
    {
        IList<PhieuXuatTPChiTiet> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        private string dataname =MainActivity.dataname;
        public PhieuXuatTPChiTietAdapter(Context context, IList<PhieuXuatTPChiTiet> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override PhieuXuatTPChiTiet this[int position]
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
                convertView = mInflater.Inflate(Resource.Layout.list_row_PXTPCT, null);
                holder = new ViewHolder();

                holder.txtMa = convertView.FindViewById<TextView>(Resource.Id.lr_MaTP_PXTPCT);
                holder.txtName = convertView.FindViewById<TextView>(Resource.Id.lr_TenTP_PXTPCT);
                holder.txtSL = convertView.FindViewById<TextView>(Resource.Id.lr_SLPXTPCT);
                holder.txtDG = convertView.FindViewById<TextView>(Resource.Id.lr_DonGiaPXTPCT);               
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXTPCT);

                btnDelete.Click += (object sender, EventArgs e) =>
                {

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirm Delete");
                    confirm.SetMessage("Are you sure delete?");
                    confirm.SetButton("OK", (s, ev) =>
                    {
                        var poldel = (int)((sender as ImageView).Tag);

                        string id = contactListArrayList[poldel].MaPhieuXuatTP;
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
                btnDelete = convertView.FindViewById<ImageView>(Resource.Id.lr_btnDelete_PXTPCT);
                holder = convertView.Tag as ViewHolder;
                btnDelete.Tag = position;
            }
            ThanhPhamDBHelper _db = new ThanhPhamDBHelper(activity,dataname);
            ICursor c = _db.getThanhPhamById(contactListArrayList[position].MaTP);
            if (c.MoveToFirst())
            {
                holder.txtName.Text = c.GetString(c.GetColumnIndex("TenTP"));
            }
            holder.txtMa.Text = contactListArrayList[position].MaTP;
            holder.txtSL.Text = contactListArrayList[position].SLXuatTP.ToString();
            holder.txtDG.Text = contactListArrayList[position].DGXuatTP.ToString();


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

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtMa { get; set; }
            public TextView txtName { get; set; }
            public TextView txtSL { get; set; }
            public TextView txtDG { get; set; }


        }    
        private void DeleteSelectedContact(string Id,string tp)
        {
            PhieuXuatTPChiTietDbHelper _db = new PhieuXuatTPChiTietDbHelper(activity, dataname);         
             _db.DeletePhieuXuatTPCT(Id, tp); 
        }
    }
}