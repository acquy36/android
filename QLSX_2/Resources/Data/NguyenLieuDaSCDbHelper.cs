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
    class NguyenLieuDaSCDbHelper : DataBase
    {
        public NguyenLieuDaSCDbHelper(Context ctx,string DataName) : base(ctx, DataName)
        {
        }
        public IList<NguyenLieuDaSoche> GetAllNLDaSC()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("NGUYENLIEUDASOCHE", new string[] { "MaNLDaSC", "TenNLDaSC", "DVTNLDaSC" }, null, null, null, null, null);

            var contacts = new List<NguyenLieuDaSoche>();

            while (c.MoveToNext())
            {
                contacts.Add(new NguyenLieuDaSoche
                {
                    MaNLDaSC = c.GetString(0),
                    TenNLDaSC = c.GetString(1),
                    DVTNLDaSC = c.GetString(2),
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<NguyenLieuDaSoche> GetNLDaSCBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("NGUYENLIEUDASOCHE", new string[] { "MaNLDaSC", "TenNLDaSC", "DVTNLDaSC" }, "upper(TenNLDaSC) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<NguyenLieuDaSoche>();

            while (c.MoveToNext())
            {
                contacts.Add(new NguyenLieuDaSoche
                {
                    MaNLDaSC = c.GetString(0),
                    TenNLDaSC = c.GetString(1),
                    DVTNLDaSC = c.GetString(2),
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewNLDaSC(NguyenLieuDaSoche contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaNLDaSC", contactinfo.MaNLDaSC);
            vals.Put("TenNLDaSC", contactinfo.TenNLDaSC);
            vals.Put("DVTNLDaSC", contactinfo.DVTNLDaSC);
            db.Insert("NGUYENLIEUDASOCHE", null, vals);
        }

        //Get contact Diachi by contact
        public ICursor getNLDaSCById(string MaNLDaSC)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from NGUYENLIEUDASOCHE where MaNLDaSC='" + MaNLDaSC + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateNLDaSC(NguyenLieuDaSoche contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaNLDaSC", contitem.MaNLDaSC);
            vals.Put("TenNLDaSC", contitem.TenNLDaSC);
            vals.Put("DVTNLDaSC", contitem.DVTNLDaSC);

            ICursor cursor = db.Query("NGUYENLIEUDASOCHE",
                    new String[] { "MaNLDaSC", "TenNLDaSC", "DVTNLDaSC" }, "MaNLDaSC=?", new string[] { contitem.MaNLDaSC }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("NGUYENLIEUDASOCHE", vals, "MaNLDaSC=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeleteNLDaSC(string contact)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("NGUYENLIEUDASOCHE",
                    new String[] { "MaNLDaSC", "TenNLDaSC", "DVTNLDaSC" }, "MaNLDaSC=?", new string[] { contact }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("NGUYENLIEUDASOCHE", "MaNLDaSC=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}