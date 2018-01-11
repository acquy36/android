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
    public class ChiPhiKhacDbHelper: DataBase
    {
        public ChiPhiKhacDbHelper(Context ctx,string DATABASENAME) : base(ctx, DATABASENAME)
        {
            DATABASENAME=base.APP_DATABASENAME;
        }

        public IList<ChiPhiKhac> GetAllChiPhiKhac()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("CHIPHIKHAC", new string[] { "MaChiPhiKhac", "TenChiPhiKhac", "DVTChiPhiKhac" }, null, null, null, null, null);

            var contacts = new List<ChiPhiKhac>();

            while (c.MoveToNext())
            {
                contacts.Add(new ChiPhiKhac
                {
                    MaChiPhiKhac = c.GetString(0),
                    TenChiPhiKhac = c.GetString(1),
                    DVTChiPhiKhac = c.GetString(2),
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<ChiPhiKhac> GetChiPhiKhacBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("CHIPHIKHAC", new string[] { "MaChiPhiKhac", "TenChiPhiKhac", "DVTChiPhiKhac" }, "upper(TenChiPhiKhac) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<ChiPhiKhac>();

            while (c.MoveToNext())
            {
                contacts.Add(new ChiPhiKhac
                {
                    MaChiPhiKhac = c.GetString(0),
                    TenChiPhiKhac = c.GetString(1),
                    DVTChiPhiKhac = c.GetString(2),
                });
            }

            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewChiPhiKhac(ChiPhiKhac contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaChiPhiKhac", contactinfo.MaChiPhiKhac);
            vals.Put("TenChiPhiKhac", contactinfo.TenChiPhiKhac);
            vals.Put("DVTChiPhiKhac", contactinfo.DVTChiPhiKhac);      
            db.Insert("CHIPHIKHAC", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getChiPhiKhacById(string MaChiPhiKhac)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from CHIPHIKHAC where MaChiPhiKhac='" + MaChiPhiKhac + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdateChiPhiKhac(ChiPhiKhac contitem)
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
            vals.Put("TenChiPhiKhac", contitem.TenChiPhiKhac);
            vals.Put("DVTChiPhiKhac", contitem.DVTChiPhiKhac);

            ICursor cursor = db.Query("CHIPHIKHAC",
                    new String[] { "MaChiPhiKhac", "TenChiPhiKhac", "DVTChiPhiKhac" }, "MaChiPhiKhac=?", new string[] { contitem.MaChiPhiKhac }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("CHIPHIKHAC", vals, "MaChiPhiKhac=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeleteChiPhiKhac(string contact)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("CHIPHIKHAC",
                    new String[] { "MaChiPhiKhac", "TenChiPhiKhac", "DVTChiPhiKhac" }, "MaChiPhiKhac=?", new string[] { contact }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("CHIPHIKHAC", "MaChiPhiKhac=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}