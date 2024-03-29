using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using QLSX.Resources.Model;
using Android.Database.Sqlite;

namespace QLSX_2.Resources.Data
{
    public class DataBase : SQLiteOpenHelper
    {
        private const int APP_DATABASE_VERSION = 1;
        protected string APP_DATABASENAME;
        SQLiteDatabase db;
        private static string TABLE_KHACHHANG = "CREATE TABLE IF NOT EXISTS KHACHHANG (MaKH TEXT PRIMARY KEY NOT NULL, TenKH TEXT, DiaChiKH TEXT, SoDTKH TEXT, EmailKH TEXT)";
        private static string TABLE_NHANVIEN = "CREATE TABLE IF NOT EXISTS NHANVIEN (MaNV TEXT PRIMARY KEY NOT NULL, TenNV TEXT, DiaChiNV TEXT, SoDTNV TEXT, EmailNV TEXT)";
        private static string TABLE_NHACUNGCAP = "CREATE TABLE IF NOT EXISTS NHACUNGCAP (MaNCC TEXT PRIMARY KEY NOT NULL, TenNCC TEXT, DiaChiNCC TEXT, SoDTNCC TEXT,EmailNCC TEXT)";
        private static string TABLE_PHIEUCHI = "CREATE TABLE IF NOT EXISTS PHIEUCHI (MaPhieuChi TEXT PRIMARY KEY , MaNCC TEXT REFERENCES NHACUNGCAP (MaNCC) , MaNV TEXT REFERENCES NHANVIEN (MaNV), NgayPhieuChi DATETIME, SoTienChi INTEGER )";
        private static string TABLE_PHIEUTHU = "CREATE TABLE IF NOT EXISTS PHIEUTHU (MaPhieuThu TEXT PRIMARY KEY NOT NULL, MaNV TEXT REFERENCES NHANVIEN (MaNV), MaKH TEXT REFERENCES KHACHHANG (MaKH), NgayPhieuThu DATETIME, SoTienThu INTEGER )";
        private static string TABLE_THANHPHAM = "CREATE TABLE IF NOT EXISTS THANHPHAM (MaTP TEXT PRIMARY KEY NOT NULL, TenTP TEXT, DVTTP TEXT)";
        private static string TABLE_LOAINGUYENLIEU = "CREATE TABLE IF NOT EXISTS LOAINGUYENLIEU (MaLoai TEXT PRIMARY KEY NOT NULL, TenLoai TEXT )";
        private static string TABLE_NGUYENLIEU = "CREATE TABLE IF NOT EXISTS NGUYENLIEU (MaNL TEXT PRIMARY KEY NOT NULL, MaLoai TEXT REFERENCES LOAINGUYENLIEU (MaLoai), TenNL TEXT,DVTNL TEXT )";
        private static string TABLE_PHIEUNHAPNGUYENLIEU = "CREATE TABLE IF NOT EXISTS PHIEUNHAPNGUYENLIEU (MaPhieuNhapNL TEXT PRIMARY KEY NOT NULL, MaNV TEXT REFERENCES NHANVIEN (MaNV), MaNCC TEXT REFERENCES NHACUNGCAP (MaNCC), NgayPhieuNhapNL DATETIME )";
        private static string TABLE_PHIEUNHAPNGUYENLIEUCT = "CREATE TABLE IF NOT EXISTS PHIEUNHAPNGUYENLIEUCT (MaPhieuNhapNL TEXT REFERENCES PHIEUNHAPNGUYENLIEU (MaPhieuNhapNL) , MaNL TEXT REFERENCES NGUYENLIEU (MaNL) ,  SLNhapNL INTEGER, DGNhapNL INTEGER,PRIMARY KEY(MaPhieuNhapNL,MaNL) )";
        private static string TABLE_PHIEUXUATNGUYENLIEUDESOCHE = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDESOCHE (MaPhieuXuatNLDeSC TEXT PRIMARY KEY NOT NULL, MaNV TEXT REFERENCES NHANVIEN (MaNV),  NgayPhieuXuatNLDeSC DATETIME )";
        private static string TABLE_PHIEUXUATNGUYENLIEUDESOCHECT = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDESOCHECT (MaPhieuXuatNLDeSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDESOCHE (MaPhieuXuatNLDeSC), MaNL TEXT REFERENCES NGUYENLIEU (MaNL) ,  SLXuatNL INTEGER, DGXuatNL INTEGER, PRIMARY KEY(MaPhieuXuatNLDeSC,MaNL))";
        private static string TABLE_PHIEUXUATTHANHPHAM = "CREATE TABLE IF NOT EXISTS PHIEUXUATTHANHPHAM (MaPhieuXuatTP TEXT PRIMARY KEY, MaNV TEXT REFERENCES NHANVIEN (MaNV), MaKH TEXT REFERENCES KHACHHANG (MaKH),  NgayPhieuXuatTP DATETIME )";
        private static string TABLE_PHIEUXUATTHANHPHAMCT = "CREATE TABLE IF NOT EXISTS PHIEUXUATTHANHPHAMCT (MaPhieuXuatTP TEXT REFERENCES PHIEUXUATTHANHPHAM (MaPhieuXuatTP) , MaTP TEXT REFERENCES THANHPHAM (MaTP),  SLXuatTP INTEGER, DGXuatTP INTEGER ,PRIMARY KEY(MaTP,MaPhieuXuatTP))";
        private static string TABLE_NGUYENLIEUDASOCHE = "CREATE TABLE IF NOT EXISTS NGUYENLIEUDASOCHE (MaNLDaSC TEXT PRIMARY KEY NOT NULL,  TenNLDaSC TEXT, DVTNLDaSC TEXT )";
        private static string TABLE_PHIEUNHAPNGUYENLIEUDASOCHECT = "CREATE TABLE IF NOT EXISTS PHIEUNHAPNGUYENLIEUDASOCHECT (MaPhieuXuatNLDeSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDESOCHE (MaPhieuXuatNLDeSC) , MaNLDaSC TEXT REFERENCES NGUYENLIEUDASOCHE (MaNLDaSC),  SLNhapNLSoche INTEGER, DGNhapNLSoche INTEGER ,PRIMARY KEY(MaNLDaSC,MaPhieuXuatNLDeSC))";
        private static string TABLE_PHIEUXUATNGUYENLIEUDASOCHE = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDASOCHE (MaPhieuXuatNLDaSC TEXT PRIMARY KEY NOT NULL, MaNV TEXT REFERENCES NHANVIEN (MaNV),  NgayPhieuXuatNLDaSC DATETIME )";
        private static string TABLE_PHIEUXUATNGUYENLIEUDASOCHECT = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDASOCHECT (MaNLDaSC TEXT REFERENCES NGUYENLIEUDASOCHE (MaNLDaSC) , MaPhieuXuatNLDaSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDASOCHE (MaPhieuXuatNLDaSC), SLXuatNLSoche INTEGER, DGXuatNLSoche INTEGER , PRIMARY KEY(MaPhieuXuatNLDaSC,MaNLDaSC))";
        private static string TABLE_PHIEUNHAPTHANHPHAMCT = "CREATE TABLE IF NOT EXISTS PHIEUNHAPTHANHPHAMCT (MaPhieuXuatNLDaSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDASOCHE (MaPhieuXuatNLDaSC), MaTP TEXT REFERENCES THANHPHAM (MaTP) , SLNhapTP INTEGER, DGNhapTP INTEGER, PRIMARY KEY (MaTP,MaPhieuXuatNLDaSC) )";
        private static string TABLE_CHIPHIKHAC = "CREATE TABLE IF NOT EXISTS CHIPHIKHAC (MaChiPhiKhac TEXT PRIMARY KEY NOT NULL, TenChiPhiKhac TEXT, DVTChiPhiKhac TEXT)";
        private static string TABLE_PHIEUXUATNGUYENLIEUDESOCHOCOCHIPHI = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDESOCHECOCHIPHI (MaPhieuXuatNLDeSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDESOCHE (MaPhieuXuatNLDeSC), MaChiPhiKhac TEXT REFERENCES CHIPHIKHAC (MaChiPhiKhac),  SLXuatNLDeSCChiPhi INTEGER, DGXuatNLDeSCChiPhi INTEGER,  PRIMARY KEY(MaChiPhiKhac,MaPhieuXuatNLDeSC) )";
        private static string TABLE_PHIEUXUATNGUYENLIEUDASOCHOCOCHIPHI = "CREATE TABLE IF NOT EXISTS PHIEUXUATNGUYENLIEUDASOCHECOCHIPHI (MaPhieuXuatNLDaSC TEXT REFERENCES PHIEUXUATNGUYENLIEUDASOCHE (MaPhieuXuatNLDaSC) , MaChiPhiKhac TEXT REFERENCES CHIPHIKHAC (MaChiPhiKhac) ,  SLXuatNLDaSCChiPhi INTEGER, DGXuatNLDaSCChiPhi INTEGER,PRIMARY KEY(MaChiPhiKhac,MaPhieuXuatNLDaSC) )";
        private static string TABLE_NGAY = "CREATE TABLE IF NOT EXISTS NGAY (NgayChiPhikhac DATETIME PRIMARY KEY)";
        private static string TABLE_CHIPHIKHACCT = "CREATE TABLE IF NOT EXISTS CHIPHIKHACCT (MaChiPhiKhac TEXT REFERENCES CHIPHIKHAC (MaChiPhiKhac)  , NgayChiPhiKhac DATETIME REFERENCES NGAY (NgayChiPhiKhac) , GiaChiPhiKhac INTEGER, PRIMARY KEY(NgayChiPhiKhac,MaChiPhiKhac))";
        private static string TABLE_LOGIN = "CREATE TABLE IF NOT EXISTS LOGINTABLE (ID INTEGER PRIMARY KEY   AUTOINCREMENT, Username Text, Password Text)";

        public DataBase(Context ctx,string APP_DATABASENAME) : base(ctx, APP_DATABASENAME, null, APP_DATABASE_VERSION)
        {
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), APP_DATABASENAME + ".db3");
            db = SQLiteDatabase.OpenOrCreateDatabase(path, null);
        }

    public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(TABLE_KHACHHANG);
            db.ExecSQL(TABLE_NHANVIEN);
            db.ExecSQL(TABLE_NHACUNGCAP);
            db.ExecSQL(TABLE_LOAINGUYENLIEU);
            db.ExecSQL(TABLE_PHIEUCHI);
            db.ExecSQL(TABLE_PHIEUTHU);
            db.ExecSQL(TABLE_NGUYENLIEU);
            db.ExecSQL(TABLE_THANHPHAM);
            db.ExecSQL(TABLE_PHIEUNHAPNGUYENLIEU);
            db.ExecSQL(TABLE_PHIEUNHAPNGUYENLIEUCT);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDESOCHE);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDESOCHECT);
            db.ExecSQL(TABLE_PHIEUXUATTHANHPHAM);
            db.ExecSQL(TABLE_PHIEUXUATTHANHPHAMCT);
            db.ExecSQL(TABLE_NGUYENLIEUDASOCHE);
            db.ExecSQL(TABLE_PHIEUNHAPNGUYENLIEUDASOCHECT);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDASOCHE);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDASOCHECT);
            db.ExecSQL(TABLE_PHIEUNHAPTHANHPHAMCT);
            db.ExecSQL(TABLE_CHIPHIKHAC);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDESOCHOCOCHIPHI);
            db.ExecSQL(TABLE_PHIEUXUATNGUYENLIEUDASOCHOCOCHIPHI);
            db.ExecSQL(TABLE_NGAY);
            db.ExecSQL(TABLE_CHIPHIKHACCT);
            db.ExecSQL(TABLE_LOGIN);

        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS KHACHHANG");
            db.ExecSQL("DROP TABLE IF EXISTS NHANVIEN");
            db.ExecSQL("DROP TABLE IF EXISTS NHACUNGCAP");
            db.ExecSQL("DROP TABLE IF EXISTS LOAINGUYENLIEU");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUCHI");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUTHU");
            db.ExecSQL("DROP TABLE IF EXISTS NGUYENLIEU");
            db.ExecSQL("DROP TABLE IF EXISTS THANHPHAM");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUNHAPNGUYENLIEU");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUNHAPNGUYENLIEUCT");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDESOCHE");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDESOCHECT");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATTHANHPHAM");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATTHANHPHAMCT");
            db.ExecSQL("DROP TABLE IF EXISTS NGUYENLIEUDASOCHE");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUNHAPNGUYENLIEUDASOCHECT");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDASOCHE");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDASOCHECT");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUNHAPTHANHPHAMCT");
            db.ExecSQL("DROP TABLE IF EXISTS CHIPHIKHAC");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDESOCHOCOCHIPHI");
            db.ExecSQL("DROP TABLE IF EXISTS PHIEUXUATNGUYENLIEUDASOCHOCOCHIPHI");
            db.ExecSQL("DROP TABLE IF EXISTS NGAY");
            db.ExecSQL("DROP TABLE IF EXISTS CHIPHIKHACCT");
            db.ExecSQL("DROP TABLE IF EXISTS LOGINTABLE");
            OnCreate(db);
        }
    
    }
}