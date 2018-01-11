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

using Android.Database;

namespace QLSX.Resources.Model
{
    public class PhieuThu
    {
        //thuoc tinh
        public string MaPhieuThu { get; set; }
        public string NgayPhieuThu { get; set; }
        public float SoTienThu { get; set; }
        public string MaNV { get; set; }
        public string MaKH { get; set; }

    }
}