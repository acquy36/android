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
    class PhieuThuDbHelper : DataBase
    {
        public PhieuThuDbHelper(Context ctx,string DataName) : base(ctx, DataName)
        {
        }
        public IList<PhieuThu> GetAllPhieuThu()
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUTHU", new string[] { "MaPhieuThu", "MaNV", "MaKH", "NgayPhieuThu", "SoTienThu" }, null, null, null, null, null);

            var contacts = new List<PhieuThu>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuThu
                {
                    MaPhieuThu = c.GetString(0),
                    MaNV = c.GetString(1),
                    MaKH = c.GetString(2),
                    NgayPhieuThu = c.GetString(3),
                    SoTienThu =c.GetFloat(4)   
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Retrive All Contact Diachi
        public IList<PhieuThu> GetPhieuThuBySearchName(string nameToSearch)
        {

            SQLiteDatabase db = this.ReadableDatabase;

            ICursor c = db.Query("PHIEUTHU", new string[] { "MaPhieuThu", "MaNV", "MaKH", "NgayPhieuThu", "SoTienThu" }, "upper(MaKH) LIKE ?", new string[] { "%" + nameToSearch.ToUpper() + "%" }, null, null, null, null);

            var contacts = new List<PhieuThu>();

            while (c.MoveToNext())
            {
                contacts.Add(new PhieuThu
                {
                    MaPhieuThu = c.GetString(0),
                    MaNV = c.GetString(1),
                    MaKH = c.GetString(2),
                    NgayPhieuThu = c.GetString(3),
                    SoTienThu = c.GetFloat(4)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewPhieuThu(PhieuThu contactinfo)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuThu", contactinfo.MaPhieuThu);
            vals.Put("MaNV", contactinfo.MaNV);
            vals.Put("MaKH", contactinfo.MaKH);
            vals.Put("NgayPhieuThu", contactinfo.NgayPhieuThu.ToString());
            vals.Put("SoTienThu", contactinfo.SoTienThu);

            db.Insert("PHIEUTHU", null, vals);
        }

        //Get contact Diachi by contact MaKH
        public ICursor getPhieuThuById(string MaPhieuThu)
        {

            SQLiteDatabase db = this.WritableDatabase;
            ICursor res = db.RawQuery("select * from PHIEUTHU where MaPhieuThu='" + MaPhieuThu + "'", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePhieuThu(PhieuThu contitem)
        {

            if (contitem == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("MaPhieuThu", contitem.MaPhieuThu);
            vals.Put("MaNV", contitem.MaNV);
            vals.Put("MaKH", contitem.MaKH);
            vals.Put("NgayPhieuThu", contitem.NgayPhieuThu.ToString());
            vals.Put("SoTienThu", contitem.SoTienThu);


            ICursor cursor = db.Query("PHIEUTHU",
                    new String[] { "MaPhieuThu", "MaNV", "MaKH", "NgayPhieuThu", "SoTienThu" }, "MaPhieuThu=?", new string[] { contitem.MaKH }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Update("PHIEUTHU", vals, "MaPhieuThu=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }


        //Delete Existing contact
        public void DeletePhieuThu(string contactMaKH)
        {

            if (contactMaKH == null)
            {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            ICursor cursor = db.Query("PHIEUTHU",
                    new String[] { "MaPhieuThu", "MaNV", "MaKH", "NgayPhieuThu", "SoTienThu" }, "MaPhieuThu=?", new string[] { contactMaKH }, null, null, null, null);

            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    // update the row
                    db.Delete("PHIEUTHU", "MaPhieuThu=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }

    }
}