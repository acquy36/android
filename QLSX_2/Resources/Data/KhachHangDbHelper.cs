using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using QLSX.Resources.Model;
using Android.Database;

using Android.Database.Sqlite;

namespace QLSX_2.Resources.Data
{
    public class KhachHangDbHelper: DataBase
    {
        public KhachHangDbHelper(Context ctx, string DATABASENAME) : base(ctx, DATABASENAME)
        {
            DATABASENAME = base.APP_DATABASENAME;

        }

        public IList<KhachHang> GetAllKhachHang()
        {

            SQLiteDatabase db = this.ReadableDatabase;
            ICursor c = db.Query("KHACHHANG", new string[] { "MaKH", "TenKH", "DiaChiKH", "SoDTKH", "EmailKH" }, null, null, null, null, null);

            var contacts = new List<KhachHang>();

            while (c.MoveToNext())
            {
                contacts.Add(new KhachHang
                {
                    MaKH = c.GetString(0),
                    TenKH = c.GetString(1),
                    Diachi = c.GetString(2),
                    SoDT = c.GetString(3),
                    Email = c.GetString(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }
        public IList<KhachHang> GetKhachHangBySearchName(string nameToSearch)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor c = db.Query("KHACHHANG", new string[] { "MaKH", "TenKH", "DiaChiKH", "SoDTKH", "EmailKH" }, "upper(TenKH) LIKE ?", 
                new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);
            var contacts = new List<KhachHang>();
            while (c.MoveToNext())
            {
                contacts.Add(new KhachHang
                {
                    MaKH = c.GetString(0),
                    TenKH = c.GetString(1),
                    Diachi = c.GetString(2),
                    SoDT = c.GetString(3),
                    Email = c.GetString(4)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

       // Add New Contact
        public void AddNewKhachHang(KhachHang contactinfo)
        {
                SQLiteDatabase db = this.WritableDatabase;
                ContentValues vals = new ContentValues();
                vals.Put("MaKH", contactinfo.MaKH);
                vals.Put("TenKH", contactinfo.TenKH);
                vals.Put("DiaChiKH", contactinfo.Diachi);
                vals.Put("SoDTKH", contactinfo.SoDT);
                vals.Put("EmailKH", contactinfo.Email);
                db.Insert("KHACHHANG", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getKhachHangById(string MaKH)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from KHACHHANG where MaKH='" + MaKH + "'", null);
            return res;
        }

       // Update Existing contact
         public void UpdateKhachHang(KhachHang contitem)
        {
            if (contitem == null)
            {
                return ;
            }
            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;
            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaKH", contitem.MaKH);
            vals.Put("TenKH", contitem.TenKH);
            vals.Put("SoDTKH", contitem.SoDT);
            vals.Put("EmailKH", contitem.Email);
            vals.Put("DiachiKH", contitem.Diachi);
            ICursor cursor = db.Query("KHACHHANG",
                    new string[] { "MaKH", "TenKH", "SoDTKH", "EmailKH", "DiaChiKH" }, "MaKH=?",
                    new string[] { contitem.MaKH }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("KHACHHANG", vals, "MaKH=?", new string[] { cursor.GetString(0) });
                }
                cursor.Close();
            }
           
        }


        //Delete Existing contact
        public void DeleteKhachHang(string contactMaKH)
        {
            if (contactMaKH == null)
            {
                return;
            }
            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("KHACHHANG",
                    new string[] { "MaKH", "TenKH", "SoDTKH", "EmailKH", "DiaChiKH" }, "MaKH=?", 
                    new string[] { contactMaKH }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("KHACHHANG", "MaKH=?", new string[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}