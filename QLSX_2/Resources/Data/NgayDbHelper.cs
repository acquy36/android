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
    class NgayDbHelper : DataBase
    {
        public NgayDbHelper(Context ctx, string DATABASENAME) : base(ctx, DATABASENAME)
        {
            DATABASENAME = base.APP_DATABASENAME;

        }
        public IList<Ngay> GetAllNgay()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("NGAY", new string[] { "NgayChiPhikhac" }, null, null, null, null, null);

            var contacts = new List<Ngay>();

            while (c.MoveToNext())
            {
                contacts.Add(new Ngay
                {
                    NgayChiPhiKhac = c.GetString(0)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        

        //Add New Contact
        public void AddNewNgay(Ngay contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("NgayChiPhiKhac", contactinfo.NgayChiPhiKhac);

            db.Insert("NGAY", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getNgayById(string NgayChiPhiKhac)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from NGAY where NgayChiPhiKhac='" + NgayChiPhiKhac + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateNgay(Ngay contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("NgayChiPhiKhac", contitem.NgayChiPhiKhac);

            ICursor cursor = db.Query("NGAY",
                    new String[] { "NgayChiPhiKhac" }, "NgayChiPhiKhac=?", new string[] { contitem.NgayChiPhiKhac }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("NGAY", vals, "NgayChiPhiKhac=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeleteNgay(string NgayChiPhiKhac)
        {

            if (NgayChiPhiKhac == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("NGAY",
                    new String[] { "NgayChiPhiKhac" }, "NgayChiPhiKhac=?", new string[] { NgayChiPhiKhac }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("NGAY", "NgayChiPhiKhac=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}