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
using Android.Database.Sqlite;
using Android.Database;

namespace QLSX_2.Resources.Data
{
    class PhieuXuatThanhPhamDbHelper : DataBase
    {
        public PhieuXuatThanhPhamDbHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuXuatTP> GetAllPhieuXuatTP()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATTHANHPHAM", new string[] { "MaPhieuXuatTP", "MaNV", "MaKH", "NgayPhieuXuatTP" }, null, null, null, null, null);

            var contacts = new List<PhieuXuatTP>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatTP
                {
                    MaPhieuXuatTP = c.GetString(0),
                    MaNV = c.GetString(1),
                    MaKH = c.GetString(2),
                    NgayXuatTP = c.GetString(3)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<PhieuXuatTP> GetPhieuXuatTPBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATTHANHPHAM", new string[] { "MaPhieuXuatTP", "MaNV", "MaKH", "NgayPhieuXuatTP" }, "upper(MaKH) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuXuatTP>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatTP
                {
                    MaPhieuXuatTP = c.GetString(0),
                    MaNV = c.GetString(1),
                    MaKH = c.GetString(2),
                    NgayXuatTP = c.GetString(3)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuXuatTP(PhieuXuatTP contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatTP", contactinfo.MaPhieuXuatTP);
            vals.Put("MaNV", contactinfo.MaNV);
            vals.Put("MaKH", contactinfo.MaKH);
            vals.Put("NgayPhieuXuatTP", contactinfo.NgayXuatTP);
            db.Insert("PHIEUXUATTHANHPHAM", null, vals);
        }

        //Get contact Diachi by contact
        public ICursor getPhieuXuatTPById(string MaPhieuXuatTP)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATTHANHPHAM where MaPhieuXuatTP='" + MaPhieuXuatTP + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuXuatTP(PhieuXuatTP contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatTP", contitem.MaPhieuXuatTP);
            vals.Put("MaNV", contitem.MaNV);
            vals.Put("MaKH", contitem.MaKH);
            vals.Put("NgayPhieuXuatTP", contitem.NgayXuatTP);

            ICursor cursor = db.Query("PHIEUXUATTHANHPHAM",
                    new String[] { "MaPhieuXuatTP", "MaNV", "MaKH", "NgayPhieuXuatTP" }, "MaPhieuXuatTP=?", new string[] { contitem.MaPhieuXuatTP }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUXUATTHANHPHAM", vals, "MaPhieuXuatTP=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuXuatTP(string contact)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("PHIEUXUATTHANHPHAM",
                    new String[] { "MaPhieuXuatTP", "MaNV", "MaKH", "NgayPhieuXuatTP" }, "MaPhieuXuatTP=?", new string[] { contact }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUXUATTHANHPHAM", "MaPhieuXuatTP=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}