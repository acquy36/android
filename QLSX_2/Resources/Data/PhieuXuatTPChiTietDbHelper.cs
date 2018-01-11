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
    class PhieuXuatTPChiTietDbHelper : DataBase
    {
        public PhieuXuatTPChiTietDbHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuXuatTPChiTiet> GetAllPhieuXuatTPCT()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATTHANHPHAMCT", new string[] { "MaPhieuXuatTP", "MaTP", "SLXuatTP", "DGXuatTP" }, null, null, null, null, null);

            var contacts = new List<PhieuXuatTPChiTiet>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatTPChiTiet
                {
                    MaPhieuXuatTP = c.GetString(0),
                    MaTP = c.GetString(1),
                    SLXuatTP = c.GetFloat(2),
                    DGXuatTP = c.GetFloat(3)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact 
        public IList<PhieuXuatTPChiTiet> GetPhieuXuatTPCTBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATTHANHPHAMCT", new string[] { "MaPhieuXuatTP", "MaTP", "SLXuatTP", "DGXuatTP" }," upper(MaTP) LIKE ? =", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuXuatTPChiTiet>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatTPChiTiet
                {
                    MaPhieuXuatTP = c.GetString(0),
                    MaTP = c.GetString(1),
                    SLXuatTP = c.GetFloat(2),
                    DGXuatTP = c.GetFloat(3)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }
        public IList<PhieuXuatTPChiTiet> GetPhieuXuatTPCTBySearchPhieu(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATTHANHPHAMCT", new string[] { "MaPhieuXuatTP", "MaTP", "SLXuatTP", "DGXuatTP" }, " upper(MaPhieuXuatTP) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuXuatTPChiTiet>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatTPChiTiet
                {
                    MaPhieuXuatTP = c.GetString(0),
                    MaTP = c.GetString(1),
                    SLXuatTP = c.GetFloat(2),
                    DGXuatTP = c.GetFloat(3)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }
        public float getSoLuongXuatByTp(string tp)
        {
            float S = 0;
            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATTHANHPHAMCT where MaTP='" + tp + "'", null);
            while (res.MoveToNext())
            {
                S = (S + res.GetFloat(res.GetColumnIndex("SLXuatTP")));
            }
            return S;
        }
        //Add New Contact
        public void AddNewPhieuXuatTPCT(PhieuXuatTPChiTiet contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatTP", contactinfo.MaPhieuXuatTP);
            vals.Put("MaTP", contactinfo.MaTP);
            vals.Put("SLXuatTP", contactinfo.SLXuatTP);
            vals.Put("DGXuatTP", contactinfo.DGXuatTP);
            db.Insert("PHIEUXUATTHANHPHAMCT", null, vals);
        }

        //Get contact Diachi by contact
        public ICursor getPhieuXuatTPCTById(string maPhieu,string matp)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATTHANHPHAMCT where MaPhieuXuatTP='" +maPhieu+ "'and MaTP='" + matp + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuXuatTPCT(PhieuXuatTPChiTiet contitem)
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
            vals.Put("MaTP", contitem.MaTP);
            vals.Put("SLXuatTP", contitem.SLXuatTP);
            vals.Put("DGXuatTP", contitem.DGXuatTP);
            ICursor cursor = db.Query("PHIEUXUATTHANHPHAMCT",
                    new String[] { "MaPhieuXuatTP", "MaTP", "SLXuatTP", "DGXuatTP" }, "MaPhieuXuatTP=?"+"MaTP=?", new string[] { contitem.MaPhieuXuatTP, contitem.MaTP }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUXUATTHANHPHAMCT", vals, "MaPhieuXuatTP=?" + "MaTP=?", new String[] { cursor.GetString(0), cursor.GetString(1) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuXuatTPCT(string contact, string tp)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //ICursor cursor = db.Query("PHIEUXUATTHANHPHAMCT",
            //        new String[] { "MaPhieuXuatTP", "MaTP", "SLXuatTP", "DGXuatTP" }, "MaTP=?"+ "MaPhieuXuatTP=?", new String[] { tp, contact }, null, null, null, null);
            ICursor cursor = db.RawQuery("select * from PHIEUXUATTHANHPHAMCT where MaPhieuXuatTP='" + contact + "'and MaTP='" + tp + "'", null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {                
                    // update the row
                    db.Delete("PHIEUXUATTHANHPHAMCT", "MaPhieuXuatTP='"+contact +"' and MaTP= '"+tp +"'" ,null);
                }

                cursor.Close();
            }

        }
    }
}