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
    class PhieuNhapTPChiTietDbHelper : DataBase
    {
        public PhieuNhapTPChiTietDbHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuNhapTPCT> GetAllPhieuNhapTPCT()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUNHAPTHANHPHAMCT", new string[] { "MaPhieuXuatNLDaSC", "MaTP", "SLNhapTP", "DGNhapTP" }, null, null, null, null, null);

            var contacts = new List<PhieuNhapTPCT>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuNhapTPCT
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaTP = c.GetString(1),
                    SLNhapTP = c.GetFloat(2),
                    DGNhapTP = c.GetFloat(3)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<PhieuNhapTPCT> GetPhieuNhapTPCTBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUNHAPTHANHPHAMCT", new string[] { "MaPhieuXuatNLDaSC", "MaTP", "SLNhapTP", "DGNhapTP" }, "upper(MaTP) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuNhapTPCT>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuNhapTPCT
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaTP = c.GetString(1),
                    SLNhapTP = c.GetFloat(2),
                    DGNhapTP = c.GetFloat(3)
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuNhapTPCT(PhieuNhapTPCT contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatNLDaSC", contactinfo.MaPhieuXuatNLDaSC);
            vals.Put("MaTP", contactinfo.MaTP);
            vals.Put("SLNhapTP", contactinfo.SLNhapTP);
            vals.Put("DGNhapTP", contactinfo.DGNhapTP);
            db.Insert("PHIEUNHAPTHANHPHAMCT", null, vals);
        }

        //Get contact Diachi by contact
        public ICursor getPhieuNhapTPCTById(string PhieuNhap,string tp)
        {

            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUNHAPTHANHPHAMCT where MaPhieuXuatNLDaSC='" + PhieuNhap + "'and MaTP='"+ tp + "'", null);
            return res;
        }
        

        public float getSoLuongNhapByTp(string tp)
        {
            float S = 0;
            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUNHAPTHANHPHAMCT where MaTP='" + tp + "'", null);
            while (res.MoveToNext())
            {
                S = (S + res.GetFloat(res.GetColumnIndex("SLNhapTP")));
            }
            return S;
        }
        //Update Existing contact
        public void UpdatePhieuNhapTPCT(PhieuNhapTPCT contitem)
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
            vals.Put("MaTP", contitem.MaTP);
            vals.Put("SLNhapTP", contitem.SLNhapTP);
            vals.Put("DGNhapTP", contitem.DGNhapTP);
            ICursor cursor = db.Query("PHIEUNHAPTHANHPHAMCT",
                    new String[] { "MaPhieuXuatNLDaSC", "MaTP", "SLNhapTP", "DGNhapTP" }, "MaPhieuXuatNLDaSC=?"+"MaTP=?", new string[] { contitem.MaPhieuXuatNLDaSC, contitem.MaTP }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUNHAPTHANHPHAMCT", vals, "MaPhieuXuatNLDaSC=?" + "MaTP=?", new String[] { cursor.GetString(0), cursor.GetString(1) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuNhapTPCT(string contact,string name)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("PHIEUNHAPTHANHPHAMCT",
                    new String[] { "MaPhieuXuatNLDaSC", "MaTP", "SLNhapTP", "DGNhapTP" }, "MaPhieuXuatNLDaSC=?" + "MaTP=?", new string[] { contact, name }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUNHAPTHANHPHAMCT", "MaPhieuXuatNLDaSC=?" + "MaTP=?", new String[] { cursor.GetString(0), cursor.GetString(1) });
                }

                cursor.Close();
            }

        }
    }
}