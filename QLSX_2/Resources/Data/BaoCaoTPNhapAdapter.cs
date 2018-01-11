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
using Java.Lang;
using QLSX.Resources.Model;
using Android.Database;

namespace QLSX_2.Resources.Data
{
    class BaoCaoTPNhapAdapter : BaseAdapter<PhieuNhapTPCT>
    {
        IList<PhieuNhapTPCT> contactListArrayList;
        private LayoutInflater mInflater;
        private Context activity;
        public BaoCaoTPNhapAdapter(Context context, IList<PhieuNhapTPCT> results)
        {
            this.activity = context;
            contactListArrayList = results;
            mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }
        public override int Count
        {
            get
            {
                return contactListArrayList.Count;
            }
        }

        public override PhieuNhapTPCT this[int position]
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
            ViewHolder holder = null;
            if (convertView == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.list_row_BCTPNhap, null);
                holder = new ViewHolder();
 
                holder.txtMa = convertView.FindViewById<TextView>(Resource.Id.lr_PhieuNhapBC);
                holder.txtNgay = convertView.FindViewById<TextView>(Resource.Id.lr_NgayNhapBC);
                holder.txtSL = convertView.FindViewById<TextView>(Resource.Id.lr_SLNhapBC);
                convertView.Tag = holder;
            }
            else
            {
                holder = convertView.Tag as ViewHolder;
            }

            holder.txtMa.Text = contactListArrayList[position].MaPhieuXuatNLDaSC;
            PhieuXuatNLDaSCDbHelper _db = new PhieuXuatNLDaSCDbHelper(activity, MainActivity.dataname);
            ICursor c = _db.getPhieuXuatNLDaSCById(contactListArrayList[position].MaPhieuXuatNLDaSC);
            if (c.MoveToFirst())
            {
                holder.txtNgay.Text = c.GetString(c.GetColumnIndex("NgayPhieuXuatNLDaSC"));
            }
            holder.txtSL.Text = contactListArrayList[position].SLNhapTP.ToString();

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
            public TextView txtNgay { get; set; }
            public TextView txtSL { get; set; }

        }
    }
}