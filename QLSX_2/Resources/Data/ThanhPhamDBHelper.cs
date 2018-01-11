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
    class ThanhPhamDBHelper : DataBase
    {
        public ThanhPhamDBHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<ThanhPham> GetAllThanhPham()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("THANHPHAM", new string[] { "MaTP", "TenTP", "DVTTP" }, null, null, null, null, null);

            var contacts = new List<ThanhPham>();

            while (c.MoveToNext())
            {
                contacts.Add(new ThanhPham
                {
                    MaTP = c.GetString(0),
                    TenTP = c.GetString(1),
                    DVTTP = c.GetString(2)          
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<ThanhPham> GetThanhPhamBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("THANHPHAM", new string[] { "MaTP", "TenTP", "DVTTP" }, "upper(TenTP) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<ThanhPham>();

            while (c.MoveToNext())
            {
                contacts.Add(new ThanhPham
                {
                    MaTP = c.GetString(0),
                    TenTP = c.GetString(1),
                    DVTTP = c.GetString(2)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewThanhPham(ThanhPham contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaTP", contactinfo.MaTP);
            vals.Put("TenTP", contactinfo.TenTP);
            vals.Put("DVTTP", contactinfo.DVTTP);
            
            db.Insert("THANHPHAM", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getThanhPhamById(string MaTP)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from THANHPHAM where MaTP='" + MaTP + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateThanhPham(ThanhPham contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaTP", contitem.MaTP);
            vals.Put("TenTP", contitem.TenTP);
            vals.Put("DVTTP", contitem.DVTTP);


            ICursor cursor = db.Query("THANHPHAM",
                    new String[] { "MaTP", "TenTP", "DVTTP" }, "MaTP=?", new string[] { contitem.MaTP }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("THANHPHAM", vals, "MaTP=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }


        //Delete Existing contact
        public void DeleteThanhPham(string contactTP)
        {

            if (contactTP == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("THANHPHAM",
                    new String[] { "MaTP", "TenTP", "DVTTP" }, "MaTP=?", new string[] { contactTP }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("THANHPHAM", "MaTP=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }

    }
}