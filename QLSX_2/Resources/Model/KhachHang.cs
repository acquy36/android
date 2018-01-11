using System;


namespace QLSX.Resources.Model
{
    public class KhachHang
    {
         
        //thuoc tinh
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string Diachi { get; set; }
        public string SoDT { get; set; }
        public string Email { get; set; }
        
        public static explicit operator KhachHang(Java.Lang.Object v)
        {
            throw new NotImplementedException();
        }
        

    }
}