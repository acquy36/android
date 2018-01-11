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
    class PhieuXuatNLDaSCDbHelper : DataBase
    {
        public PhieuXuatNLDaSCDbHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuXuatNLDaSoChe> GetAllPhieuXuatNLDaSC()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATNGUYENLIEUDASOCHE", new string[] { "MaPhieuXuatNLDaSC", "MaNV", "NgayPhieuXuatNLDaSC" }, null, null, null, null, null);

            var contacts = new List<PhieuXuatNLDaSoChe>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatNLDaSoChe
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaNV = c.GetString(1),
                    NgayPhieuXuatNLDaSC = c.GetString(2)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<PhieuXuatNLDaSoChe> GetPhieuXuatNLDaSCBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATNGUYENLIEUDASOCHE", new string[] { "MaPhieuXuatNLDaSC", "MaNV", "NgayPhieuXuatNLDaSC" }, "upper(NgayPhieuXuatNLDaSC) LIKE ?", new string[] { "%/" + nameToSearch.ToUpper() + "/%" }, null, null, null, null);

            var contacts = new List<PhieuXuatNLDaSoChe>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatNLDaSoChe
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaNV = c.GetString(1),
                    NgayPhieuXuatNLDaSC = c.GetString(2)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuXuatNLDaSC(PhieuXuatNLDaSoChe contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatNLDaSC", contactinfo.MaPhieuXuatNLDaSC);
            vals.Put("MaNV", contactinfo.MaNV);
            vals.Put("NgayPhieuXuatNLDaSC", contactinfo.NgayPhieuXuatNLDaSC);
            db.Insert("PHIEUXUATNGUYENLIEUDASOCHE", null, vals);
        }

        //Get contact Diachi by contact
        public ICursor getPhieuXuatNLDaSCById(string MaPhieuXuatNLDaSC)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATNGUYENLIEUDASOCHE where MaPhieuXuatNLDaSC='" + MaPhieuXuatNLDaSC + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuXuatNLDaSC(PhieuXuatNLDaSoChe contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatNLDaSC", contitem.MaPhieuXuatNLDaSC);
            vals.Put("MaNV", contitem.MaNV);
            vals.Put("NgayPhieuXuatNLDaSC", contitem.NgayPhieuXuatNLDaSC);

            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHE",
                    new String[] { "MaPhieuXuatNLDaSC", "MaNV", "NgayPhieuXuatNLDaSC" }, "MaPhieuXuatNLDaSC=?", new string[] { contitem.MaPhieuXuatNLDaSC }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUXUATNGUYENLIEUDASOCHE", vals, "MaPhieuXuatNLDaSC=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuXuatNLDaSC(string contact)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHE",
                    new String[] { "MaPhieuXuatNLDaSC", "MaNV", "NgayPhieuXuatNLDaSC" }, "MaPhieuXuatNLDaSC=?", new string[] { contact }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUXUATNGUYENLIEUDASOCHE", "MaPhieuXuatNLDaSC=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }
    }
}