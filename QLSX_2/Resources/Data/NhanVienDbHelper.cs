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
using Android.Database.Sqlite;
using Android.Database;
using QLSX.Resources.Model;

namespace QLSX_2.Resources.Data
{
    class NhanVienDbHelper : DataBase
    {
        public NhanVienDbHelper(Context ctx,string DataName) : base(ctx, DataName)
        {
        }

        public IList<NhanVien> GetAllNhanVien()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("NHANVIEN", new string[] { "MaNV", "TenNV", "DiaChiNV", "SoDTNV", "EmailNV" }, null, null, null, null, null);

            var contacts = new List<NhanVien>();

            while (c.MoveToNext())
            {
                contacts.Add(new NhanVien
                {
                    MaNV = c.GetString(0),
                    TenNV = c.GetString(1),
                    DiachiNV = c.GetString(2),
                    SoDTNV = c.GetString(3),
                    EmailNV = c.GetString(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<NhanVien> GetNhanVienBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("NHANVIEN", new string[] { "MaNV", "TenNV", "DiaChiNV", "SoDTNV", "EmailNV" }, "upper(TenNV) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<NhanVien>();

            while (c.MoveToNext())
            {
                contacts.Add(new NhanVien
                {
                    MaNV = c.GetString(0),
                    TenNV = c.GetString(1),
                    DiachiNV = c.GetString(2),
                    SoDTNV = c.GetString(3),
                    EmailNV = c.GetString(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewNhanVien(NhanVien contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaNV", contactinfo.MaNV);
            vals.Put("TenNV", contactinfo.TenNV);
            vals.Put("DiaChiNV", contactinfo.DiachiNV);
            vals.Put("SoDTNV", contactinfo.SoDTNV);
            vals.Put("EmailNV", contactinfo.EmailNV);

            db.Insert("NHANVIEN", null, vals);
        }

        //Get contact Diachi by contact MaNV
        public ICursor getNhanVienById(string MaNV)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from NHANVIEN where MaNV='" + MaNV + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateNhanVien(NhanVien contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaNV", contitem.MaNV);
            vals.Put("TenNV", contitem.TenNV);
            vals.Put("SoDTNV", contitem.SoDTNV);
            vals.Put("EmailNV", contitem.EmailNV);
            vals.Put("DiachiNV", contitem.DiachiNV);


            ICursor cursor = db.Query("NHANVIEN",
                    new String[] { "MaNV", "TenNV", "SoDTNV", "EmailNV", "DiaChiNV" }, "MaNV=?", new string[] { contitem.MaNV }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("NHANVIEN", vals, "MaNV=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeleteNhanVien(string contactMaNV)
        {

            if (contactMaNV == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("NHANVIEN",
                    new String[] { "MaNV", "TenNV", "SoDTNV", "EmailNV", "DiaChiNV" }, "MaNV=?", new string[] { contactMaNV }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("NHANVIEN", "MaNV=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}