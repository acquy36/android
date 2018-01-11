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
    class PhieuXuatNLDaSCCPDbHelper : DataBase
    {
        public PhieuXuatNLDaSCCPDbHelper(Context ctx,string DataName) : base(ctx, DataName)
        {
        }

        public IList<PhieuXuatNLDaSCCoCP> GetAllPhieuXuatNLDaSCCP()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI", new string[] { "MaPhieuXuatNLDaSC", "MaChiPhiKhac", "SLXuatNLDaSCChiPhi", "DGXuatNLDaSCChiPhi" }, null, null, null, null, null);

            var contacts = new List<PhieuXuatNLDaSCCoCP>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatNLDaSCCoCP
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaChiPhiKhac = c.GetString(1),
                    SLXuatNLDaSCChiPhi = c.GetFloat(2),
                    DGXuatNLDaSCChiPhi = c.GetFloat(3)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<PhieuXuatNLDaSCCoCP> GetPhieuXuatNLDaSCCPBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI", new string[] { "MaPhieuXuatNLDaSC", "MaChiPhiKhac", "SLXuatNLDaSCChiPhi", "DGXuatNLDaSCChiPhi" }, "upper(MaChiPhiKhac) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuXuatNLDaSCCoCP>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuXuatNLDaSCCoCP
                {
                    MaPhieuXuatNLDaSC = c.GetString(0),
                    MaChiPhiKhac = c.GetString(1),
                    SLXuatNLDaSCChiPhi = c.GetFloat(2),
                    DGXuatNLDaSCChiPhi = c.GetFloat(3)
                });
            }
            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuXuatNLDaSCCP(PhieuXuatNLDaSCCoCP contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuXuatNLDaSC", contactinfo.MaPhieuXuatNLDaSC);
            vals.Put("MaChiPhiKhac", contactinfo.MaChiPhiKhac);
            vals.Put("SLXuatNLDaSCChiPhi", contactinfo.SLXuatNLDaSCChiPhi);
            vals.Put("DGXuatNLDaSCChiPhi", contactinfo.DGXuatNLDaSCChiPhi);
            db.Insert("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getPhieuXuatNLDaSCCPById(string Phieu, string chiphi)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI where MaPhieuXuatNLDaSC='" + Phieu + "'and MaChiPhiKhac='" + chiphi + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuXuatNLDaSCCP(PhieuXuatNLDaSCCoCP contitem)
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
            vals.Put("MaChiPhiKhac", contitem.MaChiPhiKhac);
            vals.Put("SLXuatNLDaSCChiPhi", contitem.SLXuatNLDaSCChiPhi);
            vals.Put("DGXuatNLDaSCChiPhi", contitem.DGXuatNLDaSCChiPhi);

            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI",
                    new String[] { "MaPhieuXuatNLDaSC", "MaChiPhiKhac", "SLXuatNLDaSCChiPhi", "DGXuatNLDaSCChiPhi" }, "MaPhieuXuatNLDaSC=?", new string[] { contitem.MaPhieuXuatNLDaSC }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI", vals, "MaPhieuXuatNLDaSC=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuXuatNLDaSCCP(string contact,string chiphi)
        {

            if (contact == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI",
                    new String[] { "MaPhieuXuatNLDaSC", "MaChiPhiKhac", "SLXuatNLDaSCChiPhi", "DGXuatNLDaSCChiPhi" }, "MaPhieuXuatNLDaSC=?"+ "MaChiPhiKhac=?", new string[] { contact,chiphi }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUXUATNGUYENLIEUDESOCHECOCHIPHI", "MaPhieuXuatNLDeSC=?", new String[] { cursor.GetString(0),cursor.GetString(1) });
                }

                cursor.Close();
            }
        }
    }
}