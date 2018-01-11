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
    class PhieuXuatNLDaSCChiTietDbHelper : DataBase
    {
        public PhieuXuatNLDaSCChiTietDbHelper(Context ctx, string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuXuatNLDaSCChiTiet> GetAllPhieuXuatNLDaSCChiTiet()
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor c = db.Query("PHIEUXUATNGUYENLIEUDASOCHECT", new string[] { "MaPhieuXuatNLDaSC", "MaNLDaSC", "SLXuatNLSoche", "DGXuatNLSoche" }, null, null, null, null, null);
            var contacts = new List<PhieuXuatNLDaSCChiTiet>();
            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatNLDaSCChiTiet
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaNLDaSC = c.GetString(1),
                    SLXuatNLSoChe = float.Parse(c.GetString(2)),
                    DGXuatNLSoChe = float.Parse(c.GetString(3))
                });
            }
            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact 
        public IList<PhieuXuatNLDaSCChiTiet> GetPhieuXuatNLDaSCChiTietByName(string nameToSearch)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor c = db.RawQuery("select * from PHIEUXUATNGUYENLIEUDASOCHECT where MaPhieuXuatNLDaSC='" + nameToSearch + "'", null);
            var contacts = new List<PhieuXuatNLDaSCChiTiet>();
            while (c.MoveToNext())
            {


                contacts.Add(new PhieuXuatNLDaSCChiTiet
                {


                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaNLDaSC = c.GetString(1),
                    SLXuatNLSoChe = float.Parse(c.GetString(2)),
                    DGXuatNLSoChe = float.Parse(c.GetString(3))
                });
            }
            c.Close();
            db.Close();
            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuXuatNLDaSCChiTiet(PhieuXuatNLDaSCChiTiet contactinfo)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatNLDaSC", contactinfo.MaPhieuXuatNLDaSC);
            vals.Put("MaNLDaSC", contactinfo.MaNLDaSC);
            vals.Put("SLXuatNLSoChe", contactinfo.SLXuatNLSoChe);
            vals.Put("DGXuatNLSoChe", contactinfo.DGXuatNLSoChe);
            db.Insert("PHIEUXUATNGUYENLIEUDASOCHECT", null, vals);
        }

        //Get contact by contact
        public ICursor getPhieuXuatNLDaSCCTById(string maPhieu, string manl)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATNGUYENLIEUDASOCHECT where MaPhieuXuatNLDaSC='" + maPhieu + "'and MaNLDaSC='" + manl + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuXuatNLDaSCChiTiet(PhieuXuatNLDaSCChiTiet contitem)
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
            vals.Put("MaNLDaSC", contitem.MaNLDaSC);
            vals.Put("SLXuatNLSoChe", contitem.SLXuatNLSoChe);
            vals.Put("DGXuatNLSoChe", contitem.DGXuatNLSoChe);
            db.Insert("PHIEUXUATNGUYENLIEUDASOCHECT", null, vals);

            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHECT",
                    new String[] { "MaPhieuXuatNLDaSC", "MaNLDaSC", "SLXuatNLSoche", "DGXuatNLSoche" }, "MaPhieuXuatNLDaSC=?" + "MaNLDaSC=?", new string[] { contitem.MaPhieuXuatNLDaSC, contitem.MaNLDaSC }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUXUATNGUYENLIEUDASOCHECT", vals, "MaPhieuXuatNLDaSC=?" + "MaNLDaSC=?", new String[] { cursor.GetString(0), cursor.GetString(1) });
                }
                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuXuatNLDaSCChiTiet(string contact, string nl)
        {
            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;
            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHECT",
                    new String[] { "MaPhieuXuatNLDaSC", "MaNLDaSC", "SLXuatNLSoche", "DGXuatNLSoche" }, "MaPhieuXuatNLDaSC=?" + "MaNLDaSC=?", new string[] { contact, nl }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUXUATNGUYENLIEUDASOCHECT", "MaPhieuXuatNLDaSC='"+ contact + "' and MaNLDaSC='"+nl+"'",null);
                }
                cursor.Close();
            }
        }
    }
}