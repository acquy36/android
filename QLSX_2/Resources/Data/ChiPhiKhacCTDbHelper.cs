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
    class ChiPhiKhacCTDbHelper : DataBase
    {
        public ChiPhiKhacCTDbHelper(Context ctx,string DATABASENAME) : base(ctx, DATABASENAME)
        {
            DATABASENAME = base.APP_DATABASENAME;
        }
        public IList<ChiPhiKhacChiTiet> GetAllCPKChiTiet()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("CHIPHIKHACCT", new string[] { "MaChiPhiKhac", "NgayChiPhiKhac", "GiaChiPhiKhac"}, null, null, null, null, null);

            var contacts = new List<ChiPhiKhacChiTiet>();

            while (c.MoveToNext())
            {
                contacts.Add(new ChiPhiKhacChiTiet
                {
                    MaChiPhiKhac = c.GetString(0),
                    NgayChiPhiKhac = c.GetString(1),
                    GiaChiPhiKhac = c.GetFloat(2)              
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<ChiPhiKhacChiTiet> GetCPKChiTietBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("CHIPHIKHACCT", new string[] { "MaChiPhiKhac", "NgayChiPhiKhac", "GiaChiPhiKhac" }, "upper(MaChiPhiKhac) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<ChiPhiKhacChiTiet>();

            while (c.MoveToNext())
            {
                contacts.Add(new ChiPhiKhacChiTiet
                {
                    MaChiPhiKhac = c.GetString(0),
                    NgayChiPhiKhac = c.GetString(1),
                    GiaChiPhiKhac = c.GetFloat(2)
                });
            }
            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewCPKChiTiet(ChiPhiKhacChiTiet contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
           
            vals.Put("MaChiPhiKhac", contactinfo.MaChiPhiKhac);
            vals.Put("NgayChiPhiKhac", contactinfo.NgayChiPhiKhac);
            vals.Put("GiaChiPhiKhac", contactinfo.GiaChiPhiKhac);
            db.Insert("CHIPHIKHACCT", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getCPKChiTietById(ChiPhiKhacChiTiet Phieu)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from CHIPHIKHACCT where MaChiPhiKhac='" + Phieu.MaChiPhiKhac + "'and NgayChiPhiKhac='" + Phieu.NgayChiPhiKhac + "'", null);
            return res;
        }
        public ICursor getCPKChiTietByPhieu(string Phieu)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from CHIPHIKHACCT where MaChiPhiKhac='" + Phieu +"'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateCPKChiTiet(ChiPhiKhacChiTiet contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
           
            vals.Put("MaChiPhiKhac", contitem.MaChiPhiKhac);
            vals.Put("NgayChiPhiKhac", contitem.NgayChiPhiKhac);
            vals.Put("GiaChiPhiKhac", contitem.GiaChiPhiKhac);

            ICursor cursor = db.Query("CHIPHIKHACCT",
                    new String[] { "MaChiPhiKhac", "NgayChiPhiKhac", "GiaChiPhiKhac" }, "MaChiPhiKhac=?", new string[] { contitem.MaChiPhiKhac }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("CHIPHIKHACCT", vals, "MaChiPhiKhac=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeleteCPKChiTiet(string contact, string ngay)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("CHIPHIKHACCT",
                    new String[] { "MaChiPhiKhac", "NgayChiPhiKhac", "GiaChiPhiKhac" }, "MaChiPhiKhac=?"+ "NgayChiPhiKhac=?", new string[] { contact, ngay }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("CHIPHIKHACCT", "MaChiPhiKhac='" +contact +"' and NgayChiPhiKhac='"+ngay+"'",null );
                }

                cursor.Close();
            }
        }
    }
}